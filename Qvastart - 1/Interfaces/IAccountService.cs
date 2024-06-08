using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Qvastart___1.ViewModels;

namespace Qvastart___1.Interfaces
{
    public interface IAccountService
    {
        public Task<ServiceResponse<IdentityResult>> Register(RegisterViewModel model);
        public Task<ServiceResponse<Microsoft.AspNetCore.Identity.SignInResult>> Login(LoginViewModel model);
        public Task<ServiceResponse<string>> Logout();
        public Task<ServiceResponse<IdentityUser>> ForgetPassword(ForgetPasswordViewModel model, IUrlHelper Url, HttpContext HttpContext);
        public Task<ServiceResponse<IdentityResult>> ResetPassword(ResetPasswordViewModel model);
    }
}
