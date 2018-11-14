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



        public TaxPerOrderService(ITaxPerOrderRepository taxPerOrderRepository, ITaxBaseByMonthRepository taxBaseByMonthRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TaxPerOrderRepository = taxPerOrderRepository;
            this._TaxBaseByMonthRepository = taxBaseByMonthRepository;
        }
        public IQueryable<TaxPerOrder> TaxPerOrders
        {
            get { return _TaxPerOrderRepository.Entities; }
        }

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
        public OperationResult Update(TaxPerOrderVM model, bool isSave)
        {
            try
            {
                TaxPerOrder taxPerOrder = _TaxPerOrderRepository.Entities.FirstOrDefault(c => c.CertificateID == model.CertificateID.Trim());
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
