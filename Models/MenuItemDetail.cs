using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class MenuItemDetail
    {
        public string DisplayCode { get; set; }
        public string Item_Desc { get; set; }
        public int Item_ID { get; set; }
        public string Item_Name { get; set; }
        public decimal Item_Rate { get; set; }
        public int Menu_ID { get; set; }
        public string Menu_Name { get; set; }
        public int display_Seq_No { get; set; }
        public int mn_display_Seq_No { get; set; }
        public string display_img { get; set; }
        public string Imageurl { get; set; }
    }
}
