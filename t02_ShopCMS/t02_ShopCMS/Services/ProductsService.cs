using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<IProductsService> _logger;

        public ProductsService(t02_ShopCMSContext context, ILogger<IProductsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IndexViewModel> QueryInit()
        {
            _logger.LogTrace("取得產品與類別資料");
            var products = await _context.Product.ToListAsync();
            
            var categories = await _context.Category.ToListAsync();

            Dictionary<int, List<string>> imageArr = new();

            _logger.LogTrace("圖片轉Base64");
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

            IndexViewModel result = new()
            {
                Products = products,
                Categories = categories,
                Imgsrc = imageArr
            };

            return result;
        }

        public async Task<IndexViewModel> SearchProduct(string searchString)
        {
            _logger.LogTrace("取得對應類別的產品");
            var products = _context.Product.Where(x => searchString.Contains(x.Name)).Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Image = x.Image,
                CanOrder = x.CanOrder,
                Category = new Category
                {
                    Name = x.Category.Name
                }
            }).ToList();

            _logger.LogTrace("圖片轉Base64");
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

            var result = new IndexViewModel
            {
                Products = products,
                Imgsrc = imageArr
            };

            return result;
        }

        public async Task<IndexViewModel> CategoryFilter(int id)
        {
            if(id == 0)
            {
                _logger.LogTrace("取得全部的產品");
                var products = await _context.Product.Select(x => new Product
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image,
                    CanOrder = x.CanOrder,
                    Category = new Category
                    {
                        Name = x.Category.Name
                    }
                }).ToListAsync();

                _logger.LogTrace("圖片轉Base64");
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

                var result = new IndexViewModel
                {
                    Products = products,
                    Imgsrc = imageArr
                };

                return result;
            }
            else
            {
                _logger.LogTrace("取得對應類別的商品");
                var products = await _context.Product.Where(x => x.CategoryId == id).Select(x => new Product
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image,
                    CanOrder = x.CanOrder,
                    Category = new Category
                    {
                        Name = x.Category.Name
                    }
                }).ToListAsync();

                _logger.LogTrace("圖片轉Base64");
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

                var result = new IndexViewModel
                {
                    Products = products,
                    Imgsrc = imageArr
                };

                return result;
            }
        }

        public async Task<DetailViewModel> Details(int? id)
        {
            _logger.LogTrace("取得對應產品");
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
            _logger.LogTrace("限制圖片檔案類型");
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
                _logger.LogTrace("寫進資料庫");
                _context.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "產品新增失敗");
                return false;
            }
        }


        public async Task<EditViewModel> Edit(int? id)
        {
            _logger.LogTrace("判斷產品是否於14天內增加");
            DateTime pastDays = DateTime.Now.AddDays(-14);
            bool canEdited = _context.Product.Where(p => p.Id == id)
                .Any(p => p.CreateTime <= pastDays);

            EditViewModel res = new();

            if (canEdited)
            {
                _logger.LogTrace("取得對應產品");
                var product = await _context.Product
                                .Include(p => p.Category)
                                .FirstOrDefaultAsync(m => m.Id == id);

                DetailViewModel dvm = new();
                dvm.Product = product;
                if (product != null)
                {
                    dvm.Imgsrc = ViewImage(product.Image);
                }

                res = new EditViewModel
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
            _logger.LogTrace("取得對應產品");
            var product = _context.Product.Where(x => x.Id == dvm.Product.Id).FirstOrDefault();

            if(product != null)
            {
                _logger.LogTrace("修改產品資訊");
                product.Name = dvm.Product.Name;
                product.Description = dvm.Product.Description;
                product.Price = dvm.Product.Price;
                product.Stock = dvm.Product.Stock;
                product.CanOrder = dvm.Product.CanOrder;
                product.Content = dvm.Product.Content;
                product.CategoryId = dvm.Product.CategoryId;

                if (myImg != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        myImg.CopyTo(ms);
                        product.Image = ms.ToArray();
                    }
                }

                _logger.LogTrace("儲存於資料庫");
                _context.Update(product);
                await _context.SaveChangesAsync();

                _logger.LogTrace("修改成功");
                return true;
            }
            _logger.LogTrace("修改失敗");
            return false;
        }

        public async Task<DetailViewModel> Delete(int? id)
        {
            _logger.LogTrace("取得對應產品");
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
            _logger.LogTrace("取得對應產品");
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                var shipmentLists = _context.OrderList.Where(s => s.ProductId == id);
                _context.OrderList.RemoveRange(shipmentLists);

                _logger.LogTrace("於資料表Product移除產品");
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();

            }


            var shipOrder = await _context.OrderList.FirstOrDefaultAsync(m => m.ProductId == id);
            if (shipOrder != null)
            {
                _logger.LogTrace("於資料表OrderList移除產品");
                _context.OrderList.Remove(shipOrder);
                await _context.SaveChangesAsync();
            }
            _logger.LogTrace("產品移除成功");
            return true;
        }

        public int OrderListNum()
        {
            _logger.LogTrace("取得資料表OrderList筆數");
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