using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace t02_ShopCMS.Models
{
    public class Editresp
    {
        public Product Product { get; set; }
        public string Imgsrc { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
    }
}
