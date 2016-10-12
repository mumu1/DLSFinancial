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
    public class TaskManageController : Controller
    {
         private readonly ITaskManageService _taskManageService;
         public TaskManageController(ITaskManageService taskManageService)
        {
            this._taskManageService = taskManageService;
        }


        //
         // GET: /BasicDataManagement/TaskManage/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

        // GET: /BasicDataManagement/TaskManage/GetAllData/
        public ActionResult GetAllData()
        {
            var result = this._taskManageService.TaskManages.ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);

        }
        // POST: /BasicDataManagement/TaskManage/Create/
        [HttpPost]
        public ActionResult Create()
        {
            TaskManageVM[] datas = ClassConvert<TaskManageVM>.Process(Request.Form);
            var result = _taskManageService.Insert(datas[0]);
            if (result.ResultType != OperationResultType.Success)
                return Json(new { error = result.ResultType.GetDescription(), total = 1, data = this._taskManageService.TaskManages.ToArray() });
            else
            {
                TaskManage[] results = this._taskManageService.TaskManages.ToArray();
                return Json(new { total = 1, data = new[] { results[results.Length - 1] } });
            }

        }

        // POST: /BasicDataManagement/TaskManage/Edit/
        [HttpPost]
        public ActionResult Edit()
        {
            TaskManage[] datas = ClassConvert<TaskManage>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _taskManageService.Update(data);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/TaskManage/Delete/
        public ActionResult Delete()
        {
            TaskManage[] datas = ClassConvert<TaskManage>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _taskManageService.Delete(data);
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /BasicDataManagement/TaskManage/Import/
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
                var result = _taskManageService.Import(path, columns);
                //删除临时创建文件
                System.IO.File.Delete(path);

                result.Message = result.Message ?? result.ResultType.GetDescription();

                return Json(new { erro = result.Message });
            }

            return Json(new { erro = "上传数据失败！" });
        }
    }
}