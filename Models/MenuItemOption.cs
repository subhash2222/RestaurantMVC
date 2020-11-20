using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class MenuItemOption
    {
       
        public int Item_ID { get; set; }
        public string Option_Name { get; set; }
        public decimal Display_Seq_No { get; set; }
        public int display_Seq_No { get; set; }
   
    }
}
