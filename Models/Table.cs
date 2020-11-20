using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class Table
    {
        public int table_ID { get; set; }
        [Required(ErrorMessage = "Please enter Table name")]
        public string table_Name { get; set; }

        
        public string remark { get; set; }

        [Required(ErrorMessage = "Please select Status")]
        public string status { get; set; }      
        public string statusdesc { get; set; }

        [Required(ErrorMessage = "Please enter display sequence no")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "display sequence no is Numeric only")]
        public int display_Seq_No { get; set; }
        public string Operated_By { get; set; }
        public string IsInserted { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    }
}
