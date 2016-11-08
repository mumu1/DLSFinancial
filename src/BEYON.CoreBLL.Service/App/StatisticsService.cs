using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Data.Repositories.App;

namespace BEYON.CoreBLL.Service.App
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            this._statisticsRepository = statisticsRepository;
        }

        #region 按人统计明细表
        public Column[] GetPerPersonDetailColumns()
        {
            List<Column> columns = new List<Column>();
            columns.Add(new Column("C0", "序号"));
            columns.Add(new Column("C1", "期间"));
            columns.Add(new Column("C2", "姓名"));
            columns.Add(new Column("C3", "证件类型"));
            columns.Add(new Column("C4", "证件号码"));

            columns.Add(new Column("C5", "收入额"));
            columns.Add(new Column("C6", "免税金额"));
            columns.Add(new Column("C7", "基本扣除"));
            columns.Add(new Column("C8", "已扣缴税额"));
            columns.Add(new Column("C9", "应纳税额"));
            columns.Add(new Column("C10", "次数"));
            int count = _statisticsRepository.GetMaxCountPerMonthPerPerson();
            for (var i = 0; i < count; i++)
            {
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税前", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税后", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税额", i + 1)));
            }
            return columns.ToArray();
        }

        public List<Object> GetPerPersonDetail()
        {
            return this._statisticsRepository.GetPerMonthPerPerson();
        }
            
        #endregion

        #region 按劳务统计明细表
        public Column[] GetLaborStatisticsColumns()
        {
            List<Column> columns = new List<Column>();
            columns.Add(new Column("C0", "序号"));
            columns.Add(new Column("C1", "期间"));
            columns.Add(new Column("C2", "姓名"));
            columns.Add(new Column("C3", "证件类型"));
            columns.Add(new Column("C4", "证件号码"));

            columns.Add(new Column("C5", "收入额"));
            columns.Add(new Column("C6", "免税金额"));
            columns.Add(new Column("C7", "基本扣除"));
            columns.Add(new Column("C8", "已扣缴税额"));
            columns.Add(new Column("C9", "应纳税额"));
            columns.Add(new Column("C10", "次数"));
            int count = _statisticsRepository.GetMaxCountLaborStatistics();
            for (var i = 0; i < count; i++)
            {
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税前", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税后", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税额", i + 1)));
            }
            return columns.ToArray();
        }

        public List<Object> GetLaborStatisticsDetail()
        {
            return this._statisticsRepository.GetLaborStatisticsDetail();
        }
        #endregion

        #region 按课题统计明细表
        public Column[] GetTaskStatisticsColumns()
        {
            List<Column> columns = new List<Column>();
            columns.Add(new Column("C0", "序号"));
            columns.Add(new Column("C1", "期间"));
            columns.Add(new Column("C2", "课题号"));
            columns.Add(new Column("C3", "金额"));
            columns.Add(new Column("C4", "报销事由"));
            columns.Add(new Column("C5", "课题负责人"));
            columns.Add(new Column("C6", "工资薪金税额"));
            columns.Add(new Column("C7", "劳务费税额"));
  
            return columns.ToArray();
        }

        public List<Object> GetTaskStatisticsDetail()
        {
            return this._statisticsRepository.GetTaskStatisticsDetail();
        }
        #endregion
    }
}
