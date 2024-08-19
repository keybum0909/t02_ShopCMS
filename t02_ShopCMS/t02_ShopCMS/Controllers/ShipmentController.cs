using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        public Task<ShipmentList> Index([FromBody] Indexreq req)
        {
            var result = _shipmentService.Index(req);
            return result;
        }
    }
}
