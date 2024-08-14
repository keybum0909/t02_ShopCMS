using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace t02_ShopCMS.Models
{

    //table1
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }            //名稱
        public string Description { get; set; }     //簡介
        public string Content { get; set; }         //內容
        public int Price { get; set; }              //價格
        public int Stock { get; set; }              //庫存
        public byte[] Image { get; set; }           //圖片
        //EF core 會自訂建立FG
        [Display(Name = "Category")]
        public int CategoryId { get; set; } //類別(Foreign Key)


        public virtual Category Category { get; set; }
    }

    //table2
    
}
