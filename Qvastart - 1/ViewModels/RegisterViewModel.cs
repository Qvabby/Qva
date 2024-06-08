using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Qvastart___1.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name must be longer than 2 characters and less than 50 characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name must be longer than 2 characters and less than 50 characters.",MinimumLength = 2)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Username must be less than 30 characters and more than 2", MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [Phone]
        [RegularExpression("^\\+?[1-9][0-9]{8}$", ErrorMessage = "Invalid Phone Number, Example: 55112233")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{5,}$",ErrorMessage = "Password should contain atleast: 1 Uppercase letter, 1 lowercase letter, 1 special character, 1 digit.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirming password must be same as Passowrd!")]
        [DisplayName("Confirm Password")]
        public string ConfirmedPassword { get; set; }
    }
}
