using IQMania.Models.Quiz;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IQMania.Models.Account
{
    public class Account: Login
    {
        [DisplayName("UserID")]
        public string UId { get; set; }
        public string Role { get; set; }
        public string Phonenumber { get; set; }
        public int Score { get; set; }  
    }
    public class Signup
    {
        [Required]
        [MaxLength(50, ErrorMessage ="Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage ="Invalid Email Format")]
        public string Email { get; set; }

        [Required (ErrorMessage ="Phone number is required")]
        //[RegularExpression((?:\(?\+977\)?)?[9][6-9]\d{8}|01[-]?[0 - 9]{7})), ErrorMessage = "Entered phone format is not valid.")]
        public string Phonenumber { get; set; }

        [Required]
        [MaxLength(11)]
        [RegularExpression((@"^[a-zA-Z0-9+*@#]+$"), ErrorMessage= "Invalid Password Format. Must include atleast one uppercase, lowercase, symbol and number")]
        public string Password { get; set; }
        [Required (ErrorMessage ="Please Enter Confirm Password")]
        [Compare("Password", ErrorMessage ="Confirm Password should match the password you entered")]
        public string ConfirmPassword { get; set; }
    }

    public class Login: ResponseResult
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Phone number is required")]
        ////[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]

        
        [Required]
        [MaxLength(11)]
        [RegularExpression((@"^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[+*@#])[a-zA-Z0-9+*@#]+$"), ErrorMessage = "Invalid Password.")]
        public string Password { get; set; }
        public bool RememberLogin { get; internal set; }
    }

    public class Messages : ResponseResult 
    {
        public int QuizMessages { get; set; }
    }


}
