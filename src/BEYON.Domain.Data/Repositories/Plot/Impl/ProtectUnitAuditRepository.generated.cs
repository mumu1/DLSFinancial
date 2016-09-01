﻿using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.Domain.Data.Repositories.Plot.Impl
{
    public partial class ProtectUnitAuditRepository : EFRepositoryBase<ProtectUnitAudit, Int32>, IProtectUnitAuditRepository
    {
        public ProtectUnitAuditRepository(IUnitOfWork unitOfWork)
           :base()
       { }
    }
}