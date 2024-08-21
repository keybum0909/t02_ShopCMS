using System;

namespace t02_ShopCMS.Models
{
    public class ShipmentList
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public string Category { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
