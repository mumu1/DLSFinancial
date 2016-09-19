﻿using Newtonsoft.Json;
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
using BEYON.CoreBLL.Service.Excel.Interface;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;

namespace BEYON.Web.Areas.App.Controllers
{
    public class ApplyFormController : Controller
    {
        private readonly IApplicationFormService _applicationFormService;
        private readonly IPersonalRecordService _personalRecordService;
        private readonly IUserService _userService;
        private readonly IApplyPrintService _applyPrintService;

        public ApplyFormController(IApplicationFormService applicationFormService,  IPersonalRecordService personalRecordService,
            IUserService userService, IApplyPrintService applyPrintService)
        {
            this._applicationFormService = applicationFormService;
            this._personalRecordService = personalRecordService;
            this._userService = userService;
            this._applyPrintService = applyPrintService;
        }

        //
        // GET: /App/ApplyForm/Index
        [Layout]
        public ActionResult Index()
        {
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            var userID = Int32.Parse(userid);
            User user = this._userService.Users.FirstOrDefault(t => t.Id == userID);
            var role = user.Roles.First();
            return PartialView("Index", role.RoleName);
        }

        // GET: /App/ApplyForm/GetAllData/
        public ActionResult GetAllData()
        {
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            var userID = Int32.Parse(userid);
            User user = this._userService.Users.FirstOrDefault(t => t.Id == userID);
            var role = user.Roles.First();
            if (role.RoleName == "系统管理员")
            {
                var result = this._applicationFormService.GetApplicationFromByAdmin();
                return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = _applicationFormService.GetApplicationFromByUser(user.Email);
                //var result = this._applicationFormService.ApplicationForms.ToList();
                return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);
            }
            

        }

      
        // POST: /App/ApplyForm/Create/
        public ActionResult Create()
        {
            //申请单流水号
            var timeNow = System.DateTime.Now;
            var serialNumber = String.Format("S{0}", timeNow.ToString("yyyyMMddHHmmssffff"));

            //获取登录用户EMail
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            var userID = Int32.Parse(userid);
            User user = this._userService.Users.FirstOrDefault(t => t.Id == userID);
            
            var model = new ApplicationFormVM()
            {
                SerialNumber = serialNumber,
                AuditStatus = "待提交",
                UserEmail = user.Email
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
        // GET: /App/ApplyForm/Edit
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

        //
        // GET: /App/ApplyForm/Show
        [IsAjax]
        public ActionResult Show(String SerialNumber)
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
            return PartialView("Show", model);
        }

        [HttpPost]
        public ActionResult Save(ApplicationFormVM formVM)
        {
            ApplicationForm form = _applicationFormService.ApplicationForms.FirstOrDefault(t => t.SerialNumber == formVM.SerialNumber);
            if (form == null)
            {
                //formVM.SubmitTime = DateTime.Now;
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

#region 业务流程
        [HttpPost]
        public ActionResult ApplySubmit(String[] serialNumbers)
        {
            foreach(var serialNumber in serialNumbers)
            {
                ApplicationForm form = _applicationFormService.ApplicationForms.FirstOrDefault(t => t.SerialNumber == serialNumber);
                if (form != null)
                {
                    form.AuditStatus = "待审核";
                    form.UpdateDate = DateTime.Now;
                    _applicationFormService.Update(form);
                }
            }
            
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ApplyRevoke(String[] serialNumbers)
        {
            foreach (var serialNumber in serialNumbers)
            {
                ApplicationForm form = _applicationFormService.ApplicationForms.FirstOrDefault(t => t.SerialNumber == serialNumber);
                if (form != null)
                {
                    form.AuditStatus = "已退回";
                    form.UpdateDate = DateTime.Now;
                    _applicationFormService.Update(form);
                }
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /App/ApplyForm/Audit
        [HttpPost]
        public ActionResult Audit(String[] serialNumbers)
        {
            Session["AuditSerials"] = serialNumbers;
            return PartialView();
        }


        //
        // GET: /App/ApplyForm/ApplyAudit
        [HttpPost]
        public ActionResult ApplyAudit(ApplicationFormVM formVM)
        {
            var serialNumbers = Session["AuditSerials"] as String[];
            foreach (var serialNumber in serialNumbers)
            {
                ApplicationForm form = _applicationFormService.ApplicationForms.FirstOrDefault(t => t.SerialNumber == serialNumber);
                if(form != null)
                {
                    form.AuditStatus = formVM.AuditStatus;
                    if (!string.IsNullOrEmpty(formVM.AuditOpinion))
                    {
                        form.AuditOpinion = formVM.AuditOpinion;
                    }
                    form.UpdateDate = DateTime.Now;
                    _applicationFormService.Update(form);
                }
            }
            Session.Remove("AuditSerials");
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        // POST: /App/ApplyForm/ExportApplyPersons
        [HttpPost]
        public ActionResult ExportApplyPersons(String SerialNumber)
        {
            string fullPath = Server.MapPath("/Exports/");
             String fileName = this._applyPrintService.ApplyExcel(fullPath, SerialNumber);
             return Json(fileName);
             //return Json(new { filename = fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DownloadFile(string filePath)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath))
                    return;

                var filepath = System.IO.Path.Combine(Server.MapPath("/Exports/"), filePath);
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                if (file.Exists)//判断文件是否存在
                {
                    const long ChunkSize = 1048576;//1024K 每次读取文件，只读取100Ｋ，这样可以缓解服务器的压力
                    byte[] buffer = new byte[ChunkSize];

                    Response.Clear();
                    System.IO.FileStream iStream = System.IO.File.OpenRead(filepath);
                    long dataLengthToRead = iStream.Length;//获取下载的文件总大小
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode("申请打印单.xlsx"));
                    while (dataLengthToRead > 0 && Response.IsClientConnected)
                    {
                        int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                        Response.OutputStream.Write(buffer, 0, lengthRead);
                        Response.Flush();
                        dataLengthToRead = dataLengthToRead - lengthRead;
                    }
                    Response.Close();
                    Response.End();
                    iStream.Close();

                    System.IO.File.SetAttributes(filepath, System.IO.FileAttributes.Normal);
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        [HttpPost]
        public ActionResult DeleteFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return Json(new{});
            var filepath = System.IO.Path.Combine(Server.MapPath("/Exports/"), fileName);
            System.IO.FileInfo file = new System.IO.FileInfo(filepath);
            if(file.Exists)
            {
                if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                {
                    file.Attributes = System.IO.FileAttributes.Normal;
                }
                System.IO.File.Delete(file.FullName);
            }

            return Json(new { });
        }
        
#endregion
    }
}