using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IAuditService
    {
        IQueryable<Audit> Audits { get; }
        OperationResult Insert(AuditVM model);
        OperationResult Update(AuditVM model);
        OperationResult Delete(IEnumerable<AuditVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
