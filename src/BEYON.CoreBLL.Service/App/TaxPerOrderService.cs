using System;
using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Data.Repositories.App;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.Excel;
using EntityFramework.Extensions;

namespace BEYON.CoreBLL.Service.App
{
    public class TaxPerOrderService : CoreServiceBase, ITaxPerOrderService
    {
        private readonly ITaxPerOrderRepository _TaxPerOrderRepository;

        private readonly ITaxBaseByMonthRepository _TaxBaseByMonthRepository;

        private readonly ITaxBaseEveryMonthRepository _TaxBaseEveryMonthRepository;



        public TaxPerOrderService(ITaxPerOrderRepository taxPerOrderRepository, ITaxBaseByMonthRepository taxBaseByMonthRepository, ITaxBaseEveryMonthRepository taxBaseEveryMonthRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TaxPerOrderRepository = taxPerOrderRepository;
            this._TaxBaseByMonthRepository = taxBaseByMonthRepository;
            this._TaxBaseEveryMonthRepository = taxBaseEveryMonthRepository;
        }
        public IQueryable<TaxPerOrder> TaxPerOrders
        {
            get { return _TaxPerOrderRepository.Entities; }
        }

        /*
        public OperationResult Insert(TaxPerOrder model, bool isSave)
        {
            try
            {
                //TaxPerOrder taxPerOrder = _TaxPerOrderRepository.Entities.FirstOrDefault(c => c.SerialNumber == model.SerialNumber && c.SerialNumber == model.CertificateID.Trim());
                //if (taxPerOrder != null)
                //{
                //    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的税单纪录，请修改后重新提交！");
                //}
                //if (model.CertificateID == null || model.CertificateID.Trim() == "")
                //    return new OperationResult(OperationResultType.Warning, "证件号码不能为空，请修改后重新提交！");
                double tax = 0.0;          //最终税额
                double amountX = 0.0;     //最终税后数（不含税）
                double amountY = 0.0;       //最终税前数（含税）
                //计算税金，并存储
                if (model.PersonType.Equals("所内"))
                {
                    //按照工资进行算税
                    //1.查询已发放总金额，查询已计税总额【Tax和】
                    //double amount = GetPayTaxAmount(model.CertificateID, model.TaxOrNot);
                    //都按税前加和
                    double amount = GetPayTaxAmount(model.CertificateID, model.TaxOrNot);
                    //double amount = 0.0;
                    double deductTaxSum = GetDeductTaxSum(model.CertificateID);
                    //double deductTaxSum = 0.0;
                    //2.从基本工资表TaxBaseByMonth中查询（初始应发工资-免税额-基本扣除）
                    double baseSalary = _TaxBaseByMonthRepository.GetBaseSalary(model.CertificateID);
                    //从基本工资表里读取税
                    double baseTax = _TaxBaseByMonthRepository.GetBaseTax(model.CertificateID);
                    //double baseSalary = 4000;
                    //3.根据已发放总金额，判断算税公式 
                    //含税与不含税的税率区间不同，分开判断
                    //含税公式：T=【Y+(初始应发工资-免税额-基本扣除数)】*税率-速算扣除数-初始表税-前几次税额总数
                    

                    //4.计算Tax(税额),AmountX(税后),AmountY(税前)
                    if (model.TaxOrNot.Equals("含税"))
                    {
                        double interval = model.Amount + amount + baseSalary;
                        if (interval <= 0)
                        {
                            tax = 0.0;
                        }
                        //不超过3000元，税率3%，速算扣除数0
                        else if (interval > 0 && interval <= 3000)
                        {
                            tax = interval * 0.03 - deductTaxSum - baseTax;
                        }
                        //超过3000至12000元，税率10%，速算扣除数210
                        else if (interval > 3000 && interval <= 12000)
                        {
                            tax = interval * 0.1 - 210 - deductTaxSum - baseTax;
                        }
                        //超过12000至25000元，税率20%，速算扣除数1410
                        else if (interval > 12000 && interval <= 25000)
                        {
                            tax = interval * 0.2 - 1410 - deductTaxSum - baseTax;
                        }
                        //超过25000至35000元，税率25%，速算扣除数2660
                        else if (interval > 25000 && interval <= 35000)
                        {
                            tax = interval * 0.25 - 2660 - deductTaxSum - baseTax;
                        }
                        //超过35000至55000元，税率30%，速算扣除数4410
                        else if (interval > 35000 && interval <= 55000)
                        {
                            tax = interval * 0.3 - 4410 - deductTaxSum - baseTax;
                        }
                        //超过55000至80000元，税率35%，速算扣除数7160
                        else if (interval > 55000 && interval <= 80000)
                        {
                            tax = interval * 0.35 - 7160 - deductTaxSum - baseTax;
                        }
                        //超过80000元，税率45%，速算扣除数15160
                        else if (interval > 80000)
                        {
                            tax = interval * 0.45 - 15160 - deductTaxSum - baseTax;
                        }
                        amountX = model.Amount - tax;
                        amountY = model.Amount;
                    }
                    else if (model.TaxOrNot.Equals("不含税"))
                    {
                        double interval_1 = model.Amount + amount + baseSalary - baseTax;
                        double tax_1 = 0.0;
                        //first step
                        if (interval_1 <= 0)
                        {
                            tax_1 = 0.0;
                        }

                        //不超过2910元，税率3%，速算扣除数0
                        else if (interval_1 > 0 && interval_1 <= 2910)
                        {                           
                            
                            tax_1 = interval_1 / (1 - 0.03);
                        }

                        //超过2910至11010元，税率10%，速算扣除数210
                        else if (interval_1 > 2910 && interval_1 <= 11010)
                        {
                            tax_1 = (interval_1 - 210) / (1 - 0.1);
                           
                        }

                        //超过11010至21410元，税率20%，速算扣除数1410
                        else if (interval_1 > 11010 && interval_1 <= 21410)
                        {
                            tax_1 = (interval_1 - 1410) / (1 - 0.2);
                           
                        }

                        //超过21410至28910元，税率25%，速算扣除数2660
                        else if (interval_1 > 21410 && interval_1 <= 28910)
                        {
                            tax_1 = (interval_1 - 2660) / (1 - 0.25);
                            
                        }

                        //超过28910至42910元，税率30%，速算扣除数4410
                        else if (interval_1 > 28910 && interval_1 <= 42910)
                        {
                            tax_1 = (interval_1 - 4410) / (1 - 0.3);
                            
                        }

                        //超过42910至59160元，税率35%，速算扣除数7160
                        else if (interval_1 > 42910 && interval_1 <= 59160)
                        {
                            tax_1 = (interval_1 - 7160) / (1 - 0.35);
                           
                        }

                        //超过59160元，税率45%，速算扣除数15160
                        else if (interval_1 > 59160)
                        {
                            tax_1 = (interval_1 - 15160) / (1 - 0.45);
                           
                        }


                        //secend step

                        if (tax_1 <= 0)
                        {
                            tax = 0.0;
                        }
                        //不超过3000元，税率3%，速算扣除数0
                        else if (tax_1 > 0 && tax_1 <= 3000)
                        {
                            tax = tax_1 * 0.03 - baseTax - deductTaxSum;
                        }
                        //超过3000至12000元，税率10%，速算扣除数210
                        else if (tax_1 > 3000 && tax_1 <= 12000)
                        {
                            tax = tax_1 * 0.1 - baseTax - 210 - deductTaxSum;
                        }
                        //超过12000至25000元，税率20%，速算扣除数1410
                        else if (tax_1 > 12000 && tax_1 <= 25000)
                        {
                            tax = tax_1 * 0.2 - baseTax - 1410 - deductTaxSum;
                        }
                        //超过25000至35000元，税率25%，速算扣除数2660
                        else if (tax_1 > 25000 && tax_1 <= 35000)
                        {
                            tax = tax_1 * 0.25 - baseTax - 2660 - deductTaxSum;
                        }
                        //超过35000至55000元，税率30%，速算扣除数4410
                        else if (tax_1 > 35000 && tax_1 <= 55000)
                        {
                            tax = tax_1 * 0.3 - baseTax - 4410 - deductTaxSum;
                        }
                        //超过55000至80000元，税率35%，速算扣除数7160
                        else if (tax_1 > 55000 && tax_1 <= 80000)
                        {
                            tax = tax_1 * 0.35 - baseTax - 7160 - deductTaxSum;
                        }
                        //超过80000元，税率45%，速算扣除数15160
                        else if (tax_1 > 80000)
                        {
                            tax = tax_1 * 0.45 - baseTax - 15160 - deductTaxSum;
                        }
                        amountX = model.Amount;
                        amountY = model.Amount + tax;
                    }
                }
                else if (model.PersonType.Equals("所外"))
                {
                    //按照劳务进行算税
                    //1.查询已发放总金额
                    double amount = GetPayTaxAmount(model.CertificateID, model.TaxOrNot);
                    // double amount = 0.0;
                    double deductTaxSum = GetDeductTaxSum(model.CertificateID);
                    // double deductTaxSum = 0.0;
                    //2.根据已发放总金额，判断算税公式
                    double baseSalary = 800;
                    double interval = model.Amount + amount;
                    //3.计算Tax(税额),AmountX(税后),AmountY(税前)
                    if (model.TaxOrNot.Equals("含税"))
                    {
                        if (interval <= 800)
                        {
                            tax = 0.0;
                        }
                        //超过800至4000元，税率20%，速算扣除数0
                        else if (interval > 800 && interval <= 4000)
                        {
                            tax = (interval - baseSalary) * 0.2 - deductTaxSum;
                        }
                        //超过4000至25000元，税率20%，速算扣除数0
                        else if (interval > 4000 && interval <= 20000)
                        {
                            tax = (interval - interval * 0.2) * 0.2 - deductTaxSum;
                        }
                        //超过25000至62500元，税率30%，速算扣除数2000
                        else if (interval > 20000 && interval <= 50000)
                        {
                            tax = (interval - interval * 0.2) * 0.3 - 2000 - deductTaxSum;
                        }
                        //超过62500元，税率40%，速算扣除数7000
                        else if (interval > 50000)
                        {
                            tax = (interval - interval * 0.2) * 0.4 - 7000 - deductTaxSum;
                        }
                        amountX = model.Amount - tax;
                        amountY = model.Amount;
                    }
                    else if (model.TaxOrNot.Equals("不含税"))
                    {
                        if (interval <= 800)
                        {
                            tax = 0.0;
                        }
                        //超过800至4000元，税率20%，速算扣除数0
                        else if (interval > 800 && interval <= 3360)
                        {
                            tax = ((interval - baseSalary) / (1 - 0.2)) * 0.2 - deductTaxSum;
                        }
                        //超过4000至25000元，税率20%，速算扣除数0
                        else if (interval > 3360 && interval <= 21000)
                        {
                            tax = (interval * (1 - 0.2) * 0.2) / (1 - (1 - 0.2) * 0.2) - deductTaxSum;
                        }
                        //超过25000至62500元，税率30%，速算扣除数2000
                        else if (interval > 21000 && interval <= 49500)
                        {
                            tax = (interval * (1 - 0.2) * 0.3 - 2000) / (1 - (1 - 0.2) * 0.3) - deductTaxSum;
                        }
                        //超过62500元，税率40%，速算扣除数7000
                        else if (interval > 49500)
                        {
                            tax = (interval * (1 - 0.2) * 0.4 - 7000) / (1 - (1 - 0.2) * 0.4) - deductTaxSum;
                        }
                        amountX = model.Amount;
                        amountY = model.Amount + tax;
                    }

                }

                //5.存储该纪录
                var entity = new TaxPerOrder
                {
                    SerialNumber = model.SerialNumber,
                    ProjectNumber = model.ProjectNumber,
                    TaskName = model.TaskName,
                    RefundType = model.RefundType,
                    ProjectDirector = model.ProjectDirector,
                    Name = model.Name,
                    Agent = model.Agent,
                    PersonType = model.PersonType,
                    CertificateType = model.CertificateType,
                    CertificateID = model.CertificateID,
                    Amount = model.Amount,
                    TaxOrNot = model.TaxOrNot,
                    Tax = tax,
                    Bank = model.Bank,
                    BankDetailName = model.BankDetailName,
                    AccountName = model.AccountName,
                    AccountNumber = model.AccountNumber,
                    PaymentType = model.PaymentType,
                    AmountY = amountY,
                    AmountX = amountX,
                    UpdateDate = DateTime.Now
                };
                _TaxPerOrderRepository.Insert(entity, isSave);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败：" + ex.Message);
            }
        }

         */
         
        public OperationResult Insert(TaxPerOrder model, bool isSave)
        {
            try
            {
                //TaxPerOrder taxPerOrder = _TaxPerOrderRepository.Entities.FirstOrDefault(c => c.SerialNumber == model.SerialNumber && c.SerialNumber == model.CertificateID.Trim());
                //if (taxPerOrder != null)
                //{
                //    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的税单纪录，请修改后重新提交！");
                //}
                //if (model.CertificateID == null || model.CertificateID.Trim() == "")
                //    return new OperationResult(OperationResultType.Warning, "证件号码不能为空，请修改后重新提交！");
                double tax = 0.0;          //最终税额
                double amountX = 0.0;     //最终税后数（不含税）
                double amountY = 0.0;       //最终税前数（含税）
                //计算税金，并存储
                if (model.PersonType.Equals("所内"))
                {
                    //按照工资进行算税
                    //2019.4.12
                    
                    String period_year =DateTime.Now.Year.ToString();
                    TaxBaseEveryMonth taxBaseEveryMonth = _TaxBaseEveryMonthRepository.GetExistRecord(period_year, model.CertificateID);
                    //含税
                    //本次劳务应纳税所得额taxableIncome=当次劳务税前收入额model.Amount
                    //+本期应纳税所得额TaxBaseByMonth.InitialTaxPayable
                    //+年度累计应纳税所得额TaxBaseEveryMonth.InitialTaxPayable
                    //+本期已计算劳务税前收入总额GetPayTaxAmount(model.CertificateID, model.TaxOrNot)//含税

                    //第一步：本次劳务应纳税所得额taxableIncome=当次劳务税前收入额model.Amount
                    //+本期已计算劳务税前收入总额GetPayTaxAmount(model.CertificateID, model.TaxOrNot)//含税
                    //+（本期初始税前收入额TaxBaseByMonth.InitialEaring—本期免税收入TaxFreeIncome
                    //—本期养老保险—本期失业保险—本期医疗保险—本期职业年金—本期住房公积金
                    //—本期基本扣除—本期专项附加扣除）part1
                    //+（年度累计税前收入InitalEaring—年度累计免税收入TaxFreeIncome
                    //—年度累计养老保险—年度累计失业保险—年度累计医疗保险—年度累计职业年金—年度累计住房公积金
                    //—年度累计基本扣除—年度累计专项附加扣除）part2
                    double part1 = _TaxBaseByMonthRepository.GetPart1(model.CertificateID);
                    double part2 = 0.0;
                    if (taxBaseEveryMonth != null) { 
                        part2 = taxBaseEveryMonth.InitialEaring - taxBaseEveryMonth.TaxFreeIncome - taxBaseEveryMonth.EndowmentInsurance - taxBaseEveryMonth.UnemployedInsurance - taxBaseEveryMonth.MedicalInsurance - taxBaseEveryMonth.OccupationalAnnuity - taxBaseEveryMonth.HousingFund - taxBaseEveryMonth.AmountDeducted - taxBaseEveryMonth.SpecialDeduction;
                    }
                    //1.查询本期已计算劳务含税/不含税收入总额                                  
                    double amountTotal = GetPayTaxAmount(model.CertificateID, model.TaxOrNot);
                    //2.本期应纳税所得额
                    double InitialTaxPayable = _TaxBaseByMonthRepository.GetInitialTaxPayable(model.CertificateID);
                    //3.年度累计应纳税所得额
                    double InitialTaxPayableInYear = 0.0;
                    //4.年度累计已扣缴税额
                    double TotalTaxInYear = 0.0;
                    if(taxBaseEveryMonth != null){
                         InitialTaxPayableInYear = taxBaseEveryMonth.InitialTaxPayable;
                         TotalTaxInYear = taxBaseEveryMonth.TotalTax;
                    }
                    //5.本期已计算劳务税额总额
                    double deductTaxSum = GetDeductTaxSum(model.CertificateID);
                    //6.本期初始已扣缴税额
                    double InitialTax = _TaxBaseByMonthRepository.GetInitialTax(model.CertificateID);
                    //7.本次劳务应纳税额tax=（本次应纳税所得额taxableIncome*税率
                    //-速算扣除数）-本期初始已扣缴税额TaxBaseByMonth.InitialTax
                    //—年度累计已扣缴税额TaxBaseEveryMonth.TotalTax
                    //—本期已计算劳务税额总额deductTaxSum
                   
                    if (model.TaxOrNot.Equals("含税")){
                         double taxableIncome = model.Amount + part1 + part2 + amountTotal;
                         //比较税率
                         //不超过36000元，税率3%，速算扣除数0
                         if (taxableIncome <= 36000) {
                             tax = taxableIncome * 0.03 - InitialTax - TotalTaxInYear - deductTaxSum;
                         }
                         //超过36000至144000元，税率10%，速算扣除数2520
                         else if (taxableIncome > 36000 && taxableIncome <= 144000)
                         {
                             tax = taxableIncome * 0.1 - 2520 - InitialTax - TotalTaxInYear - deductTaxSum;
                         }
                         //超过144000至300000元，税率20%，速算扣除数16920
                         else if (taxableIncome > 144000 && taxableIncome <= 300000)
                         {
                             tax = taxableIncome * 0.2 - 16920 - InitialTax - TotalTaxInYear - deductTaxSum;
                         }
                         //超过300000至420000元，税率25%，速算扣除数31920
                         else if (taxableIncome > 300000 && taxableIncome <= 420000)
                         {
                             tax = taxableIncome * 0.25 - 31920 - InitialTax - TotalTaxInYear - deductTaxSum;
                         }
                         //超过420000至660000元，税率30%，速算扣除数52920
                         else if (taxableIncome > 420000 && taxableIncome <= 660000)
                         {
                             tax = taxableIncome * 0.3 - 52920 - InitialTax - TotalTaxInYear - deductTaxSum;
                         }
                         //超过660000至960000元，税率35%，速算扣除数85920
                         else if (taxableIncome > 660000 && taxableIncome <= 960000)
                         {
                             tax = taxableIncome * 0.35 - 85920 - InitialTax - TotalTaxInYear - deductTaxSum;
                         }
                         //超过960000元，税率45%，速算扣除数181920
                         else if (taxableIncome > 960000)
                         {
                             tax = taxableIncome * 0.45 - 181920 - InitialTax - TotalTaxInYear - deductTaxSum;
                         }
                         
                         if (tax < 0)
                         {
                             tax = 0.0;
                         }
                         amountX = model.Amount - tax;
                         amountY = model.Amount;

                     }
                    else if (model.TaxOrNot.Equals("不含税")) {
                        //【当次劳务税后收入额model.Amount+本期已计算劳务税后收入总额amountTotal
                        //+（本期初始税后收入额—本期免税收入 —本期养老保险—本期失业保险
                        //—本期医疗保险—本期职业年金—本期住房公积金—本期基本扣除
                        //—本期专项附加扣除）
                        //+（年度累计税后收入—年度累计免税收入—年度累计养老保险
                        //—年度累计失业保险—年度累计医疗保险—年度累计职业年金
                        // —年度累计住房公积金—年度累计基本扣除—年度累计专项附加扣除）】

                        //1.（本期初始税后收入额—本期免税收入 —本期养老保险—本期失业保险
                        //—本期医疗保险—本期职业年金—本期住房公积金—本期基本扣除
                        //—本期专项附加扣除）
                        double withoutInsurance = _TaxBaseByMonthRepository.GetWithoutInsurance(model.CertificateID);
                        //2.（年度累计税后收入—年度累计免税收入—年度累计养老保险
                        //—年度累计失业保险—年度累计医疗保险—年度累计职业年金
                        // —年度累计住房公积金—年度累计基本扣除—年度累计专项附加扣除）
                        double withoutInsuranceInYear = 0.0;
                        if (taxBaseEveryMonth != null)
                        {
                            withoutInsuranceInYear = taxBaseEveryMonth.TotalTemp - taxBaseEveryMonth.TaxFreeIncome - taxBaseEveryMonth.EndowmentInsurance - taxBaseEveryMonth.UnemployedInsurance - taxBaseEveryMonth.OccupationalAnnuity - taxBaseEveryMonth.HousingFund - taxBaseEveryMonth.MedicalInsurance - taxBaseEveryMonth.AmountDeducted - taxBaseEveryMonth.SpecialDeduction;
                        }
                        //3.中间对比不含税级距值
                        double interval_1 = model.Amount + amountTotal + withoutInsurance + withoutInsuranceInYear;
                        //4.不含税级距值
                        //本次劳务应纳税所得额taxableIncome1={interval_1—累计速算扣除数}/（1-税率）
                        double taxableIncome1 = 0.0;

                        //不超过34920元，税率3%，速算扣除数0
                        if (interval_1 <= 34920)
                        {

                            taxableIncome1 = interval_1 / (1 - 0.03);
                        }

                        //超过34920至132120元，税率10%，速算扣除数2520
                        else if (interval_1 > 34920 && interval_1 <= 132120)
                        {
                            taxableIncome1 = (interval_1 - 2520) / (1 - 0.1);

                        }

                        //超过132120至256920元，税率20%，速算扣除数16920
                        else if (interval_1 > 132120 && interval_1 <= 256920)
                        {
                            taxableIncome1 = (interval_1 - 16920) / (1 - 0.2);

                        }

                        //超过21410至346920元，税率25%，速算扣除数31920
                        else if (interval_1 > 256920 && interval_1 <= 346920)
                        {
                            taxableIncome1 = (interval_1 - 31920) / (1 - 0.25);

                        }

                        //超过346920至514920元，税率30%，速算扣除数52920
                        else if (interval_1 > 346920 && interval_1 <= 514920)
                        {
                            taxableIncome1 = (interval_1 - 52920) / (1 - 0.3);

                        }

                        //超过514920至709920元，税率35%，速算扣除数85920
                        else if (interval_1 > 514920 && interval_1 <= 709920)
                        {
                            taxableIncome1 = (interval_1 - 85920) / (1 - 0.35);

                        }

                        //超过709920元，税率45%，速算扣除数181920
                        else if (interval_1 > 709920)
                        {
                            taxableIncome1 = (interval_1 - 181920) / (1 - 0.45);

                        }


                        //4.本次劳务应纳税额=（本次劳务应纳税所得额*税率—速算扣除数）
                        //—本期初始已扣缴税额—年度累计已扣缴税额—本期已计算劳务税额总额

                        //5.第二次税率级距判断
                        //不超过36000元，税率3%，速算扣除数0
                        if (taxableIncome1 <= 36000)
                        {
                            tax = taxableIncome1 * 0.03 - InitialTax - TotalTaxInYear - deductTaxSum;
                        }
                        //超过36000至144000元，税率10%，速算扣除数2520
                        else if (taxableIncome1 > 36000 && taxableIncome1 <= 144000)
                        {
                            tax = taxableIncome1 * 0.1 - 2520 - InitialTax - TotalTaxInYear - deductTaxSum;
                        }
                        //超过144000至300000元，税率20%，速算扣除数16920
                        else if (taxableIncome1 > 144000 && taxableIncome1 <= 300000)
                        {
                            tax = taxableIncome1 * 0.2 - 16920 - InitialTax - TotalTaxInYear - deductTaxSum;
                        }
                        //超过300000至420000元，税率25%，速算扣除数31920
                        else if (taxableIncome1 > 300000 && taxableIncome1 <= 420000)
                        {
                            tax = taxableIncome1 * 0.25 - 31920 - InitialTax - TotalTaxInYear - deductTaxSum;
                        }
                        //超过420000至660000元，税率30%，速算扣除数52920
                        else if (taxableIncome1 > 420000 && taxableIncome1 <= 660000)
                        {
                            tax = taxableIncome1 * 0.3 - 52920 - InitialTax - TotalTaxInYear - deductTaxSum;
                        }
                        //超过660000至960000元，税率35%，速算扣除数85920
                        else if (taxableIncome1 > 660000 && taxableIncome1 <= 960000)
                        {
                            tax = taxableIncome1 * 0.35 - 85920 - InitialTax - TotalTaxInYear - deductTaxSum;
                        }
                        //超过960000元，税率45%，速算扣除数181920
                        else if (taxableIncome1 > 960000)
                        {
                            tax = taxableIncome1 * 0.45 - 181920 - InitialTax - TotalTaxInYear - deductTaxSum;
                        }

                        if (tax < 0)
                        {
                            tax = 0.0;
                        }
                        amountX = model.Amount;
                        amountY = model.Amount + tax;
                    }                  

                }
                else if (model.PersonType.Equals("所外"))
                {
                    //按照劳务进行算税
                    //1.查询已发放总金额
                    double amount = GetPayTaxAmount(model.CertificateID, model.TaxOrNot);
                    // double amount = 0.0;
                    double deductTaxSum = GetDeductTaxSum(model.CertificateID);
                    // double deductTaxSum = 0.0;
                    //2.根据已发放总金额，判断算税公式
                    double baseSalary = 800;
                    double interval = model.Amount + amount;
                    //3.计算Tax(税额),AmountX(税后),AmountY(税前)
                    if (model.TaxOrNot.Equals("含税"))
                    {
                        if (interval <= 800)
                        {
                            tax = 0.0;
                        }
                        //超过800至4000元，税率20%，速算扣除数0
                        else if (interval > 800 && interval <= 4000)
                        {
                            tax = (interval - baseSalary) * 0.2 - deductTaxSum;
                        }
                        //超过4000至25000元，税率20%，速算扣除数0
                        else if (interval > 4000 && interval <= 20000)
                        {
                            tax = (interval - interval * 0.2) * 0.2 - deductTaxSum;
                        }
                        //超过25000至62500元，税率30%，速算扣除数2000
                        else if (interval > 20000 && interval <= 50000)
                        {
                            tax = (interval - interval * 0.2) * 0.3 - 2000 - deductTaxSum;
                        }
                        //超过62500元，税率40%，速算扣除数7000
                        else if (interval > 50000)
                        {
                            tax = (interval - interval * 0.2) * 0.4 - 7000 - deductTaxSum;
                        }
                        amountX = model.Amount - tax;
                        amountY = model.Amount;
                    }
                    else if (model.TaxOrNot.Equals("不含税"))
                    {
                        if (interval <= 800)
                        {
                            tax = 0.0;
                        }
                        //超过800至4000元，税率20%，速算扣除数0
                        else if (interval > 800 && interval <= 3360)
                        {
                            tax = ((interval - baseSalary) / (1 - 0.2)) * 0.2 - deductTaxSum;
                        }
                        //超过4000至25000元，税率20%，速算扣除数0
                        else if (interval > 3360 && interval <= 21000)
                        {
                            tax = (interval * (1 - 0.2) * 0.2) / (1 - (1 - 0.2) * 0.2) - deductTaxSum;
                        }
                        //超过25000至62500元，税率30%，速算扣除数2000
                        else if (interval > 21000 && interval <= 49500)
                        {
                            tax = (interval * (1 - 0.2) * 0.3 - 2000) / (1 - (1 - 0.2) * 0.3) - deductTaxSum;
                        }
                        //超过62500元，税率40%，速算扣除数7000
                        else if (interval > 49500)
                        {
                            tax = (interval * (1 - 0.2) * 0.4 - 7000) / (1 - (1 - 0.2) * 0.4) - deductTaxSum;
                        }
                        amountX = model.Amount;
                        amountY = model.Amount + tax;
                    }

                }

                //5.存储该纪录
                var entity = new TaxPerOrder
                {
                    SerialNumber = model.SerialNumber,
                    ProjectNumber = model.ProjectNumber,
                    TaskName = model.TaskName,
                    RefundType = model.RefundType,
                    ProjectDirector = model.ProjectDirector,
                    Name = model.Name,
                    Agent = model.Agent,
                    PersonType = model.PersonType,
                    CertificateType = model.CertificateType,
                    CertificateID = model.CertificateID,
                    Amount = model.Amount,
                    TaxOrNot = model.TaxOrNot,
                    Tax = tax,
                    Bank = model.Bank,
                    BankDetailName = model.BankDetailName,
                    ProvinceCity = model.ProvinceCity,
                    AccountName = model.AccountName,
                    AccountNumber = model.AccountNumber,
                    PaymentType = model.PaymentType,
                    AmountY = amountY,
                    AmountX = amountX,
                    UpdateDate = DateTime.Now
                };
                _TaxPerOrderRepository.Insert(entity, isSave);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败：" + ex.Message);
            }
        }

        public OperationResult Update(TaxPerOrderVM model, bool isSave)
        {
            try
            {
                TaxPerOrder taxPerOrder = _TaxPerOrderRepository.Entities.FirstOrDefault(c => c.CertificateID.ToLower() == model.CertificateID.Trim().ToLower());
                if (taxPerOrder == null)
                {
                    throw new Exception();
                }
                taxPerOrder.SerialNumber = model.SerialNumber;
                taxPerOrder.ProjectNumber = model.ProjectNumber;
                taxPerOrder.TaskName = model.TaskName;
                taxPerOrder.RefundType = model.RefundType;
                taxPerOrder.ProjectDirector = model.ProjectDirector;
                taxPerOrder.Agent = model.Agent;
                taxPerOrder.Name = model.Name;
                taxPerOrder.PersonType = model.PersonType;
                taxPerOrder.CertificateType = model.CertificateType;
                taxPerOrder.CertificateID = model.CertificateID;
                taxPerOrder.Amount = model.Amount;
                taxPerOrder.TaxOrNot = model.TaxOrNot;
                taxPerOrder.Tax = model.Tax;
                taxPerOrder.Bank = model.Bank;
                taxPerOrder.BankDetailName = model.BankDetailName;
                taxPerOrder.ProvinceCity = model.ProvinceCity;
                taxPerOrder.AccountName = model.AccountName;
                taxPerOrder.AccountNumber = model.AccountNumber;
                taxPerOrder.AmountY = model.AmountY;
                taxPerOrder.AmountX = model.AmountX;
                taxPerOrder.UpdateDate = DateTime.Now;
                _TaxPerOrderRepository.Update(taxPerOrder, isSave);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> serialNumber, bool isSave)
        {
            try
            {
                if (serialNumber != null)
                {
                    int count = _TaxPerOrderRepository.Delete(_TaxPerOrderRepository.Entities.Where(c => serialNumber.Contains(c.SerialNumber)), isSave);
                    if (count > 0)
                    {
                        return new OperationResult(OperationResultType.Success, "删除数据成功！");
                    }
                    else
                    {
                        return new OperationResult(OperationResultType.Error, "删除数据失败!");
                    }
                }
                else
                {
                    return new OperationResult(OperationResultType.ParamError, "参数错误，请选择需要删除的数据!");
                }
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "删除数据失败!");
            }
        }
        public OperationResult Update(TaxPerOrder model, bool isSave)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaxPerOrderRepository.Update(model, isSave);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(TaxPerOrder model, bool isSave)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaxPerOrderRepository.Delete(model, isSave);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<TaxPerOrder>(fileName, columns);
                _TaxPerOrderRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }
        public Double GetPayTaxAmount(String certificateID, String taxOrNot)
        {
            return _TaxPerOrderRepository.GetPayTaxAmount(certificateID, taxOrNot);
        }

        public Double GetPayTaxAmountY(String certificateID)
        {
            return _TaxPerOrderRepository.GetPayTaxAmountY(certificateID);
        }

        public Double GetDeductTaxSum(String certificateID)
        {
            return _TaxPerOrderRepository.GetDeductTaxSum(certificateID);
        }

        public int GetCashCount(String certificateID)
        {
            return _TaxPerOrderRepository.GetCashCount(certificateID);
        }

        public int DeleteBySerialNumber(String serialNumber)
        {
            int count = 0;
            if (serialNumber != null)
            {
                count = _TaxPerOrderRepository.Delete(_TaxPerOrderRepository.Entities.Where(c => serialNumber.Contains(c.SerialNumber)));
            }
            return count;
        }

        public OperationResult DeleteAll()
        {
            try
            {
                _TaxPerOrderRepository.Delete(_TaxPerOrderRepository.Entities);
                return new OperationResult(OperationResultType.Success, "清空数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "清空数据失败!");
            }
        }
    }
}
