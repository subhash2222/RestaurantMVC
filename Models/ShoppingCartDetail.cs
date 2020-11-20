using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class ShoppingCartDetail
    {
        public List<ShoppingCartModel> listshoppingItems { get; set; }

        public Decimal CartTotal { get; set; }

        public Decimal GrandCartTotal { get; set; }

        public string NoofItems { get; set; }

        public string pagename { get; set; }

        public string Time { get; set; }

        public CustomerinfoDelivery customerinfoDelivery { get; set; }

        public CustomerinfoPickup customerinfoPickup { get; set; }

        public DisplayModel displayModel { get; set; }
    }

    public class CustomerinfoDelivery
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Time { get; set; }
        public string Comments { get; set; }
        public string Paymentmode { get; set; }
    }

    public class CustomerinfoPickup
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string MobileNo { get; set; }

        public string EmailId { get; set; }

        public string Time { get; set; }

        public string Paymentmode { get; set; }

        public string Comments { get; set; }
    }
}