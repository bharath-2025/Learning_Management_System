
using Microsoft.AspNetCore.Mvc;
using LearnApp.Models;
using System.Collections;
using Microsoft.AspNetCore.Identity;
using LearnApp.IdentityEntities;
using Microsoft.AspNetCore.Authorization;

namespace LearnApp.Controllers;

[Authorize(Roles ="Instructor")]
public class InstructorController : Controller
{

    private readonly ICourseRepository _courseRepository;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    //constructor Injection
    public InstructorController(ICourseRepository courseRepository,ApplicationDbContext applicationDbContext,UserManager<ApplicationUser> userManager){
        _courseRepository = courseRepository;
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
    }
    
    public IActionResult Index()
    {
        IEnumerable courses = _courseRepository.GetCourses();
        return View(courses);
    }

    public async Task<IActionResult> GetCourses(){
        if(User.Identity?.Name == null){
            //User is not authenticated
            return RedirectToAction("Logout","Account");
        }

        ApplicationUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
        Console.WriteLine(User.Identity.Name);

        if(user == null || user.BatchId == null){
            return RedirectToAction("Logout","Account");
        }

        
        string? batchid = user.BatchId;
    
        IEnumerable courses = _courseRepository.FetchCoursesForBatch(batchid);
        return View(courses);
    }



    public async Task<IActionResult> StatusReports(){

       if(User.Identity?.Name == null){
            //User is not authenticated
            return RedirectToAction("Logout","Account");
        }

        ApplicationUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
        Console.WriteLine(User.Identity.Name);

        if(user == null || user.BatchId == null){
            return RedirectToAction("Logout","Account");
        }

        
        string? batchid = user.BatchId;

        ViewBag.name = HttpContext.Session.GetString("USERNAME");
        IEnumerable reports = _applicationDbContext.StatusReports.Where(report => report.BatchId == batchid);
        return View(reports);
    }


    public IActionResult AcceptReport(int id){

        //Update in the Home Page of User by creating a Session
        HttpContext.Session.SetString("REPORTSTATUS","Accepted");

        StatusReport? report = _applicationDbContext.StatusReports.Find(id);
        if(report == null){
            return RedirectToAction("StatusReports","Instructor"); 
        }

        Console.WriteLine(report.ReportStatus);

        report.ReportStatus = "Accepted";
        _applicationDbContext.StatusReports.Update(report);
        _applicationDbContext.SaveChanges();

        return RedirectToAction("StatusReports","Instructor");
    }


    public IActionResult DeleteReport(int id){

        StatusReport? report = _applicationDbContext.StatusReports.Find(id);
        if(report == null){
            return RedirectToAction("StatusReports","Instructor"); 
        }

        _applicationDbContext.StatusReports.Remove(report);
        _applicationDbContext.SaveChanges();
        
        return RedirectToAction("StatusReports","Instructor");
    }

    
}
