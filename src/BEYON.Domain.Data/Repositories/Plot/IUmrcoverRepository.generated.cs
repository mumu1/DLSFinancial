using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.Domain.Data.Repositories.Plot
{
    public partial interface IUmrcoverRepository : IRepository<Umrcover, Int32>
    {
    }
}
