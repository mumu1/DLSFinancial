using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.CoreBLL.Service.Excel
{
    public class ColumnMap
    {
        public string ColumnName { get; set; }
        public string TitleName { get; set; }
    }

    public class ImportData
    {
        String ActionUrl { get; set; }
        ColumnMap[] columns { get; set; }
    }
}
