using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RD2Store.Domain.Abstract;
using RD2Store.WebUI.Models;

namespace RD2Store.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository myrepository;

        public ProductController(IProductRepository productrepository)
        {
            this.myrepository = productrepository;
        }
        public int PageSize = 5;
        public ViewResult List(string category, string system, int page = 1)
        {
            ProductListViewModel model = new ProductListViewModel
            {
                Products = myrepository.Products.Where(p => category == null && system == null || (system == null && p.Category == category) || p.System == system && category == null || p.Category == category && p.System == system).OrderBy(p => p.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? ( system == null ? myrepository.Products.Count() : myrepository.Products.Where(e => e.System == system).Count()) : (system == null ? myrepository.Products.Where(e => e.Category == category).Count() : myrepository.Products.Where(e => e.Category == category && e.System == system).Count()) 
                },
                CurrentCategory = category,
                CurrentSystem = system
            };
            return View(model);
        }
    }
}