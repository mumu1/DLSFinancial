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
    public class TaxBaseByMonthService : CoreServiceBase, ITaxBaseByMonthService
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITaxBaseByMonthRepository _TaxBaseByMonthRepository;

        public TaxBaseByMonthService(ITaxBaseByMonthRepository taxBaseByMonthRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TaxBaseByMonthRepository = taxBaseByMonthRepository;
        }
        public IQueryable<TaxBaseByMonth> TaxBaseByMonths
        {
            get { return _TaxBaseByMonthRepository.Entities; }
        }

        public OperationResult Insert(TaxBaseByMonthVM model)
        {
            try
            {
                TaxBaseByMonth taxBaseByMonth = _TaxBaseByMonthRepository.Entities.FirstOrDefault(c => c.CertificateID == model.CertificateID.Trim());
                if (taxBaseByMonth != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的基本工资信息，请修改后重新提交！");
                }
                if (model.Name == null || model.Name.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "姓名不能为空，请修改后重新提交！");
                var entity = new TaxBaseByMonth
                {
                    Period = model.Period,
                    Name = model.Name,
                    CertificateType = model.CertificateType,
                    CertificateID = model.CertificateID,
                    InitialEaring = model.InitialEaring,
                    TaxFree = model.TaxFree,
                    AmountDeducted = model.AmountDeducted,
                    InitialTaxPayable = model.InitialTaxPayable,
                    InitialTax = model.InitialTax,
                    //PersonType = model.PersonType,
                    //Title = model.Title,
                    UpdateDate = DateTime.Now
                };
                _TaxBaseByMonthRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(TaxBaseByMonthVM model)
        {
            try
            {
                TaxBaseByMonth taxBaseByMonth = _TaxBaseByMonthRepository.Entities.FirstOrDefault(c => c.CertificateID == model.CertificateID.Trim());
                if (taxBaseByMonth == null)
                {
                    throw new Exception();
                }
                taxBaseByMonth.Period = model.Period;
                taxBaseByMonth.Name = model.Name;
                taxBaseByMonth.CertificateType = model.CertificateType;
                taxBaseByMonth.CertificateID = model.CertificateID;
                taxBaseByMonth.InitialEaring = model.InitialEaring;
                taxBaseByMonth.InitialTax = model.InitialTax;
                taxBaseByMonth.InitialTaxPayable = model.InitialTaxPayable;
                taxBaseByMonth.AmountDeducted = model.AmountDeducted;
                //taxBaseByMonth.PersonType = model.PersonType;
                taxBaseByMonth.TaxFree = model.TaxFree;
                //taxBaseByMonth.Title = model.Title;
                taxBaseByMonth.UpdateDate = DateTime.Now;
                _TaxBaseByMonthRepository.Update(taxBaseByMonth);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> certificateID)
        {
            try
            {
                if (certificateID != null)
                {
                    int count = _TaxBaseByMonthRepository.Delete(_TaxBaseByMonthRepository.Entities.Where(c => certificateID.Contains(c.CertificateID)));
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
        public OperationResult Update(TaxBaseByMonth model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaxBaseByMonthRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(TaxBaseByMonth model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaxBaseByMonthRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ImportData importData)
        {
            try
            {
                var columns = importData == null ? null : importData.Columns;
                var maps = ImportUtil.GetColumns(columns, new TaxBaseByMonth());
                var items = ExcelService.GetObjects(fileName, columns);
                if (importData != null)
                {
                    String serialNumber = importData.Parameters[0].Value;
                    String paymentType = importData.Parameters[1].Value;
                    int num = 1;
                    foreach (var item in items)
                    {
                        TaxBaseByMonth record = new TaxBaseByMonth();
                        List<ImportFeedBack> errors = ImportUtil.ValidateImportRecord(item, num++, maps, ref record);
                        if (errors.Count > 0)
                        {
                            return new OperationResult(OperationResultType.Error, "导入数据失败", ImportUtil.ParseToHtml(errors));
                        }

                        //插入或更新数据
                        _TaxBaseByMonthRepository.InsertOrUpdate(record);
                    }
                }

                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ImportFeedBack feedBack = new ImportFeedBack();
                feedBack.ExceptionType = "未知错误";
                feedBack.ExceptionContent.Add(ex.Message);
                List<ImportFeedBack> erros = new List<ImportFeedBack>();
                return new OperationResult(OperationResultType.Error, "导入数据失败!", ImportUtil.ParseToHtml(new List<ImportFeedBack>() { feedBack }));
            }
        }

        

        //public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        //{
        //    try
        //    {
        //        var items = ExcelService.GetObjects<TaxBaseByMonth>(fileName, columns);
        //        _TaxBaseByMonthRepository.InsertOrUpdate(items);
        //        return new OperationResult(OperationResultType.Success, "导入数据成功！");
        //    }
        //    catch (Exception ex)
        //    {
        //        return new OperationResult(OperationResultType.Error, "导入数据失败!");
        //    }
        //}

        public Double GetBaseSalary(String certificateID) {
            Double baseSalary = _TaxBaseByMonthRepository.GetBaseSalary(certificateID);
            return baseSalary;
        }

        public String GetNameByCerID(String certificateID) {
            String name = _TaxBaseByMonthRepository.GetNameByCerID(certificateID);
            return name;
        }

        public OperationResult DeleteAll() {
            try
            {               
                _TaxBaseByMonthRepository.Delete(_TaxBaseByMonthRepository.Entities);
                return new OperationResult(OperationResultType.Success, "清空数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "清空数据失败!");
            }
        }
    }
}
