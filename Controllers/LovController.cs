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
    public class LovController : BaseController<LovEntryModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<LovEntry>> BindList()
        {
            List<LovEntry> dtl = new List<LovEntry>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("LovEntry/GetLovEntryinfo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<LovEntry>>(result);
            }
            return dtl;
        }

        public async Task BindLov()
        {
            List<LovModel> dtllov = new List<LovModel>();
            HttpClient client = detailAPI.Initial();
            //HttpResponseMessage res = await client.GetAsync("Lov/" + "ACTIVEINACTIVE");
            HttpResponseMessage res = await client.GetAsync("Lov/GetLovDtl" + "?lovcolumn=ACTIVEINACTIVE");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtllov = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            ViewBag.statuslist = dtllov;
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
            List<LovEntry> lovlst = await BindList();
            
            CommonViewModel.Listlov = lovlst;
            return View(CommonViewModel);
        }

        public async Task<ActionResult> Create()
        {
            CommonViewModel.AddNew = "new";
            List<LovEntry> lovlst = await BindList();
            CommonViewModel.Listlov = lovlst;
            CommonViewModel.loventry = new LovEntry();
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            await BindLov();
            //return Json(CommonViewModel);            
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LovEntryModel loventrymodel1)
        {
            if (ModelState.IsValid)
            {
                loventrymodel1.loventry.Operated_By = HttpContext.Session["UserId"].ToString();
                loventrymodel1.loventry.IsInserted = "I";
                CommonViewModel.loventry = loventrymodel1.loventry;
                var categorymdl = loventrymodel1.loventry;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(categorymdl),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("LovEntry/InsertupdateDetail", content);

                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    ShowMessaage(msgtype, message);
                }
            }
            await BindLov();
            List<LovEntry> lovlst = await BindList();
            CommonViewModel.Listlov = lovlst;
            CommonViewModel.AddNew = "new";
            return View("Index", CommonViewModel);
        }
        
        public async Task<ActionResult> Edit(string plovcolumn, string plovcode)

        {
            ViewBag.Message = "";
            ViewBag.MessageErr = "";

            CommonViewModel.Status = "edit";
            await BindLov();
            List<LovEntry> lovlst = await BindList();
            CommonViewModel.Listlov = lovlst;
            HttpClient client = detailAPI.Initial();
           
            HttpResponseMessage response = await client.GetAsync("LovEntry/GetLovEntryById?plovcolumn=" + plovcolumn +  "&plovcode=" + plovcode);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                CommonViewModel.loventry = JsonConvert.DeserializeObject<LovEntry>(result);
            }
            return View("Index", CommonViewModel);
        }

    

        [HttpPost]
        public async Task<ActionResult> Edit(LovEntryModel loventrymodel1)
        {
            if (ModelState.IsValid)
            {
                loventrymodel1.loventry.Operated_By = HttpContext.Session["UserId"].ToString();
                loventrymodel1.loventry.IsInserted = "U";

                CommonViewModel.loventry = loventrymodel1.loventry;

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(loventrymodel1.loventry),
      Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("LovEntry/InsertupdateDetail", content);
                //string statuscode = response.StatusCode.ToString();

                if (response.IsSuccessStatusCode)
                {
                    var contents = response.Content.ReadAsStringAsync().Result;
                    string[] strmsg = contents.Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    ShowMessaage(msgtype, message);
                }
            }
            CommonViewModel.Status = "edit";
            List<LovEntry> lovlst = await BindList();
            CommonViewModel.Listlov = lovlst;
            await BindLov();
            return View("Index", CommonViewModel);
        }



        public async Task<ActionResult> Delete(string plovcolumn, string plovcode)
        {
            //List<Item> itm = new List<Item>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("LovEntry/DeleteLovEntryRecord?plovcolumn=" + plovcolumn + "&plovcode=" + plovcode);

            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result;        
                string[] strmsg = contents.Split('|');
                string msgtype = strmsg[0];
                string message = strmsg[1];
                ShowMessaage(msgtype, message);
            }

            List<LovEntry> lovlst = await BindList();
            CommonViewModel.Listlov = lovlst;

            return View("Index", CommonViewModel);
        }
        #endregion


    }
}