
using System.ComponentModel.DataAnnotations;

namespace LearnApp.Models{
    public class Query{
        [Key]
        public Guid QueryId { get; set; }
        [Required(ErrorMessage ="UserId can't be blank")]
        public string? UserId { get; set; }
        [Required(ErrorMessage ="Email can't be blank")]
        [EmailAddress(ErrorMessage ="Email Address should be of valid format")]
        public string? Email { get; set; }
        [Required(ErrorMessage ="Message can't be blank")]
        public string? Message { get;set; }
    }
}