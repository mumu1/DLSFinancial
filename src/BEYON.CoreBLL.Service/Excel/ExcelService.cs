using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Data;
using LinqToExcel;
using System.Reflection;
using System.Runtime.InteropServices;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace BEYON.CoreBLL.Service.Excel
{
    public class ExcelService
    {
        private static ConcurrentDictionary<string, ImportData> excelColumnMaps = new ConcurrentDictionary<string, ImportData>();

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

        public static IQueryable<Row> GetObjects(string filePath, ColumnMap[] maps = null)
        {
            //var excel = new ExcelQueryFactory(filePath);
            //for (var i = 0; maps != null && i < maps.Length; i++)
            //{
            //    var map = maps[i];
            //    map.TitleName = map.TitleName != null ? map.TitleName : map.ColumnName;
            //    excel.AddMapping(map.ColumnName, map.TitleName);
            //}
            //var items = from c in excel.Worksheet<Row>(0) select c;
            //return items;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            if (fileInfo.Extension == ".xls")
            {
                //创建Excel对象
                Application excelApp = new ApplicationClass();
                Workbook workbook = excelApp.Workbooks.Open(filePath);
                var copyPath = filePath + "x";

                if (System.IO.File.Exists(copyPath))
                {
                    System.IO.File.Delete(copyPath);
                }

                //long XlFileFormat;
                //if(!xlFormat.CompareNoCase(_T("xls"))) XlFileFormat=56;
                //else if(!xlFormat.CompareNoCase(_T("xlsx"))) XlFileFormat=51;
                //else if(!xlFormat.CompareNoCase(_T("csv"))) XlFileFormat=6;
                //else XlFileFormat=56;

               workbook.SaveAs(copyPath, "51", Missing.Value, Missing.Value, true,
                    false, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

               System.IO.File.Delete(filePath);
               ReleaseCOM(workbook);
               excelApp.Quit();
               ReleaseCOM(excelApp);
               filePath = copyPath;

            }

            var items = from c in ExcelQueryFactory.Worksheet<Row>(0, filePath)
                            select c ;
            return items;
        }

        public static void Add(ImportData importData)
        {
            if(!excelColumnMaps.ContainsKey(importData.ActionUrl))
            {
                excelColumnMaps.TryAdd(importData.ActionUrl, importData);
            }
            else
            {
                excelColumnMaps[importData.ActionUrl] = importData;
            }
        }

        public static bool Get(string url, out ImportData columns)
        {
            return excelColumnMaps.TryGetValue(url, out columns);
        }

        private static void ReleaseCOM(object pObj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pObj);
            }
            catch
            {
                throw new Exception("Release resource Error!");
            }
            finally { pObj = null; }
        }
    }
}
