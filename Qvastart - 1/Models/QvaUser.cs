using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qvastart___1.Models
{
    public class QvaUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal XP { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserWishlistedProducts> UserWishlistedProducts { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserPurchasedProducts> UserPurchasedProducts { get; set; }
    }
}
