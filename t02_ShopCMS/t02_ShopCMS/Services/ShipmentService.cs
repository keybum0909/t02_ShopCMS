using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<ShipmentList> Index([FromBody] Indexreq req)
        {
            var shipment = new ShipmentList
            {
                OrderNumber = GenerateOrderNumber(),
                ProductName = req.ProductName,
                Amount = req.Amount,
                ProductCategory = req.ProductCategory,
                CreateTime = DateTime.Now
            };

            _context.ShipmentList.Add(shipment);
            await _context.SaveChangesAsync();
        }

        private string GenerateOrderNumber()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmm");

            string categoryCode = _context.Category.Select(x => x.Id).ToString();

            Random random = new Random();
            string randomDigits = random.Next(0, 100).ToString("D2");

            // 合成訂單編號
            return $"{timestamp}{categoryCode}{randomDigits}";
        }
    }
}
