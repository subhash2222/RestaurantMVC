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
    public class OrderDtlController : BaseController<ViewOrderModel>
    {
        DetailAPI detailAPI = new DetailAPI();
        // GET: Order_Detials

        public async Task<ActionResult> Index()
        {

            List<OrderModel> orderModel = new List<OrderModel>();
            List<LovModel> lovModel = new List<LovModel>();

            HttpClient client = detailAPI.Initial();

            string curdate = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage res = await client.GetAsync("Order/GetOrderInfo?orderdate=" + curdate + "&status=P");

            //HttpResponseMessage res = await client.GetAsync("Order/GetOrderInfo?orderdate=2020-07-08&status=P");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            }

            CommonViewModel.ListorderModelP = orderModel;

            HttpResponseMessage resS = await client.GetAsync("Lov/GetOrderStatus");
            if (resS.IsSuccessStatusCode)
            {
                var result = resS.Content.ReadAsStringAsync().Result;
                lovModel = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            CommonViewModel.ListLovModel = lovModel;
            int counts = 0;


            HttpResponseMessage resN = await client.GetAsync("Notification/GetNotificationcount?Type=O");
            if (resN.IsSuccessStatusCode)
            {
                var result = resN.Content.ReadAsStringAsync().Result;
                counts = JsonConvert.DeserializeObject<int>(result);
            }
            CommonViewModel.Notificationcount = counts;
            CommonViewModel.Status = "Pending";



            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Completed()
        {

            List<OrderModel> orderModel = new List<OrderModel>();
            List<LovModel> lovModel = new List<LovModel>();

            HttpClient client = detailAPI.Initial();

            //HttpResponseMessage res = await client.GetAsync("Order/GetOrderInfo?orderdate=2020-07-08&status=P");
            //if (res.IsSuccessStatusCode)
            //{
            //    var result = res.Content.ReadAsStringAsync().Result;
            //    orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            //}

            string curdate = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage resd = await client.GetAsync("Order/GetOrderInfo?orderdate=" + curdate + "&status=D");
            if (resd.IsSuccessStatusCode)
            {
                var result = resd.Content.ReadAsStringAsync().Result;
                orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            }

            CommonViewModel.ListorderModelP = orderModel;


            HttpResponseMessage resS = await client.GetAsync("Lov/GetOrderStatus");
            if (resS.IsSuccessStatusCode)
            {
                var result = resS.Content.ReadAsStringAsync().Result;
                lovModel = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            CommonViewModel.ListLovModel = lovModel;
            int counts = 0;


            HttpResponseMessage resN = await client.GetAsync("Notification/GetNotificationcount?Type=O");
            if (resN.IsSuccessStatusCode)
            {
                var result = resN.Content.ReadAsStringAsync().Result;
                counts = JsonConvert.DeserializeObject<int>(result);
            }
            CommonViewModel.Notificationcount = counts;
            CommonViewModel.Status = "Completed";

            //HttpResponseMessage resA = await client.GetAsync("Order/GetOrderInfo?orderdate=2020-07-08&status=A");
            //if (resA.IsSuccessStatusCode)
            //{
            //    var result = resA.Content.ReadAsStringAsync().Result;
            //    orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            //}

            //CommonViewModel.ListorderModelA = orderModel;

            //HttpResponseMessage resS = await client.GetAsync("Lov/GetOrderStatus");
            //if (resS.IsSuccessStatusCode)
            //{
            //    var result = resS.Content.ReadAsStringAsync().Result;
            //    lovModel = JsonConvert.DeserializeObject<List<LovModel>>(result);
            //}
            //CommonViewModel.ListLovModel = lovModel;
            //int counts = 0;


            //HttpResponseMessage resN = await client.GetAsync("Order/GetNotificationcount");
            //if (resN.IsSuccessStatusCode)
            //{
            //    var result = resN.Content.ReadAsStringAsync().Result;
            //    counts = JsonConvert.DeserializeObject<int>(result);
            //}
            //CommonViewModel.Notificationcount = counts;


            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> All()
        {

            List<OrderModel> orderModel = new List<OrderModel>();
            List<LovModel> lovModel = new List<LovModel>();

            HttpClient client = detailAPI.Initial();

            string curdate = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage resd = await client.GetAsync("Order/GetOrderInfo?orderdate=" + curdate + "&status=A");
            if (resd.IsSuccessStatusCode)
            {
                var result = resd.Content.ReadAsStringAsync().Result;
                orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            }

            CommonViewModel.ListorderModelP = orderModel;


            HttpResponseMessage resS = await client.GetAsync("Lov/GetOrderStatus");
            if (resS.IsSuccessStatusCode)
            {
                var result = resS.Content.ReadAsStringAsync().Result;
                lovModel = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            CommonViewModel.ListLovModel = lovModel;
            int counts = 0;


            HttpResponseMessage resN = await client.GetAsync("Notification/GetNotificationcount?Type=O");
            if (resN.IsSuccessStatusCode)
            {
                var result = resN.Content.ReadAsStringAsync().Result;
                counts = JsonConvert.DeserializeObject<int>(result);
            }
            CommonViewModel.Notificationcount = counts;
            CommonViewModel.Status = "All";

            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Inkitchen()
        {

            List<OrderModel> orderModel = new List<OrderModel>();
            List<LovModel> lovModel = new List<LovModel>();

            HttpClient client = detailAPI.Initial();


            string curdate = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage resd = await client.GetAsync("Order/GetOrderInfo?orderdate=" + curdate + "&status=I");
            if (resd.IsSuccessStatusCode)
            {
                var result = resd.Content.ReadAsStringAsync().Result;
                orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            }

            CommonViewModel.ListorderModelP = orderModel;


            HttpResponseMessage resS = await client.GetAsync("Lov/GetOrderStatus");
            if (resS.IsSuccessStatusCode)
            {
                var result = resS.Content.ReadAsStringAsync().Result;
                lovModel = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            CommonViewModel.ListLovModel = lovModel;
            int counts = 0;


            HttpResponseMessage resN = await client.GetAsync("Notification/GetNotificationcount?Type=O");
            if (resN.IsSuccessStatusCode)
            {
                var result = resN.Content.ReadAsStringAsync().Result;
                counts = JsonConvert.DeserializeObject<int>(result);
            }
            CommonViewModel.Notificationcount = counts;
            CommonViewModel.Status = "Inkitchen";

            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Ready()
        {

            List<OrderModel> orderModel = new List<OrderModel>();
            List<LovModel> lovModel = new List<LovModel>();

            HttpClient client = detailAPI.Initial();

            string curdate = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage resd = await client.GetAsync("Order/GetOrderInfo?orderdate=" + curdate + "&status=R");
            if (resd.IsSuccessStatusCode)
            {
                var result = resd.Content.ReadAsStringAsync().Result;
                orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            }

            CommonViewModel.ListorderModelP = orderModel;


            HttpResponseMessage resS = await client.GetAsync("Lov/GetOrderStatus");
            if (resS.IsSuccessStatusCode)
            {
                var result = resS.Content.ReadAsStringAsync().Result;
                lovModel = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            CommonViewModel.ListLovModel = lovModel;
            int counts = 0;


            HttpResponseMessage resN = await client.GetAsync("Notification/GetNotificationcount?Type=O");
            if (resN.IsSuccessStatusCode)
            {
                var result = resN.Content.ReadAsStringAsync().Result;
                counts = JsonConvert.DeserializeObject<int>(result);
            }
            CommonViewModel.Notificationcount = counts;
            CommonViewModel.Status = "Ready";

            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> ShowNotification()
        {

            NotificationModel notificationModel = new NotificationModel();

            notificationModel.Type = "O";
            HttpClient client = detailAPI.Initial();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(notificationModel),
            Encoding.UTF8, "application/json");

            HttpResponseMessage resn = await client.PostAsync("Notification/UpdateNotification", content);

            if (resn.IsSuccessStatusCode)
            {
                var contents = resn.Content.ReadAsStringAsync().Result.ToString();
                //string[] strmsg = contents.ToString().Split('|');
                //string msgtype = strmsg[0];
                //string message = strmsg[1];
            }

            List<OrderModel> orderModel = new List<OrderModel>();
            List<LovModel> lovModel = new List<LovModel>();

            string curdate = DateTime.Now.ToString("yyyy-MM-dd");
            HttpResponseMessage res = await client.GetAsync("Order/GetOrderInfo?orderdate=" + curdate + "&status=P");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            }

            CommonViewModel.ListorderModelP = orderModel;

            HttpResponseMessage resS = await client.GetAsync("Lov/GetOrderStatus");
            if (resS.IsSuccessStatusCode)
            {
                var result = resS.Content.ReadAsStringAsync().Result;
                lovModel = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            CommonViewModel.ListLovModel = lovModel;
            int counts = 0;


            HttpResponseMessage resN = await client.GetAsync("Notification/GetNotificationcount?Type=O");
            if (resN.IsSuccessStatusCode)
            {
                var result = resN.Content.ReadAsStringAsync().Result;
                counts = JsonConvert.DeserializeObject<int>(result);
            }
            CommonViewModel.Notificationcount = counts;
            CommonViewModel.Status = "Pending";



            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Submitorderstatus(string orderids, string status, string tabstatus)
        {
            OrderModel orderModel = new OrderModel();
            orderModel.Order_Id = Convert.ToInt32(orderids);
            orderModel.Status = status;
            //var listtownship = new List<OrderModel>();
            //listtownship.Add(new OrderModel() { Order_Id = Convert.ToInt32(orderids), Status = status});

            HttpClient client = detailAPI.Initial();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(orderModel),
            Encoding.UTF8, "application/json");

            HttpResponseMessage res = await client.PostAsync("Order/UpdateOrderStatus", content);

            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result.ToString();
                //string[] strmsg = contents.ToString().Split('|');
                //string msgtype = strmsg[0];
                //string message = strmsg[1];
            }

            string st = "";
            if (tabstatus == "Pending")
            {
                st = "Index";
            }
            else
            {
                st = tabstatus;
            }

            CommonViewModel.Status = st;

            return Json(CommonViewModel);
        }

        public async Task<ActionResult> Getitemlist(string orderid)
        {

            List<OrderItemModel> orderItemModel = new List<OrderItemModel>();

            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Order/GetOrderItem?orderid=" + orderid + "");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                orderItemModel = JsonConvert.DeserializeObject<List<OrderItemModel>>(result);
            }

            CommonViewModel.ListOrderItemModel = orderItemModel;

            return Json(CommonViewModel);
        }

        public async Task<ActionResult> Getnotificationcount()
        {

            int counts = 0;
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage resN = await client.GetAsync("Notification/GetNotificationcount?Type=O");
            if (resN.IsSuccessStatusCode)
            {
                var result = resN.Content.ReadAsStringAsync().Result;
                counts = JsonConvert.DeserializeObject<int>(result);
            }
            CommonViewModel.Notificationcount = counts;

            return Json(CommonViewModel);
        }
    }
}