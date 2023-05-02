using Microsoft.AspNetCore.Identity;

namespace Idea_Database_Interface.Viewmodels
{
    public class UsuariosListViewModel
    {
        public List<IdentityUser> Users { get; set; }
        public UserManager<IdentityUser> UserManager;
        public RoleManager<IdentityRole> RoleManager;
    }
}
