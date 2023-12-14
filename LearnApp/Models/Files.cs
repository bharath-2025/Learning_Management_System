
using System.ComponentModel.DataAnnotations;

namespace LearnApp.Models;
public class Files{
    [Required]
    public string? CourseId { get; set; }
     [Required]
    public byte[]? Material { get; set; }
    [Required]
    public string? MaterialName {get; set;}
}