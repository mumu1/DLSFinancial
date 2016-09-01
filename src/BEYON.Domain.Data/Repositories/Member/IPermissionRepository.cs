using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;

namespace BEYON.Domain.Data.Repositories.Member
{
    public partial interface IPermissionRepository
    {
        IList<PermissionVM> GetListPermissionVM(Expression<Func<Permission, bool>> wh, int limit, int offset, out int total);
    }
}
