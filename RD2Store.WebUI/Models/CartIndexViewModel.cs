using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RD2Store.Domain.Entities;

namespace RD2Store.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}