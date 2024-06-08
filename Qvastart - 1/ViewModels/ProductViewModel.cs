using Qvastart___1.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Qvastart___1.ViewModels
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFileCollection Images { get; set; }
    }
}
