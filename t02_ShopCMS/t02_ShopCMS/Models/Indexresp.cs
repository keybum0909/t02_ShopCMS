using System.Collections.Generic;
using t02_ShopCMS.Entity;

namespace t02_ShopCMS.Models
{
    public class Indexresp
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        //public string Imgsrc { get; set; }
    }
}
