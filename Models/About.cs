using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class About
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Description")]
        public string Description { get; set; }

        public string Operated_By { get; set; }
        public string IsInserted { get; set; }
    }
}
