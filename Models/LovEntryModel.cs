using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class LovEntryModel : BaseModel
    {
        public LovEntry loventry { get; set; }
        public IEnumerable<LovEntry> Listlov { get; set; }
    }
}
