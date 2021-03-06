﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;


namespace BEYON.CoreBLL.Service.Member.Interface
{
    public interface IUserService
    {
        IQueryable<User> Users { get; }
        OperationResult Insert(UserVM model);
        OperationResult Update(UserVM model);
        OperationResult Delete(IEnumerable<UserVM> list);
        OperationResult ResetPassword(IEnumerable<UserVM> list);
        OperationResult UpdateUserRoles(int roleId, string[] chkRoles);
        OperationResult UpdateUserGroups(int userId, string[] chkUserGroups);

        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);

        int GetUserIDByUserName(String userName);

        User GetUserByUserName(String userName);

        OperationResult ModifyPassword(String idAndPwd);
    }
}
