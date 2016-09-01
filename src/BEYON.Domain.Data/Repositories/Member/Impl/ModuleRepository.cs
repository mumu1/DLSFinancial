/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/2/24 14:46:29  
*************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;

namespace BEYON.Domain.Data.Repositories.Member.Impl
{
    public partial class ModuleRepository
    {
        public IList<ModuleVM> GetListModuleVM(Expression<Func<Module, bool>> wh, int limit, int offset, out int total)
        {
            var q = from m1 in Context.Modules.Where(wh)
                    join m2 in Context.Modules on m1.ParentId equals m2.Id into joinModule
                    from item in joinModule.DefaultIfEmpty()
                    select new ModuleVM
                    {
                        Id = m1.Id,
                        Name = m1.Name,
                        LinkUrl = m1.LinkUrl,
                        IsMenu = m1.IsMenu,
                        Code = m1.Code,
                        Description = m1.Description,
                        Enabled = m1.Enabled,
                        ParentName = item.Name,
                        UpdateDate = m1.UpdateDate,
                        Icon = m1.Icon
                    };
            total = q.Count();
            if (offset >= 0)
            {
                return q.OrderBy(c => c.Code).Skip(offset).Take(limit).ToList();
            }
            return q.ToList();
        }
    }
}
