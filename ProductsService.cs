using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using t02_ShopCMS.Data;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Services
{
    public class ProductsService: IProductsService
    {
        private readonly t02_ShopCMSContext _context;

        public ProductsService(t02_ShopCMSContext context)
        {
            _context = context;
        }

        public async Task<Indexresp> Index(string searchString, string currentFilter, int? pageNumber)
        {
            //點選分類後出現的產品
            IQueryable<Product> categoryProducts = _context.Product.Include(p => p.Category).AsQueryable();

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
