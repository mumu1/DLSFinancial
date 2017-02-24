using BEYON.Component.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BEYON.Component.Data.Enum;
using BEYON.Domain.Model.Member;
using BEYON.Domain.Model.App;

namespace BEYON.Component.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EFDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
#if DEBUG1
            #region 模块管理
           List<Module> modules = new List<Module>
            {
                new Module { Id = 1, ParentId = null, Name = "财务申请单", LinkUrl = "#",  Code = 100,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now, Icon = "fa-globe"},
                new Module { Id = 2, ParentId = 1, Name = "申请单管理", LinkUrl = "~/App/ApplyForm/Index",  Code = 101,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 39, ParentId = 1, Name = "常用领款人信息管理", LinkUrl = "~/App/TopContacts/Index",  Code = 102,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 3, ParentId = null, Name = "财务统计", LinkUrl = "#",  Code = 200,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now, Icon = "fa-thumb-tack"},
                new Module { Id = 4, ParentId = 3, Name = "人员劳务月统计报表", LinkUrl = "~/App/Statistics/Monthly",  Code = 201,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 5, ParentId = 3, Name = "人员工资月统计报表", LinkUrl = "~/App/Statistics/PerPersonDetail",  Code = 202,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 35, ParentId = 3, Name = "课题月统计报表", LinkUrl = "~/App/Statistics/TaskDetail",  Code = 203,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 38, ParentId = 3, Name = "按流水号月统计报表", LinkUrl = "~/App/Statistics/SerNumberStatistic",  Code = 204,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },             
                new Module { Id = 6, ParentId = null, Name = "系统管理", Code = 300,LinkUrl="#",  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now, Icon = "fa-cogs"},
                new Module { Id = 7, ParentId = 6, Name = "角色管理", LinkUrl = "~/Member/Role/Index",  Code = 301,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 8, ParentId = 6, Name = "用户管理", LinkUrl = "~/Member/User/Index", Code = 302, Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now },
                new Module { Id = 9, ParentId = 6, Name = "模块管理", LinkUrl = "~/Member/Module/Index",  Code = 303, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 10, ParentId = 6, Name = "权限管理", LinkUrl = "~/Member/Permission/Index",  Code = 304, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 11, ParentId = 6, Name = "用户组管理", LinkUrl = "~/Member/UserGroup/Index",  Code = 305, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 12, ParentId = null, Name = "基础数据管理", LinkUrl = "#",Code = 400,Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now, Icon = "fa-cog"},
                new Module { Id = 13, ParentId = 12, Name = "报销事由字典表", LinkUrl = "~/BasicDataManagement/Reimbursement/Index",  Code = 401, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 14, ParentId = 12, Name = "职称字典表", LinkUrl = "~/BasicDataManagement/Professional/Index",  Code = 402, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 15, ParentId = 12, Name = "开户银行字典表", LinkUrl = "~/BasicDataManagement/BankAccount/Index",  Code = 403, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 16, ParentId = 12, Name = "工资基础表初始化", LinkUrl = "~/BasicDataManagement/WageBaseTable/Index",  Code = 404,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 17, ParentId = 12, Name = "审核意见字典表", LinkUrl = "~/BasicDataManagement/AuditOption/Index",  Code = 405,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 18, ParentId = 12, Name = "课题字典表", LinkUrl = "~/BasicDataManagement/TaskManage/Index",  Code = 406,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 19, ParentId = 12, Name = "部门字典表", LinkUrl = "~/BasicDataManagement/Department/Index",  Code = 407,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 36, ParentId = 12, Name = "本月算税记录", LinkUrl = "~/BasicDataManagement/TaxPerOrder/Index",  Code = 408,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 37, ParentId = 12, Name = "历史算税记录", LinkUrl = "~/BasicDataManagement/TaxPerOrderHistory/Index",  Code = 409,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now}
        
            };
            DbSet<Module> moduleSet = context.Set<Module>();
            moduleSet.AddOrUpdate(t => new { t.Code }, modules.ToArray());
            context.SaveChanges();
            #endregion

            #region 权限管理
            List<Permission> permissions = new List<Permission>
            {
            #region 财务申请单
                new Permission{Id=1, Name="管理",Code=EnumPermissionCode.QuerySystemLog.ToString(), Description="申请单管理" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[1]},
                new Permission{Id=39, Name="管理",Code=EnumPermissionCode.QuerySystemLog.ToString(), Description="常用领款人信息管理" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[1]},
            #endregion

            #region 财务统计
             new Permission{Id=2, Name="劳务统计",Code=EnumPermissionCode.Input.ToString(), Description="人员劳务月统计报表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[3]},
             new Permission{Id=3, Name="工资统计",Code=EnumPermissionCode.Input.ToString(), Description="人员工资月统计报表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[4]},
             new Permission{Id=35, Name="课题统计",Code=EnumPermissionCode.Input.ToString(), Description="课题月统计报表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[5]},
             new Permission{Id=38, Name="流水号统计",Code=EnumPermissionCode.Input.ToString(), Description="按流水号月统计报表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
       
            #endregion

            #region 角色
		       new Permission{Id=4, Name="查询",Code=EnumPermissionCode.QueryRole.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
               new Permission{Id=5, Name="新增",Code=EnumPermissionCode.AddRole.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
               new Permission{Id=6, Name="修改",Code=EnumPermissionCode.UpdateRole.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
               new Permission{Id=7, Name="删除",Code=EnumPermissionCode.DeleteRole.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
               new Permission{Id=8, Name="授权",Code=EnumPermissionCode.AuthorizeRole.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]}, 
            #endregion

            #region 用户
		       new Permission{Id=9, Name="查询",Code=EnumPermissionCode.QueryUser.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=10, Name="新增",Code=EnumPermissionCode.AddUser.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=11, Name="修改",Code=EnumPermissionCode.UpdateUser.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=12, Name="删除",Code=EnumPermissionCode.DeleteUser.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=13, Name="重置密码",Code=EnumPermissionCode.ResetPwdUser.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=34, Name="导入用户",Code=EnumPermissionCode.ImportUser.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},

               new Permission{Id=14, Name="设置用户组",Code=EnumPermissionCode.SetGroupUser.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=15, Name="设置角色",Code=EnumPermissionCode.SetRolesUser.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]}, 
            #endregion
             
            #region 模块
		     new Permission{Id=16, Name="查询",Code=EnumPermissionCode.QueryModule.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[8]},
             new Permission{Id=17, Name="新增",Code=EnumPermissionCode.AddModule.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[8]},
             new Permission{Id=18, Name="修改",Code=EnumPermissionCode.UpdateModule.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[8]},
            #endregion

            #region 权限
		     new Permission{Id=19, Name="查询",Code=EnumPermissionCode.QueryPermission.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[9]},
             new Permission{Id=20, Name="新增",Code=EnumPermissionCode.AddPermission.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[9]},
             new Permission{Id=21, Name="修改",Code=EnumPermissionCode.UpdatePermission.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[9]},
            #endregion

            #region 用户组
		     new Permission{Id=22, Name="查询",Code=EnumPermissionCode.QueryUserGroup.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
             new Permission{Id=23, Name="新增",Code=EnumPermissionCode.AddUserGroup.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
             new Permission{Id=24, Name="修改",Code=EnumPermissionCode.UpdateUserGroup.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
             new Permission{Id=25, Name="删除",Code=EnumPermissionCode.DeleteUserGroup.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
             new Permission{Id=26, Name="设置角色",Code=EnumPermissionCode.SetRolesUserGroup.ToString(), Description="描述" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
            #endregion

            #region 基础数据管理
             new Permission{Id=27, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="报销事由字典表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[12]},
             new Permission{Id=28, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="职称字典表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[13]},
             new Permission{Id=29, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="开户银行字典表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[14]},
             new Permission{Id=30, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="工资基础表初始化" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[15]},
             new Permission{Id=31, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="审核意见字典表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[16]},
             new Permission{Id=32, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="课题字典表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[17]},           
             new Permission{Id=33, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="部门字典表" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[18]},                     
             new Permission{Id=36, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="本月算税记录" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[19]},     
             new Permission{Id=37, Name="操作",Code=EnumPermissionCode.Audit.ToString(), Description="历史算税记录" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[20]},  
            #endregion
            };
            DbSet<Permission> permissionSet = context.Set<Permission>();
            permissionSet.AddOrUpdate(m => new { m.Id }, permissionSet.ToArray());
            context.SaveChanges();
            #endregion

            #region 角色管理
            List<Role> roles = new List<Role>
            {
                new Role { Id=1,  RoleName = "系统管理员", Description="系统管理员",Enabled=true,OrderSort=1,UpdateDate=DateTime.Now,Permissions=permissions},
                new Role { Id=2,  RoleName = "普通用户", Description="普通用户",Enabled=true,OrderSort=1,UpdateDate=DateTime.Now ,Permissions=permissions}
            };
            DbSet<Role> roleSet = context.Set<Role>();
            roleSet.AddOrUpdate(m => new { m.RoleName }, roles.ToArray());
            context.SaveChanges();
            #endregion

            #region 用户管理
            List<User> members = new List<User>
            {
                new User { Id=1, UserName = "admin", Password = "e10adc3949ba59abbe56e057f20f883e", TrueName = "管理员", Enabled=true,Roles=new List<Role>{roles[0]} },
                new User { Id=2, UserName = "zhangs", Password = "e10adc3949ba59abbe56e057f20f883e",TrueName = "张硕", Enabled=true,Roles=new List<Role>{roles[1]} }
            };
            DbSet<User> memberSet = context.Set<User>();
            memberSet.AddOrUpdate(m => new { m.UserName }, members.ToArray());
            context.SaveChanges();
            #endregion

            #region 用户组管理
            List<UserGroup> userGroups = new List<UserGroup>
            {
                new UserGroup { Id=1, GroupName = "普通用户组",Description = "普通用户组",Enabled=true,Roles=new List<Role>{roles[1]},OrderSort = 1,Users = new List<User>(){members[1]}},
                new UserGroup { Id=2, GroupName = "系统管理组", Description = "系统管理组",Enabled=true,Roles=new List<Role>{roles[0]},OrderSort = 2,Users = new List<User>(){members[0]}}
            };
            DbSet<UserGroup> userGroupsSet = context.Set<UserGroup>();
            userGroupsSet.AddOrUpdate(m => new { m.GroupName }, userGroups.ToArray());
            context.SaveChanges();
            #endregion

            #region 职称--title
            //List<Title> titles = new List<Title>
            //{
            //    new Title { TitleCode="1000", TitleName = "研究员" },
            //    new Title { TitleCode="1001", TitleName = "副研究员" },
            //    new Title { TitleCode="1002", TitleName = "高工" },
            //    new Title { TitleCode="1003", TitleName = "工程师" },
            //    new Title { TitleCode="1004", TitleName = "助理工程师" },
            //    new Title { TitleCode="1005", TitleName = "职员" },
            //    new Title { TitleCode="1006", TitleName = "教授" },
            //    new Title { TitleCode="1007", TitleName = "副教授" },
            //    new Title { TitleCode="1008", TitleName = "讲师" },
            //    new Title { TitleCode="1009", TitleName = "博士后" },
            //    new Title { TitleCode="1010", TitleName = "博士" },
            //    new Title { TitleCode="1011", TitleName = "研究生" },
            //    new Title { TitleCode="1012", TitleName = "其他" }
            //};
            //DbSet<Title> titlesSet = context.Set<Title>();
            //titlesSet.AddOrUpdate(m => new { m.TitleCode }, titles.ToArray());
            //context.SaveChanges();
            #endregion

            #region 维护时间--SafeguardTime
            //List<SafeguardTime> safeguardTime = new List<SafeguardTime>
            //{
            //    new SafeguardTime { StartTime=new DateTime(), EndTime= new DateTime() }
            //};
            //DbSet<SafeguardTime> safeguardTimesSet = context.Set<SafeguardTime>();
            //safeguardTimesSet.AddOrUpdate(m => new { m.StartTime }, safeguardTime.ToArray());
            //context.SaveChanges();
            #endregion

            //#region 标绘--umrcover
            //List<Umrcover> umrcovers = new List<Umrcover>
            //{
            //    new Umrcover { UmrID="1000", Name = "山体遗址", UserID = "00001" }
            //};
            //DbSet<Umrcover> umrcoverSet = context.Set<Umrcover>();
            //umrcoverSet.AddOrUpdate(m => new { m.Name }, umrcovers.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--basicproperty
            //List<BasicProperty> basicPropertys = new List<BasicProperty>
            //{
            //    new BasicProperty { UmrID="1000", Address = "吉林", Longitude = "107.21", Latitude="35.2" }
            //};
            //DbSet<BasicProperty> basicPropertySet = context.Set<BasicProperty>();
            //basicPropertySet.AddOrUpdate(m => new { m.UmrID }, basicPropertys.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--points
            //List<Points> points = new List<Points>
            //{
            //    new Points { UmrID="1000", Altitude = 1000.3, Longitude = "107.21", Latitude="35.2" }
            //};
            //DbSet<Points> pointsSet = context.Set<Points>();
            //pointsSet.AddOrUpdate(m => new { m.UmrID }, points.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--sammples
            //List<Samples> samples = new List<Samples>
            //{
            //    new Samples { UmrID="1000", Name = "山体样本", UserID = "00001" }
            //};
            //DbSet<Samples> samplesSet = context.Set<Samples>();
            //samplesSet.AddOrUpdate(m => new { m.UmrID }, samples.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--others
            //List<Others> others = new List<Others>
            //{
            //    new Others { UmrID="1001", Name = "山体资料", UserID = "00001" }
            //};
            //DbSet<Others> othersSet = context.Set<Others>();
            //othersSet.AddOrUpdate(m => new { m.UmrID }, others.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--photos
            //List<Photos> photos = new List<Photos>
            //{
            //    new Photos { UmrID="1000", Name = "山体资料", UserID = "00001" }
            //};
            //DbSet<Photos> photosSet = context.Set<Photos>();
            //photosSet.AddOrUpdate(m => new { m.PhotoID }, photos.ToArray());
            //context.SaveChanges();
            //#endregion

         

            //#region 标绘--audit
            //List<Audit> audits = new List<Audit>
            //{
            //    new Audit { UmrID="1000", Operator = "Lucy", OperatorID = "00001" }
            //};
            //DbSet<Audit> auditSet = context.Set<Audit>();
            //auditSet.AddOrUpdate(m => new { m.UmrID }, audits.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--DigsituationBefore
            //List<DigsituationBefore> digsituationBefores = new List<DigsituationBefore>
            //{
            //    new DigsituationBefore { UmrID="1000", Name = "山体图纸" ,DigareaID="100"}
            //};
            //DbSet<DigsituationBefore> digsituationBeforesSet = context.Set<DigsituationBefore>();
            //digsituationBeforesSet.AddOrUpdate(m => new { m.DigareaID }, digsituationBefores.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--LayerDeposit
            //List<LayerDeposit> layerDeposits = new List<LayerDeposit>
            //{
            //    new LayerDeposit { UmrID="1000", Shape = "椭圆"}
            //};
            //DbSet<LayerDeposit> layerDepositsSet = context.Set<LayerDeposit>();
            //layerDepositsSet.AddOrUpdate(m => new { m.UmrID }, layerDeposits.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--ImportAntsites
            //List<ImportAntsites> importAntsitess = new List<ImportAntsites>
            //{
            //    new ImportAntsites { UmrID="1000", SiteID = "1001"}
            //};
            //DbSet<ImportAntsites> importAntsitessSet = context.Set<ImportAntsites>();
            //importAntsitessSet.AddOrUpdate(m => new { m.SiteID }, importAntsitess.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--Literature
            //List<Literature> literatures = new List<Literature>
            //{
            //    new Literature { UmrID="1000", LiteratureID = "1001"}
            //};
            //DbSet<Literature> literaturesSet = context.Set<Literature>();
            //literaturesSet.AddOrUpdate(m => new { m.LiteratureID }, literatures.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region 标绘--ProtectUnits
            //List<ProtectUnits> protectUnitss = new List<ProtectUnits>
            //{
            //    new ProtectUnits { ProtectunitID="1000", Name = "test"}
            //};
            //DbSet<ProtectUnits> protectUnitssSet = context.Set<ProtectUnits>();
            //protectUnitssSet.AddOrUpdate(m => new { m.ProtectunitID }, protectUnitss.ToArray());
            //context.SaveChanges();
            //#endregion
#endif
        }
    }
}
