using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class ItemModel
    {
        public string menuitem { get; set; }
        public string  itemqty { get; set; }
        public string itemprice { get; set; }
        public int itemid { get; set; }
        public string types { get; set; }
        public string deliverytype { get; set; }
        public string Optionalname { get; set; }
        public string ItemIds { get; set; }
    }
}