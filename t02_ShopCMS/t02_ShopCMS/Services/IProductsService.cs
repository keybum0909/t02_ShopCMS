using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using t02_ShopCMS.Entity;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Services
{
    public interface IProductsService
    {
        Task<Indexresp> QueryInit(string searchString);
        Task<List<Product>> CategoryFilter(int id);
        Task<DetailViewModel> Details(int? id);
        Task<bool> Create(Product product, IFormFile myImg);


        Task<Editresp> Edit(int? id);
        Task<Product> Delete(int? id);
        Task<bool> DeleteConfirmed(int? id);
    }
}