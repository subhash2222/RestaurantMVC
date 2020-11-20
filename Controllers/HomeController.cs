using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RestaurantMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["selectedmenu"] = "Home";
            return View();
        }

        public async Task<List<DisplayModel>> DisplayModelList(string date, string lang)
        {
            DetailAPI detailAPI = new DetailAPI();
            List<DisplayModel> dtl = new List<DisplayModel>();
            HttpClient client = detailAPI.Initial();
                        
            HttpResponseMessage res = await client.GetAsync("SettingMsg/GetSettingMessages?curdate=" + date + "&langcode=" + lang);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<DisplayModel>>(result);
            }
            return dtl;
        }

        public ActionResult Indexadm()
        {
            Session["UserId"] = "admin";
            return View();
        }
        public ActionResult Change(String language)
        {
            if (language != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                //Set the Language.
                //language = Request.UserLanguages[0];
            }
            HttpCookie cookie = new HttpCookie("Home");
            cookie.Value = language;
            Response.Cookies.Add(cookie);

            string viewname = "Home";
            if (Session["selectedmenu"] != null)
            {
                viewname = Session["selectedmenu"].ToString();
            }

            if (Request.Cookies["DisplayModelLists"] != null)
            {
                var c = new HttpCookie("DisplayModelLists");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("Index", viewname);
        }

    }
}