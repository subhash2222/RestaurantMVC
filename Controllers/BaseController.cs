using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestaurantMVC.Models;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace RestaurantMVC.Controllers
{
    public class BaseController<T> : Controller
    {
        public dynamic CommonViewModel = default(T);
        string controllerName = string.Empty;
        private string UserID = "";
        //private IHttpContextAccessor _accessor;
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    try
        //    {
        //        CommonViewModel.Delete = "DisplayNone";
        //        CommonViewModel.Insert = "DisplayNone";
        //        CommonViewModel.Edit = "DisplayNone";
        //        CommonViewModel.Select = "DisplayNone";

        //        //UserID = context.HttpContext.Session.GetString("UserId");
        //        UserID = "Ashish";
        //        if (UserID == "")
        //        {
        //            HttpContext.Session.Clear();
        //            context.Result = new RedirectResult("/Home/Index");
        //            return;
        //        }
        //        else if (UserID != "")// && controllerName!="")
        //        {
        //            //Check Authorization Insert Update Delete if required
        //            //var AuthorizedData = new CommonService().GetModulesMenus(UserID).Where(x => x.MenuId == controllerName).FirstOrDefault();
        //            //if (AuthorizedData != null)
        //            //{
        //            //    CommonViewModel.Delete = AuthorizedData.PrivDelete != "N" ? "DisplayBlock" : "DisplayNone";
        //            //    CommonViewModel.Insert = AuthorizedData.PrivInsert != "N" ? "DisplayBlock" : "DisplayNone";
        //            //    CommonViewModel.Edit = AuthorizedData.PrivUpdate != "N" ? "DisplayBlock" : "DisplayNone";
        //            //    CommonViewModel.Select = AuthorizedData.PrivSelect != "N" ? "DisplayBlock" : "DisplayNone";
        //            //    CommonViewModel.SelectedMenu = AuthorizedData.MenuId;
        //            //    CommonViewModel.MenuName = AuthorizedData.MenuName;
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UserID = "";
        //    }
        //    // Do something before the action executes.
        //}

        public BaseController()
        {
            controllerName = this.GetType().Name.Replace("Controller", "").Trim();
            CommonViewModel = (dynamic)Activator.CreateInstance(typeof(T));
        }

        //public BaseController(IHttpContextAccessor accessor)
        //{
        //    //this._accessor = accessor;
        //    controllerName = this.GetType().Name.Replace("Controller", "").Trim();
        //}
        //public void IntializeSessoin(string ModuleID, float EMPID, float UnitCode, string EVENT, string ProjectId)
        public void IntializeSessoin(string puserid, string pusername)
        {
            //HttpContext.Session.SetString("UserId", JsonConvert.SerializeObject(puserid));
            //HttpContext.Session.SetString("UserName", pusername);

            //TopMenuModel topMenuModel = new TopMenuModel
            //{
            //    UserName = pusername
            //};
            //UserModuleUnitModel UserModuleUnitModel1 = new UserModuleUnitModel
            //{
            //    ModuleID = ModuleID,
            //    EMPID = EMPID,
            //    UnitCode = UnitCode,
            //    EVENT = EVENT,
            //    ProjectId = ProjectId
            //};

            //var Pcglobalvalues = accountService.Pcglobalvalues(UserModuleUnitModel1);
            //if (_accessor == null)
            //{
            //    HttpContext.Session.SetInt32("EmpID", Convert.ToInt32(EMPID));
            //    HttpContext.Session.SetString("ModuleID", ModuleID);
            //    HttpContext.Session.SetString("UnitCode", Pcglobalvalues.UnitCode.ToString());
            //    HttpContext.Session.SetString("UnitDescription", Pcglobalvalues.UnitDescription.ToString());
            //    HttpContext.Session.SetString("UnitType", Pcglobalvalues.UnitType.ToString());
            //    HttpContext.Session.SetString("ProcessUnitCode", Pcglobalvalues.ProcessUnitCode.ToString());
            //    HttpContext.Session.SetString("OrcUnitcode", Pcglobalvalues.OrcUnitcode.ToString());
            //    HttpContext.Session.SetString("AreaUnitCode", Pcglobalvalues.AreaUnitCode.ToString());
            //    HttpContext.Session.SetString("EmployeeName", Pcglobalvalues.EmployeeName.ToString());
            //    HttpContext.Session.SetString("WorkUnit", Pcglobalvalues.WorkUnit.ToString());
            //    HttpContext.Session.SetString("AllDeptAccess", Pcglobalvalues.AllDeptAccess.ToString());
            //    HttpContext.Session.SetString("AllSecAccess", Pcglobalvalues.AllSecAccess.ToString());
            //    HttpContext.Session.SetString("HierYn", Pcglobalvalues.HierYn.ToString());
            //    HttpContext.Session.SetString("ModuleName", Pcglobalvalues.ModuleName.ToString());
            //    HttpContext.Session.SetString("StatusCode", Pcglobalvalues.StatusCode.ToString());
            //    HttpContext.Session.SetString("ErrorCode", Pcglobalvalues.ErrorCode.ToString());
            //    HttpContext.Session.SetString("ErrorMessage", Pcglobalvalues.ErrorMessage.ToString());
            //}
            //else
            //{
            //    _accessor.HttpContext.Session.SetInt32("EmpID", Convert.ToInt32(EMPID));
            //    _accessor.HttpContext.Session.SetString("ModuleID", ModuleID);
            //    _accessor.HttpContext.Session.SetString("UnitCode", Pcglobalvalues.UnitCode.ToString());
            //    _accessor.HttpContext.Session.SetString("UnitDescription", Pcglobalvalues.UnitDescription.ToString());
            //    _accessor.HttpContext.Session.SetString("UnitType", Pcglobalvalues.UnitType.ToString());
            //    _accessor.HttpContext.Session.SetString("ProcessUnitCode", Pcglobalvalues.ProcessUnitCode.ToString());
            //    _accessor.HttpContext.Session.SetString("OrcUnitcode", Pcglobalvalues.OrcUnitcode.ToString());
            //    _accessor.HttpContext.Session.SetString("AreaUnitCode", Pcglobalvalues.AreaUnitCode.ToString());
            //    _accessor.HttpContext.Session.SetString("EmployeeName", Pcglobalvalues.EmployeeName.ToString());
            //    _accessor.HttpContext.Session.SetString("WorkUnit", Pcglobalvalues.WorkUnit.ToString());
            //    _accessor.HttpContext.Session.SetString("AllDeptAccess", Pcglobalvalues.AllDeptAccess.ToString());
            //    _accessor.HttpContext.Session.SetString("AllSecAccess", Pcglobalvalues.AllSecAccess.ToString());
            //    _accessor.HttpContext.Session.SetString("HierYn", Pcglobalvalues.HierYn.ToString());
            //    _accessor.HttpContext.Session.SetString("ModuleName", Pcglobalvalues.ModuleName.ToString());
            //    _accessor.HttpContext.Session.SetString("StatusCode", Pcglobalvalues.StatusCode.ToString());
            //    _accessor.HttpContext.Session.SetString("ErrorCode", Pcglobalvalues.ErrorCode.ToString());
            //    _accessor.HttpContext.Session.SetString("ErrorMessage", Pcglobalvalues.ErrorMessage.ToString());
            //}
        }
    }
}