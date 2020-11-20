using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class CategoryModel : BaseModel
    {
        public Category category { get; set; }

        public IEnumerable<Category> ListCategory { get; set; }
    }
}
