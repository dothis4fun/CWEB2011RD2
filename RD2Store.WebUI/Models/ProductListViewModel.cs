using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RD2Store.Domain.Entities;

namespace RD2Store.WebUI.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
        public string CurrentSystem { get; set; }

    }
}