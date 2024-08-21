
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using t02_ShopCMS.Models;
using t02_ShopCMS.Models.Shipment;

namespace t02_ShopCMS.Services
{
    public interface IShipmentService
    {
        Task<List<ShipmentList>> Index([FromBody] Indexreq req);
    }
}