using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class DisplayModel
    {
        public string DeliveryCloseMsg { get; set; }
        public string DisplayMsgDeliveyClose { get; set; }
        public string DisplayMsgOnHome { get; set; }
        public string DisplayMsgPickupClose { get; set; }
        public string DisplayMsgResClose { get; set; }
        public string IsDeliveryAvailable { get; set; }
        public string IsPickupAvailable { get; set; }
        public string IsRestaurantOpen { get; set; }
        public string PickupCloseMsg { get; set; }
        public string ResCloseMsg { get; set; }

    }
}