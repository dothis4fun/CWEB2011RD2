using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RD2Store.Domain.Abstract;

namespace RD2Store.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;
        public NavController(IProductRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null, string system = null)
        {
            ViewBag.SelectedSystem = system;
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);
            return PartialView(categories);
        }
        public PartialViewResult SystemMenu(string system = null, string category = null)
        {
            ViewBag.SelectedCategory = category;
            ViewBag.SelectedSystem = system;
            IEnumerable<string> systems = repository.Products.Select(x => x.System).Distinct().OrderBy(x => x);
            return PartialView(systems);
        }
    }
}