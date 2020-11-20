using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class Item
    {
        public int Item_ID { get; set; }
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
        public string Item_Name { get; set; }
        public decimal Item_Rate { get; set; }
        public string status { get; set; }
        public string statusdesc { get; set; }
        public int display_Seq_No { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsDiscount { get; set; }

        //public bool CheckBoxValue
        //{
        //    get { return Boolean.Parse(IsSpecial); }
            
        //}

        //public bool CheckBoxValue1
        //{
        //    get { return Boolean.Parse(IsDiscount); }
        //}
        public string Image { get; set; }
        public decimal Disc_Percentage { get; set; }
        public string Operated_By { get; set; }
        public string IsInserted { get; set; }

        //public FileInfo[] Files { get; set; }

    }
}
