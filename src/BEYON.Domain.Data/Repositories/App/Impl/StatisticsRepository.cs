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
                                var initialEaring = reader["C5"] != null ? Convert.ToSingle(reader["C5"]) : 0;
                                var initialTax = reader["C8"] != null ? Convert.ToSingle(reader["C8"]) : 0;
                                var initialTaxPayable = reader["C9"] != null ? Convert.ToSingle(reader["C9"]) : 0;
                                var amountX = String.IsNullOrEmpty(reader["C11"].ToString()) ? 0 : Convert.ToSingle(reader["C11"]);
                                var tax = String.IsNullOrEmpty(reader["C12"].ToString()) ? 0 : Convert.ToSingle(reader["C12"]);
                                var amountY = String.IsNullOrEmpty(reader["C13"].ToString()) ? 0 : Convert.ToSingle(reader["C13"]);

                                //返回前端显示结果数据
                                if (!objects.ContainsKey(certificateID))
                                {
                                    JObject result = new JObject();
                                    result["C1"] = reader["C1"].ToString();
                                    result["C2"] = reader["C2"].ToString();
                                    result["C3"] = reader["C3"].ToString();
                                    result["C4"] = certificateID;
                                    result["C5"] = initialEaring + amountX;
                                    result["C6"] = reader["C6"].ToString();
                                    result["C7"] = reader["C7"].ToString();
                                    result["C8"] = initialTax + tax;
                                    result["C9"] = initialTaxPayable + amountY;
                                    if (String.IsNullOrEmpty(reader["C11"].ToString()))
                                        result["C10"] = 0;
                                    else
                                        result["C10"] = 1;
                                    result["C11"] = amountY;
                                    result["C12"] = amountX;
                                    result["C13"] = tax;
                                    for (var i = 1; i < addColumns; i++)
                                    {
                                        result[String.Format("C{0}", 11 + i * 3)] = "";
                                        result[String.Format("C{0}", 12 + i * 3)] = "";
                                        result[String.Format("C{0}", 13 + i * 3)] = "";
                                    }
                                    objects.Add(certificateID, result);
                                }
                                else
                                {
                                    JObject jsonObject = objects[certificateID] as JObject;
                                    jsonObject["C5"] = jsonObject["C5"].ToObject<float>() + amountX;
                                    jsonObject["C8"] = jsonObject["C8"].ToObject<float>() + tax;
                                    jsonObject["C9"] = jsonObject["C9"].ToObject<float>() + amountY;
                                    int repetTimes = jsonObject["C10"].ToObject<int>() + 1;
                                    jsonObject["C10"] = repetTimes;

                                    jsonObject[String.Format("C{0}", 11 + (repetTimes - 1) * 3)] = amountY;
                                    jsonObject[String.Format("C{0}", 12 + (repetTimes - 1) * 3)] = amountX;
                                    jsonObject[String.Format("C{0}", 13 + (repetTimes - 1) * 3)] = tax;

                                    objects[certificateID] = jsonObject;
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                _log.Error(ex);
            }
            return "";
        }

        private void GetPerOrderLaborStatistics(ref Dictionary<String, JObject> objects, int addColumns, String period)
        {
            try
            {
                //1.获取工资表和劳务都有钱的
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                //1.添加列名
                sb.Append("a.\"Name\" as C2,");
                sb.Append("a.\"CertificateType\" as C3,");
                sb.Append("a.\"CertificateID\" as C4,");
                sb.Append("a.\"AmountX\" as C5,");
                sb.Append("a.\"Tax\" as C6,");
                sb.Append("a.\"AmountY\" as C7,");
                sb.Append("a.\"UpdateDate\" updateDate ");
                //2.添加表
                sb.Append(" FROM  dbo.\"TaxPerOrders\" a ");
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
                                var amountX = String.IsNullOrEmpty(reader["C5"].ToString()) ? 0 : Convert.ToSingle(reader["C5"]);
                                var tax = String.IsNullOrEmpty(reader["C6"].ToString()) ? 0 : Convert.ToSingle(reader["C6"]);
                                var amountY = String.IsNullOrEmpty(reader["C7"].ToString()) ? 0 : Convert.ToSingle(reader["C7"]);

                                //返回前端显示结果数据
                                if (!objects.ContainsKey(certificateID))
                                {
                                    JObject result = new JObject();
                                    result["C1"] = period;
                                    result["C2"] = reader["C2"].ToString();
                                    result["C3"] = reader["C3"].ToString();
                                    result["C4"] = certificateID;
                                    result["C5"] = amountX;
                                    result["C6"] = 800;
                                    result["C7"] = 800;
                                    result["C8"] = tax;
                                    result["C9"] = amountY;
                                    if (String.IsNullOrEmpty(reader["C5"].ToString()))
                                        result["C10"] = 0;
                                    else
                                        result["C10"] = 1;
                                    result["C11"] = amountY;
                                    result["C12"] = amountX;
                                    result["C13"] = tax;
                                    for (var i = 1; i < addColumns; i++)
                                    {
                                        result[String.Format("C{0}", 11 + i * 3)] = "";
                                        result[String.Format("C{0}", 12 + i * 3)] = "";
                                        result[String.Format("C{0}", 13 + i * 3)] = "";
                                    }
                                    objects.Add(certificateID, result);
                                }
                                else
                                {
                                    JObject jsonObject = objects[certificateID] as JObject;
                                    jsonObject["C5"] = jsonObject["C5"].ToObject<float>() + amountX;
                                    jsonObject["C8"] = jsonObject["C8"].ToObject<float>() + tax;
                                    jsonObject["C9"] = jsonObject["C9"].ToObject<float>() + amountY;
                                    int repetTimes = jsonObject["C10"].ToObject<int>() + 1;
                                    jsonObject["C10"] = repetTimes;

                                    jsonObject[String.Format("C{0}", 11 + (repetTimes - 1) * 3)] = amountY;
                                    jsonObject[String.Format("C{0}", 12 + (repetTimes - 1) * 3)] = amountX;
                                    jsonObject[String.Format("C{0}", 13 + (repetTimes - 1) * 3)] = tax;

                                    objects[certificateID] = jsonObject;
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

    }


}
