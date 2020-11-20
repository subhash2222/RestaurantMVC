using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantMVC.Models
{
    public class ItemAdmModel : BaseModel
    {
        public Item item { get; set; }
        public IEnumerable<Item> ListItem { get; set; }

    }
}