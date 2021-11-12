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
    public interface IBankFullNameService
    {
        IQueryable<BankFullName> BankFullNames { get; }
        OperationResult Insert(BankFullNameVM model);
        OperationResult Update(BankFullNameVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(BankFullName model);
        OperationResult Update(BankFullName model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);

        OperationResult DeleteAll();
    }
}
