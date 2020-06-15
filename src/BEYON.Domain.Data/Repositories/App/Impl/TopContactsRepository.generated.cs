﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//	   如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类进行实现。
// </auto-generated>
//
// <copyright file="UserRepository.generated.cs">
//		Copyright(c)2013 Beyon.All rights reserved.
//		CLR版本：4.5.1
//		开发组织：北京博阳世通信息技术有限公司
//		公司网站：http://www.beyondb.com.cn
//		所属工程：BEYON.Domain.Data
//		生成时间：2016-03-01 16:09
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Npgsql.Schema;
using BEYON.Component.Data;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.App;


namespace BEYON.Domain.Data.Repositories.App.Impl
{
	/// <summary>
    ///   仓储操作层实现——用户信息
    /// </summary>
    public partial class TopContactsRepository : EFRepositoryBase<TopContacts, Int32>, ITopContactsRepository
    {
        public TopContactsRepository(IUnitOfWork unitOfWork)
            : base()
        { }

        public IList<TopContacts> GetTopContactsByUserID(String userID)
        {
            var q = from p in Context.TopContactss.Where(w => w.UserID == userID)
                    select p;
            return q.ToList();
        }

        public IList<TopContacts> GetTopContactsByName(String name, String userID)
        {
            var q = from p in Context.TopContactss.Where(w => w.Name == name && w.UserID == userID)
                    select p;
            return q.ToList();
        }

        public void InsertOrUpdate(TopContacts contact)
        {
            //1.构造插入或更新SQL
            string[] columns = new string[]{
                "UserID","Name","CertificateType","CertificateID",
                "Company","Tele","PersonType","Nationality","Title",
                "Bank","BankDetailName","AccountNumber","ProvinceCity","CityField","Gender","Birth",
                "UpdateDate"
            };
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.\"TopContacts\" ( ");
            foreach(var column in columns)
            {
                sql.Append(String.Format("\"{0}\",", column));
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" ) VALUES ( ");
            //填充值
            foreach(var column in columns)
            {
                sql.Append(String.Format(":{0},",column));
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" )  ON CONFLICT (\"UserID\", \"CertificateID\") DO UPDATE SET ");

            foreach (var column in columns)
            {
                sql.Append(String.Format("\"{0}\"=:{1},", column, column));
            }
            sql.Remove(sql.Length - 1, 1);

            //2.添加参数变量
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            foreach (var column in columns)
            {
                var value = PropertyUtil.GetPropValue(contact, column);
                if (value == null)
                    value = "";            
                    if (!String.IsNullOrEmpty(value.ToString())) {
                        value = value.ToString().Trim().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    }
                parameters.Add(new NpgsqlParameter(String.Format(":{0}", column), value));
            }
            
            //3.执行SQL
            var connectString = System.Configuration.ConfigurationManager.ConnectionStrings["BeyonDBGuMu"];
            using (var conntion = new NpgsqlConnection(connectString.ToString()))
            {
                conntion.Open();
                using (var command = conntion.CreateCommand())
                {
                    command.CommandText = sql.ToString();
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                }
            }

        }
     }
}
