/************************************
 * 描述：调前标绘
 * 作者：林永恒
 * 日期：2016/05/16 14:02:29  
*************************************/

using System.ComponentModel.DataAnnotations.Schema;

namespace BEYON.Component.Data.Configurations.Plot
{
    internal partial class PlotBeforeConfiguration
    {
        partial void PlotBeforeConfigurationAppend()
        {
            this.Property(t => t.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
