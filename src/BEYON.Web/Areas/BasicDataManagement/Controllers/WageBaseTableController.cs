﻿using Newtonsoft.Json;
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
         private readonly ITaxBaseEveryMonthService _taxBaseEveryMonthService;
         private readonly ITaxPerOrderHistoryService _taxPerOrderHistoryService;
         private readonly ITaxPerOrderService _taxPerOrderService;
         private readonly ISafeguardTimeService _safeguardTimeService;

         public WageBaseTableController(ITaxBaseByMonthService taxBaseByMonthService, ITaxBaseEveryMonthService taxBaseEveryMonthService, ITaxPerOrderHistoryService taxPerOrderHistoryService, ITaxPerOrderService taxPerOrderService, ISafeguardTimeService safeguardTimeService)
        {
            this._taxBaseByMonthService = taxBaseByMonthService;
            this._taxBaseEveryMonthService = taxBaseEveryMonthService;
            this._taxPerOrderHistoryService = taxPerOrderHistoryService;
            this._taxPerOrderService = taxPerOrderService;
            this._safeguardTimeService = safeguardTimeService;
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

        // GET: /BasicDataManagement/WageBaseTable/GetNameByCerID/
        public String GetNameByCerID(String certificateID)
        {
            String result = this._taxBaseByMonthService.GetNameByCerID(certificateID);
            return result;

        }

        // GET: /BasicDataManagement/WageBaseTable/GetSafeguardTime/
        public String GetSafeguardTime()
        {
            string result = this._safeguardTimeService.GetSafeguardTime();
            return result;

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

        //POST: /BasicDataManagement/WageBaseTable/UpdateSafeguardTime
        [HttpPost]
        public ActionResult UpdateSafeguardTime(string times) {
            SafeguardTime safeguardTime = new SafeguardTime();
            string[] t = times.Split(',');
            string startTime = DateTime.Parse(t[0]).ToString("yyyy-MM-dd");
            string endTime = DateTime.Parse(t[1]).ToString("yyyy-MM-dd");
            safeguardTime.StartTime = Convert.ToDateTime(startTime);
            safeguardTime.EndTime = Convert.ToDateTime(endTime);
            var result = _safeguardTimeService.Update(safeguardTime);
            result.Message = result.Message ?? result.ResultType.GetDescription();
            return Json(result);
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

        // POST: /BasicDataManagement/WageBaseTable/DeleteAll/
        public ActionResult DeleteAll()
        {
           
                var result = _taxBaseByMonthService.DeleteAll();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription()});
                }
                return Json( JsonRequestBehavior.AllowGet);
           
        }

        // POST: /BasicDataManagement/WageBaseTable/AddToYearBaseTable/
        public ActionResult AddToYearBaseTable()
        {
           
            var result = this._taxBaseByMonthService.TaxBaseByMonths.ToList();
            String period = _taxBaseByMonthService.TaxBaseByMonths.First().Period;
            String period_year = period.Split('-')[0]; //get year
            TaxBaseEveryMonthVM taxBaseEveryMonth = null;
            try
            {
                foreach (var model in result)
                {
                    double monthIncome = _taxPerOrderService.GetPayTaxAmount(model.CertificateID, "含税");
                    double monthIncomeAfter = _taxPerOrderService.GetPayTaxAmount(model.CertificateID, "不含税");
                    double monthTax = _taxPerOrderService.GetDeductTaxSum(model.CertificateID);
                    ;
                    //检查年度累计底表中是否已存在该人员在该年度的记录，若存在，则进行数据累计，若不存在，则新增
                    var taxBaseEveryMonth_exsit = _taxBaseEveryMonthService.GetExistRecord(period_year, model.CertificateID);
                    if (taxBaseEveryMonth_exsit != null)
                    {
                        taxBaseEveryMonth = new TaxBaseEveryMonthVM();
                        taxBaseEveryMonth.CertificateID = model.CertificateID;
                        taxBaseEveryMonth.CertificateType = model.CertificateType;
                        taxBaseEveryMonth.Name = model.Name;
                        taxBaseEveryMonth.Period = period_year;
                        taxBaseEveryMonth.InitialTaxPayable = model.InitialTaxPayable + taxBaseEveryMonth_exsit.InitialTaxPayable;
                        taxBaseEveryMonth.InitialEaring = model.InitialEaring + taxBaseEveryMonth_exsit.InitialEaring;
                        //calculate totalincome in this month
                        taxBaseEveryMonth.TotalIncome = model.InitialEaring + monthIncome + taxBaseEveryMonth_exsit.TotalIncome;
                        taxBaseEveryMonth.TotalTax = model.InitialTax + monthTax + taxBaseEveryMonth_exsit.TotalTax;
                        taxBaseEveryMonth.TotalTemp = model.InitialEaring - model.InitialTax + monthIncomeAfter + taxBaseEveryMonth_exsit.TotalTemp;
                        taxBaseEveryMonth.InitialTax = model.InitialTax + taxBaseEveryMonth_exsit.InitialTax;
                        taxBaseEveryMonth.SpecialDeduction = model.SpecialDeduction + taxBaseEveryMonth_exsit.SpecialDeduction;
                        taxBaseEveryMonth.TaxFree = model.TaxFree + taxBaseEveryMonth_exsit.TaxFree;
                        taxBaseEveryMonth.AmountDeducted = model.AmountDeducted + taxBaseEveryMonth_exsit.AmountDeducted;

                        _taxBaseEveryMonthService.Insert(taxBaseEveryMonth);
                    }
                    else
                    {
                        taxBaseEveryMonth = new TaxBaseEveryMonthVM();
                        taxBaseEveryMonth.CertificateID = model.CertificateID;
                        taxBaseEveryMonth.CertificateType = model.CertificateType;
                        taxBaseEveryMonth.Name = model.Name;
                        taxBaseEveryMonth.Period = period_year;
                        taxBaseEveryMonth.InitialTaxPayable = model.InitialTaxPayable;
                        taxBaseEveryMonth.InitialEaring = model.InitialEaring;
                        //calculate totalincome in this month
                        taxBaseEveryMonth.TotalIncome = model.InitialEaring + monthIncome;
                        taxBaseEveryMonth.TotalTax = model.InitialTax + monthTax;
                        taxBaseEveryMonth.TotalTemp = model.InitialEaring - model.InitialTax + monthIncomeAfter;
                        taxBaseEveryMonth.InitialTax = model.InitialTax;
                        taxBaseEveryMonth.SpecialDeduction = model.SpecialDeduction;
                        taxBaseEveryMonth.TaxFree = model.TaxFree;
                        taxBaseEveryMonth.AmountDeducted = model.AmountDeducted;

                        _taxBaseEveryMonthService.Insert(taxBaseEveryMonth);
                    }


                }
            }

            catch
            {
                return Json(new { erro = "计算累计数据失败！" });
            }
            return Json(new { });

        }


        // POST: /BasicDataManagement/WageBaseTable/TaxPerOrderBackUp/
        public ActionResult TaxPerOrderBackUp()
        {
            var modelList = _taxPerOrderService.TaxPerOrders;
            String period = _taxBaseByMonthService.TaxBaseByMonths.First().Period;
            TaxPerOrderHistory history = null;
            foreach(var model in modelList){
                history = new TaxPerOrderHistory();
                history.Id = model.Id;
                history.PaymentType = model.PaymentType;
                history.Period = period;
                history.PersonType = model.PersonType;
                history.ProjectDirector = model.ProjectDirector;
                history.ProjectNumber = model.ProjectNumber;
                history.RefundType = model.RefundType;
                history.SerialNumber = model.SerialNumber;
                history.TaskName = model.TaskName;
                history.Tax = model.Tax;
                history.TaxOrNot = model.TaxOrNot;
                history.UpdateDate = model.UpdateDate;
                history.AccountName = model.AccountName;
                history.AccountNumber = model.AccountNumber;
                history.Agent = model.Agent;
                history.Name = model.Name;
                history.Amount = model.Amount;
                history.AmountX = model.AmountX;
                history.AmountY = model.AmountY;
                history.Bank = model.Bank;
                history.BankDetailName = model.BankDetailName;
                history.CertificateID = model.CertificateID;
                history.CertificateType = model.CertificateType;
                _taxPerOrderHistoryService.InsertOrUpdate(history);
            }
            var result = _taxPerOrderService.DeleteAll();
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { error = result.ResultType.GetDescription() });
            }
            return Json(JsonRequestBehavior.AllowGet);

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
                ImportData importData;
                if (!ExcelService.Get(Request.Path, out importData))
                {
                    importData = null;
                }

                //实现文件导入
                var result = _taxBaseByMonthService.Import(path, importData);
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
