
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using LearnApp.Models;
using Newtonsoft.Json;
using System.Text;
using LearnApp.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using LearnApp.Enums;
using Microsoft.AspNetCore.Authorization;

#nullable disable


namespace LearnApp.Controllers;

[Authorize(Roles ="Learner")]
public class LearnerController : Controller{
    
    Uri baseAddress;
    HttpClient httpClient;   //By default available in the namespace System.Net.Http;
    //Note: Using this httpClient Obj we can call the Asp.net webAPI ie REST Api services.

    //constructor injection
    private readonly ICourseRepository _courseRepository;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public LearnerController(ICourseRepository courseRepository,ApplicationDbContext applicationDbContext,IConfiguration configuration,UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager){
        _courseRepository = courseRepository;
        _applicationDbContext = applicationDbContext;
        _configuration = configuration;
        httpClient = new HttpClient();
        _userManager = userManager;
        _roleManager = roleManager;

        //Fetching from the Property files
        httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
    }

    //Learner Dashboard Action
    public IActionResult Index(){

        IEnumerable courses = _courseRepository.GetCourses();
        ViewBag.reportStatus = HttpContext.Session.GetString("REPORTSTATUS");  //Passsing to View
        return View(courses);
    }


    //My Learning Action
    public async Task<IActionResult> MyLearning(){

        // var username = User.FindFirstValue(ClaimTypes.Name);
        // Console.WriteLine("*****************"+username);
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        if(user == null){
            return RedirectToAction("Logout","Account");
        }

        Console.WriteLine("**********My Learning Page*********");

        Console.WriteLine(user.PersonName);
        Console.WriteLine(user.UserName);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.BatchId);
        
        Console.WriteLine("****************************");
        
        IEnumerable courses = _courseRepository.FetchCoursesForBatch(user.BatchId);
        return View(courses);
    }

    //Status Update Action
    public async Task<IActionResult> StatusUpdate(){

        //Get the logged in user
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        if(user == null){
            return RedirectToAction("Logout","Account");
        }

        Console.WriteLine("**********Status Update Page**********");

        Console.WriteLine(user.PersonName);
        Console.WriteLine(user.UserName);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.BatchId);
        
        Console.WriteLine("****************************");

        ViewBag.batchid = user.BatchId;
        ViewBag.userid = user.UserName;
        return View();
    }

    [HttpPost]
    public IActionResult StatusUpdate(StatusReport report){

        //Add report into database
        report.ReportStatus = "InHold";
        _applicationDbContext.StatusReports.Add(report);
        _applicationDbContext.SaveChanges();
        
        //Update in the Home Page of User by creating a Session
        HttpContext.Session.SetString("REPORTSTATUS",report.ReportStatus);
        
        //HttpContext.Session.Remove("REPORTSTATUS");
        TempData["notify"] = "Report Submitted Successful";
        
        return RedirectToAction("StatusUpdate","Learner");
    }

   
    //Contact Action
    [HttpGet]
    public async Task<IActionResult> Contact(){

        //Get the logged in user
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        if(user == null){
            return RedirectToAction("Logout","Account");
        }

        Console.WriteLine("**********Contact Page**********");

        Console.WriteLine(user.PersonName);
        Console.WriteLine(user.UserName);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.BatchId);
        
        Console.WriteLine("****************************");

        ViewBag.userid = user.UserName;
        ViewBag.email = user.Email;

        return View();
    }

    [HttpPost]
    public IActionResult Contact(Query query){

        if(query == null){
            return BadRequest();
        }
        
        string data = JsonConvert.SerializeObject(query);
        StringContent content = new StringContent(data,Encoding.UTF8,"application/json");

        HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress+"/Queries/PostQuery/",content).Result;

        if(response.IsSuccessStatusCode){
            TempData["Notify"] = "Query received Successfully";
        }
        return RedirectToAction("Contact");
    }


    public async Task<IActionResult> Playlist(string courseid,string coursename){

        //Get the logged in user
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        if(user == null){
            return RedirectToAction("Logout","Account");
        }

        Console.WriteLine("**********Playlist Page**********");

        Console.WriteLine(user.PersonName);
        Console.WriteLine(user.UserName);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.BatchId);
        
        Console.WriteLine("****************************");

        if(await _userManager.IsInRoleAsync(user,UserRoleOptions.Learner.ToString())){
            ViewBag.role = "Learner";
        }
        else if(await _userManager.IsInRoleAsync(user,UserRoleOptions.Instructor.ToString())){
             ViewBag.role = "Instructor";
        }
        else{
            ViewBag.role = "Admin";
        }
        
        Course playlistDetails = _courseRepository.FetchDetailsForPlaylist(courseid,coursename);
        return View(playlistDetails);
    }


    //Action Method for Search
    public async Task<IActionResult> Search(IFormCollection form){      

        //IFormCollection is used to pass form data without using a Model
        //fetch the course and redirect according to that
        //Console.WriteLine(form["name"]);

        //Get the logged in user
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        if(user == null){
            return RedirectToAction("Logout","Account");
        }

        Console.WriteLine("**********Search Box action**********");

        Console.WriteLine(user.PersonName);
        Console.WriteLine(user.UserName);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.BatchId);
        
        Console.WriteLine("****************************");

        string batchid = user.BatchId;
        string coursename = form["name"];
       
        bool status = _courseRepository.CheckCourseOnBatchId(coursename,batchid);
        if(status){
            TempData["success"] = "you can Learn "+coursename+" now.";
            return RedirectToAction("MyLearning","Learner");
        }

        TempData["notify"] = "course is currently not available";
        return RedirectToAction("MyLearning","Learner");

    }



    public async Task<IActionResult> DisplayFile(string id){

        //Get the logged in user
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        if(user == null){
            return RedirectToAction("Logout","Account");
        }

        Console.WriteLine("**********File Material Page**********");

        Console.WriteLine(user.PersonName);
        Console.WriteLine(user.UserName);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.BatchId);
        
        Console.WriteLine("****************************");

        if(await _userManager.IsInRoleAsync(user,UserRoleOptions.Learner.ToString())){
            ViewBag.role = "Learner";
        }
        else if(await _userManager.IsInRoleAsync(user,UserRoleOptions.Learner.ToString())){
             ViewBag.role = "Instructor";
        }
        else{
            ViewBag.role = "Admin";
        }

        string batchid = user.BatchId;

        FileRepository fileRepository = new FileRepository();
        DataTable datatable = fileRepository.FetchFiles(id);
        
        return View(datatable);
    }



}