
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Services
{
    public interface IShipmentService
    {
        Task<ShipmentList> Index([FromBody] Indexreq req);
    }
}