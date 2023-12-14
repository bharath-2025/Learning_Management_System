using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LearnApp.Enums;

namespace LearnApp.Models
{
    public class RegisterDTO{
        [Key][Required(ErrorMessage ="PersonId can't be blank")][RegularExpression("(ADM|INT|EMP|IST)\\d{3,6}$",ErrorMessage ="Invalid Format")]
        public string? PersonId{get;set;}
        [Required(ErrorMessage ="PersonName can't be blank")]
        public string? PersonName{get;set;}
        [Required(ErrorMessage ="Email can't be blank")]
        public string? Email{get;set;}
        [Required(ErrorMessage ="Password can't be blank")][RegularExpression("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%&~])(?=.*?[A-Z]).{8,15}$",
        ErrorMessage="The Password should contains atleast 1 number,Uppercase,Lowercase,Specialchar and length should be 8")]
        public string? Password{get;set;}
        [Required(ErrorMessage ="Role can't be blank")]
        public UserRoleOptions Role{get;set;} = UserRoleOptions.Learner;
        [Required(ErrorMessage ="ConfirmPassword can't be blank")][Compare("Password",ErrorMessage ="Password and confirm password are not matched")]
        public string? ConfirmPassword{get;set;}
        [Required(ErrorMessage ="BatchId can't be blank")]
        public string? BatchId{get;set;}
    }

}

