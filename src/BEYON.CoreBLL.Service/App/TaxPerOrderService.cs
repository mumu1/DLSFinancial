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



        public TaxPerOrderService(ITaxPerOrderRepository taxPerOrderRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TaxPerOrderRepository = taxPerOrderRepository;
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
                //计算税金，并存储
                if (model.PersonType.Equals("所内"))
                {
                    //按照工资进行算税
                    //1.查询已发放总金额
                    //2.根据已发放总金额，判断算税公式
                    //3.计算Tax(税额),AmountX(税后),AmountY(税前)
                    //4.存储该纪录
                }
                else if(model.PersonType.Equals("所外")){ 
                    //按照劳务进行算税
                    //1.查询已发放总金额
                    double amount = GetPayTaxAmount(model.CertificateID, model.TaxOrNot);
                    //2.根据已发放总金额，判断算税公式
                    //3.计算Tax(税额),AmountX(税后),AmountY(税前)
                    //4.存储该纪录
                }
                var entity = new TaxPerOrder
                {
                    SerialNumber = model.SerialNumber,
                    ProjectNumber = model.ProjectNumber,
                    TaskName = model.TaskName,
                    RefundType = model.RefundType,
                    ProjectDirector = model.ProjectDirector,
                    Agent = model.Agent,
                    PersonType = model.PersonType,
                    CertificateType = model.CertificateType,
                    CertificateID = model.CertificateID,
                    Amount = model.Amount,
                    TaxOrNot = model.TaxOrNot,
                    Tax = model.Tax,
                    Bank = model.Bank,
                    BankDetailName = model.BankDetailName,
                    AccountName = model.AccountName,
                    AccountNumber = model.AccountNumber,
                    PaymentType = model.PaymentType,
                    AmountY = model.AmountY,
                    AmountX = model.AmountX, 
                    UpdateDate = DateTime.Now
                };
                _TaxPerOrderRepository.Insert(entity, isSave);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch(Exception ex)
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
    }
}
