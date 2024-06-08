using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Qvastart___1.Data;
using Qvastart___1.Interfaces;
using Qvastart___1.Models;
using Qvastart___1.ViewModels;
using Qvastart___1.ViewModels.Dtos;
using System.Collections;

namespace Qvastart___1.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(ApplicationDbContext DbContext, IMapper mapper, IWebHostEnvironment webHostEnviroment)
        {
            _dbcontext = DbContext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnviroment;
        }


        public async Task<ServiceResponse<Product>> AddProduct(ProductViewModel model)
        {
            ServiceResponse<Product> response = new ServiceResponse<Product>();
            try
            {
                //List<Image> images = new List<Image>();
                // Loop thru each selected file
                //foreach (IFormFile photo in model.Images)
                //{
                //    //var dataStream = new MemoryStream();
                //    //await photo.CopyToAsync(dataStream);

                //    //byte[] conv = dataStream.ToArray();

                //    //var image = new Image()
                //    //{
                //    //    ImageData = conv,
                //    //};

                //    //images.Add(image);

                //    //if (photo.Length > 0)
                //    //{
                //    //    using (var ms = new MemoryStream())
                //    //    {
                //    //        photo.CopyTo(ms);
                //    //        var fileBytes = ms.ToArray();
                //    //        string s = Convert.ToBase64String(fileBytes);
                //    //        // act on the Base64 data
                //    //        var image = new Image()
                //    //        {
                //    //            ImageData = fileBytes,
                //    //            _ContentDisposition = photo.ContentDisposition,
                //    //            _ContentType = photo.ContentType,
                //    //            base64 = s,
                //    //        };

                //    //        images.Add(image);
                //    //    }
                //    //}

                //}

                string filepath = _webHostEnvironment.WebRootPath + "\\upload\\product\\" + model.Name;
                List<Image> images = new List<Image>();
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                int x = 0;
                foreach (IFormFile photo in model.Images)
                {
                    string imagepath = filepath + "\\" + model.Name + $"{x++}" + ".png";
                    if (File.Exists(imagepath))
                    {
                        File.Delete(imagepath);
                    }
                    using (FileStream stream = File.Create(imagepath))
                    {
                        await photo.CopyToAsync(stream); 
                    }
                    var image = new Image()
                    {
                        ImagePath = imagepath,
                    };
                    images.Add(image);
                }


                var product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Images = images,
                };

                await _dbcontext.Products.AddAsync(product);
                await _dbcontext.SaveChangesAsync();
                response.ServiceSuccess = true;
                response.Data = product;
                response.description = model.Name + " has been added to database.";

            }
            catch (Exception e)
            {
                response.ServiceSuccess = false;
                response.errorMessage = e.Message;
                response.Data = null;

            }
            return response;
        }

        public async Task<ServiceResponse<List<GetProductDto>>> getAllProduct(string hosturl)
        {
            var response = new ServiceResponse<List<GetProductDto>>();
            try
            {
                var Products = await _dbcontext.Products
                    .Include(uwl => uwl.UserWishlistedProducts)
                    .Include(upp => upp.UserPurchasedProducts)
                    .Include(images => images.Images)
                    .ToListAsync();
                var productsAsViewModels = new List<GetProductDto>();
                string HostUrl = hosturl;
                foreach (var p in Products)
                {
                    var pv = _mapper.Map<GetProductDto>(p);
                    foreach (var image in pv.Images)
                    {
                        var s = image.ImagePath.Split("wwwroot");
                        image.ImagePath = HostUrl + s[s.Length-1];
                    }
                    //int c = 0;
                    //foreach (var x in p.Images)
                    //{
                    //var stream = new MemoryStream(x.ImageData);
                    //IFormFile formFile = new FormFile(stream, 0, x.ImageData.Length, x.product.Name, $"{x.product.Name}-{c++}");
                    //images.Add(formFile);
                    //using (var stream = new MemoryStream(x.ImageData))
                    //{
                    //    var file = new FormFile(stream, 0, x.ImageData.Length, x.product.Name, $"{x.product.Name}-{c++}")
                    //    {
                    //        Headers = new HeaderDictionary(),
                    //        ContentType = x._ContentType,
                    //    };
                    //    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                    //    {
                    //        FileName = file.FileName
                    //    };
                    //    file.ContentDisposition = cd.ToString();
                    //    images.Add(file);
                    //}
                    //}
                    //foreach (var image in p.Images)
                    //{
                    //    pv.base64s.Add(image.base64);
                    //}
                    //pv.Images = images;
                    //productsAsViewModels.Add(pv);

                    productsAsViewModels.Add(new GetProductDto
                    {
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Images = p.Images,

                    });
                }

                        
                    //}

                
                response.Data = productsAsViewModels;
                response.ServiceSuccess = true;
                response.description = "successfully got Products Data from database.";
            }
            catch (Exception e)
            {
                response.errorMessage = e.Message;
                response.ServiceSuccess = false;
                response.Data = null;
            }
            return response;
        }

        public async Task<ServiceResponse<GetProductDto>> getProduct(int id)
        {
            var response = new ServiceResponse<GetProductDto>();
            try
            {
                var product = await _dbcontext.Products
                    .Include(uwl => uwl.UserWishlistedProducts)
                    .Include(upp => upp.UserPurchasedProducts)
                    .Include(images => images.Images)
                    .FirstOrDefaultAsync(p => p.Id == id);

                response.Data = _mapper.Map<GetProductDto>(product); ;
                response.ServiceSuccess = true;
                response.description = "successfully got product Data from database.";
            }
            catch (Exception e)
            {
                response.errorMessage = e.Message;
                response.ServiceSuccess = false;
                response.Data = null;
            }
            return response;

        }
    }
}
