

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LearnApp.IdentityEntities;
public class ApplicationUser:IdentityUser<Guid>{
    [Required]
    public string? PersonName{get;set;}

    [Required]
    public string? BatchId{get;set;}

}