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
    public class CategoryController : BaseController<CategoryModel>
    {
        DetailAPI detailAPI = new DetailAPI();

        public async Task<List<Category>> BindList()
        {
            List<Category> dtl = new List<Category>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Category/GetCatgInfo");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dtl = JsonConvert.DeserializeObject<List<Category>>(result);
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

        #region Events

        public async Task<ActionResult> Index()
        {
            List<Category> catglst = await BindList();

            CommonViewModel.ListCategory = catglst;
            return View(CommonViewModel);
        }

        public async Task<ActionResult> Create()
        {
            CommonViewModel.AddNew = "new";
            List<Category> catglst = await BindList();
            CommonViewModel.ListCategory = catglst;
            CommonViewModel.category = new Category();
            ViewBag.Message = "";
            ViewBag.MessageErr = "";
            await BindLov();
            //return Json(CommonViewModel);            
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryModel catgmodel1)
        {
            if (ModelState.IsValid)
            {
                catgmodel1.category.Operated_By = HttpContext.Session["UserId"].ToString();
                catgmodel1.category.IsInserted = "I";
                CommonViewModel.category = catgmodel1.category;
                var categorymdl = catgmodel1.category;
                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(categorymdl),
      Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PostAsync("Category/InsertupdateCategory", content);
                if (res.IsSuccessStatusCode)
                {
                    var contents = res.Content.ReadAsStringAsync().Result.ToString();
                    string[] strmsg = contents.ToString().Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    ShowMessaage(msgtype, message);
                }
            }
            await BindLov();
            List<Category> catglst = await BindList();
            CommonViewModel.ListCategory = catglst;
            CommonViewModel.AddNew = "new";
            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Edit(string pcatgid)
        {
            ViewBag.Message = "";
            ViewBag.MessageErr = "";            
            CommonViewModel.Status = "edit";
            await BindLov();
            List<Category> catglst = await BindList();
            CommonViewModel.ListCategory = catglst;
            HttpClient client = detailAPI.Initial();
            HttpResponseMessage response = await client.GetAsync("Category/GetCatgInfoById?id=" + pcatgid);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                CommonViewModel.category = JsonConvert.DeserializeObject<Category>(result);
            }
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CategoryModel catgmodel1)
        {
            if (ModelState.IsValid)
            {
                catgmodel1.category.Operated_By = HttpContext.Session["UserId"].ToString();
                catgmodel1.category.IsInserted = "U";

                CommonViewModel.category = catgmodel1.category;

                HttpClient client = detailAPI.Initial();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(catgmodel1.category),
      Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Category/InsertupdateCategory", content);
                //string statuscode = response.StatusCode.ToString();

                if (response.IsSuccessStatusCode)
                {
                    var contents = response.Content.ReadAsStringAsync().Result;
                    string[] strmsg = contents.Split('|');
                    string msgtype = strmsg[0];
                    string message = strmsg[1];
                    ShowMessaage(msgtype, message);
                }
            }
            CommonViewModel.Status = "edit";
            List<Category> catglst = await BindList();
            CommonViewModel.ListCategory = catglst;
            await BindLov();
            return View("Index", CommonViewModel);
        }

        public async Task<ActionResult> Delete(string pcatgid)
        {
            //List<Item> itm = new List<Item>();
            HttpClient client = detailAPI.Initial();

            HttpResponseMessage res = await client.GetAsync("Category/DeleteCategory?id=" + pcatgid);
            if (res.IsSuccessStatusCode)
            {
                var contents = res.Content.ReadAsStringAsync().Result;
                string[] strmsg = contents.Split('|');
                string msgtype = strmsg[0];
                string message = strmsg[1];
                ShowMessaage(msgtype, message);
            }

            List<Category> catglst = await BindList();
            CommonViewModel.ListCategory = catglst;

            return View("Index", CommonViewModel);
        }
        #endregion


    }
}