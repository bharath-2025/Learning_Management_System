using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LearnApp.Models
{
    public class User{
        [Key][Required][RegularExpression("(ADM|INT|EMP|IST)\\d{3,6}$",ErrorMessage ="Invalid Format")]
        public string? UserId{get;set;}
        [Required]
        public string? UserName{get;set;}
        [Required]
        public string? Email{get;set;}
        [Required][RegularExpression("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%&~])(?=.*?[A-Z]).{8,15}$",
        ErrorMessage="The Password should contains atleast 1 number,Uppercase,Lowercase,Specialchar and length should be 8")]
        public string? Password{get;set;}
        [Required]
        public string? Role{get;set;}
        [Required][NotMapped]
        public string? ConfirmPassword{get;set;}
        [Required]
        public string? BatchId{get;set;}
    }

}

