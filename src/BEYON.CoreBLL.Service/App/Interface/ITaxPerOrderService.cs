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
    public interface ITaxPerOrderService
    {
        IQueryable<TaxPerOrder> TaxPerOrders { get; }
        OperationResult Insert(TaxPerOrder model, bool isSave = true);
        OperationResult Update(TaxPerOrderVM model, bool isSave = true);
        OperationResult Delete(List<string> list, bool isSave = true);
        OperationResult Delete(TaxPerOrder model, bool isSave = true);
        OperationResult DeleteAll();
        OperationResult Update(TaxPerOrder model, bool isSave = true);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);

        Double GetPayTaxAmount(String certificateID, String taxOrNot);

        int GetCashCount(String certificateID);

        int DeleteBySerialNumber(String serialNumber);

        Double GetDeductTaxSum(String certificateID);

    }
}
