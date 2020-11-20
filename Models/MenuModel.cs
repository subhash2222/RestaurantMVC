using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class MenuModel : BaseModel
    {
        public Menu menu { get; set; }

        public IEnumerable<Menu> ListMenu { get; set; }
    }   
}
