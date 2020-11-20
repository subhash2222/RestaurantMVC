using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class ShoppingCartModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal ItemLinePrice { get; set; }
        public int Qty { get; set; }
        public string Optionalname { get; set; }
        public string ItemIds { get; set; }
    }
}