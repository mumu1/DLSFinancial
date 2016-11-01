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

    public class Parameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ImportData
    {
        public String ActionUrl { get; set; }
        public ColumnMap[] Columns { get; set; }

        public Parameter[] Parameters { get; set; }
    }
}
