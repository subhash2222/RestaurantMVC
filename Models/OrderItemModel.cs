using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class OrderItemModel
    {
        public Int64 Order_Id { get; set; }

        public int SR_NO { get; set; }

        public Int64 Item_Id { get; set; }

        public string Item_Name { get; set; }

        public int Qty { get; set; }
    }
}