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
    public class TableController : BaseController<TableModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<Table>> BindList()
        {
            List<Table> dtl = new List<Table>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Table/GetTableinfo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<Table>>(result);
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
            List<Table> catglst = await BindList();
            
            CommonViewModel.ListTable = catglst;
            return View(CommonViewModel);
        }

   
        public async Task<ActionResult> Create()
        {
            CommonViewModel.AddNew = "new";
            List<Table> userlst = await BindList();
            CommonViewModel.ListTable = userlst;
            CommonViewModel.table = new Table();
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            await BindLov();
            //return Json(CommonViewModel);            
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TableModel usermodel)
        {
            if (ModelState.IsValid)
            {
                usermodel.table.Operated_By = HttpContext.Session["UserId"].ToString();
                usermodel.table.IsInserted = "I";
                CommonViewModel.table = usermodel.table;
                var usermdl = usermodel.table;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(usermdl),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("Table/InsertupdateUser", content);

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
            List<Table> userlst = await BindList();
            CommonViewModel.ListTable = userlst;
            CommonViewModel.AddNew = "new";
            return View("Index", CommonViewModel);
        }



        public async Task<ActionResult> Edit(string puserid)
        {
            ViewBag.Message = "";
            ViewBag.MessageErr = "";

            CommonViewModel.Status = "edit";
            await BindLov();
            List<Table> userlst = await BindList();
            CommonViewModel.ListTable = userlst;
            HttpClient client = detailAPI.Initial();
            HttpResponseMessage response = await client.GetAsync("Table/GetTableinfoById?id=" + puserid);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                CommonViewModel.table = JsonConvert.DeserializeObject<Table>(result);
            }
            return View("Index", CommonViewModel);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(TableModel usermodel)
        {
            if (ModelState.IsValid)
            {
                usermodel.table.Operated_By = HttpContext.Session["UserId"].ToString();
                usermodel.table.IsInserted = "U";

                CommonViewModel.table = usermodel.table;

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(usermodel.table),
      Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Table/InsertupdateUser", content);
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
            List<Table> userlst = await BindList();
            CommonViewModel.ListTable = userlst;
            await BindLov();
            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Delete(string puserid)
        {
            //List<Item> itm = new List<Item>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Table/DeleteTableRecord?id=" + puserid);
            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result;
                string[] strmsg = contents.Split('|');
                string msgtype = strmsg[0];
                string message = strmsg[1];
                ShowMessaage(msgtype, message);
            }

            List<Table> tablelst = await BindList();
            CommonViewModel.ListTable = tablelst;

            return View("Index", CommonViewModel);
        }
        #endregion


    }
}