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
    public class ParamformController : BaseController<ParamModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<Param>> BindList()
        {
            List<Param> dtl = new List<Param>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Paramform/GetCatgInfo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<Param>>(result);
            }
            return dtl;
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

        #region Events

        public async Task<ActionResult> Index()
        {
            return View();
        }

 

        public async Task<ActionResult> Create()
        {
            Param form = new Param();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Paramform/GetPforminfo");


            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                form = JsonConvert.DeserializeObject<Param>(result);
                CommonViewModel.param = form;
            }

            
            return View("Create", CommonViewModel);
           
        }

   
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ParamModel parammodel1)
        {
            if (ModelState.IsValid)
            {
                parammodel1.param.Operated_By = HttpContext.Session["UserId"].ToString();
                parammodel1.param.IsInserted = "I";
                CommonViewModel.param = parammodel1.param;
                var categorymdl = parammodel1.param;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(categorymdl),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("Paramform/InsertupdateForm", content);

                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                   
                    ShowMessaage(msgtype, message);
                }
            }
                 
            return View("Create", CommonViewModel);
        }



        #endregion


    }
}