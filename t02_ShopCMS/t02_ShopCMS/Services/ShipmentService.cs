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

        public List<OrderList> QueryInit()
        {
            _logger.LogTrace("取得待出貨列表內產品");
            var queryInitData = _context.OrderList.OrderByDescending(x => x.CreateTime).Select(x => new OrderList
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Amount = x.Amount,
                Category = x.Category
            }).ToList();

            return queryInitData;
        }

        public async Task<List<OrderList>> SaveOrder([FromBody] SaveDatareq req)
        {
            _logger.LogTrace("確認產品是否已於待出貨清單內");
            bool ReadyInDatabase = _context.OrderList.Any(x => x.ProductName == req.ProductName);
            var orderProoductId = _context.Product.Where(x => x.Name == req.ProductName).Select(x => x.Id).FirstOrDefault();
            if (!ReadyInDatabase)
            {
                var shipment = new OrderList
                {
                    ProductId = orderProoductId,
                    ProductName = req.ProductName,
                    Amount = req.Amount,
                    Category = req.Category,
                    CreateTime = DateTime.Now
                };

                _logger.LogTrace("新增產品於資料表OrderList");
                _context.OrderList.Add(shipment);
            }
            else
            {
                var newData = _context.OrderList.Where(x => x.ProductName == req.ProductName).ToList();
                foreach (var item in newData)
                {
                    _logger.LogTrace("新增產品於資料表OrderList");
                    item.Amount += req.Amount;
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
                Console.WriteLine(ex.Message);
            }
            return await _context.OrderList.ToListAsync();

        }

        public async Task<bool> Order([FromBody] List<Orderreq> req)
        {
            foreach (var item in req)
            {
                var product = _context.Product.FirstOrDefault(x => x.Name == item.ProductName);

                if (product.CanOrder)
                {
                    var shipment = new ShipmentList
                    {
                        ProductName = item.ProductName,
                        ShipNumber = GenerateOrderNumber(product.CategoryId.ToString()),
                        Amount = item.Amount,
                        OrderTime = DateTime.Now
                    };

                    _context.ShipmentList.Add(shipment);
                    product.Stock -= item.Amount;

                    var orderItem = _context.OrderList.FirstOrDefault(x => x.ProductName == item.ProductName);
                    if (orderItem != null)
                    {
                        _context.OrderList.Remove(orderItem);
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> Delete(int? id)
        {
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
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmm");

            Random random = new Random();
            string randomDigits = random.Next(0, 100).ToString("D2");

            return $"{timestamp}{Category}{randomDigits}";
        }
    }
}
