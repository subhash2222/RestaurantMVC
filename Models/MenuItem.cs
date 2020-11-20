using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class MenuItem
    {
        [Required(ErrorMessage = "Please enter category name")]
        public int Menu_ID { get; set; }
        [Required(ErrorMessage = "Please enter category name")]
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
        [Required(ErrorMessage = "Please enter Item name")]
        public int Item_ID { get; set; }
        public string Item_Name { get; set; }
        public bool Display_Image { get; set; }
        public string Operated_By { get; set; }
        public string IsInserted { get; set; }
    }
}
