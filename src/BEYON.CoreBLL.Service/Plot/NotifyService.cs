using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.ViewModel.Plot;
using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Data.Repositories.Plot;

namespace BEYON.CoreBLL.Service.Plot
{
    public class NotifyService : CoreServiceBase, INotifyService
    {
        private readonly IAuditRepository _AuditRepository;

        public NotifyService(IAuditRepository auditRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._AuditRepository = auditRepository;
        }

        public NotifyVM GetNotify(int userID)
        {
            return _AuditRepository.GetNotify(userID);
        }
    }
}
