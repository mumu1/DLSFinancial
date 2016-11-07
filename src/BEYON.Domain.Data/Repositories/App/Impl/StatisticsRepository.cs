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
using Newtonsoft.Json.Linq;
using BEYON.Component.Data;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.App;


namespace BEYON.Domain.Data.Repositories.App.Impl
{
    public partial class StatisticsRepository : EFRepositoryBase<TaxPerOrder, Int32>, IStatisticsRepository
    {
        public StatisticsRepository(IUnitOfWork unitOfWork)
            : base()
        { }

        public int GetMaxCountPerMonthPerPerson()
        {
            string sql = "SELECT count(a.\"CertificateID\") as c FROM dbo.\"TaxBaseByMonths\" a, dbo.\"TaxPerOrders\" b WHERE a.\"CertificateID\" = b.\"CertificateID\" GROUP BY a.\"CertificateID\" ORDER BY c DESC";
            var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
            using(var conntion = new NpgsqlConnection(connectString.ToString()))
            {
                conntion.Open();
                using(var command = conntion.CreateCommand())
                {
                    command.CommandText = sql;
                    object tt = command.ExecuteScalar();

                    return Convert.ToInt32(tt);
                }
            }                            
        }

        public List<Object> GetPerMonthPerPerson()
        {
            Dictionary<String, JObject> objects = new Dictionary<String, JObject>();

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
            sb.Append("a.\"AmountY\" as C13 ");
            //2.添加表
            sb.Append(" FROM dbo.\"TaxPerOrders\" a,");
            sb.Append(" dbo.\"TaxBaseByMonths\" b ");
            //3.条件
            sb.Append(" WHERE a.\"CertificateID\" = b.\"CertificateID\" ");

            //string sql = "SELECT count(a.\"CertificateID\") FROM dbo.\"TaxBaseByMonths\" a, dbo.\"TaxPerOrders\" b WHERE a.\"CertificateID\" = b.\"CertificateID\" GROUP BY a.\"CertificateID\" ORDER BY DESC";
            var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
            using (var conntion = new NpgsqlConnection(connectString.ToString()))
            {
                conntion.Open();
                using (var command = conntion.CreateCommand())
                {
                    command.CommandText = sb.ToString();
                    using(NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        int nPos = 0;
                        while(reader.Read())
                        {
                            var certificateID = reader["C4"].ToString();
                            var initialEaring = reader["C5"] != null ? Convert.ToDouble(reader["C5"]) : 0;
                            var initialTax = reader["C8"] != null ? Convert.ToDouble(reader["C8"]) : 0;
                            var initialTaxPayable = reader["C9"] != null ? Convert.ToDouble(reader["C9"]) : 0;
                            var amountX = reader["C11"] != null ? Convert.ToDouble(reader["C11"]) : 0;
                            var tax = reader["C12"] != null ? Convert.ToDouble(reader["C12"]) : 0;
                            var amountY = reader["C13"] != null ? Convert.ToDouble(reader["C13"]) : 0;

                            //返回前端显示结果数据
                            if (!objects.ContainsKey(certificateID))
                            {
                                ++nPos;

                                JObject result = new JObject();
                                result["C0"] = nPos;
                                result["C1"] = reader["C1"].ToString();
                                result["C2"] = reader["C2"].ToString();
                                result["C3"] = reader["C3"].ToString();
                                result["C4"] = certificateID;
                                result["C5"] = initialEaring + amountX;
                                result["C6"] = reader["C6"].ToString();
                                result["C7"] = reader["C7"].ToString();
                                result["C8"] = initialTax + tax;
                                result["C9"] = initialTaxPayable + amountY;
                                result["C10"] = 1;
                                result["C11"] = amountY;
                                result["C12"] = amountX;
                                result["C13"] = tax;

                                objects.Add(certificateID, result);
                            }
                            else
                            {
                                JObject jsonObject = objects[certificateID] as JObject;
                                var count = jsonObject.Count;
                                jsonObject["C5"] = Convert.ToDouble(jsonObject["C5"]) + amountX;
                                jsonObject["C8"] = Convert.ToDouble(jsonObject["C8"]) + tax;
                                jsonObject["C9"] = Convert.ToDouble(jsonObject["C9"]) + amountY;
                                jsonObject["C10"] = Convert.ToDouble(jsonObject["C10"]) + 1;

                                jsonObject["C" + count] = amountY;
                                jsonObject["C" + count + 1] = amountY;
                                jsonObject["C" + count + 2] = amountY;

                                objects[certificateID] = jsonObject;
                            } 
                        }
                    }
                }
            }

            var resultTemp = new List<Object>();
            var serializer = new JavaScriptSerializer();
            foreach(var item in objects.Values)
            {
                resultTemp.Add(serializer.Deserialize(Newtonsoft.Json.JsonConvert.SerializeObject(item), typeof(object)));
            }

            return resultTemp;
        }
    }
}
