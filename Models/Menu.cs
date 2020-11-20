using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class Menu
    {
        public int Menu_ID { get; set; }
        [Required(ErrorMessage = "Please enter Menu name")]
        public string Menu_Name { get; set; }
        public bool Is_Default { get; set; }
        public int Display_Seq_No { get; set; }
        public string Is_DefaultStr { get; set; }
        public string Operated_By { get; set; }
        public string IsInserted { get; set; }
    }
}
