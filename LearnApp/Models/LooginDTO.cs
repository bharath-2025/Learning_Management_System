using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LearnApp.Models
{
    public class LoginDTO{
        [Key][Required(ErrorMessage ="PersonId can't be blank")][RegularExpression("(ADM|INT|EMP|IST)\\d{3,6}$",ErrorMessage ="Invalid Format")]
        public string? PersonId{get;set;}
        [Required(ErrorMessage ="Password can't be blank")][RegularExpression("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%&~])(?=.*?[A-Z]).{8,15}$",
        ErrorMessage="The Password should contains atleast 1 number,Uppercase,Lowercase,Specialchar and length should be 8")]
        public string? Password{get;set;}
    }

}

