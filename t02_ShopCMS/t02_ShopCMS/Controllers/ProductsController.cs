using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using t02_ShopCMS.Data;
using t02_ShopCMS.Models;
using t02_ShopCMS.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using t02_ShopCMS.Entity;

namespace t02_ShopCMS.Controllers
{
    public class ProductsController : Controller
    {
        private readonly t02_ShopCMSContext _context;
        private readonly IProductsService _productsService;

        public ProductsController(t02_ShopCMSContext context, IServiceProvider provider)
        {
            _context = context;
            _productsService = provider.GetRequiredService<IProductsService>();
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString)
        {
            var result = await _productsService.QueryInit(searchString);
            return View(result);
        }

        [HttpPost]
        public async Task<List<Product>> CategoryFilter(int id)
        {
            var result = await _productsService.CategoryFilter(id);
            return result;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var reslut = await _productsService.Details(id);
            return View(reslut);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            //傳Categories model給create view
            ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile myImg)
        {
            //IFormFile name對應input type=file的name屬性)
            if (ModelState.IsValid)
            {
                var newProduct = await _productsService.Create(product, myImg);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var result = await _productsService.Edit(id);
            if (result.Product == null)
            { 
                ViewBag.ErrorMessage = "此商品於兩周內上架，不可編輯";
                return RedirectToAction("Index", new { Id = id, error = "true" });
            }
            return View(result);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(int id, DetailViewModel dvm, IFormFile myImg)
        {
            if (id != dvm.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //IFormFile name對應input type=file的name屬性)
                    if (ModelState.IsValid)
                    {
                        if (myImg != null)
                        {
                            //用IFormFile myimg欄位接收檔案
                            //用MemoryStream()把檔案轉成Byte陣列
                            using (var ms = new MemoryStream())
                            {
                                myImg.CopyTo(ms);
                                dvm.Product.Image = ms.ToArray();
                            }
                        }
                    }
                    _context.Update(dvm.Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(dvm.Product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //重新路由
                return RedirectToAction(nameof(Index));
            }
            return View(dvm.Product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _productsService.Delete(id);
            return View(result);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var result = await _productsService.DeleteConfirmed(id);
            if(result == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
            
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        //Category
        public IActionResult CreateCateGory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return View();
        }
    }
}