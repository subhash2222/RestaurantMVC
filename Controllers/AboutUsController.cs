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
    public class AboutUsController : BaseController<AboutModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        #region Events

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Create()
        {

            ViewBag.Message = "";
            ViewBag.MessageErr = "";

            About abouts = new About();

            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("About/GetAboutDetail");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                abouts = JsonConvert.DeserializeObject<About>(result);
            }
            if (abouts == null)
            {
                CommonViewModel.about = new About();
            }
            else
            {
                CommonViewModel.about = abouts;
            }

            return View("Create", CommonViewModel);
            //return View(CommonViewModel);
        }

        public void ShowMessaage(string msgtype, string message)
        {
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            message = message.Trim('"');
            if (msgtype.Contains("S"))
            {
                ViewBag.Message = string.Format(message);
            }
            else
            {
                ViewBag.MessageErr = string.Format(message);
            }
        }


        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(AboutModel aboutmodel1)
        {
            aboutmodel1.about.Operated_By = HttpContext.Session["UserId"].ToString();


            if (aboutmodel1.about.ID == 0)
            {
                aboutmodel1.about.IsInserted = "I";

            }
            else
            {

                aboutmodel1.about.IsInserted = "U";
            }


            CommonViewModel.about = aboutmodel1.about;
            var aboutmdl = aboutmodel1.about;
            HttpClient client = detailAPI.Initial();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(aboutmdl),
  Encoding.UTF8, "application/json");


            HttpResponseMessage res = await client.PostAsync("About/InsertupdateDetail", content);

            var contents = res.Content.ReadAsStringAsync().Result;
            string[] strmsg = contents.Split('|');
            string msgtype = strmsg[0];
            string message = strmsg[1];
            ShowMessaage(msgtype, message);


            return View(CommonViewModel);
        }



        #endregion


    }
}