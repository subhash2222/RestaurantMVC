using Newtonsoft.Json;
using RestaurantMVC.Helper;
using RestaurantMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace RestaurantMVC.Controllers
{
    public class ItemController : BaseController<ItemAdmModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<Item>> BindList()
        {
            List<Item> dtl = new List<Item>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Item/GetItemInfo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<Item>>(result);
            }
            return dtl;
        }

        public async Task BindLov()
        {
            List<LovModel> dtllov = new List<LovModel>();
            HttpClient client = detailAPI.Initial();
            //HttpResponseMessage res = await client.GetAsync("Lov/" + "ACTIVEINACTIVE");
            HttpResponseMessage res = await client.GetAsync("Lov/GetLovDtl" + "?lovcolumn=ACTIVEINACTIVE");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtllov = JsonConvert.DeserializeObject<List<LovModel>>(result);
            }
            ViewBag.statuslist = dtllov;
        }

        public async Task CategoryCombo()
        {
            List<CategoryComboModel> Catg = new List<CategoryComboModel>();
            HttpClient client = detailAPI.Initial();
            HttpResponseMessage res = await client.GetAsync("Item/GetCategoyCombo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                Catg = JsonConvert.DeserializeObject<List<CategoryComboModel>>(result);
            }
            ViewBag.Categorylist = Catg;
        }
        //public async Task SaveImage(HttpPostedFile files)
        //{
        //    if (GenId == null) ;
        //    {

        //        //GenId = "GenId\"";
        //        //char[] charsToTrim = { '\"' };
        //        //string cleanString = GenId.Trim(charsToTrim);



        //        var fileName = Path.GetFileName(files.FileName);
        //        var ext = Path.GetExtension(files.FileName);
        //        string name = Path.GetFileNameWithoutExtension(fileName);
        //        string myfile = /*name*/  cleanSoltring + "\\" + ext;
        //        var path = Path.Combine(Server.MapPath("~/ImageFolder"), myfile);

        //        //System.IO.Path.GetFileName(file.FileName);

        //        files.SaveAs(path);
        //    }

        //}
        public void ShowMessaage(string msgtype, string message)
        {
            ViewBag.Message = "";
            ViewBag.imagepath = "";
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

        #region Events

        public async Task<ActionResult> Index()
        {
            List<Item> Itemlst = await BindList();

            CommonViewModel.ListItem = Itemlst;
            return View(CommonViewModel);
        }

        public async Task<ActionResult> Create()
        {
            CommonViewModel.AddNew = "new";
            List<Item> Itemlst = await BindList();
            CommonViewModel.ListItem = Itemlst;
            CommonViewModel.item = new Item();
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            await CategoryCombo();
            await BindLov();
            //return Json(CommonViewModel);            
            return View("Index", CommonViewModel);
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase files, ItemAdmModel itmodel1)
        {
            if (ModelState.IsValid)
            {
                itmodel1.item.Operated_By = HttpContext.Session["UserId"].ToString();
                itmodel1.item.IsInserted = "I";
                CommonViewModel.item = itmodel1.item;
                var categorymdl = itmodel1.item;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(categorymdl),
      Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("Item/InsertupdateDetail", content);

                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    string GenId = strmsg[2];


                    //var GenId = Session["GenId"] as string;
                    if (GenId == null)
                    {

                        //GenId = "GenId\"";
                        //char[] charsToTrim = { '\"' };
                        //string cleanString = GenId.Trim(charsToTrim);

                        var fileName = Path.GetFileName(files.FileName);
                        var ext = Path.GetExtension(files.FileName);

                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = /*name*/  GenId.TrimEnd('\"') + ext;

                        var path = Path.Combine(Server.MapPath("~/ItemImages"), myfile);

                        //System.IO.Path.GetFileName(file.FileName);

                        files.SaveAs(path);
                        ViewBag.imagepath = path;



                    }


                    ShowMessaage(msgtype, message);
                }

            }

            await BindLov();

            await CategoryCombo();

            //await SaveImage();
            List<Item> Itemlst = await BindList();
            CommonViewModel.ListItem = Itemlst;
            CommonViewModel.AddNew = "new";
            return View("Index", CommonViewModel);
        }


        public async Task<ActionResult> Edit(string pitemid, HttpPostedFileBase files)
        {
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            CommonViewModel.Status = "edit";
            await BindLov();
            await BindList();
            await CategoryCombo();
            List<Item> lovlst = await BindList();
            CommonViewModel.ListItem = lovlst;
            Item item = new Item();
            //List<Item> catglst = await BindList();
            //CommonViewModel.ListItem = catglst;
            HttpClient client = detailAPI.Initial();

            //pitemid = pitemid + Path.GetExtension(files.FileName);
            HttpResponseMessage response = await client.GetAsync("Item/GetItemInfoById?pitemcode=" + pitemid);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                CommonViewModel.item = JsonConvert.DeserializeObject<Item>(result);

            }



            //pitemid = pitemid + ".jpg";
            //string path = Server.MapPath("~/Imagefolder/");
            //string[] imagesfiles = Directory.GetFiles(path);
            //ViewBag.imagearray = pitemid;

            //item.Image = pitemid;







            if (item.Item_ID == 0)
            {

                string path = Server.MapPath("~/Content/assets/img/TestItemImages");

                string[] imagesfiles = Directory.GetFiles(path);

                pitemid = (pitemid + ".jpg") /*+ imagesfiles*/;

                item.Image = pitemid;

                ViewBag.imagearray = pitemid;
            }
            else
            {

                ViewBag.imagearray1 = "file not found";
            }

            //ViewBag.imagearray = imagesfiles;
            //pitemid = pitemid + ".jpg";
            ////item.Image = pitemid;
            //ViewBag.imagearray = pitemid;

            //CommonViewModel.item.Image = pitemid;
            return View("Index", CommonViewModel);
        }




        [HttpPost]
        public async Task<ActionResult> Edit(HttpPostedFileBase files, ItemAdmModel catgmodel1)
        {
            if (ModelState.IsValid)
            {
                catgmodel1.item.Operated_By = HttpContext.Session["UserId"].ToString();
                catgmodel1.item.IsInserted = "U";

                CommonViewModel.item = catgmodel1.item;

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(catgmodel1.item),
      Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Item/InsertupdateDetail", content);
                //string statuscode = response.StatusCode.ToString();


                if (response.IsSuccessStatusCode)
                {
                    var contents = response.Content.ReadAsStringAsync().Result;
                    string[] strmsg = contents.Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    string GenId = strmsg[2];
                    //var GenId = Session["GenId"] as string;
                    if (GenId == null)
                    {
                        //GenId = "GenId\"";
                        //char[] charsToTrim = { '\"' };
                        //string cleanString = GenId.Trim(charsToTrim);

                        var fileName = Path.GetFileName(files.FileName);
                        var ext = Path.GetExtension(files.FileName);
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = /*name*/  GenId.TrimEnd('\"') + ext;
                        //myfile = myfile.TrimEnd.('\\') + '\\';
                        var path = Path.Combine(Server.MapPath("~/ImageFolder"), myfile);

                        //System.IO.Path.GetFileName(file.FileName);

                        files.SaveAs(path);
                    }

                    ShowMessaage(msgtype, message);
                }
            }
            CommonViewModel.Status = "edit";
            List<Item> catglst = await BindList();
            CommonViewModel.ListItem = catglst;
            await BindLov();
            await CategoryCombo();
            return View("Index", CommonViewModel);
        }



        public async Task<ActionResult> Delete(string pitemid)
        {
            //List<Item> itm = new List<Item>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Item/DeleteItem?id=" + pitemid);
            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result;
                string[] strmsg = contents.Split('|');
                string msgtype = strmsg[0];
                string message = strmsg[1];
                ShowMessaage(msgtype, message);
            }

            List<Item> catglst = await BindList();
            CommonViewModel.ListItem = catglst;

            return View("Index", CommonViewModel);
        }




        #endregion


    }
}