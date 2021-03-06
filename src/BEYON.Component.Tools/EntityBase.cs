﻿/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/5/15 14:50:15  
*************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.Component.Tools
{
    /// <summary>
    ///     可持久到数据库的领域模型的基类。
    /// </summary>
    [Serializable]
    public abstract class EntityBase<TKey>
    {
        protected EntityBase()
        {
            //IsDeleted = false;
            UpdateDate = DateTime.Now;
        }
        [Key]
        public TKey Id { get; set; }

        ///// <summary>
        /////  获取或设置是否禁用，逻辑上的删除，非物理删除
        ///// </summary>
        //public bool IsDeleted { get; set; }

        /// <summary>
        ///     获取或设置 添加时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime UpdateDate { get; set; }

        public String DT_RowId
        {
            get
            {
                return String.Format("row_{0}", Id);
            }
        }
    }
}
