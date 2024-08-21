using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using t02_ShopCMS.Data;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly t02_ShopCMSContext _context;
        public ShipmentService(t02_ShopCMSContext context)
        {
            _context = context;
        }

        public List<ShipmentList> Index()
        {
            var queryInitData = _context.ShipmentList.Select(x => new ShipmentList
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                ProductName = x.ProductName,
                Amount = x.Amount,
                Category = x.Category
            }).ToList(); // 将 IQueryable 转换为 List

            return queryInitData;
        }

        public async Task<List<ShipmentList>> SaveData([FromBody] SaveDatareq req)
        {
            bool ReadyInDatabase = _context.ShipmentList.Any(x => x.ProductName == req.ProductName);
            if (!ReadyInDatabase)
            {
                var shipment = new ShipmentList
                {
                    OrderNumber = GenerateOrderNumber(req.Category),
                    ProductName = req.ProductName,
                    Amount = req.Amount,
                    Category = req.Category,
                    CreateTime = DateTime.Now
                };

                _context.ShipmentList.Add(shipment);
                await _context.SaveChangesAsync();

            }

            return await _context.ShipmentList.ToListAsync();

        }

        private string GenerateOrderNumber(string Category)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmm");

            string categoryCode = _context.Category.Where(x => x.Name == Category).Select(x => x.Id).FirstOrDefault().ToString();

            Random random = new Random();
            string randomDigits = random.Next(0, 100).ToString("D2");

            // 合成訂單編號
            return $"{timestamp}{categoryCode}{randomDigits}";
        }
    }
}
