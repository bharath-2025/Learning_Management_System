//Project Title: LEARNING MANAGEMENT SYSTEM (MAX LEARN)
//Author: Mokara Bharath
//Created Date: 15-02-2023
//Last Modified Date: 05-03-2023
//Review Date: 10-03-23
//Reviewed By: Anitha Manogaran



using LearnApp.IdentityEntities;
using LearnApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Adding services for the DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Serilog 
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration) //read configuration settings from built in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

//Adding Service for the Session
builder.Services.AddSession(options =>{
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//Adding service to the IUserInterface Dependency
builder.Services.AddSingleton<IUserRepository,UserRepository>(); 
//Adding service to the ICourseRepository Dependency
builder.Services.AddSingleton<ICourseRepository,CourseRepository>();

//Adding service to the IEmailSender Dependency
builder.Services.AddTransient<IEmailSender,EmailSender>();




//Adding Identity Services for Authentication and Authorization
builder.Services.AddIdentity<ApplicationUser,ApplicationRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>();

//Authorization Service
builder.Services.AddAuthorization(options =>{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.FallbackPolicy = policy;
});

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Home/Index";
});


var app = builder.Build();

//logger messages
app.Logger.LogTrace("Log Message - Trace");
app.Logger.LogDebug("Log Message - Debug");
app.Logger.LogInformation("Log Message - Info");
app.Logger.LogWarning("Log Message - Warning");
app.Logger.LogError("Log Message - Error");
app.Logger.LogCritical("Log Message - Critical");

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

app.UseAuthentication();  //Reading Identity cookie
app.UseAuthorization();   //Validates access permission of the user

//use that service as a middleware
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
