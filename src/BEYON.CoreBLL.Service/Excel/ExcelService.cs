using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel;

namespace BEYON.CoreBLL.Service.Excel
{
    public class ExcelService
    {
        public static IQueryable<T> GetObjects<T>(string filePath, ColumnMap[] maps = null)
        {
            var excel = new ExcelQueryFactory(filePath);
            for (var i = 0; maps != null && i < maps.Length; i++)
            {
                var map = maps[i];
                map.TitleName = map.TitleName != null ? map.TitleName : map.ColumnName;
                excel.AddMapping(map.ColumnName, map.TitleName);
            }
            var items = from c in excel.Worksheet<T>(0) select c;
            return items;
        }
    }
}
