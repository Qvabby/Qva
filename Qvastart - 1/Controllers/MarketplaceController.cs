using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Qvastart___1.Data;
using Qvastart___1.Interfaces;

namespace Qvastart___1.Controllers
{
    public class MarketplaceController : Controller
    {
        //variables
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProductService _productService;
        //ctor
        public MarketplaceController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IProductService productService)
        {
            _context = context;
            _userManager = userManager;
            _productService = productService;
        }
        // ------------------------------------------------------------- GET METHODS -------------------------------------------------------------
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Store()
        {
            //yay wishlists

            //var userid = _userManager.GetUserId(User);
            //var userwishlistproducts = _context.UserWishlistsProductsTB.ToList().Where(x => x.UserId == userid);

            var serviceResponse = await _productService.getAllProduct($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
            if (serviceResponse.ServiceSuccess == true) {
                return View(serviceResponse.Data);
            }
            else
            {
                ModelState.AddModelError("products", serviceResponse.errorMessage);
                return View();
            }
        }

        // ------------------------------------------------------------- POST METHODS -------------------------------------------------------------
    }
}
