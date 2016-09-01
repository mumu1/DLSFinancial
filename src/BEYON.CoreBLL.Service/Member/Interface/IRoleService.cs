using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel;
using BEYON.ViewModel.Member;

namespace BEYON.CoreBLL.Service.Member.Interface
{
    public interface IRoleService
    {
        IQueryable<Role> Roles { get; }
        OperationResult Insert(RoleVM model);
        OperationResult Update(RoleVM model);
        OperationResult Delete(IEnumerable<RoleVM> list);
        IList<ZTreeVM> GetListZTreeVM(int id);
        OperationResult UpdateAuthorize(int roleId, int[] ids);
    }
}
