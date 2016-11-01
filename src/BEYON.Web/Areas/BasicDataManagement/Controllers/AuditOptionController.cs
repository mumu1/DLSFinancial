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

        private readonly IAuditOpinionService _auditOpinionService;

        public AuditOptionController(IAuditOpinionService auditOpinionService)
        {
            this._auditOpinionService = auditOpinionService;
        }

        //
        // GET: /BasicDataManagement/AuditOption/
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

        // GET: /BasicDataManagement/AuditOption/GetAllData/
        public ActionResult GetAllData()
        {
            var result = this._auditOpinionService.AuditOpinions.ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);
     
        }

        // POST: /BasicDataManagement/AuditOption/Create/
        [HttpPost]
        public ActionResult Create()
        {
            AuditOpinionVM[] datas = ClassConvert<AuditOpinionVM>.Process(Request.Form);
            var result = _auditOpinionService.Insert(datas[0]);
            if (result.ResultType != OperationResultType.Success)
                return Json(new { error = result.ResultType.GetDescription(), total = 1, data = this._auditOpinionService.AuditOpinions.ToArray() });
            else
            {
                AuditOpinion[] results = this._auditOpinionService.AuditOpinions.ToArray();
                return Json(new { total = 1, data = new []{results[results.Length -1]} });
            }
            
        }

        // POST: /BasicDataManagement/AuditOption/Edit/
        [HttpPost]
        public ActionResult Edit()
        {
            AuditOpinion[] datas = ClassConvert<AuditOpinion>.Process(Request.Form);
            foreach(var data in datas)
            {
                var result = _auditOpinionService.Update(data);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if(result.ResultType != OperationResultType.Success )
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/AuditOption/Delete/
        public ActionResult Delete()
        {
            AuditOpinion[] datas = ClassConvert<AuditOpinion>.Process(Request.Form);
            foreach(var data in datas)
            {
                var result = _auditOpinionService.Delete(data);
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/AuditOption/Import/
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
                ImportData importData;
                if (!ExcelService.Get(Request.Path, out importData))
                {
                    columns = null;
                }
                else
                {
                    columns = importData.Columns;
                }

                //实现文件导入
                var result = _auditOpinionService.Import(path, columns);
                //删除临时创建文件
                System.IO.File.Delete(path);

                result.Message = result.Message ?? result.ResultType.GetDescription();

                return Json(new { erro = result.Message });
            }

            return Json(new { erro = "上传数据失败！" });
        }
    }
}
