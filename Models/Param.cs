using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class Param
    {
        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter Contact NO")]
        public string Contact_NO { get; set; }
        [Required(ErrorMessage = "Please enter Email ID")]
        public string Email_ID { get; set; }
        [Required(ErrorMessage = "Please enter Location")]
        public string location { get; set; }

        public string OpenHour_lbl1 { get; set; }

        public string OpenHour_txt1 { get; set; }

        public string OpenHour_lbl2 { get; set; }


        public string OpenHour_txt2 { get; set; }

        public string OpenHour_lbl3 { get; set; }

        public string OpenHour_txt3 { get; set; }

        public string To_Email_ID { get; set; }

        public string Operated_By { get; set; }
        public string IsInserted { get; set; }


    }
}
