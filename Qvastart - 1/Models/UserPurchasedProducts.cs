using Microsoft.EntityFrameworkCore;

namespace Qvastart___1.Models
{
    [PrimaryKey("UserId")]
    public class UserPurchasedProducts
    {
        public string UserId { get; set; }
        public QvaUser qvaUser { get; set; }


        public int ProductId { get; set; }
        public Product product { get; set; }
    }
}
