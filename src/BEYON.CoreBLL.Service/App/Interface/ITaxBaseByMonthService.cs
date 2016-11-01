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
    public interface ITaxBaseByMonthService
    {
        IQueryable<TaxBaseByMonth> TaxBaseByMonths { get; }
        OperationResult Insert(TaxBaseByMonthVM model);
        OperationResult Update(TaxBaseByMonthVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(TaxBaseByMonth model);
        OperationResult Update(TaxBaseByMonth model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);

        Double GetBaseSalary(String certificateID);

        String GetNameByCerID(String certificateID);

        OperationResult DeleteAll();
    }
}
