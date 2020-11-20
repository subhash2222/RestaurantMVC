using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class AboutModel : BaseModel
    {
        public About about { get; set; }

        public IEnumerable<About> Listabout { get; set; }
    }
}
