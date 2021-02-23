using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosmeticShopWebApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Brend { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CathegoryId { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}