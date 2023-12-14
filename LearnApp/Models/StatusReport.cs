using System;
using System.ComponentModel.DataAnnotations;
namespace LearnApp.Models;
public class StatusReport{
    [Key]
    public int Id {get;set;}
    [Required]
    public string? BatchId {get;set;}
    [Required]
    public string? UserId {get;set;}
    [Required]
    public DateTime UpdatedDate {get;set;}
    [Required]
    public string? Message {get;set;}
    public string? ReportStatus {get;set;}
}