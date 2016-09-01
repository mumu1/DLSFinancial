using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IProtectUnitAuditService
    {
        IQueryable<ProtectUnitAudit> Audits { get; }
        OperationResult Insert(ProtectUnitAuditVM model);
        OperationResult Update(ProtectUnitAuditVM model);
        OperationResult Delete(IEnumerable<ProtectUnitAuditVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
