using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Net.Http.Formatting;
using System.IO;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.Domain.Model.App;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.CoreBLL.Service.Excel;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Web.Extension.Common;
using BEYON.Web.Extension.Filters;

namespace BEYON.Web.Areas.BasicDataManagement.Controllers
{
    public class AuditOptionController : Controller
    {

        private readonly IRefundTypeService _refundTypeService;

        public AuditOptionController(IRefundTypeService refundTypeService)
        {
            this._refundTypeService = refundTypeService;
        }

        //
        // GET: /BasicDataManagement/Reimbursement/
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

        // GET: /BasicDataManagement/Reimbursement/GetAllData/
        public ActionResult GetAllData()
        {
            var result = this._refundTypeService.RefundTypes.ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);
     
        }

        // POST: /BasicDataManagement/Reimbursement/Create/
        [HttpPost]
        public ActionResult Create()
        {
            RefundTypeVM[] datas = ClassConvert<RefundTypeVM>.Process(Request.Form);
            var result = _refundTypeService.Insert(datas[0]);
            if (result.ResultType != OperationResultType.Success)
                return Json(new { error = result.ResultType.GetDescription(), total = 1, data = this._refundTypeService.RefundTypes.ToArray() });
            else
            {
                RefundType[] results = this._refundTypeService.RefundTypes.ToArray();
                return Json(new { total = 1, data = new []{results[results.Length -1]} });
            }
            
        }

        // POST: /BasicDataManagement/Reimbursement/Edit/
        [HttpPost]
        public ActionResult Edit()
        {
            RefundType[] datas = ClassConvert<RefundType>.Process(Request.Form);
            foreach(var data in datas)
            {
                var result = _refundTypeService.Update(data);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if(result.ResultType != OperationResultType.Success )
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/Reimbursement/Delete/
        public ActionResult Delete()
        {
            RefundType[] datas = ClassConvert<RefundType>.Process(Request.Form);
            foreach(var data in datas)
            {
                var result = _refundTypeService.Delete(data);
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/Reimbursement/Import/
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
                var result = _refundTypeService.Import(path, columns);
                //删除临时创建文件
                System.IO.File.Delete(path);

                result.Message = result.Message ?? result.ResultType.GetDescription();

                return Json(new { erro = result.Message });
            }

            return Json(new { erro = "上传数据失败！" });
        }
    }
}
