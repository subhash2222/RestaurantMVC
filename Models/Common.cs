using Newtonsoft.Json;
using RestaurantMVC.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantMVC.Models
{
    public class Common
    {
        public List<TimeList> BindDropdownlist(string Type, string dates, string Start_Time = "NA", string End_Time = "NA")
        {
            List<TimeList> TimeLists = new List<TimeList>();
            //var curr = DateTime.Now;
            var curr = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));            
            //var curr = TimeZoneInfo.ConvertTime(DateTime.Now.AddDays(-1), TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
            int hours = 0;
            int minutes = 0;
            //int Starthours = 0;
            //int Startminutes = 0;


            int Endhours = 0;
            int Endminutes = 0;
            string Day = "";

            if (Type == "D")
            {
                //hours = Convert.ToInt16(curr.ToString("HH")) + 1;
                //minutes = Convert.ToInt16(curr.ToString("mm"));                
                hours = Convert.ToInt16(curr.ToString("HH"));
                Int32 DeliveryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["DeliveryMinutes"]);
                minutes = Convert.ToInt16(curr.ToString("mm")) + DeliveryMinutes;

                if (minutes > 60)
                {
                    hours = hours + 1;
                    minutes = minutes - 60;
                }
                //Starthours = hours;
                //Startminutes = minutes;
                Day = curr.ToString("ddd");
            }
            else if (Type == "P")
            {
                hours = Convert.ToInt16(curr.ToString("HH"));
                //hours = Convert.ToInt16(18); // for test purpose
                Int32 pickuptimeval = Convert.ToInt32(ConfigurationManager.AppSettings["PickupMinutes"]);
                minutes = Convert.ToInt16(curr.ToString("mm")) + pickuptimeval;

                if (minutes > 60)
                {
                    hours = hours + 1;
                    minutes = minutes - 60;
                }
                //Starthours = hours;
                //Startminutes = minutes;
                Day = curr.ToString("ddd");

            }
            else if (Type == "o")
            {
                DateTime time = DateTime.Parse(dates);

                hours = Convert.ToInt16(time.ToString("HH")) + 9;
                minutes = Convert.ToInt16(time.ToString("mm"));

                Day = time.ToString("ddd");
            }
            else
            {
                hours = Convert.ToInt16(curr.ToString("HH"));
                minutes = Convert.ToInt16(curr.ToString("mm"));
                Day = curr.ToString("ddd");
            }


            if (Start_Time != "NA")
            {
                string[] Start_Timearry = Start_Time.ToString().Split(':');

                int hoursdb = Convert.ToInt16(Start_Timearry[0]);
                int minutesdb = Convert.ToInt16(Start_Timearry[1]);

                if (hours < hoursdb)
                {
                    hours = hoursdb;
                    minutes = minutesdb;
                }

                //hours = Convert.ToInt16(Start_Timearry[0]);
                //minutes = Convert.ToInt16(Start_Timearry[1]);
                //hours = Starthours;
                //minutes = Startminutes;
                //Starthours = hours + 1;
                //Startminutes = minutes - 60;

                string[] End_Timearry = End_Time.ToString().Split(':');

                Endhours = Convert.ToInt16(End_Timearry[0]);
                // temp
                Endhours += 1;
                Endminutes = Convert.ToInt16(End_Timearry[1]); ;

                //    if (Day == "Sun")
                //{
                //    if (hours < 16)
                //    {
                //        hours = 16;
                //        minutes = 15;

                //        Endhours = 21;
                //        Endminutes = 30;
                //    }



                //}
                //else
                //{
                //    if (hours < 16)
                //    {
                //        hours = 16;
                //        minutes = 15;

                //        Endhours = 22;
                //        Endminutes = 60;
                //    }
                //}


                if (isMultipleof5(minutes) == true)
                    System.Console.WriteLine(minutes + "is multiple of 5");
                else
                    minutes = (minutes / 5 + 1) * 5;

                for (int i = hours; i < Endhours; i++)
                //for (int i = hours; i < 21; i++)
                {
                    if (i == hours)
                    {
                        for (int k = minutes; k < 60; k = k + 5)
                        {
                            string time = string.Format("{0:00}", i) + ":" + string.Format("{0:00}", k);

                            TimeLists.Add(new TimeList() { Value = time, Text = time });

                            //if (i == 21 && k == 30)
                            if (i == Endhours && k == 0)
                            {
                                k = 60;
                            }
                        }
                    }
                    else if (i == (Endhours - 1))
                    {
                        for (int k = 0; k <= Endminutes; k = k + 5)
                        {

                            if (k == 60)
                            {
                                string time = string.Format("{0:00}", i + 1) + ":" + string.Format("{0:00}", "00");

                                TimeLists.Add(new TimeList() { Value = time, Text = time });
                            }
                            else
                            {
                                string time = string.Format("{0:00}", i) + ":" + string.Format("{0:00}", k);

                                TimeLists.Add(new TimeList() { Value = time, Text = time });
                            }


                            //if (i == 21 && k == 30)
                            if (i == Endhours && k == 0)
                            {
                                k = 60;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < 60; k = k + 5)
                        {
                            string time = string.Format("{0:00}", i) + ":" + string.Format("{0:00}", k);

                            TimeLists.Add(new TimeList() { Value = time, Text = time });

                            //if (i == 21 && k == 30)
                            if (i == Endhours && k == 0)
                            {
                                k = 60;
                            }
                        }
                    }
                }
            }
            else
            {
                string time = string.Format("{0:00}", "00") + ":" + string.Format("{0:00}", "00");
                TimeLists.Add(new TimeList() { Value = time, Text = time });
            }


            return TimeLists;
        }

        static bool isMultipleof5(int n)
        {
            if ((n & 1) == 1)
                n <<= 1;

            float x = n;
            x = ((int)(x * 0.1)) * 10;

            if ((int)x == n)
                return true;

            return false;
        }


    }
}