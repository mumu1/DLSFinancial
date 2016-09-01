﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;

namespace BEYON.CoreBLL.Service.Member.Interface
{
    public interface IPermissionService
    {
        #region 属性
        IQueryable<Permission> Permissions { get; }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取模块分页列表
        /// </summary>
        /// <param name="wh"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IList<PermissionVM> GetListPermissionVM(Expression<Func<Permission, bool>> wh, int limit, int offset, out int total);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        OperationResult Insert(PermissionVM model);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        OperationResult Update(PermissionVM model);
        #endregion
    }
}
