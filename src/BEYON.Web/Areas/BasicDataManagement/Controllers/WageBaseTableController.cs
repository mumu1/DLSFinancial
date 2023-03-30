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

        // GET: /BasicDataManagement/WageBaseTable/IsInDepartment/
        public Boolean IsInDepartment(String certificateID)
        {
            Boolean result = this._taxBaseByMonthService.IsInDepartment(certificateID);
            return result;

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
            String period_month = period.Split('-')[1]; //get month
            TaxBaseEveryMonth taxBaseEveryMonth = new TaxBaseEveryMonth();
           
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
                        /*
                        taxBaseEveryMonth.CertificateID = model.CertificateID;
                        taxBaseEveryMonth.CertificateType = model.CertificateType;
                        taxBaseEveryMonth.Name = model.Name;
                        taxBaseEveryMonth.Period = period_year;
                        taxBaseEveryMonth.InitialEaring = model.InitialEaring + taxBaseEveryMonth_exsit.InitialEaring;
                        taxBaseEveryMonth.TaxFreeIncome = model.TaxFreeIncome + taxBaseEveryMonth_exsit.TaxFreeIncome;
                        taxBaseEveryMonth.EndowmentInsurance = model.EndowmentInsurance + taxBaseEveryMonth_exsit.EndowmentInsurance;
                        taxBaseEveryMonth.UnemployedInsurance = model.UnemployedInsurance + taxBaseEveryMonth_exsit.UnemployedInsurance;
                        taxBaseEveryMonth.MedicalInsurance = model.MedicalInsurance + taxBaseEveryMonth_exsit.MedicalInsurance;
                        taxBaseEveryMonth.OccupationalAnnuity = model.OccupationalAnnuity + taxBaseEveryMonth_exsit.OccupationalAnnuity;
                        taxBaseEveryMonth.HousingFund = model.HousingFund + taxBaseEveryMonth_exsit.HousingFund;
                        taxBaseEveryMonth.AmountDeducted = model.AmountDeducted + taxBaseEveryMonth_exsit.AmountDeducted;
                        taxBaseEveryMonth.SpecialDeduction = model.SpecialDeduction + taxBaseEveryMonth_exsit.SpecialDeduction;
                        taxBaseEveryMonth.TotalTax = model.InitialTax + monthTax + taxBaseEveryMonth_exsit.TotalTax;
                        taxBaseEveryMonth.TotalSalaryIncomeBeforeTax = model.InitialEaring + taxBaseEveryMonth_exsit.TotalSalaryIncomeBeforeTax;
                        taxBaseEveryMonth.TotalLaborIncomeBeforeTax = monthIncome + taxBaseEveryMonth_exsit.TotalLaborIncomeBeforeTax;
                        taxBaseEveryMonth.InitialTaxPayable = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TaxFreeIncome - taxBaseEveryMonth.EndowmentInsurance - taxBaseEveryMonth.UnemployedInsurance - taxBaseEveryMonth.MedicalInsurance - taxBaseEveryMonth.OccupationalAnnuity - taxBaseEveryMonth.HousingFund - taxBaseEveryMonth.AmountDeducted - taxBaseEveryMonth.SpecialDeduction;
                        taxBaseEveryMonth.TotalTemp = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TotalTax;
                        taxBaseEveryMonth.LastMonths = period_month;
                        */
                        taxBaseEveryMonth.CertificateID = model.CertificateID;     //保存证件号码
                        taxBaseEveryMonth.CertificateType = model.CertificateType;   //保存证件类型
                        taxBaseEveryMonth.Name = model.Name;//保存人员姓名
                        taxBaseEveryMonth.Period = period_year;//保存年度，如2019
                        //新的年度累计表中的“年度累计税前收入”= 该用户  当月底表中的 “本期初始税前收入额“ + 系统中已存在的年度累计表中的“年度累计税前收入” + 当月下发的劳务费含税总和
                        taxBaseEveryMonth.InitialEaring = model.InitialEaring + taxBaseEveryMonth_exsit.InitialEaring + monthIncome;
                        //新的年度累计表中的“年度累计免税收入”= 该用户 当月底表中的 “本期免税收入“ + 系统中已存在的年度累计表中的“年度累计免税收入”
                        taxBaseEveryMonth.TaxFreeIncome = model.TaxFreeIncome + taxBaseEveryMonth_exsit.TaxFreeIncome;
                        //新的年度累计表中的“年度累计养老保险”= 该用户 当月底表中的 “本期养老保险“ + 系统中已存在的年度累计表中的“年度累计养老保险”
                        taxBaseEveryMonth.EndowmentInsurance = model.EndowmentInsurance + taxBaseEveryMonth_exsit.EndowmentInsurance;
                        //新的年度累计表中的“年度累计失业保险”= 该用户 当月底表中的 “本期失业保险“ + 系统中已存在的年度累计表中的“年度累计失业保险”
                        taxBaseEveryMonth.UnemployedInsurance = model.UnemployedInsurance + taxBaseEveryMonth_exsit.UnemployedInsurance;
                        //新的年度累计表中的“年度累计医疗保险”= 该用户 当月底表中的 “本期医疗保险“ + 系统中已存在的年度累计表中的“年度累计医疗保险”
                        taxBaseEveryMonth.MedicalInsurance = model.MedicalInsurance + taxBaseEveryMonth_exsit.MedicalInsurance;
                        //新的年度累计表中的“年度累计职业年金”= 该用户 当月底表中的 “本期职业年金“ + 系统中已存在的年度累计表中的“年度累计职业年金”
                        taxBaseEveryMonth.OccupationalAnnuity = model.OccupationalAnnuity + taxBaseEveryMonth_exsit.OccupationalAnnuity;
                        //新的年度累计表中的“年度累计住房公积金”= 该用户 当月底表中的 “本期住房公积金“ + 系统中已存在的年度累计表中的“年度累计住房公积金”
                        taxBaseEveryMonth.HousingFund = model.HousingFund + taxBaseEveryMonth_exsit.HousingFund;
                        //新的年度累计表中的“年度累计基本扣除”= 该用户 当月底表中的 “本期基本扣除“ + 系统中已存在的年度累计表中的“年度累计基本扣除”
                        taxBaseEveryMonth.AmountDeducted = model.AmountDeducted + taxBaseEveryMonth_exsit.AmountDeducted;
                        //新的年度累计表中的“年度累计专项附加扣除”= 该用户 当月底表中的 “本期专项附加扣除“ + 系统中已存在的年度累计表中的“年度累计专项附加扣除”
                        taxBaseEveryMonth.SpecialDeduction = model.SpecialDeduction + taxBaseEveryMonth_exsit.SpecialDeduction;
                        //新的年度累计表中的“年度累计已扣缴税额”= 该用户 当月底表中的 “本期初始已扣缴税额“+ 当月下发的劳务费税总和+ 系统中已存在的年度累计表中的“年度累计已扣缴税额”
                        taxBaseEveryMonth.TotalTax = model.InitialTax + monthTax + taxBaseEveryMonth_exsit.TotalTax;
                        //新的年度累计表中的“年度累计工资税前收入额”= 该用户 当月底表中的 “本期初始税前收入额“ + 系统中已存在的年度累计表中的“年度累计工资税前收入额”
                        taxBaseEveryMonth.TotalSalaryIncomeBeforeTax = model.InitialEaring + taxBaseEveryMonth_exsit.TotalSalaryIncomeBeforeTax;
                        //新的年度累计表中的“年度累计劳务费税前收入额”= 该用户 当月下发的劳务费含税总和 + 系统中已存在的年度累计表中的“年度累计劳务费税前收入额”
                        taxBaseEveryMonth.TotalLaborIncomeBeforeTax = monthIncome + taxBaseEveryMonth_exsit.TotalLaborIncomeBeforeTax;
                        //新的年度累计表中的“年度累计应纳税所得额”= 更新后的年度累计表中的“年度累计税前收入”-  更新后的年度累计表中的“年度累计免税收入”- 更新后的年度累计表中的“年度累计养老保险”- 更新后的年度累计表中的“年度累计失业保险”- 更新后的年度累计表中的“年度累计医疗保险”- 更新后的年度累计表中的“年度累计职业年金”-  更新后的年度累计表中的“年度累计住房公积金”- 更新后的年度累计表中的“年度累计基本扣除”- 更新后的年度累计表中的“年度累计专项附加扣除”
                        taxBaseEveryMonth.InitialTaxPayable = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TaxFreeIncome - taxBaseEveryMonth.EndowmentInsurance - taxBaseEveryMonth.UnemployedInsurance - taxBaseEveryMonth.MedicalInsurance - taxBaseEveryMonth.OccupationalAnnuity - taxBaseEveryMonth.HousingFund - taxBaseEveryMonth.AmountDeducted - taxBaseEveryMonth.SpecialDeduction;
                        //年度累计减免税额
                        taxBaseEveryMonth.CutTax = model.CutTax + taxBaseEveryMonth_exsit.CutTax;
                        //新的年度累计表中的“年度累计税后收入”= 更新后的年度累计表中的“年度累计税前收入”- 更新后的年度累计表中的“年度累计已扣缴税额”
                        //new20200407 “年度累计税后收入” = 年度累计税前收入-年度累计已扣缴税额-年度累计减免税额
                        taxBaseEveryMonth.TotalTemp = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TotalTax - taxBaseEveryMonth.CutTax;
                        //更新“当前已累计月数”，如07
                        taxBaseEveryMonth.LastMonths = period_month;
                       
                    }
                    else
                    {
                        /*
                        taxBaseEveryMonth.CertificateID = model.CertificateID;
                        taxBaseEveryMonth.CertificateType = model.CertificateType;
                        taxBaseEveryMonth.Name = model.Name;
                        taxBaseEveryMonth.Period = period_year;
                        taxBaseEveryMonth.InitialEaring = model.InitialEaring;
                        taxBaseEveryMonth.TaxFreeIncome = model.TaxFreeIncome;
                        taxBaseEveryMonth.EndowmentInsurance = model.EndowmentInsurance;
                        taxBaseEveryMonth.UnemployedInsurance = model.UnemployedInsurance;
                        taxBaseEveryMonth.MedicalInsurance = model.MedicalInsurance;
                        taxBaseEveryMonth.OccupationalAnnuity = model.OccupationalAnnuity;
                        taxBaseEveryMonth.HousingFund = model.HousingFund;
                        taxBaseEveryMonth.AmountDeducted = model.AmountDeducted;
                        taxBaseEveryMonth.SpecialDeduction = model.SpecialDeduction;
                        taxBaseEveryMonth.TotalTax = model.InitialTax + monthTax;
                        taxBaseEveryMonth.TotalSalaryIncomeBeforeTax = model.InitialEaring;
                        taxBaseEveryMonth.TotalLaborIncomeBeforeTax = monthIncome;
                        taxBaseEveryMonth.InitialTaxPayable = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TaxFreeIncome - taxBaseEveryMonth.EndowmentInsurance - taxBaseEveryMonth.UnemployedInsurance - taxBaseEveryMonth.MedicalInsurance - taxBaseEveryMonth.OccupationalAnnuity - taxBaseEveryMonth.HousingFund - taxBaseEveryMonth.AmountDeducted - taxBaseEveryMonth.SpecialDeduction;
                        taxBaseEveryMonth.TotalTemp = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TotalTax;
                        taxBaseEveryMonth.LastMonths = period_month;                
                       */
                        //年度累计表中不存在该用户的情况（新人），将当月的数据累计到年度累计表中         
                        taxBaseEveryMonth.CertificateID = model.CertificateID;
                        taxBaseEveryMonth.CertificateType = model.CertificateType;
                        taxBaseEveryMonth.Name = model.Name;
                        taxBaseEveryMonth.Period = period_year;
                        //新的年度累计表中的“年度累计税前收入”= 该用户  当月底表中的 “本期初始税前收入额“ + 当月下发的劳务费含税总和
                        taxBaseEveryMonth.InitialEaring = model.InitialEaring + monthIncome;
                        //新的年度累计表中的“年度累计免税收入”= 该用户 当月底表中的 “本期免税收入“ 
                        taxBaseEveryMonth.TaxFreeIncome = model.TaxFreeIncome;
                        //新的年度累计表中的“年度累计养老保险”= 该用户 当月底表中的 “本期养老保险“ 
                        taxBaseEveryMonth.EndowmentInsurance = model.EndowmentInsurance;
                        //新的年度累计表中的“年度累计失业保险”= 该用户 当月底表中的 “本期失业保险“
                        taxBaseEveryMonth.UnemployedInsurance = model.UnemployedInsurance;
                        //新的年度累计表中的“年度累计医疗保险”= 该用户 当月底表中的 “本期医疗保险“
                        taxBaseEveryMonth.MedicalInsurance = model.MedicalInsurance;
                        //新的年度累计表中的“年度累计职业年金”= 该用户 当月底表中的 “本期职业年金“
                        taxBaseEveryMonth.OccupationalAnnuity = model.OccupationalAnnuity;
                        //新的年度累计表中的“年度累计住房公积金”= 该用户 当月底表中的 “本期住房公积金“
                        taxBaseEveryMonth.HousingFund = model.HousingFund;
                        //新的年度累计表中的“年度累计基本扣除”= 该用户 当月底表中的 “本期基本扣除“ 
                        taxBaseEveryMonth.AmountDeducted = model.AmountDeducted;
                        //新的年度累计表中的“年度累计专项附加扣除”= 该用户 当月底表中的 “本期专项附加扣除“ 
                        taxBaseEveryMonth.SpecialDeduction = model.SpecialDeduction;
                        //新的年度累计表中的“年度累计已扣缴税额”= 该用户 当月底表中的 “本期初始已扣缴税额“+ 当月下发的劳务费税总和
                        taxBaseEveryMonth.TotalTax = model.InitialTax + monthTax;
                        //新的年度累计表中的“年度累计工资税前收入额”= 该用户 当月底表中的 “本期初始税前收入额“ 
                        taxBaseEveryMonth.TotalSalaryIncomeBeforeTax = model.InitialEaring;
                        //新的年度累计表中的“年度累计劳务费税前收入额”= 该用户 当月下发的劳务费含税总和 
                        taxBaseEveryMonth.TotalLaborIncomeBeforeTax = monthIncome;
                        //新的年度累计表中的“年度累计应纳税所得额”= 更新后的年度累计表中的“年度累计税前收入”-  更新后的年度累计表中的“年度累计免税收入”- 更新后的年度累计表中的“年度累计养老保险”- 更新后的年度累计表中的“年度累计失业保险”- 更新后的年度累计表中的“年度累计医疗保险”- 更新后的年度累计表中的“年度累计职业年金”-  更新后的年度累计表中的“年度累计住房公积金”- 更新后的年度累计表中的“年度累计基本扣除”- 更新后的年度累计表中的“年度累计专项附加扣除”                       
                        taxBaseEveryMonth.InitialTaxPayable = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TaxFreeIncome - taxBaseEveryMonth.EndowmentInsurance - taxBaseEveryMonth.UnemployedInsurance - taxBaseEveryMonth.MedicalInsurance - taxBaseEveryMonth.OccupationalAnnuity - taxBaseEveryMonth.HousingFund - taxBaseEveryMonth.AmountDeducted - taxBaseEveryMonth.SpecialDeduction;
                        //年度累计减免税额
                        taxBaseEveryMonth.CutTax = model.CutTax;
                        //新的年度累计表中的“年度累计税后收入”= 更新后的年度累计表中的“年度累计税前收入”- 更新后的年度累计表中的“年度累计已扣缴税额”
                        //new20200407 “年度累计税后收入” = 年度累计税前收入-年度累计已扣缴税额-年度累计减免税额
                        taxBaseEveryMonth.TotalTemp = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TotalTax - taxBaseEveryMonth.CutTax;
                        //更新“当前已累计月数”，如07
                        taxBaseEveryMonth.LastMonths = period_month;
                        
                    }
                    try
                    {
                        _taxBaseEveryMonthService.InsertOrUpdate(taxBaseEveryMonth);
                 //       _taxBaseByMonthService.Delete(model);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }   
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
                history.Tele = model.Tele;
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
