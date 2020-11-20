using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace RestaurantMVC.Helper
{
    public class DetailAPI
    {
        public HttpClient Initial()
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

            var Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:44311/api/");
            //Client.BaseAddress = new Uri("https://restaurantapi.padhyasoft.com/api/");
            return Client;
        }
    }
}