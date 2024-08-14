﻿using System.Collections.Generic;

namespace t02_ShopCMS.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public virtual List<Product> Products { get; set; }
    }
}
