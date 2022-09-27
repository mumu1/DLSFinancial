using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Common;
using Office = Microsoft.Office.Core;
//using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Npgsql;

namespace ExcelCommands
{
    public class ApplyPrintService 
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ApplyPrintService()
        {
        }

        public void ApplyExcel(String filePath, String fileName, String serialNumber)
        {
            try
            {
                String connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"].ToString();
                using (var connect = new Npgsql.NpgsqlConnection(connectString))
                {
                    if (connect.State == System.Data.ConnectionState.Closed)
                        connect.Open();
                    var applicationform = ExecuteSQL(connect, String.Format("SELECT * FROM dbo.\"ApplicationForms\" WHERE \"SerialNumber\" = '{0}'", serialNumber));
                    var personalRecords = ExecuteSQL(connect, String.Format("SELECT * FROM dbo.\"PersonalRecords\" WHERE \"SerialNumber\" = '{0}'", serialNumber));
                    SaveExcel(filePath, fileName, applicationform, personalRecords);
                }
            }
            catch(Exception ex)
            {
                _log.Error(ex);
            }
            
        }

        private System.Data.DataTable ExecuteSQL(NpgsqlConnection connection, String sql)
        {
            using(var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                using(DbDataAdapter adapter = NpgsqlFactory.Instance.CreateDataAdapter())
                {
                    adapter.SelectCommand = command; 
                    System.Data.DataTable data = new System.Data.DataTable(); 
                    adapter.Fill(data); 
                    return data; 
                }
            }
        }
        private String GetFileName(String userid)
        {
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

        private String SaveExcel(String filePath, String fileName, System.Data.DataTable applicationFormTable, System.Data.DataTable personsTable)
        {
            ////1.删除已有文件
            //RemoveOldFile(filePath);

            System.Data.DataRow applicationForm = applicationFormTable.Rows[0];

            try
            {
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
                if (applicationForm["PaymentType"].ToString().Equals("银行转账"))
                {
                    //int rowCount = 0;//总行数
                    
                    const int columnCount = 15;//总列数          

                    //1.设置标题
                    Range titleRange = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnCount]);//选取单元格
                    titleRange.Merge(true);//合并单元格
                    titleRange.Value = String.Format("发放      {0}      明细表", applicationForm["RefundType"]); //设置单元格内文本
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
                    tipRange.RowHeight = 30;
                    tipRange.Value = "我声明，以下填写的内容是完全真实的，且本人与收款人不存在夫妻关系、直系血亲关系、三代以内旁系血亲关系以及近姻亲关系。"+"\n"+"如有不实，由此产生的一切后果由本人承担。---声明人签字:"; //设置单元格内文本
                    tipRange.Font.Name = "宋体";//设置字体
                    tipRange.Font.Size = 12;//字体大小
                    tipRange.Font.Bold = true;//加粗显示
                    tipRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                    tipRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中

                    //流水号
                    Range SerRange = worksheet.get_Range(worksheet.Cells[4, 1], worksheet.Cells[4, columnCount]);//选取单元格
                    SerRange.Merge(true);
                    SerRange.Value = String.Format("流水号：  {0}", applicationForm["SerialNumber"]); //设置单元格内文本
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
                    infoRange.Value = String.Format("课题号：  {0}         课题负责人：  {1}         经办人：  {2}         支付类型：  {3}         合计金额：  {4} 元", applicationForm["ProjectNumber"], applicationForm["ProjectDirector"], applicationForm["Agent"], applicationForm["PaymentType"],applicationForm["Summation"]); //设置单元格内文本
                  
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
                    for (int k = 1; k <= personsTable.Rows.Count; k++)
                    {
                        var persons = personsTable.Rows[k - 1];
                        excelApp.Cells[k + 6, 1] = string.Format("{0}", k);
                        excelApp.Cells[k + 6, 2] = persons["Name"].ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                        excelApp.Cells[k + 6, 3] = persons["CertificateType"];
                        excelApp.Cells[k + 6, 4] = persons["CertificateID"];
                        excelApp.Cells[k + 6, 5] = persons["Company"];
                        excelApp.Cells[k + 6, 6] = persons["Tele"];
                        excelApp.Cells[k + 6, 7] = persons["Nationality"];
                        excelApp.Cells[k + 6, 8] = persons["Title"];
                        excelApp.Cells[k + 6, 9] = persons["PersonType"];
                        excelApp.Cells[k + 6, 10] = persons["Amount"];
                        excelApp.Cells[k + 6, 11] = persons["TaxOrNot"];
                        // excelApp.Cells[k + 5, 12] = person.PaymentType;
                        excelApp.Cells[k + 6, 12] = persons["Bank"];
                        excelApp.Cells[k + 6, 13] = persons["AccountNumber"].ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                        excelApp.Cells[k + 6, 14] = persons["BankDetailName"];
                    }

                    //for (int k = 1; k <= rowCount; k++)
                    //{
                    //    var person = persons[k - 1];
                    //    excelApp.Cells[k + 6, 1] = string.Format("{0}", k);
                    //    excelApp.Cells[k + 6, 2] = person.Name.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    //    excelApp.Cells[k + 6, 3] = person.CertificateType;
                    //    excelApp.Cells[k + 6, 4] = person.CertificateID;
                    //    excelApp.Cells[k + 6, 5] = person.Company;
                    //    excelApp.Cells[k + 6, 6] = person.Tele;
                    //    excelApp.Cells[k + 6, 7] = person.Nationality;
                    //    excelApp.Cells[k + 6, 8] = person.Title;
                    //    excelApp.Cells[k + 6, 9] = person.PersonType;
                    //    excelApp.Cells[k + 6, 10] = person.Amount;
                    //    excelApp.Cells[k + 6, 11] = person.TaxOrNot;
                    //    // excelApp.Cells[k + 5, 12] = person.PaymentType;
                    //    excelApp.Cells[k + 6, 12] = person.Bank;
                    //    excelApp.Cells[k + 6, 13] = person.AccountNumber.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    //    excelApp.Cells[k + 6, 14] = person.BankDetailName;
                    //}

                    int nMax = personsTable.Rows.Count + 6 - 1;
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
                    string content = String.Format("    {0}", applicationForm["ApplyDesp"].ToString().Replace("\n", " "));
                
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
                    String time = DateTime.Parse(applicationForm["SubmitTime"].ToString()).ToString("yyyy年MM月dd日");
                    if (padlen > 0)
                        time = time.PadLeft(padlen, ' ');
                    sb.Append(time);
                    work.Value = sb.ToString();

                    Range noteRange = worksheet.get_Range(worksheet.Cells[nMax + 8, 1], worksheet.Cells[nMax + 10, columnCount]);//选取单元格
                    noteRange.MergeCells = true;
                    noteRange.RowHeight = 15;
                    noteRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                    noteRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                    noteRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                    noteRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                    noteRange.Font.Name = "宋体";//设置字体
                    noteRange.Font.Size = 9;//字体大小
                    noteRange.Font.Bold = false;//加粗显示
                    noteRange.Value = "填表说明:1.各类劳务费应由领款本人签收并经课题负责人、实验室（站）负责人、所领导、经办人签字。\n         2.现金和转账发放都可使用该表，如通过银行转账发放，请准确填写收款本人的银行账户、开户银行、账户名称等信息。\n         3.收款人证件类型不是居民身份证的，需要填写收款人性别和出生年月日。\n         4.单位填写分类：所内在职职工、所内注册学生、所内劳务流程。客座学生和外单位人员填写具体工作单位。";

                    Range signRange = worksheet.get_Range(worksheet.Cells[nMax + 11, 1], worksheet.Cells[nMax + 12, columnCount]);//选取单元格
                    signRange.MergeCells = true;
                    signRange.Font.Name = "黑体";//设置字体
                    signRange.Font.Size = 13;//字体大小
                    signRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                    signRange.HorizontalAlignment = XlHAlign.xlHAlignLeft; //水平靠左
                    signRange.Value = " 所领导:               实验室（站）负责人:                      研究室负责人:                      课题负责人:                      经办人:                      ";
                }
                else if (applicationForm["PaymentType"].ToString().Equals("现金支付"))
                {
                    //支付类型为现金，少三个字段
                    const int columnCount = 12;//总列数

                    //1.设置标题
                    Range titleRange = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnCount]);//选取单元格
                    titleRange.Merge(true);//合并单元格
                    titleRange.Value = String.Format("发放      {0}      明细表", applicationForm["RefundType"]); //设置单元格内文本
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
                    tipRange.RowHeight = 30;
                    tipRange.Value = "我声明，以下填写的内容是完全真实的，且本人与收款人不存在夫妻关系、直系血亲关系、三代以内旁系血亲关系以及近姻亲关系。"+"\n"+"如有不实，由此产生的一切后果由本人承担。---声明人签字:           "; //设置单元格内文本
                    tipRange.Font.Name = "宋体";//设置字体
                    tipRange.Font.Size = 10;//字体大小
                    tipRange.Font.Bold = true;//加粗显示
                    tipRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                    tipRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中           

                    //流水号
                    Range SerRange = worksheet.get_Range(worksheet.Cells[4, 1], worksheet.Cells[4, columnCount]);//选取单元格
                    SerRange.Merge(true);
                    SerRange.Value = String.Format("流水号：  {0}", applicationForm["SerialNumber"]); //设置单元格内文本
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
                    infoRange.Value = String.Format("课题号：  {0}     课题负责人：  {1}       经办人：  {2}       支付类型：  {3}       合计金额：  {4} 元", applicationForm["ProjectNumber"], applicationForm["ProjectDirector"], applicationForm["Agent"], applicationForm["PaymentType"], applicationForm["Summation"]); //设置单元格内文本
                  
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
                    for (int k = 1; k <= personsTable.Rows.Count; k++)
                    {
                        var persons = personsTable.Rows[k - 1];
                        excelApp.Cells[k + 6, 1] = string.Format("{0}", k);
                        excelApp.Cells[k + 6, 2] = persons["Name"].ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                        excelApp.Cells[k + 6, 3] = persons["CertificateType"];
                        excelApp.Cells[k + 6, 4] = persons["CertificateID"];
                        excelApp.Cells[k + 6, 5] = persons["Company"];
                        excelApp.Cells[k + 6, 6] = persons["Tele"];
                        excelApp.Cells[k + 6, 7] = persons["Nationality"];
                        excelApp.Cells[k + 6, 8] = persons["Title"];
                        excelApp.Cells[k + 6, 9] = persons["PersonType"];
                        excelApp.Cells[k + 6, 10] = persons["Amount"];
                        excelApp.Cells[k + 6, 11] = persons["TaxOrNot"];
                    }

                    //for (int k = 1; k <= rowCount; k++)
                    //{
                    //    var person = persons[k - 1];
                    //    excelApp.Cells[k + 6, 1] = string.Format("{0}", k);
                    //    excelApp.Cells[k + 6, 2] = person.Name.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    //    excelApp.Cells[k + 6, 3] = person.CertificateType;
                    //    excelApp.Cells[k + 6, 4] = person.CertificateID;
                    //    excelApp.Cells[k + 6, 5] = person.Company;
                    //    excelApp.Cells[k + 6, 6] = person.Tele;
                    //    excelApp.Cells[k + 6, 7] = person.Nationality;
                    //    excelApp.Cells[k + 6, 8] = person.Title;
                    //    excelApp.Cells[k + 6, 9] = person.PersonType;
                    //    excelApp.Cells[k + 6, 10] = person.Amount;
                    //    excelApp.Cells[k + 6, 11] = person.TaxOrNot;
                    //}

                    int nMax = personsTable.Rows.Count + 6 - 1;
                    Range contentRange = worksheet.get_Range(worksheet.Cells[6, 1], worksheet.Cells[nMax + 2, columnCount]);//选取单元格
                    contentRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                    contentRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                    contentRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;//水平居中
                    contentRange.VerticalAlignment = XlVAlign.xlVAlignCenter;//垂直居中
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
                    string content = String.Format("    {0}", applicationForm["ApplyDesp"].ToString().Replace("\n", " "));
                
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
                    String time = DateTime.Parse(applicationForm["SubmitTime"].ToString()).ToString("yyyy年MM月dd日");
                    if (padlen > 0)
                        time = time.PadLeft(padlen, ' ');
                    sb.Append(time);
                    work.Value = sb.ToString();

                    Range noteRange = worksheet.get_Range(worksheet.Cells[nMax + 8, 1], worksheet.Cells[nMax + 10, columnCount]);//选取单元格
                    noteRange.MergeCells = true;
                    noteRange.RowHeight = 15;
                    noteRange.Borders.LineStyle = XlLineStyle.xlContinuous;//设置边框
                    noteRange.Borders.Weight = XlBorderWeight.xlThin;//边框常规粗细
                    noteRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;//水平居中
                    noteRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                    noteRange.Font.Name = "宋体";//设置字体
                    noteRange.Font.Size = 9;//字体大小
                    noteRange.Font.Bold = false;//加粗显示
                    noteRange.Value = "填表说明:1.各类劳务费应由领款本人签收并经课题负责人、实验室（站）负责人、所领导、经办人签字。\n            2.现金和转账发放都可使用该表，如通过银行转账发放，请准确填写收款本人的银行账户、开户银行、账户名称等信息。\n         3.收款人证件类型不是居民身份证的，需要填写收款人性别和出生年月日。\n         4.单位填写分类：所内在职职工、所内注册学生、所内劳务流程。客座学生和外单位人员填写具体工作单位。";

                    Range signRange = worksheet.get_Range(worksheet.Cells[nMax + 11, 1], worksheet.Cells[nMax + 12, columnCount]);//选取单元格
                    signRange.MergeCells = true;
                    signRange.Font.Name = "黑体";//设置字体
                    signRange.Font.Size = 10;//字体大小
                    signRange.VerticalAlignment = XlVAlign.xlVAlignTop;//垂直居中
                    signRange.HorizontalAlignment = XlHAlign.xlHAlignLeft; //水平靠左
                    signRange.Value = " 所领导:               实验室（站）负责人:                研究室负责人:                课题负责人:                经办人:                 ";
      
                }          

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
