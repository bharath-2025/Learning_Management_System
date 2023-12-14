using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnApp.Models;
using System.Collections;

//Namespace to use DynamicModel
using System.Dynamic;
using System.Data;
using Microsoft.AspNetCore.Identity;
using LearnApp.IdentityEntities;
using Microsoft.AspNetCore.Authorization;

namespace LearnApp.Controllers;

[Authorize(Roles ="Admin")]
public class AdminsController : Controller
{
    //constructor injection
    private readonly ILogger<AdminsController> _logger;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public AdminsController(ICourseRepository courseRepository,IUserRepository userRepository,ApplicationDbContext applicationDbContext,UserManager<ApplicationUser> userManager,ILogger<AdminsController> logger){
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
        _logger = logger;
    }
    
     //Admin Dashboard 
    public IActionResult Index()
    {
        _logger.LogInformation("Admin Dashboard Page Index action method is fired");
        IEnumerable courses = _courseRepository.GetCourses();
        return View(courses);
    }

    //Manage Courses
    [HttpGet]
    public IActionResult manageCourses(string? searchBy,string? searchString,int page=1){
        _logger.LogInformation("manage courses action method is reached");
        _logger.LogDebug("Debugging values:\n searchBy: "+searchBy+" searchString: "+searchString+" PageNo :"+page);
        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        ViewBag.Notify = TempData["Notify"];

        List<Course> allCourses = _courseRepository.GetCourses();
        List<Course> matchingCourses = allCourses;

        //Business logic for pagination
        const int pageSize = 10;
        if(page < 1){
            page = 1;
        }

        int totalItems = matchingCourses.Count();

        var pager = new Pager(totalItems,page,pageSize);
        int itemSkip = (page-1)*pageSize;

        ViewBag.Pager = pager;


        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
        {
            var data1 = matchingCourses.Skip(itemSkip).Take(pager.PageSize).ToList();
            return View(data1);
        }

        switch (searchBy)
        {
            case nameof(Course.CourseId):
                matchingCourses = allCourses.Where(temp =>
                (!string.IsNullOrEmpty(temp.CourseId)?
                temp.CourseId.StartsWith(searchString,StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Course.CourseName):
                matchingCourses = allCourses.Where(temp =>
                (!string.IsNullOrEmpty(temp.CourseName) ?
                temp.CourseName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Course.BatchId):
                matchingCourses = allCourses.Where(temp =>
                (!string.IsNullOrEmpty(temp.BatchId) ?
                temp.BatchId.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            default:
                matchingCourses = allCourses;
                break;
        }

        if(page < 1){
            page = 1;
        }

         totalItems = matchingCourses.Count();

         pager = new Pager(totalItems,page,pageSize);
         itemSkip = (page-1)*pageSize;

        ViewBag.Pager = pager;

        var data = matchingCourses.Skip(itemSkip).Take(pager.PageSize).ToList();

        //return View(matchingCourses);
        return View(data);

    }


    public IActionResult CreateCourse(){
        return View();
    }

    [HttpPost]
    public IActionResult CreateCourse(IFormCollection form){
        _logger.LogInformation("createCourse action method is reached");
        Course course = new Course();

        course.CourseId = form["CourseId"];
        course.CourseName = form["CourseName"];
        course.StartDate = Convert.ToDateTime(form["StartDate"]);
        course.BatchId = form["BatchId"];
    

        foreach(var file in Request.Form.Files){
            Console.WriteLine(file.FileName);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            Console.WriteLine(Path.GetExtension(file.FileName));
            if(Path.GetExtension(file.FileName) == ".jpg" || Path.GetExtension(file.FileName) == ".png"){
                Console.WriteLine("Image Detected");
                course.PhotoPath = memoryStream.ToArray();
            }
            else{
                Console.WriteLine("Video Detected");
                course.VideoPath = memoryStream.ToArray();
            }
        }

        _courseRepository.AddCourse(course);
        return RedirectToAction("manageCourses","Admins");
    }


    //To Delete a Course
    public IActionResult DeleteCourse(string courseid){

        TempData["Notify"] = $"The {courseid} is deleted successfully";
        _courseRepository.DeleteCourse(courseid);
        return RedirectToAction("manageCourses","Admins");
    }

    //Manage Learners
    public IActionResult manageLearners(string? searchBy,string? searchString,int page=1){

        //fetching all the users that are created using Identity from 'UserManager<ApplicationUser>' bussiness layer
        ViewBag.Notify = TempData["Notify"];

        //step1: check if "SearchBy" is not null
        //step2: Get matching persons from List<ApplicationUser> based on given searchBy and searchString.
        //Return all matching ApplicationUser objects

        List<ApplicationUser> allUsers = _userManager.Users.ToList();

        List<ApplicationUser> matchingUsers = allUsers;

        //Business logic for pagination
        const int pageSize = 10;
        if(page < 1){
            page = 1;
        }

        int totalItems = matchingUsers.Count();

        var pager = new Pager(totalItems,page,pageSize);
        int itemSkip = (page-1)*pageSize;

        ViewBag.Pager = pager;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)){
            var data1 = matchingUsers.Skip(itemSkip).Take(pager.PageSize).ToList();
             return View(data1);
        }
           

        switch (searchBy)
        {
            case nameof(ApplicationUser.PersonName):
                matchingUsers = allUsers.Where(temp =>
                (!string.IsNullOrEmpty(temp.PersonName)?
                temp.PersonName.StartsWith(searchString,StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(ApplicationUser.Email):
                matchingUsers = allUsers.Where(temp =>
                (!string.IsNullOrEmpty(temp.Email) ?
                temp.Email.StartsWith(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(ApplicationUser.UserName):
                matchingUsers = allUsers.Where(temp =>
                (!string.IsNullOrEmpty(temp.UserName) ?
                temp.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(ApplicationUser.BatchId):
                matchingUsers = allUsers.Where(temp =>
                (!string.IsNullOrEmpty(temp.BatchId) ?
                temp.BatchId.Equals(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            default:
                matchingUsers = allUsers;
                break;
        }

        var data = matchingUsers.Skip(itemSkip).Take(pager.PageSize).ToList();

        return View(data);
    }


    //To Delete a User
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string username){

        Console.WriteLine("******delete action method is fired************");

        ApplicationUser? user = await _userManager.FindByNameAsync(username);

        if(user == null){
            ViewBag.ErrorMeassage = $"The user with {username} isn't found";
            return View("NotFound");
        }

        var result = await _userManager.DeleteAsync(user);
        if(result.Succeeded){
            TempData["Notify"] = $"The {username} is deleted successfully";
            return RedirectToAction("manageLearners","Admins");
        }

        foreach(var error in result.Errors){
            ModelState.AddModelError("Delete",error.Description);
        }
        return View();
    }


    //ManageMaterials
    public IActionResult ManageMaterials(){
        _logger.LogInformation("manageMaterials action method is reached");
        ViewBag.SubmitStatus = TempData["SubmitStatus"];
        ViewBag.UpdateStatus = TempData["UpdateStatus"];
        
        return View();
    }

    [HttpPost]
    public IActionResult ManageMaterials(Files fileobj){

        //Adding Files to the Database

        foreach(var file in Request.Form.Files){
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            fileobj.Material = memoryStream.ToArray();
        }

        FileRepository fileRepository = new FileRepository();
        fileRepository.AddFile(fileobj);

        TempData["SubmitStatus"] = "Material Added Successfully";

        return RedirectToAction("ManageMaterials","Admins");
    }


    public IActionResult EditMaterials(Files fileobj){
        //Adding Files to the Database

        foreach(var file in Request.Form.Files){
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            fileobj.Material = memoryStream.ToArray();
        }

        FileRepository fileRepository = new FileRepository();
        fileRepository.UpdateFileData(fileobj);

        TempData["UpdateStatus"] = "File Updated Successfully";
        return RedirectToAction("ManageMaterials","Admins");
    }

    public IActionResult ViewMaterials(){
        _logger.LogInformation("ViewMaterials action method is reached");
        FileRepository fileRepository = new FileRepository();
        DataTable dataTable = fileRepository.FetchFiles();
        return View(dataTable);
    }

    
}
