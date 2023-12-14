using Microsoft.AspNetCore.Mvc;
using QueriesAPI.Models;

namespace QueriesAPI.Controllers;

//Note: ASP.Net web Api requires only JSON/XML Data both for Request as well as Response.
//ASP.net web API uses the NewtonSoft.Json package reference to convert the Object to JSON and vice versa convertions (Both serialization and DeSerialization) in the action method which will be enabled once we decorated our controller with [ApiController] attribute.
//The same Nuget Package Reference can be used to Serialize and Deserialize of JSON Data in normal MVC application.

[ApiController]
[Route("api/[Controller]/[action]")]
public class QueriesController : ControllerBase{

    private readonly ApplicationDbContext _db;
    public QueriesController(ApplicationDbContext db){
        _db = db;
    }

    [HttpGet]
    public IEnumerable<Query> GetQueries(){
        List<Query> queries =  _db.QueriesApi.ToList();
        return queries;
    }

    [HttpGet]
    public async Task<Query> GetQuery(Guid queryId){
        Query? query = await _db.QueriesApi.FindAsync(queryId);
        if(query != null){
            return query;
        }
        return new Query(); //returns null query
    }


    [HttpPost]
    public async Task<IActionResult> PostQuery(Query query){
        if(query == null){
            return BadRequest();
        }
        await _db.AddAsync(query);
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteQuery(Guid queryId){
        if(queryId == null){
            return BadRequest();
        }

        Console.WriteLine(queryId);

        Console.WriteLine("Delete Query Request");

        Query? query = await _db.QueriesApi.FindAsync(queryId);

        if(query == null){
            return NotFound();
        }

        _db.Remove(query);
        await _db.SaveChangesAsync();

        return Ok();
    }
    

}