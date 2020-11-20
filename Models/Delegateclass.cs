using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace RestaurantMVC.Models
{
    public delegate void SendMailpickupcust(string recievermailid, string subject, CustomerinfoPickup custpickup, List<ShoppingCartModel> listshop, string Total, string path1);
    public delegate void SendMailpickupadm(string recievermailid, string subject, CustomerinfoPickup custpickup, List<ShoppingCartModel> listshop, string Total, string path1);
    public delegate void SendMaildelvcust(string recievermailid, string subject, CustomerinfoDelivery custdelv, List<ShoppingCartModel> listshop, string Total, string path1);
    public delegate void SendMaildelvadm(string recievermailid, string subject, CustomerinfoDelivery custdelv, List<ShoppingCartModel> listshop, string Total, string path1);
    public class Delegateclass
    {
        public void sendmail_Pickupcustomer(string recievermailid, string subject, CustomerinfoPickup custpickup, List<ShoppingCartModel> listshop, string Total, string path1)
        {
            try
            {
                SendMailpickupcust myAction = new SendMailpickupcust(sendmail_TemplateCustomer);
                myAction.BeginInvoke(recievermailid, subject, custpickup, listshop, Total, path1, null, null);
            }
            catch (Exception ex)
            {

            }
        }

        public void sendmail_Pickupadmin(string recievermailid, string subject, CustomerinfoPickup custpickup, List<ShoppingCartModel> listshop, string Total, string path1)
        {
            try
            {
                SendMailpickupadm myAction = new SendMailpickupadm(sendmailPickupAdmin);
                myAction.BeginInvoke(recievermailid, subject, custpickup, listshop, Total, path1, null, null);
            }
            catch (Exception ex)
            {

            }
        }

        public void sendmail_Deliverycustomer(string recievermailid, string subject, CustomerinfoDelivery custdelv, List<ShoppingCartModel> listshop, string Total, string path1)
        {
            try
            {
                SendMaildelvcust myAction = new SendMaildelvcust(sendmail_CustomerDelivery);
                myAction.BeginInvoke(recievermailid, subject, custdelv, listshop, Total, path1, null, null);
            }
            catch (Exception ex)
            {

            }
        }

        public void sendmail_Deliveryadmin(string recievermailid, string subject, CustomerinfoDelivery custdelv, List<ShoppingCartModel> listshop, string Total, string path1)
        {
            try
            {
                SendMaildelvadm myAction = new SendMaildelvadm(sendmailDeliveryAdmin);
                myAction.BeginInvoke(recievermailid, subject, custdelv, listshop, Total, path1, null, null);
            }
            catch (Exception ex)
            {

            }
        }

        private void sendmail_TemplateCustomer(string recievermailid, string subject, CustomerinfoPickup custpickup, List<ShoppingCartModel> listshop, string Total, string path1)
        {
            try
            {
                string itemhtml = MakecartItemHtml(listshop, Total);

                string Template =
                 "<div>" +
                 "<span><h2><u>Your Pickup Order is received By North Indian Restaurant</u></h2></span><br />" +
                                  "<span><h2><u>The Pickup Time is : " + custpickup.Time + "</u></h2></span><br />" +
                 "<span><h2><u>The Items that you ordered is as below</u></h2></span>" +
                 " " + itemhtml;

                bool IsAttachment = false;
                string FileName = string.Empty;
                System.Net.Mail.Attachment attachment = null;

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.To.Add(recievermailid);
                mailMessage.Bcc.Add(AppConstant.BCCEmailId);
                mailMessage.From = new MailAddress(AppConstant.SenderEmailId);

                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = Template;
                mailMessage.IsBodyHtml = true;

                mailMessage.Priority = MailPriority.High;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(AppConstant.SenderEmailId, AppConstant.SenderEmailpwd);
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;

                object userState = mailMessage;

                try
                {
                    smtpClient.Send(mailMessage);

                }
                catch (System.Net.Mail.SmtpException)
                {
                }
                this.createCsvfile(listshop, path1, "P");
            }
            catch (Exception)
            {
            }
        }

        public string MakecartItemHtml(List<ShoppingCartModel> listshop, string Total)
        {
            StringBuilder strbuild = new StringBuilder();
            string header = "<div>" +
            "<span><h2><u>Order Details</u></h2></span>";
            string TotalItems = "<span><h3>No of Items:  " + listshop.Sum(item => item.Qty).ToString() + "</h3></span>";
            string TotalAmt = "<tr>" +
                          "<td style=text-align:right colspan=3><strong> Total </strong></td> " +
                          "<td style=text-align:right><strong>" + Total + "</strong></td> " +
                          "</tr>";

            strbuild.Append(header);
            strbuild.Append(TotalItems);
            string format =
              "<table border=1 width=100% align=left cellpadding=3>" +
              "<th>Item</th>" +
              "<th>Quantity</th>" +
              "<th>Item Price</th>" +
              "<th>Total</th>";

            strbuild.Append(format);

            foreach (var item in listshop)
            {
                var itemname = item.ItemName;
                var qty = item.Qty;

                string earchrow = "<tr>" +
                  "<td style=text-align:left>" + item.ItemName + "</td> " +
                  "<td style=text-align:right>" + item.Qty + "</td> " +
                  "<td style=text-align:right>" + item.Price + "</td> " +
                  "<td style=text-align:right>" + item.ItemLinePrice + "</td> " +
                  "</tr>";
                strbuild.Append(earchrow);
            }

            strbuild.Append(TotalAmt);
            strbuild.Append("</table></div>");
            return strbuild.ToString();
        }

        public void createCsvfile(List<ShoppingCartModel> listshop, string path1, string ordertype)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!File.Exists(path1))
                {
                    string clientHeader = "ItemId" + "," + "ItemName" + "," + "Price" + "," + "ItemLinePrice" + "," + "Qty" + "OrderType";
                    sb.AppendLine(clientHeader);
                }

                for (int i = 0; i < listshop.Count; i++)
                {
                    string clientDetails = listshop[i].ItemId + "," + listshop[i].ItemName + "," + listshop[i].Price + "," + listshop[i].ItemLinePrice + "," + listshop[i].Qty + "," + ordertype;
                    sb.AppendLine(clientDetails);
                }

                File.WriteAllText(path1, sb.ToString());
            }
            catch (Exception ex)
            {

            }

        }

        public void sendmailPickupAdmin(string recievermailid, string subject, CustomerinfoPickup custpickup, List<ShoppingCartModel> listshop, string Total, string path1)
        {
            try
            {
                string Template = EmailTemplate(custpickup, listshop, Total);
                //string itemhtml = MakecartItemHtml(listshop, Total);

                bool IsAttachment = false;
                string FileName = string.Empty;
                System.Net.Mail.Attachment attachment = null;

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.To.Add(recievermailid);
                mailMessage.Bcc.Add(AppConstant.BCCEmailId);
                mailMessage.From = new MailAddress(AppConstant.SenderEmailId);

                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = Template;
                mailMessage.IsBodyHtml = true;

                mailMessage.Priority = MailPriority.High;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(AppConstant.SenderEmailId, AppConstant.SenderEmailpwd);
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;

                object userState = mailMessage;

                try
                {
                    smtpClient.Send(mailMessage);
                    //if (IsAttachment)
                    //{
                    //    attachment.ContentStream.Close();
                    //    object p = File.Delete(Server.MapPath(FileName));
                    //}
                    // return true;
                }
                catch (System.Net.Mail.SmtpException)
                {
                    // return false;
                }
                this.createCsvfile(listshop, path1, "P");
            }
            catch (Exception)
            {
                // return false;
            }
        }

        public string EmailTemplate(CustomerinfoPickup custpickup, List<ShoppingCartModel> listshop, string Total)
        {
            string itemhtml = MakecartItemHtml(listshop, Total);

            string Name = custpickup.FirstName.Trim() + " " + custpickup.LastName.Trim();
            string EmailFormat =
             "<div>" +
             "<span><h2><u>Order Type: Pickup </u></h2></span><br />" +
             "<span><h2><u>Customer Details</u></h2></span>" +
             "<table width=100% align=left cellpadding=3>" +
                          "<tr>" +
                          "<td style=width:30px><strong> Name </strong></td> " +
                          "<td style=width:70px><strong>" + Name + "</strong></td> " +
                          "</tr>" +
                          "<tr>" +
                          "<td style=width:30px><strong> Mobile </strong></td> " +
                          "<td style=width:70px><strong>" + custpickup.MobileNo + "</strong></td> " +
                          "</tr>" +
                          "<tr>" +
                          "<td style=width:30px><strong> Email Id </strong></td> " +
                          "<td style=width:70px><strong>" + custpickup.EmailId + "</strong></td> " +
                          "</tr>" +
                          "<tr>" +
                          "<td style=width:30px><strong> Estimated Pickup Time </strong></td> " +
                          "<td style=width:70px><strong>" + custpickup.Time + "</strong></td> " +
                          "</tr>" +

                     "</table>" +
             "</div><br />" + itemhtml;

            return EmailFormat;
        }

        private void sendmail_CustomerDelivery(string recievermailid, string subject, CustomerinfoDelivery custdelv, List<ShoppingCartModel> listshop, string Total, string path1)
        {
            try
            {
                string itemhtml = MakecartItemHtml(listshop, Total);
                string Name = custdelv.FirstName.Trim() + " " + custdelv.LastName.Trim();
                string Address = custdelv.AddressLine1.Trim() + ", " + custdelv.AddressLine2.Trim();
                //string Name = "Ashish";
                //string Address = "ss";
                string Template =
                 "<div>" +
                 "<span><h2><u>Order Type: Delivery </u></h2></span><br />" +
                 "<span><h3><u>Customer Details</u></h3></span>" +
                 "<table width=100% align=left cellpadding=3>" +
                              "<tr>" +
                              "<td style=width:30px><strong> Name </strong></td> " +
                              "<td style=width:70px><strong>" + Name + "</strong></td> " +
                              "</tr>" +
                              "<tr>" +
                              "<td style=width:30px><strong> Address </strong></td> " +
                              "<td style=width:70px><strong>" + Address + "</strong></td> " +
                              "</tr>" +
                              "<tr>" +
                              "<td style=width:30px><strong> Mobile </strong></td> " +
                              "<td style=width:70px><strong>" + custdelv.MobileNo + "</strong></td> " +
                              "</tr>" +
                              "<tr>" +
                              "<td style=width:30px><strong> Email Id </strong></td> " +
                              "<td style=width:70px><strong>" + custdelv.EmailId + "</strong></td> " +
                              "</tr>" +
                              "<tr>" +
                              "<td style=width:30px><strong> Estimated Delivery Time </strong></td> " +
                              "<td style=width:70px><strong>" + custdelv.Time + "</strong></td> " +
                              "</tr>" +
                         "</table><br />" +
                 "</div>" + itemhtml;

                string Template1 =
                 "<div>" +
                 "<span><h2><u>Your Pickup Order is received By North Indian Restaurant</u></h2></span><br />" +
                                  "<span><h2><u>The Pickup Time is : " + custdelv.Time + "</u></h2></span><br />" +
                 "<span><h2><u>The Items that you ordered is as below</u></h2></span>" +
                 " " + itemhtml;

                bool IsAttachment = false;
                string FileName = string.Empty;
                System.Net.Mail.Attachment attachment = null;

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.To.Add(recievermailid);
                mailMessage.Bcc.Add(AppConstant.BCCEmailId);
                mailMessage.From = new MailAddress(AppConstant.SenderEmailId);

                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = Template;
                mailMessage.IsBodyHtml = true;

                mailMessage.Priority = MailPriority.High;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(AppConstant.SenderEmailId, AppConstant.SenderEmailpwd);
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;

                object userState = mailMessage;
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (System.Net.Mail.SmtpException)
                {
                }
                this.createCsvfile(listshop, path1, "P");
            }
            catch (Exception)
            {
            }
        }

        public void sendmailDeliveryAdmin(string recievermailid, string subject, CustomerinfoDelivery custdelv, List<ShoppingCartModel> listshop, string Total, string path1)
        {
            try
            {
                string itemhtml = MakecartItemHtml(listshop, Total);
                string Template =
                 "<div>" +
                 "<span><h2><u>Your Delivery Order is received By North Indian Restaurant</u></h2></span><br />" +
                 "<span><h2><u>The Estimated Delivery Time is : " + custdelv.Time + "</u></h2></span><br />" +
                 "<span><h2><u>The Items that you ordered is as below</u></h2></span>" +
                 " " + itemhtml;

                bool IsAttachment = false;
                string FileName = string.Empty;
                System.Net.Mail.Attachment attachment = null;

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.To.Add(recievermailid);
                mailMessage.Bcc.Add(AppConstant.BCCEmailId);
                mailMessage.From = new MailAddress(AppConstant.SenderEmailId);

                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = Template;
                mailMessage.IsBodyHtml = true;

                mailMessage.Priority = MailPriority.High;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(AppConstant.SenderEmailId, AppConstant.SenderEmailpwd);
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;

                object userState = mailMessage;

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (System.Net.Mail.SmtpException)
                {
                    // return false;
                }
                this.createCsvfile(listshop, path1, "P");
            }
            catch (Exception)
            {
                // return false;
            }
        }

    }
}