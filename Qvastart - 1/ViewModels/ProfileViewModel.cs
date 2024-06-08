using Qvastart___1.Models;

namespace Qvastart___1.ViewModels
{
    public class ProfileViewModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public decimal XP { get; set; }
        public virtual ICollection<UserWishlistedProducts> UserWishlistedProducts { get; set; }
        public virtual ICollection<UserPurchasedProducts> UserPurchasedProducts { get; set; }

    }
}
