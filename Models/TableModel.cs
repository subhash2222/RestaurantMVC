using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class TableModel : BaseModel
    {
        public Table table { get; set; }

        public IEnumerable<Table> ListTable { get; set; }
    }
}
