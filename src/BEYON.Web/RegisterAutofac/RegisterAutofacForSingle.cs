/************************************
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
using BEYON.CoreBLL.Service.Plot;
using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Data.Repositories.Plot;
using BEYON.Domain.Data.Repositories.Plot.Impl;

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

            //Plot
            builder.RegisterType<PlotBeforeService>().As<IPlotBeforeService>().InstancePerHttpRequest();

            builder.RegisterType<PlotBeforeRepository>().As<IPlotBeforeRepository>().InstancePerHttpRequest();
            builder.RegisterType<PlotAfterRepository>().As<IPlotAfterRepository>().InstancePerHttpRequest();

            builder.RegisterType<SamplesService>().As<ISamplesService>().InstancePerHttpRequest();
            builder.RegisterType<UmrcoverService>().As<IUmrcoverService>().InstancePerHttpRequest();

            builder.RegisterType<SamplesRepository>().As<ISamplesRepository>().InstancePerHttpRequest();
            builder.RegisterType<UmrcoverRepository>().As<IUmrcoverRepository>().InstancePerHttpRequest();

            builder.RegisterType<PhotosService>().As<IPhotosService>().InstancePerHttpRequest();
            builder.RegisterType<PhotosRepository>().As<IPhotosRepository>().InstancePerHttpRequest();

            builder.RegisterType<AuditService>().As<IAuditService>().InstancePerHttpRequest();
            builder.RegisterType<AuditRepository>().As<IAuditRepository>().InstancePerHttpRequest();

            builder.RegisterType<ImportAntsitesService>().As<IImportAntsitesService>().InstancePerHttpRequest();
            builder.RegisterType<ImportAntsitesRepository>().As<IImportAntsitesRepository>().InstancePerHttpRequest();

            builder.RegisterType<DigsituationBeforeRepository>().As<IDigsituationBeforeRepository>().InstancePerHttpRequest();
            builder.RegisterType<DigsituationBeforeService>().As<IDigsituationBeforeService>().InstancePerHttpRequest();

            builder.RegisterType<LiteratureRepository>().As<ILiteratureRepository>().InstancePerHttpRequest();
            builder.RegisterType<LiteratureService>().As<ILiteratureService>().InstancePerHttpRequest();

            builder.RegisterType<LayerDepositRepository>().As<ILayerDepositRepository>().InstancePerHttpRequest();
            builder.RegisterType<LayerDepositService>().As<ILayerDepositService>().InstancePerHttpRequest();

            builder.RegisterType<BasicPropertyService>().As<IBasicPropertyService>().InstancePerHttpRequest();
            builder.RegisterType<BasicPropertyRepository>().As<IBasicPropertyRepository>().InstancePerHttpRequest();

            builder.RegisterType<DraftsService>().As<IDraftsService>().InstancePerHttpRequest();
            builder.RegisterType<DraftsRepository>().As<IDraftsRepository>().InstancePerHttpRequest();

            builder.RegisterType<OthersService>().As<IOthersService>().InstancePerHttpRequest();
            builder.RegisterType<OthersRepository>().As<IOthersRepository>().InstancePerHttpRequest();

            builder.RegisterType<PointsService>().As<IPointsService>().InstancePerHttpRequest();
            builder.RegisterType<PointsRepository>().As<IPointsRepository>().InstancePerHttpRequest();

            builder.RegisterType<DigsituationBeforeService>().As<IDigsituationBeforeService>().InstancePerHttpRequest();
            builder.RegisterType<DigsituationBeforeRepository>().As<IDigsituationBeforeRepository>().InstancePerHttpRequest();

            builder.RegisterType<ProtectUnitsService>().As<IProtectUnitsService>().InstancePerHttpRequest();
            builder.RegisterType<ProtectUnitsRepository>().As<IProtectUnitsRepository>().InstancePerHttpRequest();

            builder.RegisterType<ImportAntsitesService>().As<IImportAntsitesService>().InstancePerHttpRequest();
            builder.RegisterType<ImportAntsitesRepository>().As<IImportAntsitesRepository>().InstancePerHttpRequest();

            builder.RegisterType<LayerDepositService>().As<ILayerDepositService>().InstancePerHttpRequest();
            builder.RegisterType<LayerDepositRepository>().As<ILayerDepositRepository>().InstancePerHttpRequest();

            builder.RegisterType<LiteratureService>().As<ILiteratureService>().InstancePerHttpRequest();
            builder.RegisterType<LiteratureRepository>().As<ILiteratureRepository>().InstancePerHttpRequest();

            builder.RegisterType<ProtectUnitAuditRepository>().As<IProtectUnitAuditRepository>().InstancePerHttpRequest();
            builder.RegisterType<ProtectUnitAuditService>().As<IProtectUnitAuditService>().InstancePerHttpRequest();

            builder.RegisterType<StatisticsService>().As<IStatisticsService>().InstancePerHttpRequest();

            builder.RegisterType<NotifyService>().As<INotifyService>().InstancePerHttpRequest();

            builder.RegisterType<ExportService>().As<IExportService>().InstancePerHttpRequest();
            builder.RegisterType<ImportService>().As<IImportService>().InstancePerHttpRequest();
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