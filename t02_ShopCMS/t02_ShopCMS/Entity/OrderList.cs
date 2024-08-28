using System;

namespace t02_ShopCMS.Entity
{
    public class OrderList
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public string Category { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual Product Product { get; set; }
    }
}
