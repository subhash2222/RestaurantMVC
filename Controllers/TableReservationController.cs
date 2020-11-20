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
    public class TableReservationController : BaseController<TableReservationModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<TableReservation>> BindList()
        {
            List<TableReservation> dtl = new List<TableReservation>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("TableReservation/GetTableReservationinfo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<TableReservation>>(result);
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
            List<TableReservation> tablereslst = await BindList();

            CommonViewModel.Listtblreservation = tablereslst;
            return View(CommonViewModel);
        }

        public async Task<ActionResult> Create()
        {
            CommonViewModel.AddNew = "new";
            List<TableReservation> tablereslst = await BindList();
            CommonViewModel.Listtblreservation = tablereslst;
            CommonViewModel.tblreservation = new TableReservation();
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            await BindLov();
            //return Json(CommonViewModel);            
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TableReservationModel tablrmodel1)
        {
            if (ModelState.IsValid)
            {
                tablrmodel1.tblreservation.Operated_By = HttpContext.Session["UserId"].ToString();
                tablrmodel1.tblreservation.IsInserted = "I";
                CommonViewModel.tblreservation = tablrmodel1.tblreservation;
                var categorymdl = tablrmodel1.tblreservation;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(categorymdl),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("TableReservation/InsertupdateDetail", content);

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
            List<TableReservation> tablrlst = await BindList();
            CommonViewModel.Listtblreservation = tablrlst;
            CommonViewModel.AddNew = "new";
            return View("Index", CommonViewModel);
        }


        public async Task<ActionResult> Edit(string pitemid)
        {
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            
            CommonViewModel.Status = "edit";
            await BindLov();
            List<TableReservation> tablrlst = await BindList();
            CommonViewModel.Listtblreservation = tablrlst;
            HttpClient client = detailAPI.Initial();
            HttpResponseMessage response = await client.GetAsync("TableReservation/GetTableReservationinfoId?id=" + pitemid);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                CommonViewModel.tblreservation = JsonConvert.DeserializeObject<TableReservation>(result);
            }
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TableReservationModel tablrmodel1)
        {
            if (ModelState.IsValid)
            {
                tablrmodel1.tblreservation.Operated_By = HttpContext.Session["UserId"].ToString();
                tablrmodel1.tblreservation.IsInserted = "U";

                CommonViewModel.tblreservation = tablrmodel1.tblreservation;

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(tablrmodel1.tblreservation),
      Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("TableReservation/InsertupdateDetail", content);
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
              await BindLov();
           
           
        
            CommonViewModel.Status = "edit";
            List<TableReservation> tablrlst = await BindList();
            CommonViewModel.Listtblreservation = tablrlst;
            await BindLov();
            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Delete(string id)
        {
            //List<Item> itm = new List<Item>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("TableReservation/DeleteRecord?id=" + id);
            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result;
                string[] strmsg = contents.Split('|');
                string msgtype = strmsg[0];
                string message = strmsg[1];
                ShowMessaage(msgtype, message);
            }

            List<TableReservation> tablrlst = await BindList();
            CommonViewModel.Listtblreservation = tablrlst;

            return View("Index", CommonViewModel);
        }



        #endregion


    }
}