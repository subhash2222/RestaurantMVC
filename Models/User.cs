using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class User
    {
        [Required(ErrorMessage = "Please enter UserID")]
        public string UserID   { get; set; }
        [Required(ErrorMessage = "Please enter User name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter Password")]

        [DisplayName("Password")] //makes column title not split
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please select Status")]
        public string UserType { get; set; }

        public string UserTypeDesc { get; set; }
        [Required(ErrorMessage = "Please enter Valid Email Id")]
        
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string EmailID  { get; set; }

        [Required(ErrorMessage = "Please enter Valid MobileNo")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please select Status")]
        public string Status   { get; set; }
        public string Statusdesc { get; set; }
        public string Operated_By { get; set; }
        public string IsInserted { get; set; }
    }
}
