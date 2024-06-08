using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qvastart___1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [ForeignKey("ProductId")]
        public virtual ICollection<UserWishlistedProducts>? UserWishlistedProducts { get; set; }
        [ForeignKey("ProductId")]
        public virtual ICollection<UserPurchasedProducts>? UserPurchasedProducts { get; set; }
        [ForeignKey("ProductId")]
        public virtual ICollection<Image> Images { get; set; }
    }
}
