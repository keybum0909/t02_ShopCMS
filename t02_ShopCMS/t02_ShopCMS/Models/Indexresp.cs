﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace t02_ShopCMS.Models
{
    public class Indexresp
    {
        public List<Models.Product> Result { get; set; }
        public List<Models.Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        //public string Imgsrc { get; set; }
    }
}
