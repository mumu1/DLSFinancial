﻿/************************************
 * 描述：调后标绘数据表--points配置
 * 作者：张硕
 * 日期：2016/06/30 
*************************************/

using System.ComponentModel.DataAnnotations.Schema;

namespace BEYON.Component.Data.Configurations.Plot
{
    internal partial class PointsConfiguration
    {
        partial void PointsConfigurationAppend()
        {
            this.Property(t => t.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}