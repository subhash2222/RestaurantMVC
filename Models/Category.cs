using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class Category
    {
        public int category_ID { get; set; }

        [Required(ErrorMessage = "Please enter category name")]
        public string category_Name { get; set; }

        [Required(ErrorMessage = "Please select status")]
        public string status { get; set; }
        public string statusdesc { get; set; }

        [Required(ErrorMessage = "Please enter display sequence no")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "display sequence no is Numeric only")]
        public int display_Seq_No { get; set; }
        public string Operated_By { get; set; }
        public string IsInserted { get; set; }
    }
}