﻿/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/5/15 15:55:34  
*************************************/

using Autofac;
using Autofac.Integration.Mvc;
using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service.Member;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.Domain.Data.Repositories.Member;
using BEYON.Domain.Data.Repositories.Member.Impl;
using BEYON.CoreBLL.Service.App;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.CoreBLL.Service.Excel;
using BEYON.CoreBLL.Service.Excel.Interface;
using BEYON.Domain.Data.Repositories.App;
using BEYON.Domain.Data.Repositories.App.Impl;


namespace BEYON.Web
{
    public static class RegisterAutofacForSingle
    {
        public static void RegisterAutofac()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()); //开启了Controller的依赖注入功能,这里使用Autofac提供的RegisterControllers扩展方法来对程序集中所有的Controller一次性的完成注册
            builder.RegisterFilterProvider(); //开启了Filter的依赖注入功能，为过滤器使用属性注入必须在容器创建之前调用RegisterFilterProvider方法，并将其传到AutofacDependencyResolver

            #region IOC注册区域
            //倘若需要默认注册所有的，请这样写（主要参数需要修改）
            //var baseType = typeof(IDependency);
            //var asssemblys = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //builder.RegisterControllers(asssemblys.ToArray());
            //builder.RegisterAssemblyTypes(asssemblys.ToArray())
            //    .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
            //    .AsImplementedInterfaces().InstancePerMatchingLifetimeScope();

            //注册一个接口多个实现并定义多个Name的情况需要使用的Helper类
            builder.RegisterType<AutofacHelper>().As<IAutofacHelper>().InstancePerHttpRequest();

            //Member
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerHttpRequest();
            builder.RegisterType<ModuleService>().As<IModuleService>().InstancePerHttpRequest();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerHttpRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerHttpRequest();
            builder.RegisterType<UserGroupService>().As<IUserGroupService>().InstancePerHttpRequest();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerHttpRequest();
            builder.RegisterType<EFDbContext>().As<DbContext>().InstancePerHttpRequest();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerHttpRequest();
            builder.RegisterType<UserGroupRepository>().As<IUserGroupRepository>().InstancePerHttpRequest();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerHttpRequest();
            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>().InstancePerHttpRequest();
            builder.RegisterType<ModuleRepository>().As<IModuleRepository>().InstancePerHttpRequest();

            //App services
            builder.RegisterType<RefundTypeService>().As<IRefundTypeService>().InstancePerHttpRequest();
            builder.RegisterType<RefundTypeRepository>().As<IRefundTypeRepository>().InstancePerHttpRequest();
            builder.RegisterType<BankService>().As<IBankService>().InstancePerHttpRequest();
            builder.RegisterType<BankRepository>().As<IBankRepository>().InstancePerHttpRequest();
            builder.RegisterType<SafeguardTimeService>().As<ISafeguardTimeService>().InstancePerHttpRequest();
            builder.RegisterType<SafeguardTimeRepository>().As<ISafeguardTimeRepository>().InstancePerHttpRequest();
            builder.RegisterType<DepartmentService>().As<IDepartmentService>().InstancePerHttpRequest();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerHttpRequest();
            builder.RegisterType<TaskManageService>().As<ITaskManageService>().InstancePerHttpRequest();
            builder.RegisterType<TaskManageRepository>().As<ITaskManageRepository>().InstancePerHttpRequest();
            builder.RegisterType<TitleService>().As<ITitleService>().InstancePerHttpRequest();
            builder.RegisterType<TitleRepository>().As<ITitleRepository>().InstancePerHttpRequest();
            builder.RegisterType<AuditOpinionService>().As<IAuditOpinionService>().InstancePerHttpRequest();
            builder.RegisterType<AuditOpinionRepository>().As<IAuditOpinionRepository>().InstancePerHttpRequest();
            builder.RegisterType<ApplicationFormService>().As<IApplicationFormService>().InstancePerHttpRequest();
            builder.RegisterType<ApplicationFormRepository>().As<IApplicationFormRepository>().InstancePerHttpRequest();
            builder.RegisterType<TaxBaseByMonthService>().As<ITaxBaseByMonthService>().InstancePerHttpRequest();
            builder.RegisterType<TaxBaseByMonthRepository>().As<ITaxBaseByMonthRepository>().InstancePerHttpRequest();
            builder.RegisterType<PersonalRecordService>().As<IPersonalRecordService>().InstancePerHttpRequest();
            builder.RegisterType<PersonalRecordRepository>().As<IPersonalRecordRepository>().InstancePerHttpRequest();
            builder.RegisterType<TopContactsService>().As<ITopContactsService>().InstancePerHttpRequest();
            builder.RegisterType<TopContactsRepository>().As<ITopContactsRepository>().InstancePerHttpRequest();
            builder.RegisterType<TaxPerOrderService>().As<ITaxPerOrderService>().InstancePerHttpRequest();
            builder.RegisterType<TaxPerOrderRepository>().As<ITaxPerOrderRepository>().InstancePerHttpRequest();
            builder.RegisterType<TaxPerOrderHistoryService>().As<ITaxPerOrderHistoryService>().InstancePerHttpRequest();
            builder.RegisterType<TaxPerOrderHistoryRepository>().As<ITaxPerOrderHistoryRepository>().InstancePerHttpRequest();
            builder.RegisterType<StatisticsRepository>().As<IStatisticsRepository>().InstancePerHttpRequest();
            builder.RegisterType<StatisticsService>().As<IStatisticsService>().InstancePerHttpRequest();
            builder.RegisterType<TaxBaseEveryMonthService>().As<ITaxBaseEveryMonthService>().InstancePerHttpRequest();
            builder.RegisterType<TaxBaseEveryMonthRepository>().As<ITaxBaseEveryMonthRepository>().InstancePerHttpRequest();

            //Excel services
            builder.RegisterType<ApplyPrintService>().As<IApplyPrintService>().InstancePerHttpRequest();
            #endregion
            // then
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }


    }
}

#region 特殊例子配置：一个接口多个实现的情况（builder.Register(c => new ProductController(c.ResolveNamed<Interface>("给具体类型配置不同的名称")));）
//public interface ISinger { }

//public class MizukiNana : ISinger { }

//public class ShimotsukiHaruka : ISinger { }

//public class Stage
//{
//    public ISinger Singer { get; set; }

//    public Stage(ISinger singer)
//    {
//        this.Singer = singer;
//    }
//}

//class Program
//{
//    static void Main(string[] args)
//    {
//        var builder = new ContainerBuilder();
//        builder.RegisterType<MizukiNana>().Named<ISinger>("nana");
//        builder.RegisterType<ShimotsukiHaruka>().Named<ISinger>("haruka");
//        //建议在Stage中使用AutofacHelper类
//        builder.Register(c => new Stage(c.ResolveNamed<ISinger>("nana")));
//        var container = builder.Build();

//        var stage = container.Resolve<Stage>();
//        Console.WriteLine(stage.Singer.ToString());
//    }
//}
#endregion