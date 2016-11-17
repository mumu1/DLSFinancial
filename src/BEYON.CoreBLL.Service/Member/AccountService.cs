/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2015/9/8 14:18:36  
*************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Data.Repositories.Member;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Member
{
    /// <summary>
    /// 账户业务类
    /// </summary>
    public class AccountService : CoreServiceBase, IAccountService
    {

        private readonly IUserRepository _UserRepository;
        private readonly IRoleService _RoleService;
        private readonly ISafeguardTimeService _SafeguardTimeService;

        public AccountService(IUserRepository userRepository, IRoleService roleService, ISafeguardTimeService safeguardTimeService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._UserRepository = userRepository;
            this._RoleService = roleService;
            this._SafeguardTimeService = safeguardTimeService;
        }
        public IQueryable<User> Users
        {
            get
            {
                return _UserRepository.Entities;
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginVM">登录模型信息</param>
        /// <returns>登录操作结果</returns>
        public OperationResult Login(LoginVM loginVM)
        {
            PublicHelper.CheckArgument(loginVM, "loginVM");
            User user = _UserRepository.GetEntitiesByEager(new List<string> { "Roles", "UserGroups" }).SingleOrDefault(m => m.UserName == loginVM.LoginName.Trim());
            OperationResult result;
            if (user == null)
            {
                result = new OperationResult(OperationResultType.QueryNull, "指定账号的用户不存在");
            }
            else if (user.Password != EncryptionHelper.GetMd5Hash(loginVM.Password.Trim()))
            {
                result = new OperationResult(OperationResultType.Warning, "登录密码不正确。");
            }
            else
            {
                var roleIdsByUser = user.Roles.Select(r => r.Id).ToList();

                //若没有特别设定指定一到五日系统维护，普通用户无法登陆
                var now = DateTime.Now;
                var startTime = new DateTime(now.Year, now.Month, 1);
                var endTime = new DateTime(now.Year, now.Month, 6);
                //从数据库表SafeguardTime获取用户保存的系统维护时间
                var saveStartTime = _SafeguardTimeService.SafeguardTimes.First().StartTime;
                var saveEndTime = _SafeguardTimeService.SafeguardTimes.First().EndTime;
                if(now >= saveStartTime && now < saveEndTime){
                    startTime = saveStartTime;
                    endTime = saveEndTime;
                }
                if(now >= startTime && now < endTime)
                {
                    foreach (var roleId in roleIdsByUser)
                    {
                        Role role = _RoleService.Roles.FirstOrDefault(r => r.Id == roleId);
                        if (role != null && role.RoleName == "普通用户")
                        {                            
                            return new OperationResult(OperationResultType.Warning, "系统维护状态中。。。请于"+endTime.ToShortDateString()+"日之后使用。");
                        }
                    }
                }
                
                result = new OperationResult(OperationResultType.Success, "登录成功。", user);
                #region 设置用户权限缓存
                
                var roleIdsByUserGroup = user.UserGroups.SelectMany(g => g.Roles).Select(r => r.Id).ToList();
                roleIdsByUser.AddRange(roleIdsByUserGroup);
                var roleIds = roleIdsByUser.Distinct().ToList();
                List<Permission> permissions = _RoleService.Roles.Where(t => roleIds.Contains(t.Id) && t.Enabled == true).SelectMany(c => c.Permissions).Distinct().ToList();
                var strKey = CacheKey.StrPermissionsByUid + "_" + user.Id;
                //设置Cache滑动过期时间为1天
                CacheHelper.SetCache(strKey, permissions, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0, 0));
                #endregion
            }
            if (result.ResultType != OperationResultType.Success) return result;
            User userTemp = (User)result.Data;
            DateTime expiration = loginVM.IsRememberLogin
                ? DateTime.Now.AddDays(14)
                : DateTime.Now.Add(FormsAuthentication.Timeout);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, //指定版本号：可随意指定
                userTemp.UserName,//登录用户名：对应 Web.config 中 <allow users="Admin" … /> 的 users 属性
                DateTime.Now,  //发布时间
                expiration, //失效时间
                true,//是否为持久 Cookie
                userTemp.Id.ToString(), //用户数据：可用 ((System.Web.Security.FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData 获取
                FormsAuthentication.FormsCookiePath); //指定 Cookie 为 Web.config 中 <forms path="/" … /> path 属性，不指定则默认为“/”
            string str = FormsAuthentication.Encrypt(ticket); //加密身份验票
            //声明一个 Cookie，名称为 Web.config 中 <forms name=".APSX" … /> 的 name 属性，对应的值为身份验票加密后的字串
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, str);
            if (loginVM.IsRememberLogin)
            {
                cookie.Expires = DateTime.Now.AddDays(14);//此句非常重要，少了的话，就算此 Cookie 在身份验票中指定为持久性 Cookie ，也只是即时型的 Cookie 关闭浏览器后就失效；
            }
            HttpContext.Current.Response.Cookies.Set(cookie); //或Response.Cookies.Add(ck);添加至客户端
            result.Data = null;
            return result;
        }

        /// <summary>
        ///  用户退出
        /// </summary>
        public void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null) return;
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

    }
}
