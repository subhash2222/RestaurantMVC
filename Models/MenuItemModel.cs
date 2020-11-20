using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class MenuItemModel : BaseModel
    {
        public MenuItem menuItem { get; set; }

        public IEnumerable<MenuItem> ListMenuItem { get; set; }
    }
}
