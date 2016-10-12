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
    public class BankService : CoreServiceBase, IBankService
    {
        private readonly IBankRepository _BankRepository;



        public BankService(IBankRepository bankRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._BankRepository = bankRepository;
        }
        public IQueryable<Bank> Banks
        {
            get { return _BankRepository.Entities; }
        }

        public OperationResult Insert(BankVM model)
        {
            try
            {
                Bank bank = _BankRepository.Entities.FirstOrDefault(c => c.BankCode == model.BankCode.Trim());
                if (bank != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的银行信息，请修改后重新提交！");
                }
                if (model.BankName == null || model.BankName.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "开户银行名称不能为空，请修改后重新提交！");
                var entity = new Bank
                {
                    BankCode = model.BankCode,
                    BankName = model.BankName,
                    UpdateDate = DateTime.Now
                };
                _BankRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(BankVM model)
        {
            try
            {
                Bank bank = _BankRepository.Entities.FirstOrDefault(c => c.BankCode == model.BankCode.Trim());
                if (bank == null)
                {
                    throw new Exception();
                }
                bank.BankName = model.BankName;
                bank.BankCode = model.BankCode;
                bank.UpdateDate = DateTime.Now;
                _BankRepository.Update(bank);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> bankCode)
        {
            try
            {
                if (bankCode != null)
                {
                    int count = _BankRepository.Delete(_BankRepository.Entities.Where(c => bankCode.Contains(c.BankCode)));
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
        public OperationResult Update(Bank model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _BankRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新开户银行数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新开户银行数据失败!");
            }
        }

        public OperationResult Delete(Bank model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _BankRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新开户银行数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新开户银行数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<Bank>(fileName, columns);
                _BankRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }
    }
}
