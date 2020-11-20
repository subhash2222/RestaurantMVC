using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class MainModel
    {
        public LanguageModel languageModel { get; set; }

        public IEnumerable<LanguageModel> listlanguageModel { get; set; }
    }
}