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
            var timeNow = System.DateTime.Now;
            var serialNumber = String.Format("DLS{0} {1}", timeNow.ToString(), timeNow.Millisecond);
            var model = new ApplicationFormVM()
            {
                SerialNumber = serialNumber
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

        [HttpPost]
        public ActionResult Save(ApplicationFormVM formVM)
        {
            ApplicationForm form = _applicationFormService.ApplicationForms.FirstOrDefault(t => t.SerialNumber == formVM.SerialNumber);
            if (form == null)
            {
                _applicationFormService.Insert(formVM);
            }
            else
            {
                _applicationFormService.Update(formVM);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        #region Personal操作

        // GET: /App/ApplyForm/GetPersonalData/
        public ActionResult GetPersonalData(String serialNumber)
        {
            var result = this._personalRecordService.PersonalRecords.Where(t=>t.SerialNumber == serialNumber).ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);

        }

        // POST: /BasicDataManagement/Reimbursement/Create/
        [HttpPost]
        public ActionResult CreatePersonal()
        {
            PersonalRecordVM[] recordVMs = ClassConvert<PersonalRecordVM>.Process(Request.Form);
            var result = _personalRecordService.Insert(recordVMs[0], false);
            var entity = new PersonalRecord
            {
                //Id = (int)System.DateTime.Now.Ticks,
                SerialNumber = recordVMs[0].SerialNumber,
                Name = recordVMs[0].Name,
                CertificateID = recordVMs[0].CertificateID,
                CertificateType = recordVMs[0].CertificateType,
                Company = recordVMs[0].Company,
                Tele = recordVMs[0].Tele,
                PersonType = recordVMs[0].PaymentType,
                Nationality = recordVMs[0].Nationality,
                Title = recordVMs[0].Title,
                Amount = recordVMs[0].Amount,
                TaxOrNot = recordVMs[0].TaxOrNot,
                Bank = recordVMs[0].Bank,
                AccountName = recordVMs[0].AccountName,
                AccountNumber = recordVMs[0].AccountNumber,
                PaymentType = recordVMs[0].PaymentType,
                UpdateDate = DateTime.Now
            };

            if (result.ResultType != OperationResultType.Success)
                return Json(new { error = result.Message, total = 1, data = new[] { entity } });
            else
            {
                return Json(new { total = 1, data = new[] { entity } });
            }
        }

        [HttpPost]
        public ActionResult EditPersonal()
        {
            PersonalRecord[] datas = ClassConvert<PersonalRecord>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _personalRecordService.Update(data, false);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /App/ApplyForm/DeletePersonal/
        public ActionResult DeletePersonal()
        {
            List<string> serialNumberList = new List<string>();
            PersonalRecord[] datas = ClassConvert<PersonalRecord>.Process(Request.Form);
            foreach (var data in datas)
            {
                serialNumberList.Add(data.SerialNumber);
            }
            _personalRecordService.Delete(serialNumberList, false);
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertOrUpdatePersonal(PersonalRecordVM[] datas)
        {
            _personalRecordService.Delete(new List<String>() { datas[0].SerialNumber });

            foreach (var data in datas)
            {
                _personalRecordService.Insert(data);
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}