using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;


namespace BEYON.CoreBLL.Service.App.Interface
{
    public interface ITaxBaseEveryMonthService
    {
        IQueryable<TaxBaseEveryMonth> TaxBaseEveryMonths { get; }
        OperationResult Insert(TaxBaseEveryMonthVM model);
        OperationResult Update(TaxBaseEveryMonthVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(TaxBaseEveryMonth model);
        OperationResult Update(TaxBaseEveryMonth model);
        OperationResult Import(String fileName, Service.Excel.ImportData importData);

        Double GetTotalIncome(String period_year, String certificateID);
         
        Double GetTotalTax(String period_year, String certificateID);

        TaxBaseEveryMonth GetExistRecord(String period_year, String certificateID);

        OperationResult DeleteAll();
    }
}
