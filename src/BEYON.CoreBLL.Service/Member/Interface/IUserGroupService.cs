using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;

namespace BEYON.CoreBLL.Service.Member.Interface
{
    public interface IUserGroupService
    {
        IQueryable<UserGroup> UserGroups { get; }
        OperationResult Insert(UserGroupVM model);
        OperationResult Update(UserGroupVM model);
        OperationResult Delete(IEnumerable<UserGroupVM> list);
        OperationResult UpdateUserGroupRoles(int userId, string[] chkRoles);
    }
}
