using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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
        public ShipmentService(t02_ShopCMSContext context)
        {
            _context = context;
        }

        public List<ShipmentList> QueryInit()
        {
            var queryInitData = _context.ShipmentList.OrderByDescending(x => x.CreateTime).Select(x => new ShipmentList
            {
                Id = x.Id,
                ProductId = x.ProductId,
                OrderNumber = x.OrderNumber,
                ProductName = x.ProductName,
                Amount = x.Amount,
                Category = x.Category
            }).ToList();

            return queryInitData;
        }

        public async Task<List<ShipmentList>> SaveOrder([FromBody] SaveDatareq req)
        {
            bool ReadyInDatabase = _context.ShipmentList.Any(x => x.ProductName == req.ProductName);
            if (!ReadyInDatabase)
            {
                var orderProoductId = _context.Product.Where(x => x.Name == req.ProductName).Select(x => x.Id).FirstOrDefault();
                var shipment = new ShipmentList
                {
                    ProductId = orderProoductId,
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

        public async Task<bool> Delete(int? id)
        {
            var shipmentResult = await _context.ShipmentList.FindAsync(id);
            if(shipmentResult != null)
            {
                _context.ShipmentList.Remove(shipmentResult);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        private string GenerateOrderNumber(string Category)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmm");

            string categoryCode = _context.Category.Where(x => x.Name == Category).Select(x => x.Id).FirstOrDefault().ToString();

            Random random = new Random();
            string randomDigits = random.Next(0, 100).ToString("D2");

            return $"{timestamp}{categoryCode}{randomDigits}";
        }
    }
}
