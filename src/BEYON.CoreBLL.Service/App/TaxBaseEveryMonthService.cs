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
    public class TaxBaseEveryMonthService : CoreServiceBase, ITaxBaseEveryMonthService
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITaxBaseEveryMonthRepository _TaxBaseEveryMonthRepository;

        public TaxBaseEveryMonthService(ITaxBaseEveryMonthRepository taxBaseEveryMonthRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TaxBaseEveryMonthRepository = taxBaseEveryMonthRepository;
        }
        public IQueryable<TaxBaseEveryMonth> TaxBaseEveryMonths
        {
            get { return _TaxBaseEveryMonthRepository.Entities; }
        }

        public OperationResult Insert(TaxBaseEveryMonthVM model)
        {
            try
            {
                TaxBaseEveryMonth taxBaseEveryMonth = _TaxBaseEveryMonthRepository.Entities.FirstOrDefault(c => c.CertificateID == model.CertificateID.Trim());
                if (taxBaseEveryMonth != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的基本工资信息，请修改后重新提交！");
                }
                if (model.Name == null || model.Name.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "姓名不能为空，请修改后重新提交！");
                var entity = new TaxBaseEveryMonth
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
                    SpecialDeduction = model.SpecialDeduction,
                    TotalIncome = model.TotalIncome,
                    TotalTax = model.TotalTax,
                    TotalTemp = model.TotalTemp,

                    //PersonType = model.PersonType,
                    //Title = model.Title,
                    UpdateDate = DateTime.Now
                };
                _TaxBaseEveryMonthRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(TaxBaseEveryMonthVM model)
        {
            try
            {
                TaxBaseEveryMonth taxBaseEveryMonth = _TaxBaseEveryMonthRepository.Entities.FirstOrDefault(c => c.CertificateID == model.CertificateID.Trim());
                if (taxBaseEveryMonth == null)
                {
                    throw new Exception();
                }
                taxBaseEveryMonth.Period = model.Period;
                taxBaseEveryMonth.Name = model.Name;
                taxBaseEveryMonth.CertificateType = model.CertificateType;
                taxBaseEveryMonth.CertificateID = model.CertificateID;
                taxBaseEveryMonth.InitialEaring = model.InitialEaring;
                taxBaseEveryMonth.InitialTax = model.InitialTax;
                taxBaseEveryMonth.InitialTaxPayable = model.InitialTaxPayable;
                taxBaseEveryMonth.AmountDeducted = model.AmountDeducted;
                //taxBaseByMonth.PersonType = model.PersonType;
                taxBaseEveryMonth.TaxFree = model.TaxFree;
                taxBaseEveryMonth.SpecialDeduction = model.SpecialDeduction;
                taxBaseEveryMonth.TotalIncome = model.TotalIncome;
                taxBaseEveryMonth.TotalTax = model.TotalTax;
                taxBaseEveryMonth.TotalTemp = model.TotalTemp;
                //taxBaseByMonth.Title = model.Title;
                taxBaseEveryMonth.UpdateDate = DateTime.Now;
                _TaxBaseEveryMonthRepository.Update(taxBaseEveryMonth);
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
                    int count = _TaxBaseEveryMonthRepository.Delete(_TaxBaseEveryMonthRepository.Entities.Where(c => certificateID.Contains(c.CertificateID)));
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
        public OperationResult Update(TaxBaseEveryMonth model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaxBaseEveryMonthRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(TaxBaseEveryMonth model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaxBaseEveryMonthRepository.Delete(model);
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
                    int num = 1;
                    foreach (var item in items)
                    {
                        TaxBaseEveryMonth record = new TaxBaseEveryMonth();
                        List<ImportFeedBack> errors = ImportUtil.ValidateImportRecord(item, num++, maps, ref record);
                        if (errors.Count > 0)
                        {
                            return new OperationResult(OperationResultType.Error, "导入数据失败", ImportUtil.ParseToHtml(errors));
                        }

                        //插入或更新数据
                        _TaxBaseEveryMonthRepository.InsertOrUpdate(record);
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

        public Double GetTotalIncome(String period_year, String certificateID)
        {
            Double totalIncome = _TaxBaseEveryMonthRepository.GetTotalIncome(period_year, certificateID);
            return totalIncome;
        }

        public Double GetTotalTax(String period_year, String certificateID)
        {
            Double totalTax = _TaxBaseEveryMonthRepository.GetTotalTax(period_year, certificateID);
            return totalTax;
        }


        public TaxBaseEveryMonth GetExistRecord(String period_year, String certificateID) {
            TaxBaseEveryMonth taxBaseEveryMonth = _TaxBaseEveryMonthRepository.GetExistRecord(period_year, certificateID);
            return taxBaseEveryMonth;
        }
   

        public OperationResult DeleteAll() {
            try
            {               
                _TaxBaseEveryMonthRepository.Delete(_TaxBaseEveryMonthRepository.Entities);
                return new OperationResult(OperationResultType.Success, "清空数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "清空数据失败!");
            }
        }
    }
}
