using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantMVC.Models
{
    public class ViewOrderModel : BaseModel
    {
        public OrderModel orderModel { get; set; }
        public IEnumerable<OrderModel> ListorderModelP { get; set; }
        public IEnumerable<OrderModel> ListorderModelD { get; set; }

        public IEnumerable<OrderModel> ListorderModelA { get; set; }

        public IEnumerable<LovModel> ListLovModel { get; set; }

        public OrderItemModel  orderItemModel { get; set; }
        public IEnumerable<OrderItemModel> ListOrderItemModel { get; set; }

        public int Notificationcount { get; set; }

        public string  Status { get; set; }

    }
}