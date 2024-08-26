
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using t02_ShopCMS.Entity;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Services
{
    public interface IShipmentService
    {
        List<ShipmentList> QueryInit();
        Task<List<ShipmentList>> SaveOrder([FromBody] SaveDatareq req);
        Task<bool> Delete(int? id);
    }
}