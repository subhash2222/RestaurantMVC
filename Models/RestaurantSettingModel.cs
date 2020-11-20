using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class RestaurantSettingModel : BaseModel
    {
        public RestaurantSetting RestaurantSetting { get; set; }

        public RestaurantSettingDTO RestaurantSettingDTO { get; set; }

        public IEnumerable<RestaurantSetting> ListRestaurantSetting { get; set; }

        public Weekday weekday { get; set; }
    }

    public class Weekday
    {
        public string dayName { get; set; }
        public string Description { get; set; }
    }
    
}