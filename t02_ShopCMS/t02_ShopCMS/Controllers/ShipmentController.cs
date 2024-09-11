using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using t02_ShopCMS.Data;
using t02_ShopCMS.Models;
using t02_ShopCMS.Services;

namespace t02_ShopCMS.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly t02_ShopCMSContext _context;
        private readonly IShipmentService _shipmentService;

        public ShipmentController(t02_ShopCMSContext context, IServiceProvider provider)
        {
            _context = context;
            _shipmentService = provider.GetRequiredService<IShipmentService>();
        }

        public IActionResult Index()
        {
            var result = _shipmentService.QueryInit();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder([FromBody] SaveDatareq req)
        {
            var result = await _shipmentService.SaveOrder(req);
            if (result != null && result.Count > 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody] List<Orderreq> req)
        {
            var result = await _shipmentService.Order(req);
            if (result == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _shipmentService.Delete(id);
            if (result == true)
            {
                return Json(new { success = true });
            }
            else
            {
                ViewBag.alertSign = "刪除產品發生錯誤";
                return Json(new { success = false });
            }
        }
    }
}
