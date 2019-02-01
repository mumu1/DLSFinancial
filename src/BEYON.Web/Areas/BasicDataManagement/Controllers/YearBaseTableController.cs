using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
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
    public class YearBaseTableController : Controller
    {
         private readonly ITaxBaseEveryMonthService _taxBaseEveryMonthService;
         private readonly ITaxPerOrderHistoryService _taxPerOrderHistoryService;
         private readonly ITaxPerOrderService _taxPerOrderService;
        

         public YearBaseTableController(ITaxBaseEveryMonthService taxBaseEveryMonthService, ITaxPerOrderHistoryService taxPerOrderHistoryService, ITaxPerOrderService taxPerOrderService)
        {
            this._taxBaseEveryMonthService = taxBaseEveryMonthService;
            this._taxPerOrderHistoryService = taxPerOrderHistoryService;
            this._taxPerOrderService = taxPerOrderService;
            
        }


        //
        // GET: /BasicDataManagement/YearBaseTable/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }
        // GET: /BasicDataManagement/YearBaseTable/GetAllData/
        public ActionResult GetAllData()
        {
            var result = this._taxBaseEveryMonthService.TaxBaseEveryMonths.ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);

        }



        // POST: /BasicDataManagement/YearBaseTable/Create/
        [HttpPost]
        public ActionResult Create()
        {
            TaxBaseEveryMonthVM[] datas = ClassConvert<TaxBaseEveryMonthVM>.Process(Request.Form);
            var result = _taxBaseEveryMonthService.Insert(datas[0]);
            if (result.ResultType != OperationResultType.Success)
                return Json(new { error = result.ResultType.GetDescription(), total = 1, data = this._taxBaseEveryMonthService.TaxBaseEveryMonths.ToArray() });
            else
            {
                TaxBaseEveryMonth[] results = this._taxBaseEveryMonthService.TaxBaseEveryMonths.ToArray();
                return Json(new { total = 1, data = new[] { results[results.Length - 1] } });
            }

        }


        // POST: /BasicDataManagement/YearBaseTable/Edit/
        [HttpPost]
        public ActionResult Edit()
        {
            TaxBaseEveryMonth[] datas = ClassConvert<TaxBaseEveryMonth>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _taxBaseEveryMonthService.Update(data);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/YearBaseTable/Delete/
        public ActionResult Delete()
        {
            TaxBaseEveryMonth[] datas = ClassConvert<TaxBaseEveryMonth>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _taxBaseEveryMonthService.Delete(data);
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/YearBaseTable/DeleteAll/
        public ActionResult DeleteAll()
        {

            var result = _taxBaseEveryMonthService.DeleteAll();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription()});
                }
                return Json( JsonRequestBehavior.AllowGet);
           
        }


        // POST: /BasicDataManagement/YearBaseTable/Import/
        [HttpPost]
        public ActionResult Import(System.Web.HttpPostedFileBase upload)
        {
            string fileName = "";
            String filePath = Server.MapPath("~/Imports");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string tempName = DateTime.Now.ToString("yyyyMMddHHMMss") + DateTime.Now.Millisecond;
            if (upload.ContentLength > 0)
            {
                fileName = tempName + Path.GetFileName(upload.FileName);
                var path = Path.Combine(filePath, fileName);
                upload.SaveAs(path);

                //获取映射文件
                ImportData importData;
                if (!ExcelService.Get(Request.Path, out importData))
                {
                    importData = null;
                }

                //实现文件导入
                var result = _taxBaseEveryMonthService.Import(path, importData);
                //删除临时创建文件
                System.IO.File.Delete(path);

                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.LogMessage });
                }
                else
                {
                    return Json(new { });
                }
            }

            return Json(new { erro = "上传数据失败！" });
        }
    }
}
