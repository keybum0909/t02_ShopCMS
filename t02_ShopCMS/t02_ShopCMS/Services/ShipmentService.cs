using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using t02_ShopCMS.Data;
using t02_ShopCMS.Entity;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly t02_ShopCMSContext _context;
        private readonly ILogger<IShipmentService> _logger;
        public ShipmentService(t02_ShopCMSContext context, ILogger<IShipmentService> logger)
        {
            _context = context;
            _logger= logger;
        }

        public ShipmentViewModel QueryInit()
        {
            _logger.LogTrace("取得待出貨列表內產品");
            var orders = _context.OrderList.OrderByDescending(x => x.CreateTime).Select(x => new OrderList
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Amount = x.Amount,
                Category = x.Category
            }).ToList();

            _logger.LogTrace("圖片轉Base64");
            Dictionary<int, List<string>> imageArr = new();
            foreach (var item in orders)
            {
                if (orders != null)
                {
                    var productImage = _context.Product.Where(x => x.Id == item.ProductId).Select(x => x.Image).ToList();
                    foreach (var image in productImage)
                    {
                        var imageList = new List<string> { "data:image/png;base64," + Convert.ToBase64String(image, 0, image.Length) };
                        imageArr[item.ProductId] = imageList;
                    }
                }
            }

            //_logger.LogTrace("圖片轉Base64");
            //Dictionary<int, string> imageArr = new();
            //foreach (var item in orders)
            //{
            //    var productImage = _context.Product.Where(x => x.Id == item.ProductId).Select(x => x.Image).ToList();
            //    foreach (var image in productImage)
            //    {
            //        string imageList = "data:image/png;base64," + Convert.ToBase64String(image, 0, image.Length);
            //        imageArr[item.ProductId] = imageList;
            //    }
            //}

            var queryInitData = new ShipmentViewModel
            {
                Orders = orders,
                Imgsrc = imageArr
            };

            return queryInitData;
        }

        public async Task<bool> SaveOrder([FromBody] SaveDatareq req)
        {
            var productStock = _context.Product.Where(x => x.Id == req.ProductId).Select(x => x.Stock).FirstOrDefault();
            if (productStock < req.Amount)
            {
                return false;
            }
            else
            {
                _logger.LogTrace("確認產品是否已於待出貨清單內");
                bool ReadyInDatabase = _context.OrderList.Any(x => x.Id == req.ProductId);
                if (!ReadyInDatabase)
                {
                    var shipment = new OrderList
                    {
                        ProductId = _context.Product.Where(x => x.Id == req.ProductId).Select(x => x.Id).FirstOrDefault(),
                        ProductName = _context.Product.Where(x => x.Id == req.ProductId).Select(x => x.Name).FirstOrDefault(),
                        Amount = req.Amount,
                        Category = req.Category,
                        CreateTime = DateTime.Now
                    };

                    _logger.LogTrace("新增產品於資料表OrderList");
                    _context.OrderList.Add(shipment);
                }
                else
                {
                    var newData = _context.OrderList.Where(x => x.Id == req.ProductId).ToList();
                    foreach (var item in newData)
                    {
                        _logger.LogTrace("新增產品於資料表OrderList");
                        item.Amount = req.Amount;
                    }
                }

                try
                {
                    _logger.LogTrace("資料表更新");
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "資料表更新失敗");
                }
                await _context.OrderList.ToListAsync();
                return true;
            }
        }

        public async Task<bool> Order([FromBody] List<Orderreq> req)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var addShipmments = new List<ShipmentList>();
                var removeOrders = new List<OrderList>();

                try
                {
                    foreach (var item in req)
                    {
                        var product = _context.Product.FirstOrDefault(x => x.Name == item.ProductName);

                        _logger.LogTrace("確認是否下架");
                        if (product != null && product.CanOrder)
                        {
                            var shipment = new ShipmentList
                            {
                                ProductName = item.ProductName,
                                ShipNumber = GenerateOrderNumber(product.CategoryId.ToString()),
                                Amount = item.Amount,
                                OrderTime = DateTime.Now,
                                TotalPrice = item.Amount * product.Price
                            };

                            _logger.LogTrace("出貨後產生出貨清單");
                            addShipmments.Add(shipment);

                            product.Stock -= item.Amount;

                            _logger.LogTrace("出貨後將出貨列表移除");
                            var orderItem = _context.OrderList.FirstOrDefault(x => x.ProductName == item.ProductName);

                            removeOrders.Add(orderItem);
                        }

                    }
                    _logger.LogTrace("資料庫更動");
                    await _context.ShipmentList.AddRangeAsync(addShipmments);
                    _context.OrderList.RemoveRange(removeOrders);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "出貨時發生錯誤，回滾所有變更");
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> Delete(int? id)
        {
            _logger.LogTrace("取得待出貨列表內對應產品");
            var shipmentResult = await _context.OrderList.FindAsync(id);
            if(shipmentResult != null)
            {
                _context.OrderList.Remove(shipmentResult);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        private string GenerateOrderNumber(string Category)
        {
            _logger.LogTrace("生產訂單編號");
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            Random random = new Random();
            string randomDigits = random.Next(0, 100).ToString("D2");

            return $"{timestamp}{Category}{randomDigits}";
        }
    }
}
