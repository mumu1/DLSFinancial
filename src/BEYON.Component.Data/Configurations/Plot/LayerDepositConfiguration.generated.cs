﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//	   如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类进行实现。
// </auto-generated>
//
// <copyright file="RoleConfiguration.generated.cs">
//		Copyright(c)2015 Beyon.All rights reserved.
//		CLR版本：4.5.1
//		开发组织：北京博阳世通信息技术有限公司
//		公司网站：http://www.beyondb.com.cn
//		所属工程：BEYON.Component.Data
//		生成时间：2016-02-22 14:41
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Domain.Model.Plot;


namespace BEYON.Component.Data.Configurations.Plot
{
	/// <summary>
    ///   实体类数据表关系配置——角色信息
    /// </summary>
    internal partial class LayerDepositConfiguration : EntityTypeConfiguration<LayerDeposit>
    {
        public LayerDepositConfiguration()
        {
            LayerDepositConfigurationAppend();
        }
         /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void LayerDepositConfigurationAppend();
    }
}
