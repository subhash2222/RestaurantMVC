using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantMVC.Models
{
    public class RestaurantSetting
    {
        public int Setting_ID { get; set; }
        public DateTime Res_Close_Start_Date { get; set; }
        public DateTime Res_Close_End_Date { get; set; }
        public string Res_Close_Message { get; set; }
        public string Res_Close_Message_Dan { get; set; }
        public int Res_Msg_No_of_Days { get; set; }
        public DateTime Pickup_Close_Start_Date { get; set; }
        public DateTime Pickup_Close_End_Date { get; set; }
        public string Pickup_Close_Message { get; set; }
        public string Pickup_Close_Message_Dan { get; set; }
        public int Pickup_Msg_No_of_Days { get; set; }
        public DateTime Delv_Close_Start_Date { get; set; }
        public DateTime Delv_Close_End_Date { get; set; }
        public string Delv_Close_Message { get; set; }
        public string Delv_Close_Message_Dan { get; set; }
        public int Delv_Msg_No_of_Days { get; set; }
        public string WeeklyOffDay { get; set; }
        public string WeeklyOffMsg { get; set; }
        public string WeeklyOffMsgDan { get; set; }
        public string Status { get; set; }
        public string OpratedBy { get; set; }
    }
}