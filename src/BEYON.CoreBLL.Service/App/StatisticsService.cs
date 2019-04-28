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

        #region 按工资统计明细表
        public Column[] GetPerPersonDetailColumns()
        {
            List<Column> columns = new List<Column>();
            columns.Add(new Column("C0", "序号"));
            columns.Add(new Column("C1", "期间"));
            columns.Add(new Column("C2", "姓名"));
            columns.Add(new Column("C3", "证件类型"));
            columns.Add(new Column("C4", "证件号码"));

            columns.Add(new Column("C5", "本期税前收入总额"));
            columns.Add(new Column("C6", "本期免税收入"));
            columns.Add(new Column("C7", "本期基本扣除"));
            columns.Add(new Column("C8", "本期养老保险"));
            columns.Add(new Column("C9", "本期失业保险"));
            columns.Add(new Column("C10", "本期医疗保险"));
            columns.Add(new Column("C11", "本期职业年金"));
            columns.Add(new Column("C12", "本期住房公积金"));
            columns.Add(new Column("C13", "本期专项附加扣除"));
        
           
            columns.Add(new Column("C14", "已扣缴税额"));
            columns.Add(new Column("C15", "应纳税所得额"));
           
            columns.Add(new Column("C16", "联系电话"));
            columns.Add(new Column("C17", "国籍"));
            columns.Add(new Column("C18", "单位"));
            columns.Add(new Column("C19", "职称"));
            columns.Add(new Column("C20", "次数"));
            int count = _statisticsRepository.GetMaxCountPerMonthPerPerson();
            for (var i = 0; i < count; i++)
            {
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税前", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税后", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税额", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次课题号", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次课题负责人", i + 1)));
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
            columns.Add(new Column("C9", "应纳税所得额"));//从应纳税额改过来20170214
            //应纳税所得额=收入额C5-基本扣除C7
            columns.Add(new Column("C10", "联系电话"));
            columns.Add(new Column("C11", "国籍"));
            columns.Add(new Column("C12", "单位"));
            columns.Add(new Column("C13", "职称"));
            columns.Add(new Column("C14", "次数"));
            int count = _statisticsRepository.GetMaxCountLaborStatistics();
            for (var i = 0; i < count; i++)
            {
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税前", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税后", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次税额", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次课题号", i + 1)));
                columns.Add(new Column(String.Format("C{0}", columns.Count), String.Format("第{0}次课题负责人", i + 1)));
            }
            return columns.ToArray();
        }

        public List<Object> GetLaborStatisticsDetail()
        {
            return this._statisticsRepository.GetLaborStatisticsDetail();
        }
        #endregion

        #region 按课题统计明细表
       /*
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
        * */
        #endregion

        #region 按课题统计明细表(修改)
        public Column[] GetTaskStatisticsColumns()
        {
            List<Column> columns = new List<Column>();
            columns.Add(new Column("C0", "序号"));
            columns.Add(new Column("C1", "期间"));
            columns.Add(new Column("C2", "课题号"));
            columns.Add(new Column("C3", "课题负责人"));
            columns.Add(new Column("C4", "报销事由"));           
            //columns.Add(new Column("C6", "工资薪金税额"));
            //columns.Add(new Column("C7", "劳务费税额"));
            columns.Add(new Column("C5", "会计科目代码"));
            columns.Add(new Column("C6", "课题支付金额（银行转账）"));
            columns.Add(new Column("C7", "劳务税金（银行转账）"));
            columns.Add(new Column("C8", "工资税金（银行转账）"));
            columns.Add(new Column("C9", "总税金（银行转账）"));
            columns.Add(new Column("C10", "课题名称"));
            //columns.Add(new Column("C0", "序号"));
            //columns.Add(new Column("C1", "期间"));
            //columns.Add(new Column("C2", "课题号"));
            //columns.Add(new Column("C8", "会计科目代码"));
            //columns.Add(new Column("C4", "报销事由"));
            //columns.Add(new Column("C5", "课题负责人"));
            //columns.Add(new Column("C6", "工资薪金税额"));        //=全部所内该课题的Tax和
            //columns.Add(new Column("C7", "劳务费税额"));            //=全部所外该课题的Tax和
            //columns.Add(new Column("C9", "银行转账总金额"));    //=全部银行转账该课题的Amount(填报)和
            //columns.Add(new Column("C10", "现金支付总金额"));  //=全部现金支付该课题的Amount(填报)和
            //columns.Add(new Column("C11", "含税总金额"));        //=全部含税该课题的Amount和
            //columns.Add(new Column("C12", "不含税总金额"));    //=全部不含税该课题的Amount和
            //columns.Add(new Column("C13", "总税额"));             //=全部该课题的Tax和
            //columns.Add(new Column("C14", "课题支付总额"));     
            return columns.ToArray();
        }

        public List<Object> GetTaskStatisticsDetail()
        {
            return this._statisticsRepository.GetTaskStatisticsDetail1();
        }
        #endregion

        #region 按流水号统计明细表
        public Column[] GetSerNumberStatisticsColumns()
        {
            List<Column> columns = new List<Column>();
            columns.Add(new Column("C0", "序号"));
            columns.Add(new Column("C1", "申请单流水号"));
            columns.Add(new Column("C2", "课题号"));
            columns.Add(new Column("C3", "报销合计"));
            columns.Add(new Column("C4", "税额合计"));
            columns.Add(new Column("C5", "工资税额合计"));
            columns.Add(new Column("C6", "劳务税额合计"));
            columns.Add(new Column("C7", "支付类型"));
            columns.Add(new Column("C8", "报销事由"));
            columns.Add(new Column("C9", "课题负责人"));
            columns.Add(new Column("C10", "经办人"));
            columns.Add(new Column("C11", "更新时间"));
            
            return columns.ToArray();
        }

        public List<Object> GetSerNumberStatisticsDetail()
        {
            return this._statisticsRepository.GetSerNumberStatisticsDetail();
        }
        #endregion
    }
}
