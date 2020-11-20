using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace RestaurantMVC.Controllers
{
    public class MenuItemController : BaseController<MenuItemModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<MenuItem>> BindList(int pmenuid)
        {
            { 
                int id1 = pmenuid;
                TempData["ID9"] = id1;


            }

            List<MenuItem> itm = new List<MenuItem>();
            HttpClient client = detailAPI.Initial();

            int id = Convert.ToInt32(TempData["ID1"]);
            MenuItem menu = new MenuItem();
            menu.Menu_ID = id;



            //int id3 = Convert.ToInt32(TempData["ID"]);

            //menu.Menu_ID = id3;

            //int id6 = Convert.ToInt32(TempData["ID"]);

            //menu.Menu_ID = id6;

            HttpResponseMessage res = await client.GetAsync("MenuItem/GetMenuItemInfo?menuid=" + pmenuid);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                itm = JsonConvert.DeserializeObject<List<MenuItem>>(result);
            }
            return itm;
        }


        public async Task CategoryCombo()
        {
            List<CategoryComboModel> Catg = new List<CategoryComboModel>();
            HttpClient client = detailAPI.Initial();
            HttpResponseMessage res = await client.GetAsync("MenuItem/GetCategoyCombo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                Catg = JsonConvert.DeserializeObject<List<CategoryComboModel>>(result);
            }
            ViewBag.Categorylist = Catg;
            await ItemCombo("");

        }


        public async Task<dynamic> ItemCombo(string categoryid)
        {
            MenuItem menu = new MenuItem();
            menu.Category_ID = menu.Category_ID;

            if (menu.Category_ID == menu.Category_ID)
            {
                List<ItemComboModel> itm = new List<ItemComboModel>();
                HttpClient client = detailAPI.Initial();
                HttpResponseMessage res = await client.GetAsync("MenuItem/GetItemCombo?Categoryid=" + categoryid);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    itm = JsonConvert.DeserializeObject<List<ItemComboModel>>(result);
                }
                ViewBag.Itemlist = itm;

            }
            else
            {
                ViewBag.imagearray1 = "error";
            }
            return Json(ViewBag.Itemlist, JsonRequestBehavior.AllowGet);
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

        public async Task<ActionResult> Index(int pmenuid)
        {
            ViewBag.pmenuid1 = pmenuid;
            {
                int id1 = pmenuid;
                TempData["ID1"] = id1;


            }

            int id = ViewBag.pmenuid1;
            TempData["ID"] = id;
            List<MenuItem> menuitem = await BindList(pmenuid);

            CommonViewModel.ListMenuItem = menuitem;
            return View(CommonViewModel);

        }
       
        public async Task<ActionResult> Create(int pmenuid)
        {
            //MenuItem menu = new MenuItem();
            //menu.Menu_ID = Int32.Parse(pmenuid);

            //temp data pass//
            {
                int id5 = pmenuid;
                TempData["ID5"] = id5;

            }


            ViewBag.Menu_ID = pmenuid;
            TempData[""] = pmenuid;
            CommonViewModel.AddNew = "new";
            List<MenuItem> Menuitmlst = await BindList(pmenuid);
            CommonViewModel.ListMenuItem = Menuitmlst;

            CommonViewModel.menuItem = new MenuItem();
            ViewBag.Message = "";
            ViewBag.MessageErr = "";

            await CategoryCombo();


            return View("Index", CommonViewModel);
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuItemModel menuitem1)
        {
            if (menuitem1.menuItem.Item_ID == 0)
            {
                ModelState.AddModelError("Item Name", "Item Name is required.");
                //List<MenuItem> menulst = await BindList(menuitem1.menuItem.Menu_ID);
                List<MenuItem> menulst1 = await BindList(menuitem1.menuItem.Menu_ID);
                await CategoryCombo();
                
                CommonViewModel.ListMenuItem = menulst1;
                CommonViewModel.AddNew = "new";
                return View("Index", CommonViewModel);
            }

            if (ModelState.IsValid)
            {
                menuitem1.menuItem.Operated_By = HttpContext.Session["UserId"].ToString();
                menuitem1.menuItem.IsInserted = "I";
                if (menuitem1.menuItem.Menu_ID == 0)
                {
                    int id5 = Convert.ToInt32(TempData["ID5"]);
                    MenuItem menu = new MenuItem();
                    menu.Menu_ID = id5;
                    menuitem1.menuItem.Menu_ID = menu.Menu_ID;
                }
                else
                {
                    ViewBag.imagearray1 = "error";

                };


                CommonViewModel.menuItem = menuitem1.menuItem;
                var categorymdl = menuitem1.menuItem;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(categorymdl),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("MenuItem/InsertupdateDetail", content);

                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    ShowMessaage(msgtype, message);
                }
            }
            

            //{

            //    int id3 = menuitem1.menuItem.Menu_ID;
            //    TempData["ID"] = id3;

            //}
            List<MenuItem> menulst = await BindList(menuitem1.menuItem.Menu_ID);

            await CategoryCombo();

            CommonViewModel.ListMenuItem = menulst;
            CommonViewModel.AddNew = "new";
            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Edit(int pmenuid)
        {
            //{
            //    int id6 = pmenuid;
            //    TempData["ID1"] = id6;


            //}

            ViewBag.Message = "";
            ViewBag.MessageErr = "";

            CommonViewModel.Status = "edit";
            await CategoryCombo();
            List<MenuItem> Menuitmlst = await BindList(pmenuid);
            CommonViewModel.ListMenuItem = Menuitmlst;
            HttpClient client = detailAPI.Initial();
            HttpResponseMessage response = await client.GetAsync("MenuItem/GetMenuItemById?menuid=" + pmenuid);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                CommonViewModel.menuItem = JsonConvert.DeserializeObject<MenuItem>(result);
            }
            return View("Index", CommonViewModel);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(MenuItemModel menuitem1)
        {
            if (ModelState.IsValid)
            {
                menuitem1.menuItem.Operated_By = HttpContext.Session["UserId"].ToString();
                menuitem1.menuItem.IsInserted = "U";

                CommonViewModel.menuItem = menuitem1.menuItem;

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(menuitem1.menuItem),
      Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("MenuItem/InsertupdateDetail", content);
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
            List<MenuItem> catglst = await BindList(menuitem1.menuItem.Menu_ID);
            CommonViewModel.ListMenuItem = catglst;
            await CategoryCombo();
            return View("Index", CommonViewModel);
        }




        public async Task<ActionResult> Delete(int pmenuid , int cid , int itemid)
        {

            HttpClient client = detailAPI.Initial(); 

            HttpResponseMessage res = await client.GetAsync("MenuItem/DeleteMenu?id=" + pmenuid + "&cid=" + cid + "&itemid=" + itemid);
            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result;
                string[] strmsg = contents.Split('|');
                string msgtype = strmsg[0];
                string message = strmsg[1];
                ShowMessaage(msgtype, message);
            }
            List<MenuItem> menulst = await BindList(pmenuid);
            CommonViewModel.ListMenuItem = menulst;


            return View("Index", CommonViewModel);
        }

        #endregion


    }
}