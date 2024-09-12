using System.Collections.Generic;
using t02_ShopCMS.Entity;

namespace t02_ShopCMS.Models
{
    public class ShipmentViewModel
    {
        public List<OrderList> Orders { get; set; }
        public Dictionary<int, List<string>> Imgsrc { get; set; }
    }
}
