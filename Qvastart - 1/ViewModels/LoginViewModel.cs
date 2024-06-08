using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Qvastart___1.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Username is Required.")]
        [StringLength(30, ErrorMessage = "Username must be less than 30 characters and more than 2", MinimumLength = 2)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }
}
