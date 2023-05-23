using Idea_Database_Interface;
using Idea_Database_Interface.Data;
using Idea_Database_Interface.Data.Repository;
using Idea_Database_Interface.Data.Repository.Interfaces;
using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing.Text;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region language
CultureInfo[] supportedCultures = new[]
{
    new CultureInfo("es-ES"),
    new CultureInfo("en")
};
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        };
});
#endregion
builder.Services.AddControllersWithViews().AddViewLocalization().AddMvcLocalization().AddDataAnnotationsLocalization();
builder.Services.AddDbContext<IdeaDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdeaDb")));
builder.Services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<IdeaDBContext>();
builder.Services.AddScoped<IGenericRepository<Empresa>, GenericRepository<Empresa>>();
builder.Services.AddScoped<IGenericRepository<Correspondencia>, GenericRepository<Correspondencia>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRazorPages();


builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("SendGrid"));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<IdeaDBContext>();
    context.Database.EnsureCreated();
    // DbInitializer.Initialize(context);
}
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
