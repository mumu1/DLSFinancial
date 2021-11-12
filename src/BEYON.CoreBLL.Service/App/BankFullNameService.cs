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
    public class BankFullNameService : CoreServiceBase, IBankFullNameService
    {
        private readonly IBankFullNameRepository _BankFullNameRepository;



        public BankFullNameService(IBankFullNameRepository bankFullNameRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._BankFullNameRepository = bankFullNameRepository;
        }
        public IQueryable<BankFullName> BankFullNames
        {
            get { return _BankFullNameRepository.Entities; }
        }

        public OperationResult Insert(BankFullNameVM model)
        {
            try
            {
                BankFullName bankFullName = _BankFullNameRepository.Entities.FirstOrDefault(c => c.BankFullCode == model.BankFullCode.Trim());
                if (bankFullName != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的银行全称信息，请修改后重新提交！");
                }
                if (model.BankFullNameAddr == null || model.BankFullNameAddr.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "银行全称不能为空，请修改后重新提交！");
                var entity = new BankFullName
                {
                    BankFullCode = model.BankFullCode,
                    BankFullNameAddr = model.BankFullNameAddr,
                    UpdateDate = DateTime.Now
                };
                _BankFullNameRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(BankFullNameVM model)
        {
            try
            {
                BankFullName bankFullName = _BankFullNameRepository.Entities.FirstOrDefault(c => c.BankFullCode == model.BankFullCode.Trim());
                if (bankFullName == null)
                {
                    throw new Exception();
                }
                bankFullName.BankFullNameAddr = model.BankFullNameAddr;
                bankFullName.BankFullCode = model.BankFullCode;
                bankFullName.UpdateDate = DateTime.Now;
                _BankFullNameRepository.Update(bankFullName);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> bankFullCode)
        {
            try
            {
                if (bankFullCode != null)
                {
                    int count = _BankFullNameRepository.Delete(_BankFullNameRepository.Entities.Where(c => bankFullCode.Contains(c.BankFullCode)));
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
        public OperationResult Update(BankFullName model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _BankFullNameRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新银行全称数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新银行全称数据失败!");
            }
        }

        public OperationResult Delete(BankFullName model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _BankFullNameRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新银行全称数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新银行全称数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<BankFullName>(fileName, columns);
                _BankFullNameRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }

         public OperationResult DeleteAll()
        {
            try
            {
                _BankFullNameRepository.Delete(_BankFullNameRepository.Entities);
                return new OperationResult(OperationResultType.Success, "清空数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "清空数据失败!");
            }
        }
    }
    
}
