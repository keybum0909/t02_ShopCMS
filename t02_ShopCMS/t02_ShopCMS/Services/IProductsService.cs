using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Services
{
    public interface IProductsService
    {
        Task<Indexresp> Index(string searchString, string currentFilter, int? pageNumber);
        Task<bool> Create(Product product, IFormFile myImg);


        Task<Editresp> Edit(int? id);
    }
}