using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.ViewModel.Plot;

namespace BEYON.Domain.Data.Repositories.Plot
{
    public partial interface IProtectUnitAuditRepository 
    {
        NotifyVM GetNotify(int userID);
        IList<ProtectUnitAudit> Export(IList<String> umrids);
        void InsertOrUpdate(IQueryable<ProtectUnitAudit> umrcovers);
    }
}
