using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Dynamic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Npgsql;
using Npgsql.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using BEYON.Component.Data;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.App;


namespace BEYON.Domain.Data.Repositories.App.Impl
{
    public class DoubleConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return JToken.Load(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(String.Format("{0:N2}", value));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(float) || objectType == typeof(double);
        }
    }

    public partial class StatisticsRepository : EFRepositoryBase<TaxPerOrder, Int32>, IStatisticsRepository
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public StatisticsRepository(IUnitOfWork unitOfWork)
            : base()
        { }

        #region 按人明细表统计功能
        public int GetMaxCountPerMonthPerPerson()
        {
            try
            {
                string sql = "SELECT count(a.\"CertificateID\") as c FROM dbo.\"TaxBaseByMonths\" a, dbo.\"TaxPerOrders\" b WHERE a.\"CertificateID\" = b.\"CertificateID\" GROUP BY a.\"CertificateID\" ORDER BY c DESC";
                var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
                using (var conntion = new NpgsqlConnection(connectString.ToString()))
                {
                    conntion.Open();
                    using (var command = conntion.CreateCommand())
                    {
                        command.CommandText = sql;
                        object tt = command.ExecuteScalar();

                        return Convert.ToInt32(tt);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return 0;
        }

        public List<Object> GetPerMonthPerPerson()
        {
            Dictionary<String, JObject> objects = new Dictionary<String, JObject>();

            int addColumns = GetMaxCountPerMonthPerPerson();
            GetMonthWithinPerOrder(ref objects, addColumns);

            var resultTemp = new List<Object>();
            var serializer = new JavaScriptSerializer();
            int nPos = 1;
            foreach (var item in objects.Values)
            {
                item["C0"] = nPos++;
                resultTemp.Add(serializer.Deserialize(JsonConvert.SerializeObject(item, Formatting.Indented, new DoubleConverter()), typeof(object)));
            }

            return resultTemp;
        }

        private void GetMonthWithinPerOrder(ref Dictionary<String, JObject> objects, int addColumns)
        {
            try
            {
                //1.获取工资表和劳务都有钱的
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                //1.添加列名
                sb.Append("b.\"Period\" as C1,");
                sb.Append("b.\"Name\" as C2,");
                sb.Append("b.\"CertificateType\" as C3,");
                sb.Append("b.\"CertificateID\" as C4,");
                sb.Append("b.\"InitialEaring\" as C5,");
                sb.Append("b.\"TaxFree\" as C6,");
                sb.Append("b.\"AmountDeducted\" as C7,");
                sb.Append("b.\"InitialTax\" as C8,");
                sb.Append("b.\"InitialTaxPayable\" as C9,");
                sb.Append("a.\"AmountX\" as C11,");
                sb.Append("a.\"Tax\" as C12,");
                sb.Append("a.\"AmountY\" as C13,");
                sb.Append("a.\"ProjectNumber\" as C14,");
                sb.Append("a.\"ProjectDirector\" as C15,");
                sb.Append("a.\"UpdateDate\" updateDate ");
                //2.添加表
                sb.Append(" FROM dbo.\"TaxBaseByMonths\" b ");
                sb.Append(" LEFT OUTER JOIN  dbo.\"TaxPerOrders\" a ");
                //3.条件
                sb.Append(" ON a.\"CertificateID\" = b.\"CertificateID\" ORDER BY updateDate ASC");

                //string sql = "SELECT count(a.\"CertificateID\") FROM dbo.\"TaxBaseByMonths\" a, dbo.\"TaxPerOrders\" b WHERE a.\"CertificateID\" = b.\"CertificateID\" GROUP BY a.\"CertificateID\" ORDER BY DESC";
                var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
                using (var conntion = new NpgsqlConnection(connectString.ToString()))
                {
                    conntion.Open();
                    using (var command = conntion.CreateCommand())
                    {
                        command.CommandText = sb.ToString();
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var certificateID = reader["C4"].ToString();
                                var PersonalRecord = GetPersonRecordInfo(certificateID);
                                var initialEaring = reader["C5"] != null ? Convert.ToSingle(reader["C5"]) : 0;
                                var taxFree = reader["C6"] != null ? Convert.ToSingle(reader["C6"]) : 0;
                                var initialTax = reader["C8"] != null ? Convert.ToSingle(reader["C8"]) : 0;
                                var initialTaxPayable = reader["C9"] != null ? Convert.ToSingle(reader["C9"]) : 0;
                                var amountX = String.IsNullOrEmpty(reader["C11"].ToString()) ? 0 : Convert.ToSingle(reader["C11"]);
                                var tax = String.IsNullOrEmpty(reader["C12"].ToString()) ? 0 : Convert.ToSingle(reader["C12"]);
                                var amountY = String.IsNullOrEmpty(reader["C13"].ToString()) ? 0 : Convert.ToSingle(reader["C13"]);
                                var projectNumber = reader["C14"].ToString();
                                var projectDirector = reader["C15"].ToString();
                                //返回前端显示结果数据
                                if (!objects.ContainsKey(certificateID))
                                {
                                    JObject result = new JObject();
                                    result["C1"] = reader["C1"].ToString();
                                    result["C2"] = reader["C2"].ToString();
                                    result["C3"] = reader["C3"].ToString();
                                    result["C4"] = certificateID;
                                    result["C5"] = initialEaring + amountY;
                                    result["C6"] = taxFree;
                                    result["C7"] = reader["C7"].ToString();
                                    result["C8"] = initialTax + tax;
                                    result["C9"] =0.0;
                                    if (PersonalRecord != null)
                                    {
                                        if (!String.IsNullOrEmpty(PersonalRecord.Tele))
                                        {
                                            result["C10"] = PersonalRecord.Tele;
                                        }
                                        else {
                                            result["C10"] = "";
                                        }
                                        if (!String.IsNullOrEmpty(PersonalRecord.Nationality))
                                        {
                                            result["C11"] = PersonalRecord.Nationality;
                                        }
                                        else
                                        {
                                            result["C11"] = "";
                                        }
                                        if (!String.IsNullOrEmpty(PersonalRecord.Company))
                                        {
                                            result["C12"] = PersonalRecord.Company;
                                        }
                                        else
                                        {
                                            result["C12"] = "";
                                        }
                                        if (!String.IsNullOrEmpty(PersonalRecord.Title))
                                        {
                                            result["C13"] = PersonalRecord.Title;
                                        }
                                        else
                                        {
                                            result["C13"] = "";
                                        }
                                    }
                                    if (String.IsNullOrEmpty(reader["C11"].ToString()))
                                        result["C14"] = 0;
                                    else
                                        result["C14"] = 1;
                                    result["C15"] = amountY;
                                    result["C16"] = amountX;
                                    result["C17"] = tax;
                                    result["C18"] = projectNumber;
                                    result["C19"] = projectDirector;
                                    for (var i = 1; i < addColumns; i++)
                                    {
                                        result[String.Format("C{0}", 15 + i * 5)] = "";
                                        result[String.Format("C{0}", 16 + i * 5)] = "";
                                        result[String.Format("C{0}", 17 + i * 5)] = "";
                                        result[String.Format("C{0}", 18 + i * 5)] = "";
                                        result[String.Format("C{0}", 19 + i * 5)] = "";
                                    }
                                    objects.Add(certificateID, result);
                                }
                                else
                                {
                                    JObject jsonObject = objects[certificateID] as JObject;
                                    jsonObject["C5"] = jsonObject["C5"].ToObject<float>() + amountY;
                                    jsonObject["C8"] = jsonObject["C8"].ToObject<float>() + tax;
                                    jsonObject["C9"] = jsonObject["C9"].ToObject<float>() ;
                                    int repetTimes = jsonObject["C14"].ToObject<int>() + 1;
                                    jsonObject["C14"] = repetTimes;

                                    jsonObject[String.Format("C{0}", 15 + (repetTimes - 1) * 5)] = amountY;
                                    jsonObject[String.Format("C{0}", 16 + (repetTimes - 1) * 5)] = amountX;
                                    jsonObject[String.Format("C{0}", 17 + (repetTimes - 1) * 5)] = tax;
                                    if (!String.IsNullOrEmpty(projectNumber))
                                    {
                                        jsonObject[String.Format("C{0}", 18 + (repetTimes - 1) * 5)] = projectNumber;
                                        
                                    }
                                    if (!String.IsNullOrEmpty(projectDirector))
                                    {
                                        jsonObject[String.Format("C{0}", 19 + (repetTimes - 1) * 5)] = projectDirector;
                                    }

                                    objects[certificateID] = jsonObject;
                                }
                            }
                            //计算应纳税所得额    =收入额C5-免税金额C6-基本扣除C7，若值<0,则改为0
                            foreach (var obj in objects)
                            {
                                if (obj.Value["C5"].ToObject<float>() - obj.Value["C6"].ToObject<float>() - obj.Value["C7"].ToObject<float>() < 0)
                                {
                                    obj.Value["C9"] = 0.0;
                                }
                                else
                                {
                                    obj.Value["C9"] = obj.Value["C5"].ToObject<float>() - obj.Value["C6"].ToObject<float>() - obj.Value["C7"].ToObject<float>();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
        #endregion

        #region 按劳务统计功能
        public int GetMaxCountLaborStatistics()
        {
            try
            {
                string sql = "SELECT count(a.\"CertificateID\") as c FROM dbo.\"TaxPerOrders\" a GROUP BY a.\"CertificateID\" ORDER BY c DESC";
                var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
                using (var conntion = new NpgsqlConnection(connectString.ToString()))
                {
                    conntion.Open();
                    using (var command = conntion.CreateCommand())
                    {
                        command.CommandText = sql;
                        object tt = command.ExecuteScalar();

                        return Convert.ToInt32(tt);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return 0;
        }

        public List<Object> GetLaborStatisticsDetail()
        {
            Dictionary<String, JObject> objects = new Dictionary<String, JObject>();

            int addColumns = GetMaxCountLaborStatistics();
            String period = GetPerid();
            GetPerOrderLaborStatistics(ref objects, addColumns, period);

            var resultTemp = new List<Object>();
            var serializer = new JavaScriptSerializer();
            int nPos = 1;
            foreach (var item in objects.Values)
            {
                item["C0"] = nPos++;
                resultTemp.Add(serializer.Deserialize(Newtonsoft.Json.JsonConvert.SerializeObject(item, Formatting.Indented, new DoubleConverter()), typeof(object)));
            }

            return resultTemp;
        }

        private String GetPerid()
        {
            try
            {
                string sql = "SELECT a.\"Period\" FROM dbo.\"TaxBaseByMonths\" a ";
                var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
                using (var conntion = new NpgsqlConnection(connectString.ToString()))
                {
                    conntion.Open();
                    using (var command = conntion.CreateCommand())
                    {
                        command.CommandText = sql;
                        return command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return "";
        }

        private void GetPerOrderLaborStatistics(ref Dictionary<String, JObject> objects, int addColumns, String period)
        {
            try
            {
                
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                //1.添加列名
                sb.Append("a.\"Name\" as C2,");
                sb.Append("a.\"CertificateType\" as C3,");
                sb.Append("a.\"CertificateID\" as C4,");
                sb.Append("a.\"AmountX\" as C5,");
                sb.Append("a.\"Tax\" as C6,");
                sb.Append("a.\"AmountY\" as C7,");
                sb.Append("a.\"ProjectNumber\" as C8,");
                sb.Append("a.\"ProjectDirector\" as C9,");
                sb.Append("a.\"UpdateDate\" updateDate ");
                //2.添加表
                sb.Append(" FROM  dbo.\"TaxPerOrders\" a WHERE  \"PersonType\" = '所外' ");
                //3.条件
                sb.Append(" ORDER BY updateDate ASC");

                //string sql = "SELECT count(a.\"CertificateID\") FROM dbo.\"TaxBaseByMonths\" a, dbo.\"TaxPerOrders\" b WHERE a.\"CertificateID\" = b.\"CertificateID\" GROUP BY a.\"CertificateID\" ORDER BY DESC";
                var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
                using (var conntion = new NpgsqlConnection(connectString.ToString()))
                {
                    conntion.Open();
                    using (var command = conntion.CreateCommand())
                    {
                        command.CommandText = sb.ToString();
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var certificateID = reader["C4"].ToString();
                                var PersonalRecord = GetPersonRecordInfo(certificateID);
                                
                                var amountX = String.IsNullOrEmpty(reader["C5"].ToString()) ? 0 : Convert.ToSingle(reader["C5"]);
                                var tax = String.IsNullOrEmpty(reader["C6"].ToString()) ? 0 : Convert.ToSingle(reader["C6"]);
                                var amountY = String.IsNullOrEmpty(reader["C7"].ToString()) ? 0 : Convert.ToSingle(reader["C7"]);
                                var projectNumber = reader["C8"].ToString();
                                var projectDirector = reader["C9"].ToString();

                                //返回前端显示结果数据
                                if (!objects.ContainsKey(certificateID))
                                {
                                    JObject result = new JObject();
                                    result["C1"] = period;
                                    result["C2"] = reader["C2"].ToString();
                                    result["C3"] = reader["C3"].ToString();
                                    result["C4"] = certificateID;
                                    result["C5"] = amountY;
                                    result["C6"] = 800;
                                    result["C7"] = 800;
                                    result["C8"] = tax;
                                    result["C9"] = 0.0;
                                    if (PersonalRecord != null)
                                    {
                                        if (!String.IsNullOrEmpty(PersonalRecord.Tele))
                                        {
                                            result["C10"] = PersonalRecord.Tele;
                                        }
                                        else
                                        {
                                            result["C10"] = "";
                                        }
                                        if (!String.IsNullOrEmpty(PersonalRecord.Nationality))
                                        {
                                            result["C11"] = PersonalRecord.Nationality;
                                        }
                                        else
                                        {
                                            result["C11"] = "";
                                        }
                                        if (!String.IsNullOrEmpty(PersonalRecord.Company))
                                        {
                                            result["C12"] = PersonalRecord.Company;
                                        }
                                        else
                                        {
                                            result["C12"] = "";
                                        }
                                        if (!String.IsNullOrEmpty(PersonalRecord.Title))
                                        {
                                            result["C13"] = PersonalRecord.Title;
                                        }
                                        else
                                        {
                                            result["C13"] = "";
                                        }
                                    }
                                  
                                    if (String.IsNullOrEmpty(reader["C5"].ToString()))
                                        result["C14"] = 0;
                                    else
                                        result["C14"] = 1;
                                    result["C15"] = amountY;
                                    result["C16"] = amountX;
                                    result["C17"] = tax;
                                    result["C18"] = projectNumber;
                                    result["C19"] = projectDirector;
                                    for (var i = 1; i < addColumns; i++)
                                    {
                                        result[String.Format("C{0}", 15 + i * 5)] = "";
                                        result[String.Format("C{0}", 16 + i * 5)] = "";
                                        result[String.Format("C{0}", 17 + i * 5)] = "";
                                        result[String.Format("C{0}", 18 + i * 5)] = "";
                                        result[String.Format("C{0}", 19 + i * 5)] = "";
                                    }
                                    objects.Add(certificateID, result);
                                }
                                else
                                {
                                    JObject jsonObject = objects[certificateID] as JObject;
                                    jsonObject["C5"] = jsonObject["C5"].ToObject<float>() + amountY;
                                    jsonObject["C8"] = jsonObject["C8"].ToObject<float>() + tax;
                                    jsonObject["C9"] = jsonObject["C9"].ToObject<float>() ;
                                    int repetTimes = jsonObject["C14"].ToObject<int>() + 1;
                                    jsonObject["C14"] = repetTimes;

                                    jsonObject[String.Format("C{0}", 15 + (repetTimes - 1) * 5)] = amountY;
                                    jsonObject[String.Format("C{0}", 16 + (repetTimes - 1) * 5)] = amountX;
                                    jsonObject[String.Format("C{0}", 17 + (repetTimes - 1) * 5)] = tax;
                                    if (!String.IsNullOrEmpty(projectNumber))
                                    {
                                        jsonObject[String.Format("C{0}", 18 + (repetTimes - 1) * 5)] = projectNumber;

                                    }
                                    if (!String.IsNullOrEmpty(projectDirector))
                                    {
                                        jsonObject[String.Format("C{0}", 19 + (repetTimes - 1) * 5)] = projectDirector;
                                    }

                                    objects[certificateID] = jsonObject;
                                }
                            }
                            //计算应纳税所得额    =收入额C5-基本扣除C7
                            foreach (var obj in objects)
                            {
                                if (obj.Value["C5"].ToObject<float>() - obj.Value["C7"].ToObject<float>() < 0)
                                {
                                    obj.Value["C9"] = 0.0;
                                }
                                else
                                {
                                    obj.Value["C9"] = obj.Value["C5"].ToObject<float>() - obj.Value["C7"].ToObject<float>();
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
        #endregion

        #region 按课题统计功能

        public List<Object> GetTaskStatisticsDetail()
        {
            Dictionary<String, JObject> objects = new Dictionary<String, JObject>();
            String period = GetPerid();
            GetPerOrderTaskStatistics(ref objects, period);

            var resultTemp = new List<Object>();
            var serializer = new JavaScriptSerializer();
            int nPos = 1;
            foreach (var item in objects.Values)
            {
                item["C0"] = nPos++;
                resultTemp.Add(serializer.Deserialize(Newtonsoft.Json.JsonConvert.SerializeObject(item, Formatting.Indented, new DoubleConverter()), typeof(object)));
            }

            return resultTemp;
        }

        private void GetPerOrderTaskStatistics(ref Dictionary<String, JObject> objects, String period)
        {
            try
            {
                //1.获取数据[{"data":"C0","title":"序号"},{"data":"C1","title":"期间"},{"data":"C2","title":"课题号"},{"data":"C3","title":"金额"},{"data":"C4","title":"报销事由"},{"data":"C5","title":"课题负责人"},{"data":"C6","title":"工资薪金税额"},{"data":"C7","title":"劳务费税额"}]
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                //1.添加列名
                sb.Append("a.\"ProjectNumber\" as C2,");
                sb.Append("a.\"AmountY\" as C3,");
                sb.Append("a.\"RefundType\" as C4,");
                sb.Append("a.\"ProjectDirector\" as C5,");
                sb.Append("a.\"Tax\" as C6,");
                sb.Append("a.\"PersonType\" as C7,");
                sb.Append("a.\"UpdateDate\" updateDate ");
                //2.添加表
                sb.Append(" FROM  dbo.\"TaxPerOrders\" a ");
                //3.条件
                sb.Append(" ORDER BY C2 ASC");

                var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
                using (var conntion = new NpgsqlConnection(connectString.ToString()))
                {
                    conntion.Open();
                    using (var command = conntion.CreateCommand())
                    {
                        command.CommandText = sb.ToString();
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var projectNumber = reader["C2"].ToString();
                                var amountY = String.IsNullOrEmpty(reader["C3"].ToString()) ? 0 : Convert.ToSingle(reader["C3"]);
                                var refundType = reader["C4"].ToString();
                                var projectDirector = reader["C5"].ToString();
                                var tax = String.IsNullOrEmpty(reader["C6"].ToString()) ? 0 : Convert.ToSingle(reader["C6"]);
                                var personType = reader["C7"].ToString();
                                //返回前端显示结果数据
                                if (!objects.ContainsKey(projectNumber + "," + refundType))
                                {
                                    JObject result = new JObject();
                                    result["C1"] = period;
                                    result["C2"] = projectNumber;
                                    result["C3"] = amountY;
                                    result["C4"] = refundType;
                                    result["C5"] = projectDirector;
                                    if (personType.Equals("所内"))
                                    {
                                        result["C6"] = tax;
                                        result["C7"] = 0.00;
                                    }
                                    else
                                    {
                                        result["C6"] = 0.00;
                                        result["C7"] = tax;
                                    }
                                    objects.Add(projectNumber + "," + refundType, result);
                                }
                                else
                                {
                                    JObject jsonObject = objects[projectNumber + "," + refundType] as JObject;
                                    jsonObject["C3"] = jsonObject["C3"].ToObject<float>() + amountY;
                                    if (personType.Equals("所内"))
                                    {
                                        jsonObject["C6"] = jsonObject["C6"].ToObject<float>() + tax;
                                    }
                                    else
                                    {
                                        jsonObject["C7"] = jsonObject["C7"].ToObject<float>() + tax;
                                    }
                                    objects[projectNumber + "," + refundType] = jsonObject;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
        #endregion

        #region 按申请单流水号统计功能

        public List<Object> GetSerNumberStatisticsDetail()
        {
            Dictionary<String, JObject> objects = new Dictionary<String, JObject>();
            GetPerOrderSerNumberStatistics(ref objects);

            var resultTemp = new List<Object>();
            var serializer = new JavaScriptSerializer();
            int nPos = 1;
            foreach (var item in objects.Values)
            {
                item["C0"] = nPos++;
                resultTemp.Add(serializer.Deserialize(Newtonsoft.Json.JsonConvert.SerializeObject(item, Formatting.Indented, new DoubleConverter()), typeof(object)));
            }

            return resultTemp;
        }

        private void GetPerOrderSerNumberStatistics(ref Dictionary<String, JObject> objects)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                //1.添加列名
                sb.Append("a.\"SerialNumber\" as C1,");
                sb.Append("a.\"ProjectNumber\" as C2,");
                sb.Append("a.\"RefundType\" as C3,");
                sb.Append("a.\"ProjectDirector\" as C4,");
                sb.Append("a.\"Tax\" as C5,");
                sb.Append("a.\"Agent\" as C6,");
                sb.Append("a.\"Amount\" as C7,");
                sb.Append("a.\"PersonType\" as C8,");
                sb.Append("a.\"PaymentType\" as C9,");
                sb.Append("a.\"UpdateDate\" updateDate ");
                //2.添加表
                sb.Append(" FROM  dbo.\"TaxPerOrders\" a ");
                //3.条件
                sb.Append(" ORDER BY C1 ASC");

                var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
                using (var conntion = new NpgsqlConnection(connectString.ToString()))
                {
                    conntion.Open();
                    using (var command = conntion.CreateCommand())
                    {
                        command.CommandText = sb.ToString();
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {                        
                                var serNumber = reader["C1"].ToString();
                                var projectNumber = reader["C2"].ToString();
                                var refundType = reader["C3"].ToString();
                                var projectDirector = reader["C4"].ToString();
                                var tax = String.IsNullOrEmpty(reader["C5"].ToString()) ? 0 : Convert.ToSingle(reader["C5"]);
                                var agent = reader["C6"].ToString();
                                var amount = String.IsNullOrEmpty(reader["C7"].ToString()) ? 0 : Convert.ToSingle(reader["C7"]);
                                var personType = reader["C8"].ToString();
                                var paymentType = reader["C9"].ToString();
                                var updateTime = reader["updateDate"].ToString();
                                //返回前端显示结果数据
                                if (!objects.ContainsKey(serNumber))
                                {
                                    JObject result = new JObject();
                                    result["C1"] = serNumber;
                                    result["C2"] = projectNumber;
                                    result["C3"] = amount;
                                    result["C4"] = tax;
                                    if (personType.Equals("所内"))
                                    {
                                        result["C5"] = tax;
                                        result["C6"] = 0.00;
                                    }
                                    else
                                    {
                                        result["C5"] = 0.00;
                                        result["C6"] = tax;
                                    }
                                    result["C7"] = paymentType;
                                    result["C8"] = refundType;
                                    result["C9"] = projectDirector;
                                    result["C10"] = agent;
                                    result["C11"] = updateTime;
                                    objects.Add(serNumber, result);
                                }
                                else
                                {
                                    JObject jsonObject = objects[serNumber] as JObject;
                                    jsonObject["C3"] = jsonObject["C3"].ToObject<float>() + amount;
                                    jsonObject["C4"] = jsonObject["C4"].ToObject<float>() + tax;
                                    if (personType.Equals("所内"))
                                    {
                                        jsonObject["C5"] = jsonObject["C5"].ToObject<float>() + tax;
                                    }
                                    else
                                    {
                                        jsonObject["C6"] = jsonObject["C6"].ToObject<float>() + tax;
                                    }
                                    objects[serNumber] = jsonObject;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
        #endregion

        #region 按课题统计功能(新需求)

        public List<Object> GetTaskStatisticsDetail1()
        {
            Dictionary<String, JObject> objects = new Dictionary<String, JObject>();
            String period = GetPerid();
            GetPerOrderTaskStatistics1(ref objects, period);

            var resultTemp = new List<Object>();
            var serializer = new JavaScriptSerializer();
            int nPos = 1;
            foreach (var item in objects.Values)
            {
                item["C0"] = nPos++;
                resultTemp.Add(serializer.Deserialize(Newtonsoft.Json.JsonConvert.SerializeObject(item, Formatting.Indented, new DoubleConverter()), typeof(object)));
            }

            return resultTemp;
        }

        private void GetPerOrderTaskStatistics1(ref Dictionary<String, JObject> objects, String period)
        {
            try
            {
                //1.获取数据[{"data":"C0","title":"序号"},{"data":"C1","title":"期间"},{"data":"C2","title":"课题号"},{"data":"C3","title":"金额"},{"data":"C4","title":"报销事由"},{"data":"C5","title":"课题负责人"},{"data":"C6","title":"工资薪金税额"},{"data":"C7","title":"劳务费税额"}]
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                //1.添加列名
                sb.Append("a.\"ProjectNumber\" as C2,");
                sb.Append("a.\"AmountY\" as C3,");
                sb.Append("a.\"RefundType\" as C4,");
                sb.Append("a.\"ProjectDirector\" as C5,");
                sb.Append("a.\"Tax\" as C6,");
                sb.Append("a.\"PersonType\" as C7,");
                sb.Append("a.\"AmountX\" as C8,");
                sb.Append("a.\"PaymentType\" as C9,");
                sb.Append("a.\"Amount\" as C10,");
                sb.Append("a.\"TaxOrNot\" as C11,");
                sb.Append("a.\"UpdateDate\" updateDate ");
                //2.添加表
                sb.Append(" FROM  dbo.\"TaxPerOrders\" a ");
                //3.条件
                sb.Append(" ORDER BY C2 ASC");
                
                var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
                using (var conntion = new NpgsqlConnection(connectString.ToString()))
                {
                    conntion.Open();
                    using (var command = conntion.CreateCommand())
                    {
                        command.CommandText = sb.ToString();
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var projectNumber = reader["C2"].ToString();                               

                                var amountY = String.IsNullOrEmpty(reader["C3"].ToString()) ? 0 : Convert.ToSingle(reader["C3"]);
                                var refundType = reader["C4"].ToString();
                                //通过refundType获取refundTypeCode
                                var refundTypeCode = GetRefundTypeCode(refundType);
                                var projectDirector = reader["C5"].ToString();
                                var tax = String.IsNullOrEmpty(reader["C6"].ToString()) ? 0 : Convert.ToSingle(reader["C6"]);
                                var personType = reader["C7"].ToString();
                                var paymentType = reader["C9"].ToString();
                                var amountX = String.IsNullOrEmpty(reader["C8"].ToString()) ? 0 : Convert.ToSingle(reader["C8"]);
                                var amount = String.IsNullOrEmpty(reader["C10"].ToString()) ? 0 : Convert.ToSingle(reader["C10"]);
                                var taxOrNot = reader["C11"].ToString();
                                //返回前端显示结果数据
                                if (!objects.ContainsKey(projectNumber + "," + refundType))
                                {
                                    JObject result = new JObject();
                                    result["C1"] = period;
                                    
                                    if (!String.IsNullOrEmpty(projectNumber) && !projectNumber.Equals("无") && projectNumber.Contains('|'))
                                    {
                                        string[] projectNumberSep = projectNumber.Split('|');
                                        if (projectNumberSep.Length > 1)
                                        {
                                            result["C2"] = projectNumberSep[0].Trim();
                                            result["C10"] = projectNumberSep[1].Trim();
                                        }
                                        else {
                                            result["C2"] = projectNumber;
                                            result["C10"] = "";
                                        }
                                    }
                                    else
                                    {
                                        result["C2"] = projectNumber;
                                        result["C10"] = "";
                                    }
                                
                                    result["C3"] = projectDirector;
                                    result["C4"] = refundType;
                                    result["C5"] = refundTypeCode;
                                    result["C6"] = 0.0;
                                    result["C7"] = 0.0;
                                    result["C8"] = 0.0;
                                    result["C9"] = 0.0;
                                    if (!String.IsNullOrEmpty(paymentType) && paymentType.Equals("银行转账")) {
                                        if (!String.IsNullOrEmpty(taxOrNot) && taxOrNot.Equals("含税"))
                                        {
                                            result["C6"] = amount;
                                        }
                                        else if (!String.IsNullOrEmpty(taxOrNot) && taxOrNot.Equals("不含税"))
                                        {
                                            result["C6"] = amount + tax;
                                        }
                                        if (!String.IsNullOrEmpty(personType) && personType.Equals("所内"))
                                        {
                                            result["C8"] = tax;
                                        }
                                        else if (!String.IsNullOrEmpty(personType) && personType.Equals("所外"))
                                        {
                                            result["C7"] = tax;
                                        }
                                        result["C9"] = tax;
                                    }
                                    
                                    objects.Add(projectNumber + "," + refundType, result);
                                }
                                else
                                {
                                    JObject jsonObject = objects[projectNumber + "," + refundType] as JObject;
                                    if (!String.IsNullOrEmpty(paymentType) && paymentType.Equals("银行转账")) {
                                        if (!String.IsNullOrEmpty(taxOrNot) && taxOrNot.Equals("含税"))
                                        {
                                            jsonObject["C6"] =  jsonObject["C6"].ToObject<float>() + amount ;
                                        }
                                        else if (!String.IsNullOrEmpty(taxOrNot) && taxOrNot.Equals("不含税"))
                                        {
                                            jsonObject["C6"] = jsonObject["C6"].ToObject<float>() + amount + tax;
                                        }
                                        if (!String.IsNullOrEmpty(personType) && personType.Equals("所内"))
                                        {
                                            jsonObject["C8"] = jsonObject["C8"].ToObject<float>() + tax;
                                        }
                                        else if (!String.IsNullOrEmpty(personType) && personType.Equals("所外"))
                                        {
                                            jsonObject["C7"] = jsonObject["C7"].ToObject<float>() + tax;
                                        }
                                        jsonObject["C9"] = jsonObject["C9"].ToObject<float>() + tax;
                                    }                                 
                                    objects[projectNumber + "," + refundType] = jsonObject;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
        #endregion

        private String GetRefundTypeCode(String refundType)
        {
            var refundTypeCode = from p in Context.RefundTypes.Where(w => w.RefundTypeName == refundType)
                                 select new { RefundTypeCode = p.RefundTypeCode, RefundTypeName = p.RefundTypeName };
            var lists = refundTypeCode.ToList();

            if (lists.Count > 0)
                return lists[0].RefundTypeCode.ToString();
            else
                return "";
        }

        private PersonalRecord GetPersonRecordInfo(String certificateID)
        {
            var person = from p in Context.PersonalRecords.Where(w => w.CertificateID == certificateID).OrderByDescending(t => t.UpdateDate)
                         select new { Tele = p.Tele, Nationality = p.Nationality, Company = p.Company, Title = p.Title };
            var lists = person.ToList();

            if (lists.Count > 0)
                return new PersonalRecord()
                {
                    Tele = lists[0].Tele,
                    Nationality = lists[0].Nationality,
                    Company = lists[0].Company,
                    Title = lists[0].Title

                };
            else
                return null;
        }

    }
}
