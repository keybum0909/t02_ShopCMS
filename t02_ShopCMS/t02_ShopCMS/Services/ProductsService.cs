using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using t02_ShopCMS.Data;
using t02_ShopCMS.Entity;
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

        public async Task<Indexresp> QueryInit()
        {
            var products = await _context.Product.ToListAsync();
            
            var categories = await _context.Category.ToListAsync();

            Dictionary<int, List<string>> imageArr = new();

            foreach (var item in products)
            {
                if (products != null)
                {
                    if (item.Image != null)
                    {
                        var imageList = new List<string> { ViewImage(item.Image) };
                        imageArr[item.Id] = imageList;
                    }
                }
            }

            Indexresp result = new()
            {
                Products = products,
                Categories = categories,
                Imgsrc = imageArr
            };

            return result;
        }

        public List<Product> SearchProduct(string searchString)
        {
            IQueryable<Product> categoryProducts = _context.Product.Include(p => p.Category);

            if (!string.IsNullOrEmpty(searchString))
            {
                categoryProducts = categoryProducts.Where(s => s.Name.Contains(searchString));
            }

            List<Product> searchResult = categoryProducts.ToList();

            return searchResult;
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

        public async Task<DetailViewModel> Details(int? id)
        {
            DetailViewModel dvm = new();
            if (id != null)
            {
                var product = await _context.Product
                                .Include(p => p.Category)
                                .FirstOrDefaultAsync(m => m.Id == id);
                if (product != null)
                {
                    dvm.Product = product;
                    if (product.Image != null)
                    {
                        dvm.Imgsrc = ViewImage(product.Image);
                    }
                }

            }

            var result = dvm;
            return result;
        }

        public async Task<bool> Create(Product product, IFormFile myImg)
        {
            var allowedImageFormats = new List<string> { "image/jpeg", "image/png", "image/gif", "image/svg" };

            if (myImg != null && allowedImageFormats.Contains(myImg.ContentType))
            {
                using (MemoryStream ms = new())
                {
                    await myImg.CopyToAsync(ms);  
                    product.Image = ms.ToArray();
                }
            }

            product.CreateTime = DateTime.Now;

            try
            {
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

        public async Task<bool> EditSave(DetailViewModel dvm, IFormFile myImg)
        {
            var product = _context.Product.Where(x => x.Id == dvm.Product.Id).FirstOrDefault();
            if(product != null)
            {
                product.Name = dvm.Product.Name;
                product.Description = dvm.Product.Description;
                product.Price = dvm.Product.Price;
                product.Stock = dvm.Product.Stock;
                product.CanOrder = dvm.Product.CanOrder;
                product.Content = dvm.Product.Content;
                product.CategoryId = dvm.Product.CategoryId;

                if (myImg != null)
                {
                    //用IFormFile myimg欄位接收檔案
                    //用MemoryStream()把檔案轉成Byte陣列
                    using (var ms = new MemoryStream())
                    {
                        myImg.CopyTo(ms);
                        product.Image = ms.ToArray();
                    }
                }
                _context.Update(product);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<DetailViewModel> Delete(int? id)
        {
            DetailViewModel dvm = new();
            if (id != null)
            {
                var product = await _context.Product
                                .Include(p => p.Category)
                                .FirstOrDefaultAsync(m => m.Id == id);
                if (product != null)
                {
                    dvm.Product = product;
                    if (product.Image != null)
                    {
                        dvm.Imgsrc = ViewImage(product.Image);
                    }
                }

            }

            var result = dvm;
            return result;
        }

        public async Task<bool> DeleteConfirmed(int? id)
         {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                var shipmentLists = _context.OrderList.Where(s => s.ProductId == id);
                _context.OrderList.RemoveRange(shipmentLists);

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();

            }


            var shipOrder = await _context.OrderList.FirstOrDefaultAsync(m => m.ProductId == id);
            if (shipOrder != null)
            {
                _context.OrderList.Remove(shipOrder);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public int OrderListNum()
        {
            var orders = _context.OrderList.Count();
            return orders;
        }

        private string ViewImage(byte[] arrayImage)
        {
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);

            return "data:image/png;base64," + base64String;
        }

    }


}