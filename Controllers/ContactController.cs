using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RestaurantMVC.Controllers
{
    public class ContactController : Controller
    {
        DetailAPI detailAPI = new DetailAPI();
        // GET: Conatct
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
        public async Task<ActionResult> Index()
        {
            Session["selectedmenu"] = "Contact";
            DisplayModel dt = new DisplayModel();

            dt = await DisplayModelList1();
            if(dt.DisplayMsgResClose == "Y")
            {
                ViewBag.IsMsgDisplay = "Y";
                ViewBag.displaymsg = dt.ResCloseMsg;

            }
            else
            {
                ViewBag.IsMsgDisplay = "N";
                ViewBag.displaymsg = "";
            }
            

            return View();
        }
        //public async Task<ActionResult> Create()
        //{
        //    return View();
        //}
        [HttpPost]
        public async Task<ActionResult> Create(ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                var con = contact;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(con),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("Contact/InsertContact", content);

                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    ShowMessaage(msgtype, message);
                    //bool result1 = sendmail_Template("nidhipatel131196@gmail.com", "Test", "D",contact);
                }
            }
            return View("Index");

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
                HttpResponseMessage res = await client.GetAsync("https://restaurantapi.padhyasoft.com/api/SettingMsg/GetSettingMessages?curdate=" + date + "&langcode=" + lang);
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
        //public bool sendmail_Template(string recievermailid, string subject, string bodyText, ContactModel contact)
        //{
        //    string Template = "";
        //    try
        //    {
        //        Template = EmailTemplateDelivery(contact);
        //        bool IsAttachment = false;
        //        string FileName = string.Empty;
        //        System.Net.Mail.Attachment attachment = null;

        //        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
        //        mailMessage.To.Add(recievermailid);
        //        mailMessage.Bcc.Add(AppConstant.BCCEmailId);
        //        mailMessage.From = new MailAddress(AppConstant.SenderEmailId);
        //        mailMessage.Subject = subject;
        //        mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
        //        mailMessage.Body = Template;
        //        mailMessage.IsBodyHtml = true;
        //        mailMessage.Priority = MailPriority.High;
        //        SmtpClient smtpClient = new SmtpClient();
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = new System.Net.NetworkCredential(AppConstant.SenderEmailId, AppConstant.SenderEmailpwd);
        //        smtpClient.Port = 587;
        //        smtpClient.Host = "smtp.gmail.com";
        //        smtpClient.EnableSsl = true;
        //        object userState = mailMessage;
        //        try
        //        {
        //            smtpClient.Send(mailMessage);
        //            return true;
        //        }
        //        catch (System.Net.Mail.SmtpException)
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public string EmailTemplateDelivery(ContactModel Contact)
        //{
        //    string Name = "Ashish";
        //    string Address = "ss";
        //    string EmailFormat =
        //     "<div>" +
        //     "<span><h3><u>Contact Details</u></h3></span>" +
        //     "<table border=1 width=100% align=left cellpadding=3>" +
        //                  "<tr>" +
        //                  "<td style=width:30px><strong> Name </strong></td> " +
        //                  "<td style=width:70px><strong>" + Contact.Name + "</strong></td> " +
        //                  "</tr>" +
        //                  "<tr>" +
        //                  "<td style=width:30px><strong> Email </strong></td> " +
        //                  "<td style=width:70px><strong>" + Contact.Email + "</strong></td> " +
        //                  "</tr>" +
        //                  "<tr>" +
        //                  "<td style=width:30px><strong> Subject </strong></td> " +
        //                  "<td style=width:70px><strong>" + Contact.Subject + "</strong></td> " +
        //                  "</tr>" +
        //                  "<tr>" +
        //                  "<td style=width:30px><strong> Mobile </strong></td> " +
        //                  "<td style=width:70px><strong>" + Contact.Message + "</strong></td> " +
        //                  "</tr>" +
        //             "</table><br />" +
        //     "</div>";

        //    return EmailFormat;
        //}
    }
}