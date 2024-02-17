using Graduation_Project.Shared.Models;
using Graduation_Project_Dashboard.Data;
using Graduation_Project_Dashboard.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reflections.Framework.DataAccessLayer;
using Reflections.Framework.RoleBasedSecurity;
using M = Graduation_Project.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register DbContext
builder.Services.AddDbContext<ApplicationDatabaseContext>(options =>
    options.UseSqlServer(connectionString));



// Configure Identity


// Add Razor Pages
builder.Services.AddRazorPages();

// Configure application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/identity/account/login";
    //options.IdleTimeout = TimeSpan.FromDays(1);
});



builder.Services.AddIdentity<User, M.Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<M.Role>()
    .AddEntityFrameworkStores<ApplicationDatabaseContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


// Add services

builder.Services.AddTransient<UserResolverService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromDays(1);
});

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Errors/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
