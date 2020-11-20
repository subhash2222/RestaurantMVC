using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class Reservation
    {
        public ReservationModel reser { get; set; }
        public IEnumerable<Reservation> ListReservation { get; set; }
    }
}