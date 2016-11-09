using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.Domain.Data.Repositories.App
{
    public interface IStatisticsRepository
    {
         int GetMaxCountPerMonthPerPerson();

         List<Object> GetPerMonthPerPerson();

         int GetMaxCountLaborStatistics();

         List<Object> GetLaborStatisticsDetail();

         List<Object> GetTaskStatisticsDetail();
    }
}
