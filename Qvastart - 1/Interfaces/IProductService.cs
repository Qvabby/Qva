using Qvastart___1.Models;
using Qvastart___1.ViewModels;
using Qvastart___1.ViewModels.Dtos;

namespace Qvastart___1.Interfaces
{
    public interface IProductService
    {
        public Task<ServiceResponse<Product>> AddProduct(ProductViewModel model);
        public Task<ServiceResponse<List<GetProductDto>>> getAllProduct(string hosturl);
        public Task<ServiceResponse<GetProductDto>> getProduct(int id);

    }
}
