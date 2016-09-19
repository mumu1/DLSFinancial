using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyon.Test
{
    [TestClass]
    public class ExcelPrintTest
    {
        [TestMethod]
        public void ExcelPrint()
        {
            int nMax = 9;
            int nMin = 6;
            int rowCount = nMax - nMin + 1;//总行数
            const int columnCount = 12;//总列数

            //创建Excel对象
            Excel.Application excelApp = new Excel.ApplicationClass();
            //新建工作簿
            Excel.Workbook workBook = excelApp.Workbooks.Add(true);
            //新建工作表
            Excel.Worksheet worksheet = workBook.ActiveSheet as Excel.Worksheet;
            worksheet.Cells.NumberFormat = "@";     //文本输出格式

            //1.设置标题
            Excel.Range titleRange = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnCount]);//选取单元格
            titleRange.Merge(true);//合并单元格
            titleRange.Value = "发放__________________________________明细表"; //设置单元格内文本
            titleRange.Font.Name = "宋体";//设置字体
            titleRange.Font.Size = 14;//字体大小
            titleRange.Font.Bold = true;//加粗显示
            titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; //水平居中
            titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;   //垂直居中

            Excel.Range tipRange = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[2, columnCount]);//选取单元格
            tipRange.Merge(true);//合并单元格
            tipRange.Value = "我声明，以下填写的内容是完全真实的，如有不实，由此产生的一切后果由本人承担。---声明人签字:           "; //设置单元格内文本
            tipRange.Font.Name = "宋体";//设置字体
            tipRange.Font.Size = 10;//字体大小
            tipRange.Font.Bold = true;//加粗显示
            tipRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;//水平居中
            tipRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;//垂直居中

            Excel.Range Range1 = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[3, columnCount]);//选取单元格
            Range1.Merge(true);

            //2.设置表头
            string[] strHead = new string[columnCount] { "序号", "姓名", "证件类型", "身份证件号码", "单位", "联系电话", "国籍", "职称", "金额(元)", "银行存折帐号", "开户银行", "领取人签字" };
            int[] columnWidth = new int[columnCount] { 4, 8, 8, 20, 10, 8, 8, 8, 8, 16, 8, 10 };
            for (int i = 0; i < columnCount; i++)
            {
                Excel.Range headRange = worksheet.Cells[4, i + 1] as Excel.Range;//获取表头单元格,不用标题则从1开始
                headRange.Value2 = strHead[i];//设置单元格文本
                headRange.Font.Name = "宋体";//设置字体
                headRange.Font.Size = 12;//字体大小
                headRange.Font.Bold = false;//加粗显示
                headRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;//水平居中
                headRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;//垂直居中
                headRange.ColumnWidth = columnWidth[i];//设置列宽
            }

            //3.填充数据
            for (int k = 5; k <= nMax; k++) {
                excelApp.Cells[k, 1] = string.Format("{0}", k-4);
                excelApp.Cells[k, 2] = string.Format("{0}", "林老师");
                excelApp.Cells[k, 3] = string.Format("{0}", "身份证");
                excelApp.Cells[k, 4] = string.Format("{0}", "440823198812291156");
            }

            //4.设置表尾格式
            worksheet.Cells[nMax + 1, 2] = "报销事由";
            worksheet.Cells[nMax + 1, 5] = "课题号";
            worksheet.Cells[nMax + 1, 7] = "课题负责人";
            worksheet.Cells[nMax + 1, 9] = "显示合计金额";
            worksheet.Cells[nMax + 1, 12] = "所内/所外(选项)";
            Excel.Range range = worksheet.get_Range(worksheet.Cells[nMax + 1, 3], worksheet.Cells[nMax+1, 4]);//选取单元格
            range.Merge(true);
            Excel.Range tailRange = worksheet.Cells[nMax+1, 11] as Excel.Range;
            tailRange.ColumnWidth = 10;
            tailRange.Value2 = "人员类型";

            //// Get ole objects and add new one
            //OLEObjects objs = worksheet.OLEObjects() as OLEObjects;
            //OLEObject obj = objs.Add("Forms.CheckBox.1", Missing.Value, Missing.Value, false, false,
            //    Missing.Value, Missing.Value, 560, 124, 108, 18);
            //obj.PrintObject = true;
            //OLEObject obj1 = objs.Add("Forms.CheckBox.1", Missing.Value, Missing.Value, false, false,
            //    Missing.Value, Missing.Value, 560, 138, 108, 18);
            //obj1.Name = "所外";

            Excel.Range contentRange = worksheet.get_Range(worksheet.Cells[4, 1], worksheet.Cells[nMax+1, columnCount]);//选取单元格
            contentRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;//设置边框
            contentRange.Borders.Weight = Excel.XlBorderWeight.xlThin;//边框常规粗细
            contentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;//水平居中
            contentRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;//垂直居中
            contentRange.WrapText = true;

            Excel.Range work = worksheet.get_Range(worksheet.Cells[nMax + 2, 1], worksheet.Cells[nMax + 6, columnCount]);//选取单元格
            work.MergeCells = true;
            work.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;//设置边框
            work.Borders.Weight = Excel.XlBorderWeight.xlThin;//边框常规粗细
            work.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;//水平居中
            work.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;//垂直居中
            work.Font.Name = "宋体";//设置字体
            work.Font.Size = 11;//字体大小
            work.Font.Bold = false;//加粗显示
            work.Value = "关于工作内容、工作时间等的描述（必填）：\n\n\n                                                                                                   年    月    日";

            Excel.Range noteRange = worksheet.get_Range(worksheet.Cells[nMax + 7, 1], worksheet.Cells[nMax + 9, columnCount]);//选取单元格
            noteRange.MergeCells = true;
            noteRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;//设置边框
            noteRange.Borders.Weight = Excel.XlBorderWeight.xlThin;//边框常规粗细
            noteRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;//水平居中
            noteRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;//垂直居中
            noteRange.Font.Name = "宋体";//设置字体
            noteRange.Font.Size = 9;//字体大小
            noteRange.Font.Bold = false;//加粗显示
            noteRange.Value = "说明:1、给类劳务费用由领取人本人签收并经课题负责人、经办人签字。\n     2.现金和。\n     3.单位填写分类";

            Excel.Range signRange = worksheet.get_Range(worksheet.Cells[nMax + 10, 1], worksheet.Cells[nMax + 11, columnCount]);//选取单元格
            signRange.MergeCells = true;
            signRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;//垂直居中
            signRange.Value = "                                                        课题负责人:               经办人:          ";
            try
            {
                //long XlFileFormat;
                //if (!xlFormat.CompareNoCase(_T("xls"))) XlFileFormat = 56;
                //else if (!xlFormat.CompareNoCase(_T("xlsx"))) XlFileFormat = 51;
                //else if (!xlFormat.CompareNoCase(_T("csv"))) XlFileFormat = 6;
                //else XlFileFormat = 56;

                //worksheet.SaveAs(@"D:\Tomin.xlsx", "51", "test", "test", true,
                //    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                workBook.SaveAs(@"D:\Tomin.xlsx", "51", Missing.Value, Missing.Value, true,
                    false, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                //释放工作资源
                ReleaseCOM(worksheet);
                ReleaseCOM(workBook);
                excelApp.Quit();
                ReleaseCOM(excelApp);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void ReleaseCOM(object pObj)
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
