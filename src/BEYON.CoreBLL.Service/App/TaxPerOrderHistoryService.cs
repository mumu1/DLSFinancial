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
    public class TaxPerOrderHistoryService : CoreServiceBase, ITaxPerOrderHistoryService
    {
        private readonly ITaxPerOrderHistoryRepository _TaxPerOrderHistoryRepository;

        public TaxPerOrderHistoryService(ITaxPerOrderHistoryRepository taxPerOrderHistoryRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TaxPerOrderHistoryRepository = taxPerOrderHistoryRepository;
        }
        public IQueryable<TaxPerOrderHistory> TaxPerOrderHistorys
        {
            get { return _TaxPerOrderHistoryRepository.Entities; }
        }

        public OperationResult Insert(TaxPerOrderHistory model, bool isSave)
        {

            try
            {
                //TaxPerOrderHistory history = _TaxPerOrderHistoryRepository.Entities.FirstOrDefault(c => c.Id == model.Id);
                //if (history != null)
                //{
                    //_TaxPerOrderHistoryRepository.Update(model, true);
                //}
                _TaxPerOrderHistoryRepository.Insert(model,true);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
               
        }     

        public OperationResult Delete(List<string> serialNumber, bool isSave)
        {
            try
            {
                if (serialNumber != null)
                {
                    int count = _TaxPerOrderHistoryRepository.Delete(_TaxPerOrderHistoryRepository.Entities.Where(c => serialNumber.Contains(c.SerialNumber)), isSave);
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
        public OperationResult Update(TaxPerOrderHistory model, bool isSave)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaxPerOrderHistoryRepository.Update(model, isSave);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(TaxPerOrderHistory model, bool isSave)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaxPerOrderHistoryRepository.Delete(model, isSave);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }


        public void InsertOrUpdate(TaxPerOrderHistory contact)
        {
            this._TaxPerOrderHistoryRepository.InsertOrUpdate(contact);
        }
    }
}
