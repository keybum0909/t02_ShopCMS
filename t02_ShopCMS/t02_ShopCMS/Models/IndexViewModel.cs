using System.Collections.Generic;
using t02_ShopCMS.Entity;

namespace t02_ShopCMS.Models
{
    public class IndexViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public Dictionary<int, List<string>> Imgsrc { get; set; }
    }
}
