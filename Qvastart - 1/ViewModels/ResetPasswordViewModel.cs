using System.ComponentModel.DataAnnotations;

namespace Qvastart___1.ViewModels
{
    public class ResetPasswordViewModel
    {
        public Guid UserId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{5,}$", ErrorMessage = "Password should contain atleast: 1 Uppercase letter, 1 lowercase letter, 1 special character, 1 digit.")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Must be same as password")]
        public string ConfirmedPassword { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
