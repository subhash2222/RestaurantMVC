using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RestaurantMVC.Controllers
{
    public class LoginController : Controller
    {
        DetailAPI detailAPI = new DetailAPI();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST : Login
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            //return View();
            if (ModelState.IsValid)
            {
                UserLogininfo user = new UserLogininfo();
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model),
                Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PostAsync("Login/CheckAuthentication", content);
                var contents = res.Content.ReadAsStringAsync().Result.ToString();
                user = JsonConvert.DeserializeObject<UserLogininfo>(contents);
                if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    Session["UserName"] = user.UserName;
                    return RedirectToAction("Indexadm", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "UserName or Password Invalid";
                    return View();
                }
                //if (model.Password == "admin" && model.UserId == "admin")
                //{
                //    Session["UserId"] = "admin";
                //    return RedirectToAction("Indexadm", "Home");
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "UserName or Password Invalid";
                //    return View();
                //}
            }
            else
            {
                return View();
            }

            //return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}