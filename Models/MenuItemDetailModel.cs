using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantMVC.Models
{
    public class MenuItemDetailModel : BaseModel
    {
        public MenuItemDetail menuItemDetail { get; set; }

        public ShoppingCartDetail shoppingCartDetail { get; set; }
        public IEnumerable<MenuItemDetail> ListmenuItemMain { get; set; }
        public IEnumerable<MenuItemDetail> ListmenuItemDetail { get; set; }
        public IEnumerable<MenuItemOption> ListMenuItemOption { get; set; }

    }
}