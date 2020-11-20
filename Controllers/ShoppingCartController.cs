using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RestaurantMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        DetailAPI detailAPI = new DetailAPI();
        private decimal dlvcharge = 25;
        public async Task<ActionResult> Index()
        {            
            Session["selectedmenu"] = "ShoppingCart";
            ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
            BindCombo();

            DisplayModel dt = new DisplayModel();

            dt = await DisplayModelList1();
            shoppingCartDetail.displayModel = dt;
            if (Session["Shoppingcart"] != null)
            {
                var calgranttotal = decimal.Parse(Session["GrandTotal"].ToString());
                if (Session["deliverytypemenu"] != null)
                {
                    if (Convert.ToString(Session["deliverytypemenu"]) == "Delivery")
                    {
                        calgranttotal += 25;
                    }
                }

                var listshop = (List<ShoppingCartModel>)Session["Shoppingcart"];
                shoppingCartDetail.listshoppingItems = listshop;
                shoppingCartDetail.CartTotal = decimal.Parse(Session["SumTotal"].ToString());
                shoppingCartDetail.GrandCartTotal = calgranttotal;
                //shoppingCartDetail.GrandCartTotal = decimal.Parse(Session["GrandTotal"].ToString());
                if (Session["CustSelectedTime"] != null)
                {
                    shoppingCartDetail.Time = Convert.ToString(Session["CustSelectedTime"]);
                }                
                shoppingCartDetail.NoofItems = listshop.Sum(item => item.Qty).ToString();
                shoppingCartDetail.pagename = "S";
                ViewBag.isempty = "N";
            }
            if (shoppingCartDetail.listshoppingItems == null)
            {
                ViewBag.isempty = "Y";
            }

           
            return View(shoppingCartDetail);
        }

        //public async void BindCombo()
        //{
        //    Common common = new Common();
        //    List<TimeList> timeListsdelivery = common.BindDropdownlist("D", "");
        //    List<TimeList> timeListspick = common.BindDropdownlist("P", "");
        //    ViewBag.timeListsdelivery = timeListsdelivery;
        //    ViewBag.timeListspick = timeListspick;
        //}

        public async void BindCombo()
        {
            Common common = new Common();

            MenuController MenuControllers = new MenuController();
            TimeModel timeModel = MenuControllers.GetModules();
            List<TimeList> timeListsdelivery = common.BindDropdownlist("D", "", timeModel.Start_Time, timeModel.End_Time);
            List<TimeList> timeListspick = common.BindDropdownlist("P", "", timeModel.Start_Time, timeModel.End_Time);
            ViewBag.timeListsdelivery = timeListsdelivery;
            ViewBag.timeListspick = timeListspick;
        }

        public async Task<DisplayModel> DisplayModelList1()
        {

            var date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            string lang = "E";
            HttpCookie cookie = HttpContext.Request.Cookies["Home"];
            if (cookie != null && cookie.Value != null)
            {
                if (cookie.Value == "en-Us")
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
            HttpCookie cookie = new HttpCookie("DisplayModelLists");
            string objCartListString = "";
            Int16 CookiesExpireTime = Convert.ToInt16(ConfigurationManager.AppSettings["CookiesExpireTime"]);
            objCartListString += string.Join(",", basketList.DeliveryCloseMsg, basketList.DisplayMsgDeliveyClose, basketList.DisplayMsgOnHome, basketList.DisplayMsgPickupClose, basketList.DisplayMsgResClose, basketList.IsDeliveryAvailable, basketList.IsPickupAvailable, basketList.IsRestaurantOpen, basketList.PickupCloseMsg, basketList.ResCloseMsg);
            //var date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            //HttpContext.Response.Cookies["DisplayModelLists"].Value = objCartListString;
            cookie.Value = objCartListString;
            cookie.Expires = DateTime.Now.AddHours(CookiesExpireTime);
            HttpContext.Response.Cookies.Add(cookie);
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

                    DisplayModels = model;
                }

                //}
            }
            else
            {
                DisplayModels = null;
            }
            return DisplayModels;
        }

        public async Task<ActionResult> ModifyShoppingcart(string data)
        {
            var result = JsonConvert.DeserializeObject<ShoppingCartModel>(data);

            ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
            var listshop = (List<ShoppingCartModel>)Session["Shoppingcart"];

            var Finditem = listshop.Find(x => x.ItemName.Contains(result.ItemName));
            int oldqty = Finditem.Qty;
            int newqty = Convert.ToInt32(result.Qty);

            string deliverytype = "Pickup";

            if (Session["deliverytype"] != null)
            {
                deliverytype = Session["deliverytype"].ToString();
            }

            Finditem.Qty = newqty;
            decimal dd = Convert.ToDecimal(result.Price);
            Finditem.ItemLinePrice = Finditem.Price * newqty;
            //Decimal dd = decimal.Parse(result.Price);
            //Finditem.ItemLinePrice = Convert.ToDecimal(result.Price, CultureInfo.InvariantCulture) * newqty;

            shoppingCartDetail.listshoppingItems = listshop;
            decimal total = listshop.Sum(item => item.ItemLinePrice);

            //decimal GrandTotal = total;
            //if (deliverytype == "Delivery")
            //{
            //    GrandTotal = GrandTotal + dlvcharge;
            //}

            Session["SumTotal"] = total;
            Session["GrandTotal"] = total;
            //Session["GrandTotal"] = GrandTotal;
            shoppingCartDetail.CartTotal = total;
            shoppingCartDetail.GrandCartTotal = total;
            shoppingCartDetail.NoofItems = listshop.Sum(item => item.Qty).ToString();
            ViewBag.isempty = "N";

            //return PartialView("Index", shoppingCartDetail);

            //return View("Index", shoppingCartDetail);

            return PartialView("_Shoppingcart", shoppingCartDetail);

            //shoppingCartDetail.listshoppingItems = myList;
            //shoppingCartDetail.CartTotal = Convert.ToDecimal(Session["SumTotal"].ToString());
            //ViewBag.isempty = "N";

            //ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
            //shoppingCartDetail.listshoppingItems = 
            //List<ShoppingCartModel> listshop = new List<ShoppingCartModel>();
            //listshop = (List<ShoppingCartModel>)Session["Shoppingcart"];
            //return Json("Hi");
        }

        public async Task<ActionResult> DeleteItem(string pitemname)
        {
            //var result = JsonConvert.DeserializeObject<ShoppingCartModel>(data);
            ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
            var listshop = (List<ShoppingCartModel>)Session["Shoppingcart"];

            var Finditem = listshop.Find(x => x.ItemName.Contains(pitemname));
            listshop.Remove(Finditem);
            //shoppingCartDetail.listshoppingItems.Remove(Finditem);
            //listshop = shoppingCartDetail.listshoppingItems;
            shoppingCartDetail.listshoppingItems = listshop;
            decimal total = listshop.Sum(item => item.ItemLinePrice);

            decimal GrandTotal = decimal.Parse(Session["SumTotal"].ToString());
            string deliverytype = "Pickup";

            if (Session["deliverytype"] != null)
            {
                deliverytype = Session["deliverytype"].ToString();
            }
            if (deliverytype == "Delivery")
            {
                GrandTotal = GrandTotal + 25;
            }
            Session["GrandTotal"] = GrandTotal;

            Session["SumTotal"] = total;
            Session["cartitemcount"] = listshop.Count;
            shoppingCartDetail.CartTotal = total;
            shoppingCartDetail.GrandCartTotal = GrandTotal;
            shoppingCartDetail.NoofItems = listshop.Sum(item => item.Qty).ToString();
            ViewBag.isempty = "N";
            await HeaderLoad();
            BindCombo();
            DisplayModel dt = new DisplayModel();
            dt = await DisplayModelList1();
            shoppingCartDetail.displayModel = dt;
            return View("Index", shoppingCartDetail);
            //return PartialView("_Shoppingcart", shoppingCartDetail);
        }

        public async Task<ActionResult> HeaderLoad()
        {
            return PartialView("_Header");
        }

        [HttpPost]
        public async Task<ActionResult> SaveDelivery(string data)
        {
            try
            {
                //string Total = Session["SumTotal"].ToString();
                string Total = Session["GrandTotal"].ToString();
                var Custdelnmodel = JsonConvert.DeserializeObject<CustomerinfoDelivery>(data);

                OrderEntry orderent = new OrderEntry();
                List<ShoppingCartModel> listshop;
                listshop = (List<ShoppingCartModel>)Session["Shoppingcart"];
                orderent.listshoppingItems = listshop;
                orderent.customerinfoDelivery = Custdelnmodel;
                orderent.OrderType = "D";
                orderent.EstimatedTime = Custdelnmodel.Time;
                orderent.Comments = Custdelnmodel.Comments;
                orderent.DeliveryCharge = dlvcharge;
                //orderent.OrderAmt = decimal.Parse(Session["SumTotal"].ToString());
                orderent.OrderAmt = decimal.Parse(Session["GrandTotal"].ToString());
                if (Custdelnmodel.Paymentmode.Equals("COD"))
                {
                    orderent.Paymentmode = "C";

                    HttpClient client = detailAPI.Initial();
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(orderent),
          Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await client.PostAsync("ShoppingCart/InsertCartItem", content);
                    if (res.IsSuccessStatusCode)
                    {
                        Session["cartitemcount"] = null;
                        Session["Shoppingcart"] = null;
                        await HeaderLoad();
                    }
                    return Json(new { msgType = "S", price = Total });
                }
                else
                {
                    // Store the Values in the session
                    orderent.Paymentmode = "O";
                    Session["OrderInfo"] = orderent;
                    var OrderAmt = decimal.Parse(Session["GrandTotal"].ToString());
                    string key1 = DateTime.Now.ToString("hh.mm.ss.ffffff");
                    return Json(new { msgType = "O", price = OrderAmt, orderno = key1 });
                }
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Savepickup(string data)
        {
            try
            {   
                //string Total = Session["SumTotal"].ToString();
                string Total = Session["GrandTotal"].ToString();

                //var Custdelnmodel = JsonConvert.DeserializeObject<CustomerinfoDelivery>(data);
                var Custpkpmodel = JsonConvert.DeserializeObject<CustomerinfoPickup>(data);

                OrderEntry orderent = new OrderEntry();
                List<ShoppingCartModel> listshop;
                listshop = (List<ShoppingCartModel>)Session["Shoppingcart"];
                orderent.listshoppingItems = listshop;
                orderent.customerinfoPickup = Custpkpmodel;
                orderent.OrderType = "P";
                //orderent.OrderAmt = decimal.Parse(Session["SumTotal"].ToString());
                orderent.OrderAmt = decimal.Parse(Session["GrandTotal"].ToString());
                //orderent.EstimatedTime = Custpkpmodel.Time;
                orderent.EstimatedTime = Custpkpmodel.Time;
                orderent.Comments = Custpkpmodel.Comments;

                if (Custpkpmodel.Paymentmode.Equals("COD"))
                {
                    orderent.Paymentmode = "C";
                    HttpClient client = detailAPI.Initial();
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(orderent),
          Encoding.UTF8, "application/json");

                    HttpResponseMessage res = await client.PostAsync("ShoppingCart/InsertCartItem", content);

                    if (res.IsSuccessStatusCode)
                    {
                        Session["cartitemcount"] = null;
                        Session["Shoppingcart"] = null;
                        await HeaderLoad();
                    }

                    //return Json(new { name = "Hi" });
                    //return Json(new { msgType = "S", price = Total, orderno = "1111", customername = "testcustomer" });
                    return Json(new { msgType = "S", price = Total });
                }
                else
                {
                    // Store the Values in the session
                    orderent.Paymentmode = "O";
                    Session["OrderInfo"] = orderent;

                    var OrderAmt = decimal.Parse(Session["GrandTotal"].ToString());
                    //return View("payment_page");
                    //return RedirectToAction("Action", new { id = 99 });
                    string key1 = DateTime.Now.ToString("hh.mm.ss.ffffff");
                    return Json(new { msgType = "O", price = OrderAmt, orderno = key1 });
                    //return RedirectToAction("payment_page", "ShoppingCart", new { price = OrderAmt });
                }
                //return Json(new { msgType = "E" });
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public async Task<ActionResult> Payment_confirmation()
        {
            string orderno = Request.QueryString["invoice"];

            // Save the order detail in database

            if (Session["OrderInfo"] != null)
            {
                OrderEntry orderent = (OrderEntry)Session["OrderInfo"];

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(orderent),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("ShoppingCart/InsertCartItem", content);

                if (res.IsSuccessStatusCode)
                {
                    Session["cartitemcount"] = null;
                    Session["Shoppingcart"] = null;
                    await HeaderLoad();
                    Session["OrderInfo"] = null;
                }
            }
            //Session["cartitemcount"] = null;
            //Session["Shoppingcart"] = null;
            //await HeaderLoad();
            return RedirectToAction("Payment_confirmation_succsess");
        }

        public async Task<ActionResult> Payment_confirmation_succsess()
        {
            return View();
        }

        public async Task<ActionResult> Payment_Cancel()
        {
            return View();
        }

        //public async Task<ActionResult> payment_page(string price, string orderno, string customername)
        public async Task<ActionResult> payment_page(string price, string orderno)
        {
            ViewBag.total = price;
            ViewBag.orderno = orderno;
            //ViewBag.customername = customername;
            return View();
        }

        public async Task<ActionResult> ModifyShoppingcart_partial(string deliverytype)
        {
            Session["selectedmenu"] = "ShoppingCart";
            Session["deliverytype"] = deliverytype;
            ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
            BindCombo();
            if (Session["Shoppingcart"] != null)
            {
                var listshop = (List<ShoppingCartModel>)Session["Shoppingcart"];
                shoppingCartDetail.listshoppingItems = listshop;
                shoppingCartDetail.CartTotal = decimal.Parse(Session["SumTotal"].ToString());

                //decimal GrandTotal = decimal.Parse(Session["SumTotal"].ToString());
                //if (deliverytype == "Delivery")
                //{
                //    GrandTotal = GrandTotal + dlvcharge;
                //}
                Session["GrandTotal"] = decimal.Parse(Session["SumTotal"].ToString());

                shoppingCartDetail.GrandCartTotal = decimal.Parse(Session["GrandTotal"].ToString());
                //Decimal dd = decimal.Parse(Session["SumTotal"].ToString());
                //shoppingCartDetail.CartTotal = Convert.ToDecimal(Session["SumTotal"].ToString(), CultureInfo.InvariantCulture);
                shoppingCartDetail.NoofItems = listshop.Sum(item => item.Qty).ToString();
                ViewBag.isempty = "N";

            }
            if (shoppingCartDetail.listshoppingItems == null)
            {
                ViewBag.isempty = "Y";
            }
            return PartialView("_Shoppingcart", shoppingCartDetail);
        }

    }
}