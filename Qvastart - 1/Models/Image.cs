using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Qvastart___1.Models
{
    [PrimaryKey("ImageId")]
    public class Image
    {
        public int ImageId { get; set; }
        //public byte[] ImageData { get; set; }
        //public string _ContentDisposition { get; set; }
        //public string _ContentType { get; set; }
        //public string base64 { get; set; }
        public string ImagePath { get; set; }


        public int ProductId { get; set; }
        public Product product { get; set; }
    }
}
