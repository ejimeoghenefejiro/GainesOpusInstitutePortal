using Autofac;
using Autofac.Extensions.DependencyInjection;
using GainesOpusInstitute.DataEntity;
using GainesOpusInstitute.DataEntity.Entity;
using GainesOpusInstitute.Repository;
using GainesOpusInstitute.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
    builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
    builder.Services.AddDbContext<GOSContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
            ));

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
    //builder.Services.AddDefaultIdentity<User>()
        builder.Services.AddIdentityCore<User>()
                   .AddRoles<Role>()
                   .AddUserManager<UserManager<User>>()
                   .AddRoleManager<RoleManager<Role>>()
                   .AddSignInManager<SignInManager<User>>()
                   .AddEntityFrameworkStores<GOSContext>();


builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policyBuilder =>
            policyBuilder.RequireClaim(Claims.Admin, "true"));

        options.AddPolicy("Cashier", policyBuilder =>
            policyBuilder.RequireClaim(Claims.User, "true"));
    });
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register services directly with Autofac here. Don't
// call builder.Populate(), that happens in AutofacServiceProviderFactory.
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacRepoModule()));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

 async Task CreateUserRoles(IServiceProvider serviceProvider)
{
    var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
    var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();


    var user = new User() { Email = Constants.Email, UserName = Constants.UserName };
    //var user = new User() { Email = "admin@gmail.com", UserName = "admin" };

    var result = await UserManager.CreateAsync(user, Constants.AdminPassword);
    await UserManager.AddClaimAsync(user, new Claim("admin", user.Id.ToString()));


}
app.Run();


