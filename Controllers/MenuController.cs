using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RestaurantMVC.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu

        #region Page Load
        public async Task<ActionResult> Index()
        {
            Session["selectedmenu"] = "Menu";
            ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
            HeaderController headerController = new HeaderController();
            DisplayModel dt = new DisplayModel();

            dt = await DisplayModelList1();

            BindCombo();
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
                //shoppingCartDetail.GrandCartTotal = decimal.Parse(Session["GrandTotal"].ToString());
                shoppingCartDetail.GrandCartTotal = calgranttotal;
                if (Session["CustSelectedTime"] != null)
                {
                    shoppingCartDetail.Time = Convert.ToString(Session["CustSelectedTime"]);
                }
                shoppingCartDetail.NoofItems = listshop.Sum(item => item.Qty).ToString();
                shoppingCartDetail.pagename = "M";
                ViewBag.isempty = "N";
            }
            if (shoppingCartDetail.listshoppingItems == null)
            {
                ViewBag.isempty = "Y";
            }
            shoppingCartDetail.displayModel = dt;
            return View(shoppingCartDetail);
        }
        #endregion

        #region Methods

        public async void BindCombo()
        {
            Common common = new Common();
            List<TimeList> timeListspick = new List<TimeList>();

            TimeModel timeModel = GetModules();

            if (Session["deliverytypemenu"] != null)
            {
                string Deliverytipe = Session["deliverytypemenu"].ToString();
                if (Deliverytipe == "Pickup")
                {
                    timeListspick = common.BindDropdownlist("P", "", timeModel.Start_Time, timeModel.End_Time);
                }
                else
                {
                    timeListspick = common.BindDropdownlist("D", "", timeModel.Start_Time, timeModel.End_Time);
                }
            }
            else
            {
                timeListspick = common.BindDropdownlist("P", "", timeModel.Start_Time, timeModel.End_Time);
                //timeListspick = common.BindDropdownlist("D", "", timeModel.Start_Time, timeModel.End_Time);
            }

            ViewBag.timeListspick = timeListspick;

            List<TasteType> listTasteType = new List<TasteType>();
            listTasteType.Add(new TasteType { Taste = "Mild", TasteDesc = "Mild" });
            listTasteType.Add(new TasteType { Taste = "Medium", TasteDesc = "Medium" });
            listTasteType.Add(new TasteType { Taste = "Spicy", TasteDesc = "Spicy" });

        }

        public TimeModel GetModules()
        {
            var task1 = Task.Run(async () =>
            {
                return await GetTime();
            });
            var obj1 = task1.Result;
            return obj1;
        }

        public async Task<TimeModel> GetTime()
        {

            DetailAPI detailAPI = new DetailAPI();
            TimeModel dtllov = new TimeModel();
            HttpClient client = detailAPI.Initial();

            var curr = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
            //var curr = TimeZoneInfo.ConvertTime(DateTime.Now.AddDays(2), TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
            string Day = curr.ToString("ddd");
            HttpResponseMessage res = await client.GetAsync("RestaurantTiming/GetResTiming?dayname=" + Day + "");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtllov = JsonConvert.DeserializeObject<TimeModel>(result);
            }
            return dtllov;
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

            objCartListString += string.Join(",", basketList.DeliveryCloseMsg, basketList.DisplayMsgDeliveyClose, basketList.DisplayMsgOnHome, basketList.DisplayMsgPickupClose, basketList.DisplayMsgResClose, basketList.IsDeliveryAvailable, basketList.IsPickupAvailable, basketList.IsRestaurantOpen, basketList.PickupCloseMsg, basketList.ResCloseMsg);
            //var date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            //HttpContext.Response.Cookies["DisplayModelLists"].Value = objCartListString;
            cookie.Value = objCartListString;
            Int16 CookiesExpireTime = Convert.ToInt16(ConfigurationManager.AppSettings["CookiesExpireTime"]);
            cookie.Expires = DateTime.Now.AddMinutes(CookiesExpireTime);
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
        #endregion

        #region Events
        public async Task<ActionResult> AddItem(string data)
        {
            var result = JsonConvert.DeserializeObject<ItemModel>(data);
            string price = result.itemprice;
            int qty = Convert.ToInt32(result.itemqty);
            int itemid = Convert.ToInt32(result.itemid);
            string itemname = result.menuitem;
            string types = result.types;
            string deliverytype = result.deliverytype;
            //string custselectedTime = result.custselectedTime;
            //Session["CustSelectedTime"] = custselectedTime;
            var retstr = await AddItemToCart(itemname, price, qty, itemid, types, deliverytype);
            //var retstr = await AddItemToCart(itemname, price, qty, itemid, types, deliverytype, custselectedTime);
            return retstr;
            //return Json(new { msgType = "S", message = "Item added successfully" });
        }

        public async Task<ActionResult> AddItemToCart(string itemname, string itmprice, int qty, int itemid, string types, string deliverytype)
        {
            try
            {
                string message = "";
                Session["deliverytypemenu"] = deliverytype;

                List<ShoppingCartModel> listshop = new List<ShoppingCartModel>();
                if (Session["Shoppingcart"] != null)
                {
                    listshop = (List<ShoppingCartModel>)Session["Shoppingcart"];
                }

                var Itemnames = itemname.Replace("--", "''");
                // if list contain item name then add qty
                //string itemname = lblitem1_1.Text;
                var itemexist = listshop.Exists(x => x.ItemName.Contains(Itemnames));
                //string itmprice = lblmnu1_1.Text.Replace("kr.", "");
                itmprice = itmprice.Replace("kr.", "");
                if (itemexist == false)
                {
                    listshop.Add(new ShoppingCartModel
                    {
                        ItemId = itemid,
                        ItemName = Itemnames,
                        Price = Convert.ToDecimal(itmprice, CultureInfo.InvariantCulture),
                        Qty = qty,
                        ItemLinePrice = Convert.ToDecimal(itmprice, CultureInfo.InvariantCulture) * qty
                    });
                    decimal total1 = listshop.Sum(item => item.ItemLinePrice);
                    Session["Shoppingcart"] = listshop;
                    Session["SumTotal"] = total1;
                }
                else
                {
                    var Finditem = listshop.Find(x => x.ItemName.Contains(Itemnames));
                    int oldqty = Finditem.Qty;
                    int newqty = 0;
                    if (types == "minuse")
                    {
                        newqty = oldqty - qty;
                    }
                    else
                    {
                        newqty = oldqty + qty;
                    }
                    Finditem.Qty = newqty;
                    Finditem.ItemLinePrice = Convert.ToDecimal(itmprice, CultureInfo.InvariantCulture) * newqty;

                    if (newqty <= 0)
                    {
                        listshop.Remove(Finditem);
                    }
                }
                decimal total = listshop.Sum(item => item.ItemLinePrice);

                decimal GrandTotal = total;

                //if (deliverytype == "Delivery")
                //{
                //    GrandTotal = GrandTotal + 25;
                //}
                Session["SumTotal"] = total;
                Session["GrandTotal"] = GrandTotal;
                Session["cartitemcount"] = listshop.Count;
                return Json(new { msgType = "S", message = "Item added successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> ChangeTime(string seltime)
        {
            try
            {
                Session["CustSelectedTime"] = seltime;
                return Json(new { });
            }
            catch (Exception ex)
            {
                //objlog.InsertLogEntry(ex.Message, ex.StackTrace, ex.Source, "9");
                //return null;
            }
            return Json(new { });
        }

        public async Task<ActionResult> Setdeltype(string deltype)
        {
            try
            {
                Session["deliverytypemenu"] = deltype;
                return Json(new { });
            }
            catch (Exception)
            {
            }
            return Json(new { });
        }


        public async Task<ActionResult> HeaderLoad()
        {
            return PartialView("_Header");
        }

        public async Task<ActionResult> Cart_load()
        {
            Session["selectedmenu"] = "Menu";
            Session["selectedmenu"] = "ShoppingCart";
            ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
            BindCombo();
            DisplayModel dt = new DisplayModel();
            dt = await DisplayModelList1();
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
                shoppingCartDetail.Time = Convert.ToString(Session["CustSelectedTime"]);
                shoppingCartDetail.NoofItems = listshop.Sum(item => item.Qty).ToString();
                ViewBag.isempty = "N";
            }
            if (shoppingCartDetail.listshoppingItems == null)
            {
                ViewBag.isempty = "Y";
            }

            shoppingCartDetail.displayModel = dt;

            return PartialView("_cart_partail", shoppingCartDetail);
        }


        #endregion

    }
}