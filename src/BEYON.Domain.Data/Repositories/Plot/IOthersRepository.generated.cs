using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.Domain.Data.Repositories.Plot
{
    public partial interface IOthersRepository : IRepository<Others, Int32>
    {
        IList<Others> Export(IList<String> umrids);
        void InsertOrUpdate(IQueryable<Others> umrcovers);
    }
}
