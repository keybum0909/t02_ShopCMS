using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using t02_ShopCMS.Data;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Services
{
    public class ProductsService : IProductsService
    {
        private readonly t02_ShopCMSContext _context;

        public ProductsService(t02_ShopCMSContext context)
        {
            _context = context;
        }

        public async Task<Indexresp> Index(string searchString)
        {
            IQueryable<Product> categoryProducts = _context.Product.Include(p => p.Category);

            if (!string.IsNullOrEmpty(searchString))
            {
                categoryProducts = categoryProducts.Where(s => s.Name.Contains(searchString));
            }

            List<Product> searchResult = [.. categoryProducts.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Content = p.Content,
                Price = p.Price,
                Stock = p.Stock,
                Image = p.Image
            })];

            //讀取產品
            var products = await _context.Product.ToListAsync();
            //讀取分類類別
            var categories = await _context.Category.ToListAsync();

            Indexresp result = new()
            {
                Result = searchResult,
                Products = products,
                Categories = categories
            };

            return result;
        }

        public async Task<List<Product>> CategoryFilter(int id)
        {
            if(id == 1)
            {
                var result = await _context.Product.ToListAsync();
                return result;
            }
            else
            {
                var result = await _context.Product.Where(x => x.CategoryId == id).ToListAsync();
                return result;
            }
            
        }

        public async Task<bool> Create(Product product, IFormFile myImg)
        {
            // 限制允許的圖片格式 (MIME 類型)
            var allowedImageFormats = new List<string> { "image/jpeg", "image/png", "image/gif", "image/svg" };

            if (myImg != null && allowedImageFormats.Contains(myImg.ContentType))
            {
                // 用 IFormFile myImg 欄位接收檔案
                // 用 MemoryStream 把檔案轉成 Byte 陣列
                using (MemoryStream ms = new())
                {
                    await myImg.CopyToAsync(ms);  
                    product.Image = ms.ToArray();
                }
            }

            // 新增商品時間
            product.CreateTime = DateTime.Now;

            try
            {
                // 將資料新增到資料庫
                _context.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Editresp> Edit(int? id)
        {
            DateTime pastDays = DateTime.Now.AddDays(-14);
            bool canEdited = _context.Product.Where(p => p.Id == id)
                .Any(p => p.CreateTime <= pastDays);

            Editresp res = new();

            if (canEdited)
            {
                var product = await _context.Product
                                .Include(p => p.Category)
                                .FirstOrDefaultAsync(m => m.Id == id);

                DetailViewModel dvm = new();
                dvm.Product = product;
                if (product != null)
                {
                    dvm.Imgsrc = ViewImage(product.Image);
                }

                res = new Editresp
                {
                    Product = product,
                    Imgsrc = dvm.Imgsrc,
                    CategoryList = [.. _context.Category.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })]
                };
                return res;
            }

            return res;
        }

        private string ViewImage(byte[] arrayImage)
        {
            //二進制圖檔轉字串
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);

            return "data:image/png;base64," + base64String;
        }

    }


}