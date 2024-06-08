using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qvastart___1.Interfaces;
using Qvastart___1.ViewModels;

namespace Qvastart___1.Controllers
{
    [Authorize(Policy = "RequireAdministrationRole")] //that means only a user with Administrator role can reach those pages.
    public class AdministratorController : Controller
    {
        private readonly IProductService _productService;

        public AdministratorController(IProductService ProductService)
        {
                _productService = ProductService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        //=============== POSTS


        
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //foreach (var image in model.Images)
            //{
            //    if (!_allowedExtensions.Contains(Path.GetExtension(image.FileName.ToLower())))
            //    {
            //        ModelState.AddModelError("Pic", "Only .PNG, .JPG images are allowed!");
            //        return View(model);
            //    }
            //}
           
            //add product.
            var service_response = await _productService.AddProduct(model);
            if (service_response.ServiceSuccess == true)
            {
                ViewBag.SaveResult = true;
                return View();
            }
            else
            {
                ModelState.AddModelError("NotSuccess", "Couldn't add product.");
                return View(model);
            }

            
        }
    }
}
