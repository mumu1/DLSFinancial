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
    public interface IRefundTypeService
    {
        IQueryable<RefundType> RefundTypes { get; }
        OperationResult Insert(RefundTypeVM model);
        OperationResult Update(RefundTypeVM model);
        OperationResult Delete(List<string> list);
    }
}
