using Microsoft.AspNetCore.Mvc;
using LearnApp.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

//Package References: NewtonSoft.Json to Serialize and Deserialize the Json data.

namespace LearnApp.Controllers;

[Authorize(Roles ="Admin")]
public class QueriesAPIController:Controller{

    Uri baseAddress;
    HttpClient httpClient;   //By default available in the namespace System.Net.Http;
    //Note: Using this httpClient Obj we can call the Asp.net webAPI ie REST Api.

    private readonly IConfiguration _configuration;
    private readonly ILogger<QueriesAPIController> _logger;
    private readonly IEmailSender _emailSender;

    public QueriesAPIController(IConfiguration configuration,ILogger<QueriesAPIController> logger,IEmailSender emailSender){
        _configuration = configuration;
        _logger = logger;
        httpClient = new HttpClient();
        baseAddress = new Uri(_configuration["BaseAddress"]);
        httpClient.BaseAddress = baseAddress;
        _emailSender = emailSender;
    }

    [HttpGet]
    public IActionResult ViewQueries(string? searchBy,string? searchString,int page=1){

         _logger.LogInformation("ViewQueries action method is reached and pagination is added successfully");
        
        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        List<Query>? queries = new List<Query>();

        HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress + "/Queries/GetQueries/").Result;

        //Console.WriteLine(response.IsSuccessStatusCode);
        if(response.IsSuccessStatusCode){
            string data = response.Content.ReadAsStringAsync().Result;
            queries = JsonConvert.DeserializeObject<List<Query>>(data);
        }

        //Business logic for pagination
        const int pageSize = 10;
        if(page < 1){
            page = 1;
        }

        if(queries == null){
             return View(queries);
        }

        int totalItems = queries.Count();

        var pager = new Pager(totalItems,page,pageSize);
        int itemSkip = (page-1)*pageSize;

        ViewBag.Pager = pager;

        List<Query> matchingQueries = queries;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
        {
            var data1 = matchingQueries.Skip(itemSkip).Take(pager.PageSize).ToList();
            return View(data1);
        }

        switch (searchBy)
        {
            case nameof(Query.UserId):
                matchingQueries = queries.Where(temp =>
                (!string.IsNullOrEmpty(temp.UserId)?
                temp.UserId.StartsWith(searchString,StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Query.Email):
                matchingQueries = queries.Where(temp =>
                (!string.IsNullOrEmpty(temp.Email) ?
                temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            default:
                matchingQueries = queries;
                break;
        }

        if(page < 1){
            page = 1;
        }

        totalItems = matchingQueries.Count();

        pager = new Pager(totalItems,page,pageSize);
        itemSkip = (page-1)*pageSize;

        ViewBag.Pager = pager;

        var finalData = matchingQueries.Skip(itemSkip).Take(pager.PageSize).ToList();

        return View(finalData);
    }

    public IActionResult ClearQuery(Guid queryId){

        if(queryId == null){
            return BadRequest();
        }

        //Get the Query obj based on queryId
        HttpResponseMessage responseQuery = httpClient.GetAsync(httpClient.BaseAddress + "/Queries/GetQuery?queryId="+queryId).Result;

        HttpResponseMessage response = httpClient.DeleteAsync(httpClient.BaseAddress+"/Queries/DeleteQuery?queryId="+queryId).Result;

        if(response.IsSuccessStatusCode){
            string data = responseQuery.Content.ReadAsStringAsync().Result;
            Query? query = JsonConvert.DeserializeObject<Query>(data);

            if(query != null){
                var receiver = query.Email;
                var subject = $"Query '{query.QueryId}' Resolved Status";
                string message = $"Hi {query.UserId},\nGreetings. \nyour query '{query.Message}' is resolved successfully.\nplease verify from your side and contact our admin for any issues.\nThank you.\n\nsystem generated mail do not reply.";

                 _emailSender.SendEmailAsync(receiver,subject,message);
            }

            return RedirectToAction("ViewQueries");
        }

        return RedirectToAction("ViewQueries");

    }

}