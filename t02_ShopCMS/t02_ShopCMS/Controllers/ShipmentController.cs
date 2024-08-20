using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using t02_ShopCMS.Data;
using t02_ShopCMS.Models;
using t02_ShopCMS.Models.Shipment;
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

        [HttpPost]
        public IActionResult Index([FromBody] Indexreq req)
        {
            var result = _shipmentService.Index(req);
            if (result != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Product/Index");
            }
            
        }
    }
}
