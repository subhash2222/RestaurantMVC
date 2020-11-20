using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class TableReservation
    {
        public int Reservation_ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string No_of_Person { get; set; }
        public string Phone_No { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public string Reservation_Date { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
        public string Booking_Date { get; set; }    
        public string Status { get; set; }
        public string StatusDesc { get; set; }
        public string Operated_By { get; set; }
        public string IsInserted { get; set; }
    }
}
