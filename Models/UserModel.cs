using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class UserModel : BaseModel
    {
        public User user { get; set; }

        public IEnumerable<User> ListUser { get; set; }
    }
}
