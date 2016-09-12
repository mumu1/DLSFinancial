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
    public class WageBaseTableController : Controller
    {
         private readonly ITaxBaseByMonthService _taxBaseByMonthService;
         public WageBaseTableController(ITaxBaseByMonthService taxBaseByMonthService)
        {
            this._taxBaseByMonthService = taxBaseByMonthService;
        }


        //
        // GET: /BasicDataManagement/WageBaseTable/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }
        // GET: /BasicDataManagement/WageBaseTable/GetAllData/
        public ActionResult GetAllData()
        {
            var result = this._taxBaseByMonthService.TaxBaseByMonths.ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);

        }
        // POST: /BasicDataManagement/WageBaseTable/Create/
        [HttpPost]
        public ActionResult Create()
        {
            TaxBaseByMonthVM[] datas = ClassConvert<TaxBaseByMonthVM>.Process(Request.Form);
            var result = _taxBaseByMonthService.Insert(datas[0]);
            if (result.ResultType != OperationResultType.Success)
                return Json(new { error = result.ResultType.GetDescription(), total = 1, data = this._taxBaseByMonthService.TaxBaseByMonths.ToArray() });
            else
            {
                TaxBaseByMonth[] results = this._taxBaseByMonthService.TaxBaseByMonths.ToArray();
                return Json(new { total = 1, data = new[] { results[results.Length - 1] } });
            }

        }

        // POST: /BasicDataManagement/WageBaseTable/Edit/
        [HttpPost]
        public ActionResult Edit()
        {
            TaxBaseByMonth[] datas = ClassConvert<TaxBaseByMonth>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _taxBaseByMonthService.Update(data);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/WageBaseTable/Delete/
        public ActionResult Delete()
        {
            TaxBaseByMonth[] datas = ClassConvert<TaxBaseByMonth>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _taxBaseByMonthService.Delete(data);
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/WageBaseTable/Import/
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
                ColumnMap[] columns;
                if (!ExcelService.Get(Request.Path, out columns))
                {
                    columns = null;
                }

                //实现文件导入
                var result = _taxBaseByMonthService.Import(path, columns);
                //删除临时创建文件
                System.IO.File.Delete(path);

                result.Message = result.Message ?? result.ResultType.GetDescription();

                return Json(new { erro = result.Message });
            }

            return Json(new { erro = "上传数据失败！" });
        }
    }
}
