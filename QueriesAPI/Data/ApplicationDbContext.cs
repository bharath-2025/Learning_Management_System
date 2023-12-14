
using Microsoft.EntityFrameworkCore;
using QueriesAPI.Models;
public class ApplicationDbContext : DbContext{

    public ApplicationDbContext(DbContextOptions options):base(options){

    }

    public DbSet<Query> QueriesApi{get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Query>().ToTable("QueriesApi");

        modelBuilder.Entity<Query>().HasData(
            new Query(){
                QueryId = Guid.NewGuid(),
                UserId = "ADM001",
                Email = "Admin@gmail.com",
                Message = "This is a test Query"
            }
        );
    }

}