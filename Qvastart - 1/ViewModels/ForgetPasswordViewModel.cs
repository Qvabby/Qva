using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Qvastart___1.ViewModels
{
    public class ForgetPasswordViewModel
    {
        
        [EmailAddress]
        [System.ComponentModel.DataAnnotations.Required]
        public string Email { get; set; }
    }
}
