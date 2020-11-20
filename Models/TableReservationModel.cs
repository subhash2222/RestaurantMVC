using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class TableReservationModel : BaseModel
    {
        public TableReservation tblreservation { get; set; }
        public IEnumerable<TableReservation> Listtblreservation { get; set; }
    }
}
