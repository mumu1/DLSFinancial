﻿/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/3/28 14:54:31  
*************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using EntityFramework.Extensions;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.Domain.Data.Repositories.Member;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;

namespace BEYON.CoreBLL.Service.Member
{
    public class UserGroupService : CoreServiceBase, IUserGroupService
    {

        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IRoleService _roleService;

        public UserGroupService(IUserGroupRepository userGroupRepository, IRoleService roleService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._userGroupRepository = userGroupRepository;
            this._roleService = roleService;
        }

        public IQueryable<UserGroup> UserGroups
        {
            get { return _userGroupRepository.Entities; }
        }

        public OperationResult Insert(UserGroupVM model)
        {
            try
            {
                UserGroup oldGroup = _userGroupRepository.Entities.FirstOrDefault(c => c.GroupName == model.GroupName.Trim());
                if (oldGroup != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同名称的用户组，请修改后重新提交！");
                }
                var entity = new UserGroup()
                {
                    GroupName = model.GroupName.Trim(),
                    Description = model.Description,
                    OrderSort = model.OrderSort,
                    Enabled = model.Enabled,
                    UpdateDate = DateTime.Now
                };
                _userGroupRepository.Insert(entity);
                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }

        public OperationResult Update(UserGroupVM model)
        {
            try
            {
                var oldRole = UserGroups.FirstOrDefault(c => c.Id == model.Id);
                if (oldRole == null)
                {
                    throw new Exception();
                }
                var other = UserGroups.FirstOrDefault(c => c.Id != model.Id && c.GroupName == model.GroupName.Trim());
                if (other != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同名称的用户组，请修改后重新提交！");
                }
                oldRole.GroupName = model.GroupName.Trim();
                oldRole.Description = model.Description;
                oldRole.OrderSort = model.OrderSort;
                oldRole.Enabled = model.Enabled;
                oldRole.UpdateDate = DateTime.Now;
                _userGroupRepository.Update(oldRole);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(IEnumerable<UserGroupVM> list)
        {
            try
            {
                if (list != null)
                {
                    var groupIds = list.Select(c => c.Id).ToList();
                    int count = _userGroupRepository.Delete(_userGroupRepository.Entities.Where(c => groupIds.Contains(c.Id)));
                    if (count > 0)
                    {
                        return new OperationResult(OperationResultType.Success, "删除数据成功！");
                    }
                    else
                    {
                        return new OperationResult(OperationResultType.Error, "删除数据失败!");
                    }
                }
                else
                {
                    return new OperationResult(OperationResultType.ParamError, "参数错误，请选择需要删除的数据!");
                }
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "删除数据失败!");
            }
        }

        public OperationResult UpdateUserGroupRoles(int userGroupId, string[] chkRoles)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var oldUserGroup = UserGroups.FirstOrDefault(c => c.Id == userGroupId);
                    if (oldUserGroup == null)
                    {
                        throw new Exception();
                    }
                    oldUserGroup.Roles.Clear();
                    List<Role> newRoles = new List<Role>();
                    if (chkRoles != null && chkRoles.Length > 0)
                    {
                        int[] idInts = Array.ConvertAll<string, int>(chkRoles, Convert.ToInt32);
                        newRoles = _roleService.Roles.Where(c => idInts.Contains(c.Id)).ToList();
                        oldUserGroup.Roles = newRoles;
                    }
                    UnitOfWork.Commit();
                    scope.Complete();
                    return new OperationResult(OperationResultType.Success, "设置用户组角色成功！");
                }
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "设置用户组角色失败!");
            }
        }
    }
}
