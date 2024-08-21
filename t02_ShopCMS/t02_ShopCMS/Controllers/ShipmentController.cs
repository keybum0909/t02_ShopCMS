using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            var result = _shipmentService.Index();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveData([FromBody] SaveDatareq req)
        {
            var result = await _shipmentService.SaveData(req);
            if (result != null && result.Any())
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}
