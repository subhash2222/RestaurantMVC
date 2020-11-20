using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace RestaurantMVC.Controllers
{
    public class SettingsController : BaseController<RestaurantSettingModel>
    {
        DetailAPI detailAPI = new DetailAPI();
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
        // GET: Settings

        public void Bindcombo()
        {
            List<Weekday> daylist = new List<Weekday>();
            daylist.Insert(0, new Weekday { dayName = "0", Description = "--Select--" });
            daylist.Add(new Weekday { dayName = "Monday", Description = "Monday" });
            daylist.Add(new Weekday { dayName = "Tuesday", Description = "Tuesday" });
            daylist.Add(new Weekday { dayName = "Wednesday", Description = "Wednesday" });
            daylist.Add(new Weekday { dayName = "Thursday", Description = "Thursday" });
            daylist.Add(new Weekday { dayName = "Friday", Description = "Friday" });
            daylist.Add(new Weekday { dayName = "Saturday", Description = "Saturday" });
            daylist.Add(new Weekday { dayName = "Sunday", Description = "Sunday" });
            ViewBag.Weekdays = daylist;
        }

        public async Task<ActionResult> Index()
        {
            Session["selectedmenu"] = "Settings";
            Bindcombo();

            RestaurantSettingDTO dtl = new RestaurantSettingDTO();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("RestaurantSetting/GetResSetting");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<RestaurantSettingDTO>(result);
            }
            CommonViewModel.RestaurantSettingDTO = dtl;

            return View(CommonViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RestaurantSettingModel rest1)
        {
            if (ModelState.IsValid)
            {
                RestaurantSettingDTO rest = rest1.RestaurantSettingDTO;
                rest.OpratedBy = "1";
                rest.Status = "A";
                RestaurantSetting resObj = new RestaurantSetting();
                DateTime Res_Close_Start_Date = DateTime.ParseExact(rest.Res_Close_Start_Date, "dd/MM/yyyy", null);
                DateTime Res_Close_End_Date = DateTime.ParseExact(rest.Res_Close_End_Date, "dd/MM/yyyy", null);
                DateTime Pickup_Close_Start_Date = DateTime.ParseExact(rest.Pickup_Close_Start_Date, "dd/MM/yyyy", null);
                DateTime Pickup_Close_End_Date = DateTime.ParseExact(rest.Pickup_Close_End_Date, "dd/MM/yyyy", null);
                DateTime Delv_Close_Start_Date = DateTime.ParseExact(rest.Delv_Close_Start_Date, "dd/MM/yyyy", null);
                DateTime Delv_Close_End_Date = DateTime.ParseExact(rest.Delv_Close_End_Date, "dd/MM/yyyy", null);

                resObj.Res_Close_Start_Date = Res_Close_Start_Date;
                resObj.Res_Close_End_Date = Res_Close_End_Date;
                resObj.Res_Close_Message = rest.Res_Close_Message;
                resObj.Res_Msg_No_of_Days = rest.Res_Msg_No_of_Days;
                resObj.Pickup_Close_Start_Date = Pickup_Close_Start_Date;
                resObj.Pickup_Close_End_Date = Pickup_Close_End_Date;
                resObj.Pickup_Close_Message = rest.Pickup_Close_Message;
                resObj.Pickup_Msg_No_of_Days = rest.Pickup_Msg_No_of_Days;
                resObj.Delv_Close_Start_Date = Delv_Close_Start_Date;
                resObj.Delv_Close_End_Date = Delv_Close_End_Date;
                resObj.Delv_Close_Message = rest.Delv_Close_Message;
                resObj.Delv_Msg_No_of_Days = rest.Delv_Msg_No_of_Days;
                resObj.Res_Close_Message_Dan = rest.Res_Close_Message_Dan;
                resObj.Pickup_Close_Message_Dan = rest.Pickup_Close_Message_Dan;
                resObj.Delv_Close_Message_Dan = rest.Delv_Close_Message_Dan;
                resObj.WeeklyOffDay = rest.WeeklyOffDay;
                resObj.WeeklyOffMsg = rest.WeeklyOffMsg;
                resObj.WeeklyOffMsgDan = rest.WeeklyOffMsgDan;
                //resObj.Status = "A";
                resObj.OpratedBy = Convert.ToString(Session["UserId"]);

                //var con = resObj;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(resObj),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("RestaurantSetting/InsertResSetting", content);

                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    //ShowMessaage(msgtype, message);
                    ViewBag.Message = "";
                    ViewBag.MessageErr = "";
                    message = message.Trim('"');
                    if (msgtype.Contains("S"))
                    {
                        CommonViewModel.Message = string.Format(message);  //Message
                    }
                    else
                    {
                        CommonViewModel.ErrorMessage = string.Format(message);
                    }
                    //CommonViewModel.Message = message;
                }

                //RestaurantSettingDTO RestaurantSettingDTO = new RestaurantSettingDTO();
                //RestaurantSettingDTO rest = rest1.RestaurantSettingDTO;
                CommonViewModel.RestaurantSettingDTO = rest;
            }
            //return View("Index", CommonViewModel);
            return Json(CommonViewModel);
        }
    }
}