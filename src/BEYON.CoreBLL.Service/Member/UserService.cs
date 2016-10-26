﻿/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/2/4 17:04:40  
*************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Caching;
using EntityFramework.Extensions;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.Domain.Data.Repositories.Member;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;
using BEYON.CoreBLL.Service.Excel;

namespace BEYON.CoreBLL.Service.Member
{
    public class UserService : CoreServiceBase, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;
        private readonly IUserGroupService _userGroupService;

        public UserService(IUserRepository userRepository, IRoleService roleService,IUserGroupService userGroupService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._userRepository = userRepository;
            this._roleService = roleService;
            this._userGroupService = userGroupService;
        }
        public IQueryable<User> Users
        {
            get { return _userRepository.Entities; }
        }

        public OperationResult Insert(UserVM model)
        {
            try
            {
                User oldUser = _userRepository.Entities.FirstOrDefault(c => c.UserName == model.UserName.Trim());
                if (oldUser != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的用户名称，请修改后重新提交！");
                }
                var entity = new User
                {
                    UserName = model.UserName.Trim(),
                    TrueName = model.TrueName.Trim(),
                    Password = model.Password,
                    //Phone = model.Phone,
                    //Email = model.Email,
                    //Address = model.Address,
                    CertificateID = model.CertificateID,
                    Department = model.Department,
                    Gender =model.Gender,
                    Title = model.Title,
                    Enabled = model.Enabled,
                    UpdateDate = DateTime.Now
                };
                _userRepository.Insert(entity);
                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }

        public OperationResult Update(UserVM model)
        {
            try
            {
                var user = Users.FirstOrDefault(c => c.Id == model.Id);
                if (user == null)
                {
                    throw new Exception();
                }
                var other = Users.FirstOrDefault(c => c.Id != model.Id && c.UserName == model.UserName.Trim());
                if (other != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的用户名称，请修改后重新提交！");
                }
                user.TrueName = model.TrueName.Trim();
                user.UserName = model.UserName.Trim();
                //user.Address = model.Address;
                user.CertificateID = model.CertificateID;
                user.Gender = model.Gender;
                user.Department = model.Department;
                user.Title = model.Title;
                //user.Phone = model.Phone;
                //user.Email = model.Email;
                user.UpdateDate = DateTime.Now;
                _userRepository.Update(user);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(IEnumerable<UserVM> list)
        {
            try
            {
                if (list != null)
                {
                    var userIds = list.Select(c => c.Id).ToList();
                    int count = _userRepository.Delete(_userRepository.Entities.Where(c => userIds.Contains(c.Id)));
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
            catch(Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "删除数据失败!");
            }
        }

        public OperationResult ResetPassword(IEnumerable<UserVM> list)
        {

            var listIds = list.Select(c => c.Id).ToList();
            try
            {
                string md5Pwd = EncryptionHelper.GetMd5Hash("123456");
                _userRepository.Entities.Where(u => listIds.Contains(u.Id))
                    .Update(u => new User() { Password = md5Pwd });
                UnitOfWork.Commit();
                return new OperationResult(OperationResultType.Success, "密码重置成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "密码重置失败!");
            }

        }


        public OperationResult UpdateUserRoles(int userId, string[] chkRoles)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var oldUser = Users.FirstOrDefault(c => c.Id == userId);
                    if (oldUser == null)
                    {
                        throw new Exception();
                    }
                    oldUser.Roles.Clear();
                    List<Role> newRoles = new List<Role>();
                    if (chkRoles != null && chkRoles.Length > 0)
                    {
                        int[] idInts = Array.ConvertAll<string, int>(chkRoles, Convert.ToInt32);
                        newRoles = _roleService.Roles.Where(c => idInts.Contains(c.Id)).ToList();
                        oldUser.Roles = newRoles;
                    }
                    UnitOfWork.Commit();
                    #region 重置权限缓存
                    var roleIdsByUser = newRoles.Select(r => r.Id).ToList();
                    var roleIdsByUserGroup = oldUser.UserGroups.SelectMany(g => g.Roles).Select(r => r.Id).ToList();
                    roleIdsByUser.AddRange(roleIdsByUserGroup);
                    var roleIds = roleIdsByUser.Distinct().ToList();
                    List<Permission> permissions = _roleService.Roles.Where(t => roleIds.Contains(t.Id) && t.Enabled == true).SelectMany(c => c.Permissions).Distinct().ToList();
                    var strKey = CacheKey.StrPermissionsByUid + "_" + oldUser.Id;
                    //设置Cache滑动过期时间为1天
                    CacheHelper.SetCache(strKey, permissions, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0, 0));
                    #endregion
                    scope.Complete();
                    return new OperationResult(OperationResultType.Success, "设置用户角色成功！");
                }
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "设置用户角色失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<User>(fileName, columns).ToList();
               // _userRepository.InsertOrUpdate(items);
                //逐一将导入的数据插入并将密码重置为123456，同时设定为普通用户权限
                UserVM userVm = null;
                for (int i = 0; i < items.Count; i++)
                { 
                    userVm = new UserVM();
                    userVm.UserName = items[i].UserName;
                    userVm.TrueName = items[i].TrueName;
                    //userVm.Email = items[i].Email;
                    userVm.CertificateID = items[i].CertificateID;
                    userVm.Department = items[i].Department;
                    userVm.Gender = items[i].Gender;
                    userVm.Title = items[i].Title;
                    userVm.Password = EncryptionHelper.GetMd5Hash("123456");
                    //向User表插入该数据
                    Insert(userVm);
                    //获取插入用户的userID
                    int userID = GetUserIDByUserName(userVm.UserName);
                    String[] roles = { "2" };    //导入的用户设为普通用户角色
                    //为用户设置角色
                    UpdateUserRoles(userID, roles);
                }
                    

                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }

        public int GetUserIDByUserName(String userName) {
            return _userRepository.GetUserIDByUserName(userName);
        }

        public User GetUserByUserName(String userName)
        {
            return _userRepository.GetUserByUserName(userName);
        }

        public OperationResult UpdateUserGroups(int userId, string[] chkUserGroups)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var oldUser = Users.FirstOrDefault(c => c.Id == userId);
                    if (oldUser == null)
                    {
                        throw new Exception();
                    }
                    oldUser.UserGroups.Clear();
                    List<UserGroup> newUserGroups = new List<UserGroup>();
                    if (chkUserGroups != null && chkUserGroups.Length > 0)
                    {
                        int[] idInts = Array.ConvertAll<string, int>(chkUserGroups, Convert.ToInt32);
                        newUserGroups = _userGroupService.UserGroups.Where(c => idInts.Contains(c.Id)).ToList();
                        oldUser.UserGroups = newUserGroups;
                    }
                    UnitOfWork.Commit();
                    #region 重置权限缓存
                    var roleIdsByUser = newUserGroups.Select(r => r.Id).ToList();
                    var roleIdsByUserGroup = oldUser.UserGroups.SelectMany(g => g.Roles).Select(r => r.Id).ToList();
                    roleIdsByUser.AddRange(roleIdsByUserGroup);
                    var roleIds = roleIdsByUser.Distinct().ToList();
                    List<Permission> permissions = _roleService.Roles.Where(t => roleIds.Contains(t.Id) && t.Enabled == true).SelectMany(c => c.Permissions).Distinct().ToList();
                    var strKey = CacheKey.StrPermissionsByUid + "_" + oldUser.Id;
                    //设置Cache滑动过期时间为1天
                    CacheHelper.SetCache(strKey, permissions, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0, 0));
                    #endregion
                    scope.Complete();
                    return new OperationResult(OperationResultType.Success, "设置用户组成功！");
                }
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "设置用户组失败!");
            }
        }
    }
}
