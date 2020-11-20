using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace RestaurantMVC.Controllers
{
    public class HeaderController : BaseController<HeaderModel>
    {


        //public ActionResult RenderMenu()
        //{
        //    DetailAPI detailAPI = new DetailAPI();
        //    List<DisplayModel> dtl = new List<DisplayModel>();
        //    HttpClient client = detailAPI.Initial();

        //    string date = "10/01/2020";
        //    string lang = "E";

        //    dtl = await DisplayModelList(date, lang);



        //    return PartialView("_LeftMenu", CommonViewModel);
        //}

        [ChildActionOnly]
        public async Task<ActionResult> DisplayModelList()
        {
            DisplayModel dtl = new DisplayModel();
            dtl = await DisplayModelList1();
            return PartialView("_Header", dtl);
        }

        public async Task<DisplayModel> DisplayModelList1()
        {

            var date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            string lang = "E";
            HttpCookie cookie = HttpContext.Request.Cookies["Home"];
            if (cookie != null && cookie.Value != null)
            {
                if(cookie.Value == "en-Us")
                {
                    lang = "E";
                }
                else
                {
                    lang = "da";
                }
            }
             
            DetailAPI detailAPI = new DetailAPI();
            DisplayModel dtl = new DisplayModel();
            HttpClient client = detailAPI.Initial();
            CommonViewModel.DisplayModels = new DisplayModel();

            dtl = GetCookie("DisplayModelLists");
            if (dtl != null)
            {
            }
            else
            {                
                HttpResponseMessage res = await client.GetAsync("SettingMsg/GetSettingMessages?curdate=" + date + "&langcode=" + lang);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    dtl = JsonConvert.DeserializeObject<DisplayModel>(result);
                    AddCookie(dtl);
                }
            }

            // CommonViewModel.DisplayModels = dtl;

            return dtl;
        }

        public void AddCookie(DisplayModel basketList)
        {

            var response = HttpContext.Response;
            HttpCookie cookie = new HttpCookie("DisplayModelLists");
            string objCartListString = "";
            
            objCartListString += string.Join(",", basketList.DeliveryCloseMsg, basketList.DisplayMsgDeliveyClose, basketList.DisplayMsgOnHome, basketList.DisplayMsgPickupClose, basketList.DisplayMsgResClose, basketList.IsDeliveryAvailable, basketList.IsPickupAvailable, basketList.IsRestaurantOpen, basketList.PickupCloseMsg, basketList.ResCloseMsg);
            //var date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            //HttpContext.Response.Cookies["DisplayModelLists"].Value = objCartListString;
            cookie.Value = objCartListString;
            Int16 CookiesExpireTime = Convert.ToInt16(ConfigurationManager.AppSettings["CookiesExpireTime"]);
            cookie.Expires = DateTime.Now.AddMinutes(CookiesExpireTime);

           // cookie.Expires = TimeSpan.FromHours(24);
            response.Cookies.Add(cookie);
        }
        public DisplayModel GetCookie(string name)
        {
            HttpCookie cookie = HttpContext.Request.Cookies[name];
            DisplayModel DisplayModels = new DisplayModel();
            if (cookie != null)
            {
                string objCartListString = cookie.Value.ToString();
                //string[] objCartListStringSplit = objCartListString.Split('|');
                //foreach (string s in objCartListStringSplit)
                //{
                    string[] ss = objCartListString.Split(',');
                    if (ss[0] != "")
                    {
                        DisplayModel model = new DisplayModel()
                        {
                            DeliveryCloseMsg = ss[0],
                            DisplayMsgDeliveyClose = ss[1],
                            DisplayMsgOnHome = ss[2],
                            DisplayMsgPickupClose = ss[3],
                            DisplayMsgResClose = ss[4],
                            IsDeliveryAvailable = ss[5],
                            IsPickupAvailable = ss[6],
                            IsRestaurantOpen = ss[7],
                            PickupCloseMsg = ss[8],
                            ResCloseMsg = ss[9],
                        };

                        DisplayModels =  model;
                    }

                //}
            }else
            {
                DisplayModels = null;
            }
            return DisplayModels;
        }

    }
}