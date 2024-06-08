using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qvastart___1.Controllers;
using Qvastart___1.Data;
using Qvastart___1.Interfaces;
using Qvastart___1.Models;
using Qvastart___1.ViewModels;
using System.Security.Policy;
using System.Web;

namespace Qvastart___1.Services
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<IdentityUser> _usermanager;
        private readonly SignInManager<IdentityUser> _signinmanager;
        private readonly ApplicationDbContext _dbcontext;
        private readonly ICustomEmailSender _emailSender;
        public AccountService(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> Signinmanager, ApplicationDbContext Dbcontext, ICustomEmailSender emailSender)
        {
            _usermanager = UserManager;
            _signinmanager = Signinmanager;
            _dbcontext = Dbcontext;
            _emailSender = emailSender;
        }

        public async Task<ServiceResponse<IdentityUser>> ForgetPassword(ForgetPasswordViewModel model, IUrlHelper Url, HttpContext HttpContext)
        {
            var serviceResponse = new ServiceResponse<IdentityUser>();
            try
            {
                var user = await _usermanager.FindByEmailAsync(model.Email);
                if (user == null) {
                    serviceResponse.description = "user couldn't be found.";
                    serviceResponse.Data = null;
                    return serviceResponse; 
                }
                //password reseting token
                var Token = await _usermanager.GeneratePasswordResetTokenAsync(user);
                //using encoding/decoding coz of an error
                var code = HttpUtility.UrlEncode(Token);
                //Callback url which creates url of reseting password
                var CallbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, Code = code }, protocol: HttpContext.Request.Scheme);
                //Sending Email
                await _emailSender.SendEmailAsync(
                    model.Email,
                    $"Password Recover Mail for {user.UserName}",
                    $"Please Reset your password by clicking this link: <a href=\" {CallbackUrl} \">Click Here</a>");
                //Returning to confirmation page.
                serviceResponse.description = "Mail has been sent to Email.";
                serviceResponse.Data = user;
                serviceResponse.ServiceSuccess = true;
            }
            catch (Exception e)
            {
                serviceResponse.errorMessage = e.Message;
                serviceResponse.ServiceSuccess = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Microsoft.AspNetCore.Identity.SignInResult>> Login(LoginViewModel model)
        {
            ServiceResponse<Microsoft.AspNetCore.Identity.SignInResult> response = new ServiceResponse<Microsoft.AspNetCore.Identity.SignInResult>();
            try
            {
                //trying to sign in
                var result = await _signinmanager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, true);
                response.Data = result;
                

                //checks result.
                if (result.Succeeded) { response.description = "User signed in"; return response; }

                //if (result.RequiresTwoFactor) { }

                //if account is locked out
                else if (result.IsLockedOut)
                {
                    //get user
                    var user = _dbcontext.QvaUsers.FirstOrDefault(u => u.UserName == model.Username);
                    //get time
                    var time = user.LockoutEnd - DateTime.UtcNow;
                    var Seconds = time.Value.Seconds;
                    var Minutes = time.Value.Minutes;
                    //display how much time left.
                    string ErrorMessage = $"Hello {user.Name}, Your Account is Locked for {Minutes} Minutes {Seconds} seconds.";

                    response.description = "User is on LockOut";
                    response.essentialData = ErrorMessage;
                    //return response;
                    return response;
                }
                //FOR NOW RETURNING RESPONSE COZ WE DON'T HAVE EVERY CASE.
                return response;
            }
            catch (Exception e)
            {
                response.errorMessage = e.Message;
                return response;
            }
            
        }

        public async Task<ServiceResponse<string>> Logout()
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                //signout Method
                await _signinmanager.SignOutAsync();
                serviceResponse.Data = "User Logged Out";
            }
            catch (Exception e)
            {
                serviceResponse.errorMessage = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<IdentityResult>> Register(RegisterViewModel model)
        {
            ServiceResponse<IdentityResult> response = new ServiceResponse<IdentityResult>();
            try
            {
                //Create user.
                var user = new QvaUser
                {
                    Name = model.Name,
                    LastName = model.LastName,
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    XP = 0,
                };
                //Sign Up User to database.
                var result = await _usermanager.CreateAsync(user, model.Password);
                //check if registered.
                if (result.Succeeded)
                {
                    //automatically signs user in.
                    await _signinmanager.SignInAsync(user, isPersistent: false);

                }
                response.Data = result;
                response.description = "This Response Service carries Register Result Type.";
                return response;
            }
            catch (Exception e)
            {
                response.errorMessage = e.Message;
                return response;
            }
           
            
            
        }

        public async Task<ServiceResponse<IdentityResult>> ResetPassword(ResetPasswordViewModel model)
        {
            var serviceResponse = new ServiceResponse<IdentityResult>();
            try
            {
                //getting user.
                var user = await _usermanager.FindByIdAsync(model.UserId.ToString());
                //checking user we got from database
                if (user == null)
                {
                    serviceResponse.description = "User couldn't be found.";
                    serviceResponse.ServiceSuccess = false;
                    serviceResponse.Data = null;
                    //return RedirectToAction("ForgotPasswordConfirmation");
                }
                //decoding httpurl which is in model
                var code = HttpUtility.UrlDecode(model.Code);
                //resettingn password.
                var result = await _usermanager.ResetPasswordAsync(user, code, model.Password);
                //if succed return to resetPaswordConfirmation view.
                serviceResponse.Data = result;
                serviceResponse.ServiceSuccess = true;
                serviceResponse.description = "Url Decoded, Result recieved.";
            }
            catch (Exception e)
            {
                serviceResponse.errorMessage = e.Message;
                serviceResponse.ServiceSuccess = false;
            }
            return serviceResponse;
        }
    }
}
