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
        Task<IndexViewModel> QueryInit();
        Task<IndexViewModel> SearchProduct(string searchString);
        Task<IndexViewModel> CategoryFilter(int id);
        Task<DetailViewModel> Details(int? id);
        Task<bool> Create(Product product, IFormFile myImg);


        Task<EditViewModel> Edit(int? id);
        Task<bool> EditSave(DetailViewModel dvm, IFormFile myImg);
        Task<DetailViewModel> Delete(int? id);
        Task<bool> DeleteConfirmed(int? id);
        int OrderListNum();
    }
}