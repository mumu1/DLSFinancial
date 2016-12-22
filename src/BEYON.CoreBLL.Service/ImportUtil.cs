﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.CoreBLL.Service.Excel;

namespace BEYON.CoreBLL.Service
{
    public class ImportUtil
    {
        public static List<ImportFeedBack> ValidateImportRecord<T>(LinqToExcel.Row record, int num, Dictionary<String, String> map, ref T personal)
        {
            List<ImportFeedBack> list = new List<ImportFeedBack>();
            ImportFeedBack feedBack = null;
            //非空验证
            feedBack = new ImportFeedBack();
            feedBack.ExceptionType = "空值";

            var properties = personal.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (String.IsNullOrEmpty(ImportUtil.GetValue(record, map, property.Name)))
                {
                    feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                }
                else
                {
                    property.SetValue(personal, ImportUtil.GetValue(record, map, property.Name));
                }
            }

            if (feedBack.ExceptionContent.Count > 0)
            {
                list.Add(feedBack);
            }

            return list;
        }

        public static String GetValue(LinqToExcel.Row record, Dictionary<String, String> map, String name)
        {
            if (map.ContainsKey(name))
            {
                return record[map[name]];
            }
            else
            {
                return record[name];
            }
        }

        public static Dictionary<String, String> GetColumns(ColumnMap[] colums, object record)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            if (colums != null)
            {
                foreach (var column in colums)
                {
                    result.Add(column.ColumnName, column.TitleName);
                }
            }
            else
            {
                var properties = record.GetType().GetProperties();
                foreach (var property in properties)
                {
                    result.Add(property.Name, property.Name);
                }
            }

            return result;
        }

        public static String ParseToHtml(List<ImportFeedBack> feedbacks)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (ImportFeedBack feedback in feedbacks)
            {
                sb.Append("<li>");
                sb.Append(String.Format("错误类型[ {0} ]", feedback.ExceptionType));
                sb.Append("</li>");
                if (feedback.ExceptionContent.Count > 0)
                {
                    sb.Append("<li>详细信息错误如下: </li>");
                    foreach (var error in feedback.ExceptionContent)
                    {
                        sb.Append("<li>");
                        sb.Append(error);
                        sb.Append("</li>");
                    }
                }
            }

            return sb.ToString();
        }
    }

    public class ImportFeedBack
    {
        public ImportFeedBack()
        {
            this.ExceptionContent = new List<String>();
        }

        public String ExceptionType { get; set; }
        public List<String> ExceptionContent { get; private set; }

    }
}
