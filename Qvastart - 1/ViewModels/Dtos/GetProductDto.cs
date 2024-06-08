using Qvastart___1.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qvastart___1.ViewModels.Dtos
{
    public class GetProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<UserWishlistedProducts>? UserWishlistedProducts { get; set; }
        public ICollection<UserPurchasedProducts>? UserPurchasedProducts { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
