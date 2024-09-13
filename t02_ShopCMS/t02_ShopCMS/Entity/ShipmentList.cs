using System;

namespace t02_ShopCMS.Entity
{
    public class ShipmentList
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ShipNumber { get; set; }
        public int Amount { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
