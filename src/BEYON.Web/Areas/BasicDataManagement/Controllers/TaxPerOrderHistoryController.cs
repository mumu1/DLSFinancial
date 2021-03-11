using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel;
using BEYON.ViewModel.Member;
using BEYON.Web.Extension.Common;
using BEYON.Web.Extension.Filters;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.App.Interface;
using System.IO;
using BEYON.CoreBLL.Service.Excel;

namespace BEYON.Web.Areas.BasicDataManagement.Controllers
{
    public class TaxPerOrderHistoryController : Controller
    {
         private readonly ITaxPerOrderHistoryService _taxPerOrderHistoryService;
         public TaxPerOrderHistoryController(ITaxPerOrderHistoryService taxPerOrderHistoryService)
        {
            this._taxPerOrderHistoryService = taxPerOrderHistoryService;
        }


        //
         // GET: /BasicDataManagement/TaxPerOrderHistory/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

        // GET: /BasicDataManagement/TaxPerOrderHistory/GetAllData/
        public ActionResult GetAllData()
        {
            //var echo = int.Parse(HttpContext.Request.Params["sEcho"]);
            //var displayLength = int.Parse(HttpContext.Request.Params["iDisplayLength"]);
            //var displayStart = int.Parse(HttpContext.Request.Params["iDisplayStart"]);
            //var sortOrder = HttpContext.Request.Params["sSortDir_0"].ToString();

            //var records = this._taxPerOrderHistoryService.TaxPerOrderHistorys.ToList();

            //var orderedResults = sortOrder == "asc"
            //                  ? records.OrderBy(o => o.Id)
            //                  : records.OrderByDescending(o => o.Id);

            //var itemsToSkip = displayStart == 0
            //                  ? 0
            //                  : displayStart + 1;

            //var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();


            //return Json(new { sEcho = echo, 
            //    recordsTotal = records.Count,
            //    recordsFiltered = records.Count,
            //    iTotalRecords = records.Count,
            //    iTotalDisplayRecords = records.Count,
            //    aaData = pagedResults
            //}, JsonRequestBehavior.AllowGet);
           
            var records = this._taxPerOrderHistoryService.TaxPerOrderHistorys.ToList();
            /*
           var result = Json(new { total = records.Count, data = records }, JsonRequestBehavior.AllowGet);
           result.MaxJsonLength = Int32.MaxValue;

           return result;
            * */
            return new JsonResult()
            {
                Data = new { total = records.Count, data = records },
                MaxJsonLength = int.MaxValue,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    
    } 
}