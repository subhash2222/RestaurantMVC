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
    public class UserController : BaseController<UserModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<User>> BindList()
        {
            List<User> dtl = new List<User>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("User/GetUserinfo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<User>>(result);
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
            List<LovModel> dtllov1 = new List<LovModel>();
            HttpResponseMessage res1 = await client.GetAsync("Lov/GetLovDtl" + "?lovcolumn=USERTYPE");
            if (res.IsSuccessStatusCode)
            {
                var result = res1.Content.ReadAsStringAsync().Result;
                dtllov1 = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            ViewBag.usertypelist = dtllov1;
            
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
            List<User> userlst = await BindList();
            
            CommonViewModel.ListUser = userlst;
            return View(CommonViewModel);
        }

        public async Task<ActionResult> Create()
        {
            CommonViewModel.AddNew = "new";
            List<User> userlst = await BindList();
            CommonViewModel.ListUser = userlst;
            CommonViewModel.user = new User();
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            await BindLov();
            //return Json(CommonViewModel);            
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                usermodel.user.Operated_By = HttpContext.Session["UserId"].ToString();
                usermodel.user.IsInserted = "I";
                CommonViewModel.user = usermodel.user;
                var usermdl = usermodel.user;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(usermdl),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("User/InsertupdateUser", content);

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
            List<User> userlst = await BindList();
            CommonViewModel.ListUser = userlst;
            CommonViewModel.AddNew = "new";
            return View("Index", CommonViewModel);
        }
        
        public async Task<ActionResult> Edit(string puserid)
        {
            ViewBag.Message = "";
            ViewBag.MessageErr = "";

            CommonViewModel.Status = "edit";
            await BindLov();
            List<User> userlst = await BindList();
            CommonViewModel.ListUser = userlst;
            HttpClient client = detailAPI.Initial();
            HttpResponseMessage response = await client.GetAsync("User/GetUserById?puserid=" + puserid);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                CommonViewModel.user = JsonConvert.DeserializeObject<User>(result);
            }
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                usermodel.user.Operated_By = HttpContext.Session["UserId"].ToString();
                usermodel.user.IsInserted = "U";

                CommonViewModel.user = usermodel.user;

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(usermodel.user),
      Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("User/InsertupdateUser", content);
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
            List<User> userlst = await BindList();
            CommonViewModel.ListUser = userlst;
            await BindLov();
            return View("Index", CommonViewModel);
        }
        
        public async Task<ActionResult> Delete(string puserid)
        {
            //List<Item> itm = new List<Item>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("User/DeleteUserRecord?puserid=" + puserid);
            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result;
                string[] strmsg = contents.Split('|');
                string msgtype = strmsg[0];
                string message = strmsg[1];
                ShowMessaage(msgtype, message);
            }

            List<User> userlst = await BindList();
            CommonViewModel.ListUser = userlst;

            return View("Index", CommonViewModel);
        }
        #endregion


    }
}