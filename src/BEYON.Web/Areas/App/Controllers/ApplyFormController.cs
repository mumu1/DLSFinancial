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
using BEYON.CoreBLL.Service.Excel.Interface;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using System.IO;
using BEYON.CoreBLL.Service.Excel;

namespace BEYON.Web.Areas.App.Controllers
{
    public class ApplyFormController : Controller
    {
        private static object _lockObj = new object();
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IApplicationFormService _applicationFormService;
        private readonly IPersonalRecordService _personalRecordService;
        private readonly ITopContactsService _topContactsService;
        private readonly IUserService _userService;
        private readonly IApplyPrintService _applyPrintService;
        private readonly IAuditOpinionService _auditOpinionService;
        private readonly ITaxPerOrderService _taxPerOrderService;
        public ApplyFormController(IApplicationFormService applicationFormService, IPersonalRecordService personalRecordService,
            IUserService userService, IApplyPrintService applyPrintService, IAuditOpinionService auditOpinionService, ITaxPerOrderService taxPerOrderService, ITopContactsService topContactsService)
        {
            this._applicationFormService = applicationFormService;
            this._personalRecordService = personalRecordService;
            this._userService = userService;
            this._applyPrintService = applyPrintService;
            this._auditOpinionService = auditOpinionService;
            this._taxPerOrderService = taxPerOrderService;
            this._topContactsService = topContactsService;
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
                var jsonResult = Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
            else
            {
                var result = _applicationFormService.GetApplicationFromByUser(user.UserName);
                //var result = this._applicationFormService.ApplicationForms.ToList();
                var jsonResult = Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }

            //var search = HttpContext.Request.Params["sSearch"].ToString();
            //var sortOrder = HttpContext.Request.Params["sSortDir_0"].ToString();
            //var sortCol = HttpContext.Request.Params["iSortCol_0"].ToString();
            //var echo = int.Parse(HttpContext.Request.Params["sEcho"]);
            //var displayLength = int.Parse(HttpContext.Request.Params["iDisplayLength"]);
            //var displayStart = int.Parse(HttpContext.Request.Params["iDisplayStart"]);
            //var itemsToSkip = displayStart == 0 ? 0 : displayStart + 1;
            //var sortName = GetSortCol(sortCol);
            //IList<ApplicationForm> records;
            //if (role.RoleName == "系统管理员")
            //{
            //    records = this._applicationFormService.GetApplicationFromByAdmin(search, sortName, sortOrder);
            //}
            //else
            //{
            //    records = _applicationFormService.GetApplicationFromByUser(user.UserName, search, sortName, sortOrder);
            //}

     
            //var orderedResults = sortOrder == "asc"
            //                  ? records.OrderBy(o => o.UpdateDate)
            //                  : records.OrderByDescending(o => o.UpdateDate);

            //var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();

            //return Json(new
            //{
            //    sEcho = echo,
            //    recordsTotal = records.Count,
            //    recordsFiltered = records.Count,
            //    iTotalRecords = records.Count,
            //    iTotalDisplayRecords = records.Count,
            //    aaData = pagedResults
            //}, JsonRequestBehavior.AllowGet);
        }

        private String GetSortCol(String sortCol)
        {
            switch(sortCol)
            {
                case "8":
                    return "\"AuditStatus\"";
                case "7":
                    return "\"SubmitTime\"";
                case "6":
                    return "\"PaymentType\"";
                case "5":
                    return "\"Summation\"";
                case "4":
                    return "\"Agent\"";
                case "3":
                    return "\"ProjectDirector\"";
                case "2":
                    return "\"RefundType\"";
                case "1":
                    return "\"ProjectNumber\"";
                default:
                    return "\"SerialNumber\"";
            }

        }

        // POST: /App/ApplyForm/Create/
        public ActionResult Create()
        {
            //申请单流水号
            var timeNow = System.DateTime.Now;
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            var serialNumber = String.Format("S{0}_{1}", userid, timeNow.ToString("yyyyMMddHHmmssffff"));

            //获取登录用户名
            //string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            var userID = Int32.Parse(userid);
            User user = this._userService.Users.FirstOrDefault(t => t.Id == userID);

            var model = new ApplicationFormVM()
            {
                SerialNumber = serialNumber,
                AuditStatus = "待提交",
                SubmitTime = DateTime.Now,
                UserName = user.UserName
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
                TaskName = application.TaskName,
                ProjectDirector = application.ProjectDirector,
                DepartmentName = application.DepartmentName,
                PaymentType = application .PaymentType,
                Agent = application.Agent,
                SubmitTime = application.SubmitTime,
                AuditStatus = application.AuditStatus,
                AuditOpinion = application.AuditOpinion,
                ApplyDesp = application.ApplyDesp,
                AuditTime = application.AuditTime,
                Summation = application.Summation,
                RefundType = application.RefundType,
                UserName = application.UserName
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
                TaskName = application.TaskName,
                ProjectDirector = application.ProjectDirector,
                DepartmentName = application.DepartmentName,
                Agent = application.Agent,
                PaymentType = application.PaymentType,
                SubmitTime = application.SubmitTime,
                AuditStatus = application.AuditStatus,
                AuditOpinion = application.AuditOpinion,
                ApplyDesp = application.ApplyDesp,
                AuditTime = application.AuditTime,
                Summation = application.Summation,
                RefundType = application.RefundType,
                UserName = application.UserName
            };
            return PartialView("Show", model);
        }

        [HttpPost]
        public ActionResult Save(ApplicationFormVM formVM)
        {
            ApplicationForm form = _applicationFormService.ApplicationForms.FirstOrDefault(t => t.SerialNumber == formVM.SerialNumber);

            if (form == null)
            {
                formVM.SubmitTime = DateTime.Now;
                _applicationFormService.Insert(formVM);
            }
            else
            {
                form.UpdateDate = DateTime.Now;

                _applicationFormService.Update(formVM);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        #region Personal操作

        // GET: /App/ApplyForm/GetPersonalData/
        public ActionResult GetPersonalData(String serialNumber)
        {
            var result = this._personalRecordService.PersonalRecords.Where(t => t.SerialNumber == serialNumber).ToList();
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }

        List<PersonalRecord> GetPersonalDatas(String serialNumber)
        {
            return this._personalRecordService.PersonalRecords.Where(t => t.SerialNumber == serialNumber).ToList();
        }

        // POST: /App/ApplyForm/CreatePersonal/
        [HttpPost]
        public ActionResult CreatePersonal()
        {
            PersonalRecordVM[] recordVMs = ClassConvert<PersonalRecordVM>.Process(Request.Form);
            var result = _personalRecordService.Insert(recordVMs[0], true);
            var entity = new PersonalRecord
            {
                //Id = (int)System.DateTime.Now.Ticks,
                SerialNumber = recordVMs[0].SerialNumber,
                Name = recordVMs[0].Name,
                CertificateID = recordVMs[0].CertificateID,
                CertificateType = recordVMs[0].CertificateType,
                Company = recordVMs[0].Company,
                Tele = recordVMs[0].Tele,
                PersonType = recordVMs[0].PersonType,
                Nationality = recordVMs[0].Nationality,
                Title = recordVMs[0].Title,
                Amount = recordVMs[0].Amount,
                TaxOrNot = recordVMs[0].TaxOrNot,
                Bank = recordVMs[0].Bank,
                BankDetailName = recordVMs[0].BankDetailName,
                ProvinceCity = recordVMs[0].ProvinceCity,
                CityField = recordVMs[0].CityField,
                AccountName = recordVMs[0].Name,
                AccountNumber = recordVMs[0].AccountNumber,
                PaymentType = recordVMs[0].PaymentType,
                Gender = recordVMs[0].Gender,
                Birth = recordVMs[0].Birth,
                UpdateDate = DateTime.Now
            };

            if (result.ResultType != OperationResultType.Success)
                return Json(new { error = result.Message, total = 1, data = new[] { entity } });
            else
            {
                return Json(new { total = 1, data = new[] { result.Data } });
            }
        }

        // POST: /App/ApplyForm/EditPersonal/
        [HttpPost]
        public ActionResult EditPersonal()
        {
            PersonalRecord[] datas = ClassConvert<PersonalRecord>.Process(Request.Form);
            foreach (var data in datas)
            {
                var result = _personalRecordService.Update(data, true);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.Message, total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /App/ApplyForm/DeletePersonal/
        public ActionResult DeletePersonal()
        {
            //List<string> serialNumberList = new List<string>();
            List<PersonalRecord> list = new List<PersonalRecord>();
            PersonalRecord[] datas = ClassConvert<PersonalRecord>.Process(Request.Form);
            foreach (var data in datas)
            {
                //serialNumberList.Add(data.SerialNumber);
                list.Add(data);
            }
           // _personalRecordService.Delete(serialNumberList, true);
            _personalRecordService.DeleteModel(list, true);
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertOrUpdatePersonal(PersonalRecordVM[] datas)
        {
            if (datas != null)
            {
                _personalRecordService.Delete(new List<String>() { datas[0].SerialNumber });

                foreach (var data in datas)
                {
                    _personalRecordService.Insert(data);
                   
                }
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        // POST: /App/ApplyForm/ImportPersonal/
        [HttpPost]
        public ActionResult ImportPersonal(System.Web.HttpPostedFileBase upload)
        {
            string fileName = "";
            String filePath = Server.MapPath("~/Imports");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            string tempName = String.Format("{0}_{1}{2}", userid, DateTime.Now.ToString("yyyyMMddHHMMss"), DateTime.Now.Millisecond);
            if (upload.ContentLength > 0)
            {
                fileName = tempName + Path.GetFileName(upload.FileName);
                var path = Path.Combine(filePath, fileName);
                upload.SaveAs(path);

                //防止Excel并发读取
                OperationResult result;
                lock(_lockObj)
                {
                    //获取映射文件
                    ImportData importData;
                    if (!ExcelService.Get(Request.Path, out importData))
                    {
                        importData = null;
                    }

                    //实现文件导入
                    result = _personalRecordService.Import(path, importData);
                }
                

                //删除临时创建文件
                System.IO.File.Delete(path);

                if(result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.LogMessage });
                }
                else
                {
                    return Json(new { });
                }
            }

            return Json(new { error = "上传数据失败！" });
        }

        #endregion

        #region 业务流程
        [HttpPost]
        public ActionResult ApplySubmit(String[] serialNumbers)
        {
            foreach (var serialNumber in serialNumbers)
            {
                ApplicationForm form = _applicationFormService.ApplicationForms.FirstOrDefault(t => t.SerialNumber == serialNumber);
                if (form != null)
                {
                    form.AuditStatus = "待审核";
                    form.UpdateDate = DateTime.Now;
                    form.SubmitTime = DateTime.Now;
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
                    form.AuditStatus = "待提交";
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
            //1.获取审核意见表
            var result = this._auditOpinionService.AuditOpinions.ToList();
            return PartialView(result);
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
                if (form != null)
                {
                    if (form.AuditStatus.Equals("审核通过") && formVM.AuditStatus.Equals("审核通过"))
                    {
                        continue;
                    }
                    form.AuditStatus = formVM.AuditStatus;
                    if (!string.IsNullOrEmpty(formVM.AuditOpinion))
                    {
                        form.AuditOpinion = formVM.AuditOpinion;
                        form.AuditTime = DateTime.Now;
                    }
                    
                    //若审核通过，开始算税，单笔税保存在TaxPerOrder表
                    if (formVM.AuditStatus.Equals("审核通过"))
                    {
                        List<PersonalRecord> records = GetPersonalDatas(serialNumber);
                        TaxPerOrder taxPerOrder = null;
                        if (records != null)
                        {
                            for (int i = 0; i < records.Count; i++)
                            {
                                taxPerOrder = new TaxPerOrder();
                                taxPerOrder.SerialNumber = form.SerialNumber;
                                if (!String.IsNullOrEmpty(form.ProjectNumber) && !form.ProjectNumber.Equals("无"))
                                {
                                    string[] projectSep = form.ProjectNumber.Split('|');
                                    taxPerOrder.ProjectNumber = projectSep[0].Trim();
                                    if (projectSep.Length > 1)
                                        taxPerOrder.TaskName = projectSep[1].Trim();
                                    else
                                        taxPerOrder.TaskName = form.TaskName;
                                }
                                else {
                                    taxPerOrder.ProjectNumber = "无";
                                    taxPerOrder.TaskName = "无";
                                }                                                                                         
                                taxPerOrder.RefundType = form.RefundType;
                                taxPerOrder.ProjectDirector = form.ProjectDirector;
                                taxPerOrder.Agent = form.Agent;
                                taxPerOrder.Name = records[i].Name;
                                taxPerOrder.PersonType = records[i].PersonType;
                                taxPerOrder.CertificateType = records[i].CertificateType;
                                taxPerOrder.CertificateID = records[i].CertificateID;
                                taxPerOrder.Amount = records[i].Amount;
                                taxPerOrder.TaxOrNot = records[i].TaxOrNot;
                                taxPerOrder.Bank = records[i].Bank;
                                taxPerOrder.BankDetailName = records[i].BankDetailName;
                                taxPerOrder.ProvinceCity = records[i].ProvinceCity;
                                taxPerOrder.CityField = records[i].CityField;
                                taxPerOrder.AccountName = records[i].AccountName;
                                taxPerOrder.AccountNumber = records[i].AccountNumber;
                                taxPerOrder.PaymentType = form.PaymentType;
                                _taxPerOrderService.Insert(taxPerOrder);
                            }
                        }
                    }
                    else if (formVM.AuditStatus.Equals("已退回"))
                    {
                        //删除TaxPerOrder表中审核通过时的记录（主要是保证多次审核的情况，如第一次审核通过，发现问题，重新审核为退回）
                        _taxPerOrderService.DeleteBySerialNumber(serialNumber);
                    }
                    form.UpdateDate = DateTime.Now;
                    _applicationFormService.Update(form);
                }
            }
            Session.Remove("AuditSerials");
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        // GET: /App/ApplyForm/CashCountCheckBeforeAudit
        public ActionResult CashCountCheckBeforeAudit(String[] serialNumbers)
        {
            //var serialNumbers = Session["AuditSerials"] as String[];
            List<string> notQualify = new List<string>();
            foreach (var serialNumber in serialNumbers)
            {
                ApplicationForm form = _applicationFormService.ApplicationForms.FirstOrDefault(t => t.SerialNumber == serialNumber);
                if (form != null)
                {
                    //再次检查现金发放次数是否已满三次                       
                    List<PersonalRecord> records = GetPersonalDatas(serialNumber);
                    if (records != null)
                        for (int k = 0; k < records.Count; k++)
                        {
                            if (records[k].PaymentType.Equals("现金支付"))
                            {
                                String str = GetCashCount(records[k].CertificateID);
                                if (!str.Equals(""))
                                {
                                    notQualify.Add(records[k].Name);
                                }
                            }

                        }
                }
            }
            return Json(notQualify, JsonRequestBehavior.AllowGet);
        }

        // GET: /App/ApplyForm/GetCashCount
        public String GetCashCount(String certificateID)
        {
            String feedback = "";
            int count = _taxPerOrderService.GetCashCount(certificateID);
            if (count >= 3)
            {
                feedback = "该人员本月已发放3次现金，若需再次发放，需在支付方式为【银行转账】的申请单中填报！";
            }
            return feedback;
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
                _log.Info(filepath);
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                if (file.Exists)//判断文件是否存在
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("申请打印单.xlsx"));
                    Response.AddHeader("Content-Length", new FileInfo(filepath).Length.ToString());
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.TransmitFile(filepath);

                    //const long ChunkSize = 1048576;//1024K 每次读取文件，只读取100Ｋ，这样可以缓解服务器的压力
                    //byte[] buffer = new byte[ChunkSize];

                    //Response.Clear();
                    //System.IO.FileStream iStream = System.IO.File.OpenRead(filepath);
                    //long dataLengthToRead = iStream.Length;//获取下载的文件总大小
                    //Response.ContentType = "application/octet-stream";
                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode("申请打印单.xlsx"));
                    //while (dataLengthToRead > 0 && Response.IsClientConnected)
                    //{
                    //    int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                    //    Response.OutputStream.Write(buffer, 0, lengthRead);
                    //    Response.Flush();
                    //    dataLengthToRead = dataLengthToRead - lengthRead;
                    //}
                    //iStream.Close();

                    if (Response.IsClientConnected)
                    {
                        //Response.Close();
                        Response.End();
                    }

                    System.IO.File.SetAttributes(filepath, System.IO.FileAttributes.Normal);
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        [HttpPost]
        public ActionResult DeleteFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return Json(new { });
            var filepath = System.IO.Path.Combine(Server.MapPath("/Exports/"), fileName);
            System.IO.FileInfo file = new System.IO.FileInfo(filepath);
            if (file.Exists)
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