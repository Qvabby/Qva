using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qvastart___1.Data;
using Qvastart___1.Interfaces;
using Qvastart___1.Models;
using Qvastart___1.Services;
using Qvastart___1.ViewModels;
using System.Web;

namespace Qvastart___1.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //Variables
        private readonly IAccountService _accountService;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly SignInManager<IdentityUser> _signinmanager;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        //private readonly ICustomEmailSender _emailSender;
        //ctor
        public AccountController(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> SignInManager, ApplicationDbContext dbcontext, IMapper mapper, ICustomEmailSender emailSender, IAccountService Accountservice)
        {
            _usermanager = UserManager;
            _signinmanager = SignInManager;
            _dbcontext = dbcontext;
            _mapper = mapper;
            //_emailSender = emailSender;
            _accountService = Accountservice;
        }
        // ------------------------------------------------------------- GET METHODS -------------------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated)
            {
                //if ReturnUrl in url is null then give it an default index
                returnUrl = returnUrl ?? Url.Content("~/");
                //pass through view as a viewdata.
                ViewData["ReturnUrl"] = returnUrl;
                //return View
                return View();
            }
            return RedirectToAction(returnUrl);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            //declare RegisterViewModel
            RegisterViewModel registerViewModel = new RegisterViewModel();
            //pass through View
            return View(registerViewModel);
        }
        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            //getting User Incluing Purchased Products and Wishlist.
            var user = await _dbcontext.QvaUsers
                .Include(x => x.UserPurchasedProducts)
                .Include(y => y.UserWishlistedProducts)
                .FirstOrDefaultAsync(x => x.Id == _usermanager.GetUserId(User));
            if (user != null)
            {
                //Parsing into ProfileViewModel 
                var model = _mapper.Map<ProfileViewModel>(user);
                return View(model);
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string? Code = null, string? UserId = null)
        {
            //if code is null then it returns error. Otherwise goes to ResetPassword View.
            return Code == null ? RedirectToAction("Error", "Home") : View();
        }
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // ------------------------------------------------------------- POST METHODS -------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //get result from account service register.
                var serviceResponse = await _accountService.Register(model);
                //Routing to Home page if succed.
                if (serviceResponse.Data.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            //if ReturnUrl in url is null then give it an default index
            returnUrl = returnUrl ?? Url.Content("~/");
            //passing through view
            ViewData["ReturnUrl"] = returnUrl;
            //Checking ModelState, if its not valid return same view again.
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //getting service Response
            var serviceResponse = await _accountService.Login(model);
            //Checking If Succed.
            if (serviceResponse.Data.Succeeded) return LocalRedirect(returnUrl);
            //Checking if Locked Out
            else if (serviceResponse.Data.IsLockedOut) { ModelState.AddModelError(string.Empty, serviceResponse.essentialData); return View(model); }
            //overall return login.
            return RedirectToAction(nameof(Login), new { ReturnUrl = returnUrl, });
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //calling Logout Method (we can also get response but we dont need)
            _accountService.Logout();
            //return to Index.
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid) { return RedirectToAction("Error", "Home"); }
            //getting Service Response.
            var ServiceResponse = await _accountService.ForgetPassword(model, Url, HttpContext);
            //checking Service Data if null.
            if(ServiceResponse.Data == null) return RedirectToAction("ForgetPassword", "Account");
            //Returning to confirmation page.
            return RedirectToAction("ForgotPasswordConfirmation");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            //checking modelstate first, if something is off returning error
            if (!ModelState.IsValid) { return RedirectToAction("Error", "Home"); }
            //getting Service Response.
            var serviceResponse = await _accountService.ResetPassword(model);
            //checking if service response's data is null.
            if(serviceResponse.Data == null) return RedirectToAction("ForgotPasswordConfirmation");
            //if succed return to resetPaswordConfirmation view.
            if (serviceResponse.Data.Succeeded) 
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }
            else
            {
                //else return error.
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
