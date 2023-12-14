
using LearnApp.IdentityEntities;
using LearnApp.Enums;
using LearnApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace LearnApp.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;     //to manipulate roles we are using this rolesManager of Identity

    public AccountController(ILogger<AccountController> logger,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [Authorize(Roles="Admin")]
    public IActionResult Register(){
        return View();
    }

    [HttpPost][Authorize(Roles="Admin")]
    public async Task<IActionResult> Register(RegisterDTO registerDTO){
        Console.WriteLine(registerDTO.BatchId);
        Console.WriteLine(registerDTO.Role);
        //ToDo: Store user regestration details into Identity database

        //check for validation
        if(!ModelState.IsValid){
            ViewBag.Errors = ModelState.Values.SelectMany(val => val.Errors).Select(err => err.ErrorMessage);
            
            return View(registerDTO);
        }

        ApplicationUser applicationUser = new ApplicationUser();
        applicationUser.PersonName = registerDTO.PersonName;
        applicationUser.UserName = registerDTO.PersonId;
        applicationUser.BatchId = registerDTO.BatchId;
        applicationUser.Email = registerDTO.Email;

        if(string.IsNullOrEmpty(registerDTO.Password)){
            return View(registerDTO); 
        }

        IdentityResult result = await _userManager.CreateAsync(applicationUser,registerDTO.Password);

        if(result.Succeeded){
            //check status of the role
            if(registerDTO.Role == UserRoleOptions.Admin){
                //TODO: 1. Add Admin Role to to the AspNetRoles table
                //      2. Add particular user into the Admin Role ie into AspNetUserRoles 

                //Create 'Admin' role if it is not created for first time
                if(await _roleManager.FindByNameAsync(UserRoleOptions.Admin.ToString()) is null){
                    ApplicationRole applicationRole = new ApplicationRole(){
                        Name = UserRoleOptions.Admin.ToString()
                    };

                    await _roleManager.CreateAsync(applicationRole);
                }

                //Add the new user into 'Admin' role
                await _userManager.AddToRoleAsync(applicationUser,UserRoleOptions.Admin.ToString());
            }
            else if(registerDTO.Role == UserRoleOptions.Instructor){
                //Create 'Instructor' role if it is not created for first time
                if(await _roleManager.FindByNameAsync(UserRoleOptions.Instructor.ToString()) is null){
                    ApplicationRole applicationRole = new ApplicationRole(){
                        Name = UserRoleOptions.Instructor.ToString()
                    };

                    await _roleManager.CreateAsync(applicationRole);
                }

                //Add the new user into 'Instructor' role
                await _userManager.AddToRoleAsync(applicationUser,UserRoleOptions.Instructor.ToString());

            }
            else{
                 //Create 'Instructor' role if it is not created for first time
                if(await _roleManager.FindByNameAsync(UserRoleOptions.Learner.ToString()) is null){
                    ApplicationRole applicationRole = new ApplicationRole(){
                        Name = UserRoleOptions.Learner.ToString()
                    };

                    await _roleManager.CreateAsync(applicationRole);
                }
                //Add the new user into 'Learner' role
                await _userManager.AddToRoleAsync(applicationUser,UserRoleOptions.Learner.ToString());
            }
            return RedirectToAction("ManageLearners","Admins");
        }
        else{
            foreach(IdentityError error in result.Errors){
                ModelState.AddModelError("Register",error.Description);
            }
        }

        return View(registerDTO);
    }

    //Logout
    [AllowAnonymous]
    public async Task<IActionResult> Logout(){
         _logger.LogInformation("Account controller logout action method is fired: User Logged out successfully");
         _logger.LogWarning("Sample warning message");
         _logger.LogError("Sample Error message");
         _logger.LogCritical("Sample Critical/fatal message");

        // Console.WriteLine("User is logged out and Identity cookiee is deleted ");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index","Home");
    }

    
}
