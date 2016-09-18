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
using BEYON.Component.Data;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.App;


namespace BEYON.Domain.Data.Repositories.App.Impl
{
	/// <summary>
    ///   仓储操作层实现——用户信息
    /// </summary>
    public partial class ApplicationFormRepository : EFRepositoryBase<ApplicationForm, Int32>, IApplicationFormRepository
    {
        public ApplicationFormRepository(IUnitOfWork unitOfWork)
            : base()
        { 
            
        }

        public IList<ApplicationForm> GetApplicationFromByUser(String email)
        {
            var q = from p in Context.ApplicationForms.Where(w => w.UserEmail == email).OrderByDescending(t => t.UpdateDate)
                        select p;
            return q.ToList();
        }

        public IList<ApplicationForm> GetApplicationFromByAdmin()
        {
            var q = from p in Context.ApplicationForms.Where(w => w.AuditStatus == "待审核" || w.AuditStatus == "审核通过").OrderByDescending(t => t.UpdateDate)
                    select p;
            return q.ToList();
        }
     }
}
