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
    public class MenuAdmController : BaseController<MenuModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<Menu>> BindList()
        {
            List<Menu> dtl = new List<Menu>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Menu/GetMenuList");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<Menu>>(result);
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
            List<Menu> mnu = await BindList();

            CommonViewModel.ListMenu = mnu;
            return View(CommonViewModel);
        }


        public async Task<ActionResult> Create()
        {
            CommonViewModel.AddNew = "new";
            List<Menu> mnulst = await BindList();
            CommonViewModel.ListMenu = mnulst;
            CommonViewModel.menu = new Menu();
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            return View("Index", CommonViewModel);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuModel menumodel1)
        {
            if (ModelState.IsValid)
            {
                menumodel1.menu.Operated_By = HttpContext.Session["UserId"].ToString();
                menumodel1.menu.IsInserted = "I";
                CommonViewModel.menu = menumodel1.menu;
                var categorymdl = menumodel1.menu;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(categorymdl),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("Menu/InsertupdateMenu", content);

                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    ShowMessaage(msgtype, message);
                }
            }

            List<Menu> menulst = await BindList();
            CommonViewModel.ListMenu = menulst;
            CommonViewModel.AddNew = "new";
            return View("Index", CommonViewModel);
        }


        public async Task<ActionResult> Edit(string pmenuid)
        {

            CommonViewModel.Status = "edit";

            List<Menu> catglst = await BindList();
            CommonViewModel.ListMenu = catglst;
            HttpClient client = detailAPI.Initial();
            HttpResponseMessage response = await client.GetAsync("Menu/GetMenuById?id=" + pmenuid);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                CommonViewModel.menu = JsonConvert.DeserializeObject<Menu>(result);
            }
            return View("Index", CommonViewModel);
        }



        [HttpPost]
        public async Task<ActionResult> Edit(MenuModel menumodel1)
        {
            if (ModelState.IsValid)
            {
                menumodel1.menu.Operated_By = HttpContext.Session["UserId"].ToString();
                menumodel1.menu.IsInserted = "U";

                CommonViewModel.menu = menumodel1.menu;

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(menumodel1.menu),
      Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Menu/InsertupdateMenu", content);
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
            List<Menu> catglst = await BindList();
            CommonViewModel.ListMenu = catglst;

            return View("Index", CommonViewModel);
        }






        public async Task<ActionResult> Delete(string pmenuid)
        {
            //List<Item> itm = new List<Item>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Menu/DeleteMenu?id=" + pmenuid);
            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result;
                string[] strmsg = contents.Split('|');
                string msgtype = strmsg[0];
                string message = strmsg[1];
                ShowMessaage(msgtype, message);
            }

            List<Menu> catglst = await BindList();
            CommonViewModel.ListMenu = catglst;

            return View("Index", CommonViewModel);
        }
        #endregion
    }
}