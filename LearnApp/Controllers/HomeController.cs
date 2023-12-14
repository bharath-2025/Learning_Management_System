
using Microsoft.AspNetCore.Mvc;
using LearnApp.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;
using LearnApp.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using LearnApp.Enums;

namespace LearnApp.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{

    public HttpClient httpClient;
    public Uri baseAddress;

    //Constructor Injection
    private readonly IUserRepository _userRepository;           //Reference to the interface
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public HomeController(IConfiguration configuration,IUserRepository userRepository,SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager,ILogger<HomeController> logger){
        _userRepository = userRepository;
        httpClient = new HttpClient();
        _logger = logger;
        _configuration = configuration;

        _userManager = userManager;
        _signInManager = signInManager;

        //Fetching from the Property files
        httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);

    }

    [HttpGet]
    public IActionResult Index(){
        _logger.LogInformation("Home Page action method is reached");
        return View();
    }

    // [HttpPost]
    // public IActionResult Index(User user){
    //     //Server Side Validations
        
    //         //valiadte user
            

    //         if(_userRepository.ValidUser(user)){
    //             //fetch role
    //             string role = _userRepository.FetchRole(user);
    //             string batch = _userRepository.FetchBatch(user);
    //             string username = _userRepository.FetchName(user);
    //             HttpContext.Session.SetString("USERID",user.UserId);
    //             HttpContext.Session.SetString("BATCHID",batch);
    //             HttpContext.Session.SetString("USERNAME",username);
    //             HttpContext.Session.SetString("ROLE",role);

    //             //Console.WriteLine(role);
    //             if(role == "Admin"){
    //                 //fetch the UserName
    //                 string ?name = _userRepository.FetchName(user);
    //                 Console.WriteLine("AdminName: "+name);
    //                 return RedirectToAction("Index","Admins");
    //             }
    //             else if(role == "Learner"){
    //                 string ?name = _userRepository.FetchName(user);
    //                 Console.WriteLine("Learner Name: "+name);
    //                 return RedirectToAction("Index","Learner");
    //             }
    //             else if(role == "Instructor"){
    //                 //fetch the UserName
    //                 string ?name = _userRepository.FetchName(user);
    //                 Console.WriteLine("Instructor Name: "+name);
    //                 return RedirectToAction("Index","Instructor");
    //             }
    //         }
    //     //redirect to that Dashboard
    //     ViewData["Notify"] = "Invalid Credentials";
    //     return View();
    // }

    // Login

    [HttpPost]
    public async Task<IActionResult> Index(LoginDTO loginDTO){
        _logger.LogInformation("Home Page action method loginDTO is posted");
        
        //Model Validation
        if(!ModelState.IsValid){
            ViewBag.Errors = ModelState.Values.SelectMany(val => val.Errors).Select(err => err.ErrorMessage);
            return View(loginDTO);
        }

        if(loginDTO.Password == null || loginDTO.PersonId == null){
            return View(loginDTO);
        }

        var result = await _signInManager.PasswordSignInAsync(loginDTO.PersonId,loginDTO.Password,isPersistent:false,lockoutOnFailure:false);

        if(result.Succeeded){
            //redirection based on roles
            ApplicationUser? user = await _userManager.FindByNameAsync(loginDTO.PersonId);

            if(user != null){
                if(await _userManager.IsInRoleAsync(user,UserRoleOptions.Admin.ToString())){
                    Console.WriteLine("Role: Admin");
                    return RedirectToAction("Index","Admins");
                }
                else if(await _userManager.IsInRoleAsync(user,UserRoleOptions.Instructor.ToString())){
                    Console.WriteLine("Role: Instructor");
                    return RedirectToAction("Index","Instructor");
                }
                else{
                    Console.WriteLine("Role: Learner");
                    return RedirectToAction("Index","Learner");
                }
            }
        }

        ModelState.AddModelError("Login","Invalid userid or password");
        
        return View(loginDTO);

    }


    public IActionResult HelpDesk(){
        ViewBag.Notify = TempData["Notify"];
        return View();
    }

    [HttpPost]
    public IActionResult HelpDesk(Query query){

        if(query == null){
            return BadRequest();
        }
        
        string data = JsonConvert.SerializeObject(query);
        StringContent content = new StringContent(data,Encoding.UTF8,"application/json");

        HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress+"/Queries/PostQuery/",content).Result;

        if(response.IsSuccessStatusCode){
            TempData["Notify"] = "Query received Successfully";
        }
        return RedirectToAction("HelpDesk");
    }

    
}
