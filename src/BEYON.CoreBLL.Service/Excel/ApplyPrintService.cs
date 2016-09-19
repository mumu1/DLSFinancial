using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Office = Microsoft.Office.Core;
//using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using BEYON.Domain.Model.App;
using BEYON.Domain.Data.Repositories.App;
using BEYON.CoreBLL.Service.App;
using BEYON.CoreBLL.Service.Excel.Interface;

namespace BEYON.CoreBLL.Service.Excel
{
    public class ApplyPrintService : IApplyPrintService
    {
        private readonly IApplicationFormRepository _applicationFormRepository;
        private readonly IPersonalRecordRepository _personalRecordRepository;

        public ApplyPrintService(IApplicationFormRepository applicationFormRepository, IPersonalRecordRepository personalRecordRepository)
        {
            this._applicationFormRepository = applicationFormRepository;
            this._personalRecordRepository = personalRecordRepository;
        }

        public String ApplyExcel(String filePath, String serialNumber)
        {
            ApplicationForm applicationForm = _applicationFormRepository.Entities.FirstOrDefault(c => c.SerialNumber == serialNumber.Trim());
            if (applicationForm == null)
                return null;
            IList<PersonalRecord> persons = _personalRecordRepository.GetPersonalRecordBySerialNumber(serialNumber);
            return SaveExcel(filePath, applicationForm, persons);
        }

        private String GetFileName()
        {
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            return string.Format("DLS_{0}_{1}.xlsx", userid, DateTime.Now.ToString("yyyyMMddHHMMssffff"));
        }

        private String GetFilePath(String directory, string fileName)
        {
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            var tableFile = String.Format("{0}{1}", directory, fileName);
            return tableFile;
        }

        private String SaveExcel(String  filePath, ApplicationForm applicationForm, IList<PersonalRecord> persons)
        {
            var fileName = GetFileName();
            var fullPath = GetFilePath(filePath, fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            int rowCount = persons.Count();//总行数
            int nMax = rowCount + 5 - 1;
            const int columnCount = 12;//总列数
             
            //创建Excel对象
            Application excelApp = new ApplicationClass();
            //新建工作簿
            Workbook workBook = excelApp.Workbooks.Add(true);
            //新建工作表
            Worksheet worksheet = workBook.ActiveSheet as Worksheet;
            worksheet.Cells.NumberFormat = "@";     //文本输出格式

            //1.设置标题
            Range titleRange = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnCount]);//选取单元格
            titleRange.Merge(true);//合并单元格
            titleRange.Value = "发放__________________________________明细表"; //设置单元格内文本
            titleRange.Font.Name = "宋体";//设置字体
            titleRange.Font.Size = 14;//字体大小
            titleRange.Font.Bold = true;//加粗显示
            titleRange.HorizontalAlignment = XlHAlign.xlHAlignCenter; //水平居中
            titleRange.VerticalAlignment = XlVAlign.xlVAlignCenter;   //垂直居中

            Range tipRange = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[2, columnCount]);//选取单元格
            tipRange.Merge(true);//合并单元格
            tipRange.Value = "我声明，以下填写的内容是完全真实的，如有不实，由此产生的一切后果由本人承担。---声明人签字:           "; //设置单元格内文本
            tipRange.Font.Name = "宋体";//设置字体
            tipRange.Font.Size = 10;//字体大小
            tipRange.Font.Bold = true;//加粗显示
            tipRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
            tipRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中

            Range Range1 = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[3, columnCount]);//选取单元格
            Range1.Merge(true);

            //2.设置表头
            string[] strHead = new string[columnCount] { "序号", "姓名", "证件类型", "身份证件号码", "单位", "联系电话", "国籍", "职称", "金额(元)", "银行存折帐号", "开户银行", "领取人签字" };
            int[] columnWidth = new int[columnCount] { 4, 8, 8, 20, 20, 12, 8, 8, 8, 20, 8, 10 };
            for (int i = 0; i < columnCount; i++)
            {
                Range headRange = worksheet.Cells[4, i + 1] as Range;//获取表头单元格,不用标题则从1开始
                headRange.Value2 = strHead[i];//设置单元格文本
                headRange.Font.Name = "宋体";//设置字体
                headRange.Font.Size = 12;//字体大小
                headRange.Font.Bold = false;//加粗显示
                headRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;//水平居中
                headRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中
                headRange.ColumnWidth = columnWidth[i];//设置列宽
            }

            //3.填充数据
            for (int k = 1; k <= rowCount; k++)
            {
                var person = persons[k - 1];
                excelApp.Cells[k+4, 1] = string.Format("{0}", k);
                excelApp.Cells[k + 4, 2] = person.Name;
                excelApp.Cells[k + 4, 3] = person.CertificateType;
                excelApp.Cells[k + 4, 4] = person.CertificateID;
                excelApp.Cells[k + 4, 5] = person.Company;
                excelApp.Cells[k + 4, 6] = person.Tele;
                excelApp.Cells[k + 4, 7] = person.Nationality;
                excelApp.Cells[k + 4, 8] = person.Title;
                excelApp.Cells[k + 4, 9] = person.Amount;
                excelApp.Cells[k + 4, 10] = person.AccountNumber;
                excelApp.Cells[k + 4, 11] = person.Bank;
            }

            //4.设置表尾格式
            worksheet.Cells[nMax + 1, 2] = "报销事由";
            worksheet.Cells[nMax + 1, 3] = applicationForm.RefundType; 
            worksheet.Cells[nMax + 1, 5] = "课题号";
            worksheet.Cells[nMax + 1, 6] = applicationForm.ProjectNumber;
            worksheet.Cells[nMax + 1, 7] = "课题负责人";
            worksheet.Cells[nMax + 1, 8] = applicationForm.ProjectDirector;
            worksheet.Cells[nMax + 1, 9] = "显示合计金额";
            worksheet.Cells[nMax + 1, 12] = "所内/所外(选项)";
            Range range = worksheet.get_Range(worksheet.Cells[nMax + 1, 3], worksheet.Cells[nMax + 1, 4]);//选取单元格
            range.Merge(true);
            Range tailRange = worksheet.Cells[nMax + 1, 11] as Range;
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

            Range contentRange = worksheet.get_Range(worksheet.Cells[4, 1], worksheet.Cells[nMax + 1, columnCount]);//选取单元格
            contentRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
            contentRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
            contentRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;//水平居中
            contentRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中
            contentRange.WrapText = true;

            Range work = worksheet.get_Range(worksheet.Cells[nMax + 2, 1], worksheet.Cells[nMax + 6, columnCount]);//选取单元格
            work.MergeCells = true;
            work.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
            work.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
            work.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
            work.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
            work.Font.Name = "宋体";//设置字体
            work.Font.Size = 11;//字体大小
            work.Font.Bold = false;//加粗显示
            StringBuilder sb = new StringBuilder();
            sb.Append("关于工作内容、工作时间等的描述（必填）：\n");
            string content = String.Format("    {0}", applicationForm.AuditOpinion);
            int padlen = 130;
            sb.Append(content);
            if (content.Length < 76)
                sb.Append("\n\n\n");
            else if (content.Length < 151)
                sb.Append("\n\n");
            else if (content.Length < 226)
                sb.Append("\n");
            else
                padlen = padlen - (content.Length - 225) * 2;
            String time = applicationForm.AuditTime.ToString("yyyy年MM月dd日");
            if(padlen > 0)
                time = time.PadLeft(padlen,' ');
            sb.Append(time);  
            work.Value = sb.ToString();

            Range noteRange = worksheet.get_Range(worksheet.Cells[nMax + 7, 1], worksheet.Cells[nMax + 9, columnCount]);//选取单元格
            noteRange.MergeCells = true;
            noteRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
            noteRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
            noteRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
            noteRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
            noteRange.Font.Name = "宋体";//设置字体
            noteRange.Font.Size = 9;//字体大小
            noteRange.Font.Bold = false;//加粗显示
            noteRange.Value = "说明:1、给类劳务费用由领取人本人签收并经课题负责人、经办人签字。\n     2.现金和。\n     3.单位填写分类";

            Range signRange = worksheet.get_Range(worksheet.Cells[nMax + 10, 1], worksheet.Cells[nMax + 11, columnCount]);//选取单元格
            signRange.MergeCells = true;
            signRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
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

                workBook.SaveAs(fullPath, "51", Missing.Value, Missing.Value, true,
                    false, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                //释放工作资源
                ReleaseCOM(worksheet);
                ReleaseCOM(workBook);
                excelApp.Quit();
                ReleaseCOM(excelApp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return fileName;
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
