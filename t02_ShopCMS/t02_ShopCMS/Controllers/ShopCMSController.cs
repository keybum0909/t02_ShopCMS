using Microsoft.AspNetCore.Mvc;

namespace t02_ShopCMS.Controllers
{
    public class ShopCMSController : Controller
    {
        public IActionResult Index()
        {
            ViewData["data"] = "Im viewdata";
            ViewBag.bag = "Im bag";
            return View();
        }
    }
}
