using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class ContactModel
    {
        public int Con_Id { get; set; }

        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        public string Subject { get; set; }
        public string Message { get; set; }
        public string IsInserted { get; set; }
    }
}