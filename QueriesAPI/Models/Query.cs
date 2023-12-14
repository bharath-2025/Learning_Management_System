
using System.ComponentModel.DataAnnotations;

namespace QueriesAPI.Models;
public class Query{
    [Key]
    public Guid QueryId { get; set; }
    [Required]
    public string? UserId { get; set; }
    [Required]
    [EmailAddress(ErrorMessage ="Enter a valid email")]
    public string? Email { get; set; }
    [Required]
    public string? Message { get;set; }
}
