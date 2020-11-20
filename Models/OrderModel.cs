using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class OrderModel
    {
        public Int64 Order_Id { get; set; }

        public DateTime Order_Date { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Mobile_No { get; set; }

        public string Addressline1 { get; set; }

        public string Addressline2 { get; set; }

        public string EmailId { get; set; }

        public string Order_Type { get; set; }

        public string Order_Amt { get; set; }

        public string Payment_Mode { get; set; }

        public string Status { get; set; }

        public string EstimatedTime { get; set; }
    }
}