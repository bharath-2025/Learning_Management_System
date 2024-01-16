
namespace LearnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityEntities;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>{

    //Note: IdentityDbContext will offers certain predefined DbSets to manage Identity of user for authentication and authorization. (User Management)

    //constructor overloading
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){

    }

    public DbSet<StatusReport> StatusReports{get;set;}

}