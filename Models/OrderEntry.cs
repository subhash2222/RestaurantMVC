using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class OrderEntry
    {
        public List<ShoppingCartModel> listshoppingItems { get; set; }

        public string OrderType { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal OrderAmt { get; set; }
        public string Paymentmode { get; set; }
        public string EstimatedTime { get; set; }
        public string Comments { get; set; }
        public CustomerinfoDelivery customerinfoDelivery { get; set; }

        public CustomerinfoPickup customerinfoPickup { get; set; }
    }
}