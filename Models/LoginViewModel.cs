using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}