using Idea_Database_Interface.Data;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles ="admin")]
    public class UsersController : Controller
    {
        private readonly IdeaDBContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UsersController(IdeaDBContext context, UserManager<IdentityUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _userManager = user;
            _roleManager = role;
            
        }
        //shows a list of all the users in the database
        public IActionResult Index()
        {
            UsuariosListViewModel model = new UsuariosListViewModel()
            {
                Users = _userManager.Users.ToList(),
                RoleManager = _roleManager,
                UserManager = _userManager

            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //deletes the selected user from the database
        public async Task<IActionResult> DeleteUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
            }
            else
                ModelState.AddModelError("", "User not found");

            return RedirectToAction("Index");
        }
        //gives the admin role to the selected user
        public async Task<IActionResult> UserAddAdmin(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            IdentityRole role = await _roleManager.FindByNameAsync("admin");
            //if the admin role doesnt exist, creates it
            if(role == null) { await _roleManager.CreateAsync(new IdentityRole("admin")); }
            IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name);
            return RedirectToAction("Index");
        }
    }
}
