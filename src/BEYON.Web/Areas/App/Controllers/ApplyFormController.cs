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
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;

namespace BEYON.Web.Areas.App.Controllers
{
    public class ApplyFormController : Controller
    {
        private readonly IApplicationFormService _applicationFormService;
        private readonly IPersonalRecordService _personalRecordService;
        public ApplyFormController(IApplicationFormService applicationFormService,  IPersonalRecordService personalRecordService)
        {
            this._applicationFormService = applicationFormService;
            this._personalRecordService = personalRecordService;
        }

        //
        // GET: /App/ApplyForm/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

        // GET: /App/ApplyForm/GetAllData/
        public ActionResult GetAllData()
        {
            var result = this._applicationFormService.ApplicationForms.ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);

        }

      
        // POST: /App/ApplyForm/Create/
        public ActionResult Create()
        {
            //产生流水线号
            var model = new ApplicationFormVM()
            {
                SerialNumber = "1"
            };
            return PartialView(model);
        }

        //[HttpPost]
        //public ActionResult Create(RoleVM roleVm)
        //{
        //    if (!ModelState.IsValid) return Json(new OperationResult(OperationResultType.ParamError, "参数错误，请重新检查输入"));
        //    var result = _roleService.Insert(roleVm);
        //    result.Message = result.Message ?? result.ResultType.GetDescription();
        //    return Json(result);
        //}


        //
        // GET: /Member/Role/Edit/5
        [IsAjax]
        public ActionResult Edit(String SerialNumber)
        {
            var application = _applicationFormService.ApplicationForms.FirstOrDefault(c => c.SerialNumber == SerialNumber);
            if (application == null) 
                return PartialView("Create", new ApplicationFormVM());
            var model = new ApplicationFormVM()
            {
                SerialNumber = application.SerialNumber,
                ProjectNumber = application.ProjectNumber,
                ProjectDirector = application.ProjectDirector,
                Agent = application.Agent,
                SubmitTime = application.SubmitTime,
                AuditStatus = application.AuditStatus,
                AuditOpinion = application.AuditOpinion,
                AuditTime = application.AuditTime,
                Summation = application.Summation,
                RefundType = application.RefundType,
                UserEmail = application.UserEmail
            };
            return PartialView("Create", model);
        }

        // POST: /App/ApplyForm/Edit/
        [HttpPost]
        public ActionResult Edit()
        {
            ApplicationForm[] datas = ClassConvert<ApplicationForm>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _applicationFormService.Update(data);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /App/ApplyForm/Delete/
        public ActionResult Delete()
        {
            List<string> serialNumberList = new List<string>();
            ApplicationForm[] datas = ClassConvert<ApplicationForm>.Process(Request.Form);
            foreach (var data in datas)
            {
                serialNumberList.Add(data.SerialNumber);
                var result = _applicationFormService.Delete(data);          
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }
            _personalRecordService.Delete(serialNumberList);
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }


        #region Personal操作

        // GET: /App/ApplyForm/GetPersonalData/
        public ActionResult GetPersonalData(String serialNumber)
        {
            var result = this._personalRecordService.PersonalRecords.Where(t=>t.SerialNumber == serialNumber).ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}