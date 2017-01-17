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
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        private void RemoveOldFile(String filePath)
        {
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            var list = System.IO.Directory.GetFiles(filePath, "*.xlsx").Where(t => t.Contains(string.Format("DLS_{0}", userid))).ToList();
            foreach (var file in list)
            {
                if (System.IO.File.Exists(file))
                {
                    try
                    {
                        System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
                        System.IO.File.Delete(file);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private String SaveExcel(String filePath, ApplicationForm applicationForm, IList<PersonalRecord> persons)
        {
            ////1.删除已有文件
            //RemoveOldFile(filePath);

            var fileName = GetFileName();
            var fullPath = GetFilePath(filePath, fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            //创建Excel对象
            Application excelApp = new ApplicationClass();
            //新建工作簿
            Workbook workBook = excelApp.Workbooks.Add(true);
            //新建工作表
            Worksheet worksheet = workBook.ActiveSheet as Worksheet;
            worksheet.Cells.NumberFormat = "@";     //文本输出格式

            worksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;//页面方向横向

            worksheet.PageSetup.Zoom =false ; //打印时页面设置,必须设置为false,下面的二行页高,页宽才有效
            worksheet.PageSetup.FitToPagesWide = 1; //页宽
            worksheet.PageSetup.FitToPagesTall = false; //页高
           
            //银行转账支付方式，多银行信息等列需打印
            if (applicationForm.PaymentType.Equals("银行转账"))
            {
                int rowCount = persons.Count();//总行数
                int nMax = rowCount + 6 - 1;
                const int columnCount = 15;//总列数          

                //1.设置标题
                Range titleRange = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnCount]);//选取单元格
                titleRange.Merge(true);//合并单元格
                titleRange.Value = String.Format("发放      {0}      明细表", applicationForm.RefundType); //设置单元格内文本
                titleRange.Font.Name = "黑体";//设置字体
                titleRange.Font.Size = 15;//字体大小
                titleRange.Font.Bold = true;//加粗显示
                titleRange.Font.Underline = true;
                titleRange.HorizontalAlignment = XlHAlign.xlHAlignCenter; //水平居中
                titleRange.VerticalAlignment = XlVAlign.xlVAlignCenter;   //垂直居中

                //设置空行
                Range Range2 = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[2, columnCount]);//选取单元格
                Range2.Merge(true);

                Range tipRange = worksheet.get_Range(worksheet.Cells[3, 1], worksheet.Cells[3, columnCount]);//选取单元格
                tipRange.Merge(true);//合并单元格
                tipRange.Value = "我声明，以下填写的内容是完全真实的，如有不实，由此产生的一切后果由本人承担。---声明人签字:"; //设置单元格内文本
                tipRange.Font.Name = "宋体";//设置字体
                tipRange.Font.Size = 12;//字体大小
                tipRange.Font.Bold = true;//加粗显示
                tipRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                tipRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中

                //流水号
                Range SerRange = worksheet.get_Range(worksheet.Cells[4, 1], worksheet.Cells[4, columnCount]);//选取单元格
                SerRange.Merge(true);
                SerRange.Value = String.Format("流水号：  {0}", applicationForm.SerialNumber); //设置单元格内文本
                SerRange.Font.Name = "黑体";//设置字体
                SerRange.Font.Size = 13;//字体大小
                SerRange.Font.Bold = true;//加粗显示
                SerRange.HorizontalAlignment = XlHAlign.xlHAlignRight; //水平靠右
                SerRange.VerticalAlignment = XlVAlign.xlVAlignCenter;   //垂直居中
                

                Range infoRange = worksheet.get_Range(worksheet.Cells[5, 1], worksheet.Cells[5, columnCount]);//选取单元格
                infoRange.MergeCells = true;
                infoRange.Font.Name = "黑体";//设置字体
                infoRange.Font.Size = 12;//字体大小
                infoRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                infoRange.HorizontalAlignment = XlHAlign.xlHAlignLeft; //水平靠左
                infoRange.Value = String.Format("课题号：  {0}         课题负责人：  {1}         经办人：  {2}         支付类型：  {3}         合计金额：  {4} 元", applicationForm.ProjectNumber, applicationForm.ProjectDirector, applicationForm.Agent, applicationForm.PaymentType,applicationForm.Summation); //设置单元格内文本
                  

                //2.设置申请单常规信息格式
                //Range rangeSer1 = worksheet.get_Range(worksheet.Cells[3, 1], worksheet.Cells[3, 2]);//选取单元格
                //rangeSer1.Merge(true);
                //Range rangeSer = worksheet.get_Range(worksheet.Cells[3, 3], worksheet.Cells[3, 4]);//选取单元格
                //rangeSer.Merge(true);
                //Range rangeTask = worksheet.get_Range(worksheet.Cells[3, 6], worksheet.Cells[3, 7]);//选取单元格
                //rangeTask.Merge(true);
          
                //worksheet.Cells[3, 1] = "流水号";
                //worksheet.Cells[3, 3] = applicationForm.SerialNumber;
                //worksheet.Cells[3, 5] = "课题号";
                //worksheet.Cells[3, 6] = applicationForm.ProjectNumber;
                //worksheet.Cells[3, 8] = "课题负责人";
                //worksheet.Cells[3, 9] = applicationForm.ProjectDirector;
                //worksheet.Cells[3, 10] = "经办人";
                //worksheet.Cells[3, 11] = applicationForm.Agent;
                //worksheet.Cells[3, 12] = "合计金额";
                //worksheet.Cells[3, 13] = applicationForm.Summation;
                //worksheet.Cells[3, 14] = "支付类型";
                //worksheet.Cells[3, 15] = applicationForm.PaymentType;

                //Range info = worksheet.get_Range(worksheet.Cells[3, 1], worksheet.Cells[3, columnCount]);//选取单元格           
                //info.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                //info.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                //info.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                //info.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                //info.Font.Name = "宋体";//设置字体
                //info.Font.Size = 12;//字体大小
                //info.Font.Bold = true;//加粗显示
               

                //2.设置表头
                string[] strHead = new string[columnCount] { "序号", "姓名", "证件类型", "证件号码", "单位", "联系电话", "国籍", "职称", "人员类型", "金额(元)", "是否含税", "开户银行", "银行存折帐号", "开户银行详细名称", "领取人签字" };
                int[] columnWidth = new int[columnCount] { 4, 10, 12, 14, 20, 12, 8, 8, 10, 10, 10, 10, 16, 16, 12 };
                for (int i = 0; i < columnCount; i++)
                {
                    Range headRange = worksheet.Cells[6, i + 1] as Range;//获取表头单元格,不用标题则从1开始
                    headRange.Value2 = strHead[i];//设置单元格文本
                    headRange.Font.Name = "宋体";//设置字体
                    headRange.Font.Size = 13;//字体大小
                    headRange.Font.Bold = true;//加粗显示
                    headRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;//水平居中
                    headRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中
                    headRange.ColumnWidth = columnWidth[i];//设置列宽
                }

                //3.填充数据
                for (int k = 1; k <= rowCount; k++)
                {
                    var person = persons[k - 1];
                    excelApp.Cells[k + 6, 1] = string.Format("{0}", k);
                    excelApp.Cells[k + 6, 2] = person.Name.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    excelApp.Cells[k + 6, 3] = person.CertificateType;
                    excelApp.Cells[k + 6, 4] = person.CertificateID;
                    excelApp.Cells[k + 6, 5] = person.Company;
                    excelApp.Cells[k + 6, 6] = person.Tele;
                    excelApp.Cells[k + 6, 7] = person.Nationality;
                    excelApp.Cells[k + 6, 8] = person.Title;
                    excelApp.Cells[k + 6, 9] = person.PersonType;
                    excelApp.Cells[k + 6, 10] = person.Amount;
                    excelApp.Cells[k + 6, 11] = person.TaxOrNot;
                    // excelApp.Cells[k + 5, 12] = person.PaymentType;
                    excelApp.Cells[k + 6, 12] = person.Bank;
                    excelApp.Cells[k + 6, 13] = person.AccountNumber.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    excelApp.Cells[k + 6, 14] = person.BankDetailName;
                }



                //Range tailRange = worksheet.Cells[nMax + 1, 11] as Range;
                //tailRange.ColumnWidth = 10;
                //tailRange.Value2 = "";

                //// Get ole objects and add new one
                //OLEObjects objs = worksheet.OLEObjects() as OLEObjects;
                //OLEObject obj = objs.Add("Forms.CheckBox.1", Missing.Value, Missing.Value, false, false,
                //    Missing.Value, Missing.Value, 560, 124, 108, 18);
                //obj.PrintObject = true;
                //OLEObject obj1 = objs.Add("Forms.CheckBox.1", Missing.Value, Missing.Value, false, false,
                //    Missing.Value, Missing.Value, 560, 138, 108, 18);
                //obj1.Name = "所外";

                Range contentRange = worksheet.get_Range(worksheet.Cells[6, 1], worksheet.Cells[nMax + 2, columnCount]);//选取单元格
                contentRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                contentRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                contentRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;//水平居中
                contentRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中
                //contentRange.Font.Name = "宋体";//设置字体
                //contentRange.Font.Size = 13;//字体大小
                //contentRange.Font.Bold = false;//加粗显示
                contentRange.WrapText = true;

                Range work = worksheet.get_Range(worksheet.Cells[nMax + 3, 1], worksheet.Cells[nMax + 7, columnCount]);//选取单元格
                work.MergeCells = true;
                work.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                work.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                work.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                work.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                work.Font.Name = "宋体";//设置字体
                work.Font.Size = 13;//字体大小
                work.Font.Bold = false;//加粗显示
                StringBuilder sb = new StringBuilder();
                sb.Append("关于工作内容、工作时间等的描述（必填）：\n");
                string content = String.Format("    {0}", applicationForm.ApplyDesp.Replace("\n", " "));
                
                int padlen = 130;
                sb.Append(content);
                if (content.Length < 76)
                    sb.Append("\n\n");
                else if (content.Length < 151)
                    sb.Append("\n");
                else if (content.Length < 226)
                    sb.Append("\n");
                else
                    padlen = padlen - (content.Length - 225) * 2;
                padlen = 100;
                String time = applicationForm.SubmitTime.ToString("yyyy年MM月dd日");
                if (padlen > 0)
                    time = time.PadLeft(padlen, ' ');
                sb.Append(time);
                work.Value = sb.ToString();

                Range noteRange = worksheet.get_Range(worksheet.Cells[nMax + 8, 1], worksheet.Cells[nMax + 10, columnCount]);//选取单元格
                noteRange.MergeCells = true;
                noteRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                noteRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                noteRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                noteRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                noteRange.Font.Name = "宋体";//设置字体
                noteRange.Font.Size = 9;//字体大小
                noteRange.Font.Bold = false;//加粗显示
                noteRange.Value = "填表说明:1.各类劳务费应由领款本人签收并经课题负责人、部门负责人、单位负责人、经办人签字。\n         2.现金和转账发放都可使用该表，如通过银行转账发放，请准确填写收款本人的银行账户、开户银行、账户名称等信息。\n         3.单位填写分类：所内在职职工、所内注册学生、所内劳务流程。客座学生和外单位人员填写具体工作单位。";

                Range signRange = worksheet.get_Range(worksheet.Cells[nMax + 11, 1], worksheet.Cells[nMax + 12, columnCount]);//选取单元格
                signRange.MergeCells = true;
                signRange.Font.Name = "黑体";//设置字体
                signRange.Font.Size = 13;//字体大小
                signRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                signRange.HorizontalAlignment = XlHAlign.xlHAlignLeft; //水平靠左
                signRange.Value = " 单位负责人:                       部门负责人:                      项目负责人:                      课题负责人:                      经办人:                      ";
            }
            else if (applicationForm.PaymentType.Equals("现金支付"))
            {
                //支付类型为现金，少三个字段
                int rowCount = persons.Count();//总行数
                int nMax = rowCount + 6 - 1;
                const int columnCount = 12;//总列数

                //1.设置标题
                Range titleRange = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnCount]);//选取单元格
                titleRange.Merge(true);//合并单元格
                titleRange.Value = String.Format("发放      {0}      明细表", applicationForm.RefundType); //设置单元格内文本
                titleRange.Font.Name = "黑体";//设置字体
                titleRange.Font.Size = 13;//字体大小
                titleRange.Font.Bold = true;//加粗显示
                titleRange.Font.Underline = true;
                titleRange.HorizontalAlignment = XlHAlign.xlHAlignCenter; //水平居中
                titleRange.VerticalAlignment = XlVAlign.xlVAlignCenter;   //垂直居中

                //设置空行
                Range Range2 = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[2, columnCount]);//选取单元格
                Range2.Merge(true);

                Range tipRange = worksheet.get_Range(worksheet.Cells[3, 1], worksheet.Cells[3, columnCount]);//选取单元格
                tipRange.Merge(true);//合并单元格
                tipRange.Value = "我声明，以下填写的内容是完全真实的，如有不实，由此产生的一切后果由本人承担。---声明人签字:           "; //设置单元格内文本
                tipRange.Font.Name = "宋体";//设置字体
                tipRange.Font.Size = 10;//字体大小
                tipRange.Font.Bold = true;//加粗显示
                tipRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                tipRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中           

                //流水号
                Range SerRange = worksheet.get_Range(worksheet.Cells[4, 1], worksheet.Cells[4, columnCount]);//选取单元格
                SerRange.Merge(true);
                SerRange.Value = String.Format("流水号：  {0}", applicationForm.SerialNumber); //设置单元格内文本
                SerRange.Font.Name = "黑体";//设置字体
                SerRange.Font.Size = 11;//字体大小
                SerRange.Font.Bold = true;//加粗显示
                SerRange.HorizontalAlignment = XlHAlign.xlHAlignRight; //水平靠右
                SerRange.VerticalAlignment = XlVAlign.xlVAlignCenter;   //垂直居中


                Range infoRange = worksheet.get_Range(worksheet.Cells[5, 1], worksheet.Cells[5, columnCount]);//选取单元格
                infoRange.MergeCells = true;
                infoRange.Font.Name = "黑体";//设置字体
                infoRange.Font.Size = 10;//字体大小
                infoRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                infoRange.HorizontalAlignment = XlHAlign.xlHAlignLeft; //水平靠左
                infoRange.Value = String.Format("课题号：  {0}     课题负责人：  {1}       经办人：  {2}       支付类型：  {3}       合计金额：  {4} 元", applicationForm.ProjectNumber, applicationForm.ProjectDirector, applicationForm.Agent, applicationForm.PaymentType, applicationForm.Summation); //设置单元格内文本
                  
                //worksheet.Cells[3, 1] = "流水号";
                //worksheet.Cells[3, 2] = applicationForm.SerialNumber;
                //worksheet.Cells[3, 3] = "课题号";
                //worksheet.Cells[3, 4] = applicationForm.ProjectNumber;
                //worksheet.Cells[3, 5] = "课题负责人";
                //worksheet.Cells[3, 6] = applicationForm.ProjectDirector;
                //worksheet.Cells[3, 7] = "经办人";
                //worksheet.Cells[3, 8] = applicationForm.Agent;
                //worksheet.Cells[3, 9] = "合计金额";
                //worksheet.Cells[3, 10] = applicationForm.Summation;
                //worksheet.Cells[3, 11] = "支付类型";
                //worksheet.Cells[3, 12] = applicationForm.PaymentType;

                //Range info = worksheet.get_Range(worksheet.Cells[3, 1], worksheet.Cells[3, columnCount]);//选取单元格           
                //info.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                //info.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                //info.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                //info.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                //info.Font.Name = "宋体";//设置字体
                //info.Font.Size = 12;//字体大小
                //info.Font.Bold = true;//加粗显示
                //设置空行
                //Range Range2 = worksheet.get_Range(worksheet.Cells[4, 1], worksheet.Cells[4, columnCount]);//选取单元格
                //Range2.Merge(true);

                //2.设置表头
                string[] strHead = new string[columnCount] { "序号", "姓名", "证件类型", "证件号码", "单位", "联系电话", "国籍", "职称", "人员类型", "金额(元)", "是否含税", "领取人签字" };
                int[] columnWidth = new int[columnCount] { 4, 10, 10, 14, 16, 12, 8, 10, 10, 10, 10,  12 };
                for (int i = 0; i < columnCount; i++)
                {
                    Range headRange = worksheet.Cells[6, i + 1] as Range;//获取表头单元格,不用标题则从1开始
                    headRange.Value2 = strHead[i];//设置单元格文本
                    headRange.Font.Name = "宋体";//设置字体
                    headRange.Font.Size = 11;//字体大小
                    headRange.Font.Bold = true;//加粗显示
                    headRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;//水平居中
                    headRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中
                    headRange.ColumnWidth = columnWidth[i];//设置列宽
                }

                //3.填充数据
                for (int k = 1; k <= rowCount; k++)
                {
                    var person = persons[k - 1];
                    excelApp.Cells[k + 6, 1] = string.Format("{0}", k);
                    excelApp.Cells[k + 6, 2] = person.Name.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    excelApp.Cells[k + 6, 3] = person.CertificateType;
                    excelApp.Cells[k + 6, 4] = person.CertificateID;
                    excelApp.Cells[k + 6, 5] = person.Company;
                    excelApp.Cells[k + 6, 6] = person.Tele;
                    excelApp.Cells[k + 6, 7] = person.Nationality;
                    excelApp.Cells[k + 6, 8] = person.Title;
                    excelApp.Cells[k + 6, 9] = person.PersonType;
                    excelApp.Cells[k + 6, 10] = person.Amount;
                    excelApp.Cells[k + 6, 11] = person.TaxOrNot;
                    // excelApp.Cells[k + 5, 12] = person.PaymentType;
                    //excelApp.Cells[k + 5, 12] = person.Bank;
                    //excelApp.Cells[k + 5, 13] = person.AccountNumber;
                    //excelApp.Cells[k + 5, 14] = person.BankDetailName;
                }



                //Range tailRange = worksheet.Cells[nMax + 1, 11] as Range;
                //tailRange.ColumnWidth = 10;
                //tailRange.Value2 = "";

                //// Get ole objects and add new one
                //OLEObjects objs = worksheet.OLEObjects() as OLEObjects;
                //OLEObject obj = objs.Add("Forms.CheckBox.1", Missing.Value, Missing.Value, false, false,
                //    Missing.Value, Missing.Value, 560, 124, 108, 18);
                //obj.PrintObject = true;
                //OLEObject obj1 = objs.Add("Forms.CheckBox.1", Missing.Value, Missing.Value, false, false,
                //    Missing.Value, Missing.Value, 560, 138, 108, 18);
                //obj1.Name = "所外";

                Range contentRange = worksheet.get_Range(worksheet.Cells[6, 1], worksheet.Cells[nMax + 2, columnCount]);//选取单元格
                contentRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                contentRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                contentRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;//水平居中
                contentRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中
                //contentRange.Font.Name = "宋体";//设置字体
                //contentRange.Font.Size = 13;//字体大小
                //contentRange.Font.Bold = false;//加粗显示
                contentRange.WrapText = true;

                Range work = worksheet.get_Range(worksheet.Cells[nMax + 3, 1], worksheet.Cells[nMax + 7, columnCount]);//选取单元格
                work.MergeCells = true;
                work.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                work.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                work.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                work.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                work.Font.Name = "宋体";//设置字体
                work.Font.Size = 10;//字体大小
                work.Font.Bold = false;//加粗显示
                StringBuilder sb = new StringBuilder();
                sb.Append("关于工作内容、工作时间等的描述（必填）：\n");
                string content = String.Format("    {0}", applicationForm.ApplyDesp.Replace("\n", " "));
                
                int padlen = 130;
                sb.Append(content);
                if (content.Length < 76)
                    sb.Append("\n\n");
                else if (content.Length < 151)
                    sb.Append("\n");
                else if (content.Length < 226)
                    sb.Append("\n");
                else
                    padlen = padlen - (content.Length - 225) * 2;
                padlen = 100;
                String time = applicationForm.SubmitTime.ToString("yyyy年MM月dd日");
                if (padlen > 0)
                    time = time.PadLeft(padlen, ' ');
                sb.Append(time);
                work.Value = sb.ToString();

                Range noteRange = worksheet.get_Range(worksheet.Cells[nMax + 8, 1], worksheet.Cells[nMax + 10, columnCount]);//选取单元格
                noteRange.MergeCells = true;
                noteRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                noteRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                noteRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                noteRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                noteRange.Font.Name = "宋体";//设置字体
                noteRange.Font.Size = 9;//字体大小
                noteRange.Font.Bold = false;//加粗显示
                noteRange.Value = "填表说明:1.各类劳务费应由领款本人签收并经课题负责人、部门负责人、单位负责人、经办人签字。\n         2.现金和转账发放都可使用该表，如通过银行转账发放，请准确填写收款本人的银行账户、开户银行、账户名称等信息。\n         3.单位填写分类：所内在职职工、所内注册学生、所内劳务流程。客座学生和外单位人员填写具体工作单位。";

                Range signRange = worksheet.get_Range(worksheet.Cells[nMax + 11, 1], worksheet.Cells[nMax + 12, columnCount]);//选取单元格
                signRange.MergeCells = true;
                signRange.Font.Name = "黑体";//设置字体
                signRange.Font.Size = 10;//字体大小
                signRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                signRange.HorizontalAlignment = XlHAlign.xlHAlignLeft; //水平靠左
                signRange.Value = " 单位负责人:               部门负责人:                项目负责人:                课题负责人:                经办人:                 ";
      
            }          

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
                _log.Error(ex);
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
