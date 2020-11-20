using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class BaseModel
    {
        public string AddNew { get; set; }
        public string Message { get; set; }
        public string Alert { get; set; }
        public string ActionMode { get; set; }
        public bool ShowSucessMessage { get; set; }
        public bool RememberMe { get; set; }
        public string RedirectUrl { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string Insert { get; set; }
        public string Delete { get; set; }
        public string Edit { get; set; }
        public string Select { get; set; }
        public string SelectedAction { get; set; }
        public bool IsAlertBox { get; set; } = true;
        public bool IsTableExists { get; set; } = true;
        public string ReadOnly { get; set; }
        public string Disabled { get; set; }
    }
}
