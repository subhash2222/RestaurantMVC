using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RestaurantMVC.Controllers
{
    //public class ReservationController : BaseController<ReservationModel>
    public class ReservationController : Controller
    {
        DetailAPI detailAPI = new DetailAPI();
        // GET: Reservation
        public ActionResult Index()
        {   
            Session["selectedmenu"] = "Reservation";
            BindCombo();
            return View();
        }

        public async void BindCombo()
        {
            Common common = new Common();
            MenuController MenuControllers = new MenuController();
            TimeModel timeModel = MenuControllers.GetModules();
            List<TimeList> timeListsres = common.BindDropdownlist("R", "", timeModel.Start_Time, timeModel.End_Time);
            ViewBag.timeListsres = timeListsres;
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

        [HttpPost]
        public ActionResult Index(ReservationModel reservation)
        {
            

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                //reservation.reser.Operated_By = HttpContext.Session["Request_ID"].ToString();
                //reservation.reser.IsInserted = "I";
                //CommonViewModel.reser = reservation.ListReservation;
                var reserv = reservation.reser;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(reserv),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("Reservation/InsertReservation", content);

                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    ShowMessaage(msgtype, message);
                }
            }
            BindCombo();
            return View("Index");
        }

        public async Task<JsonResult> Gettime(string dates)
        {
            Common common = new Common();
            MenuController MenuControllers = new MenuController();

            TimeModel timeModel = MenuControllers.GetModules();

            List<TimeList> timeListsres = common.BindDropdownlist("o", dates, timeModel.Start_Time, timeModel.End_Time);
            ViewBag.timeListsres = timeListsres;
            return Json(ViewBag.timeListsres);
        }
    }
}