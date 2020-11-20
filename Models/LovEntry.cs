using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class LovEntry
    {
        [Required(ErrorMessage = "Please enter Lov Column")]
        public string Lov_Column { get; set; }
        [Required(ErrorMessage = "Please enter Lov Code")]
        public string Lov_Code { get; set; }
        [Required(ErrorMessage = "Please enter  Description")]
        public string Lov_Desc { get; set; }
        [Required(ErrorMessage = "Please select Status")]
        public string Status { get; set; }
       
        public string Statusdesc { get; set; }

        [Required(ErrorMessage = "Please enter display sequence no")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "display sequence no is Numeric only")]
        public int display_Seq_No { get; set; }

        public string Operated_By { get; set; }
        public string IsInserted { get; set; }



    }
}
