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
            #region ģ�����
           List<Module> modules = new List<Module>
            {
                new Module { Id = 1, ParentId = null, Name = "�������뵥", LinkUrl = "#",  Code = 100,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now, Icon = "fa-globe"},
                new Module { Id = 2, ParentId = 1, Name = "���뵥����", LinkUrl = "~/App/ApplyForm/Index",  Code = 101,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 39, ParentId = 1, Name = "�����������Ϣ����", LinkUrl = "~/App/TopContacts/Index",  Code = 102,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 3, ParentId = null, Name = "����ͳ��", LinkUrl = "#",  Code = 200,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now, Icon = "fa-thumb-tack"},
                new Module { Id = 4, ParentId = 3, Name = "��Ա������ͳ�Ʊ���", LinkUrl = "~/App/Statistics/Monthly",  Code = 201,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 5, ParentId = 3, Name = "��Ա������ͳ�Ʊ���", LinkUrl = "~/App/Statistics/PerPersonDetail",  Code = 202,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 35, ParentId = 3, Name = "������ͳ�Ʊ���", LinkUrl = "~/App/Statistics/TaskDetail",  Code = 203,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 38, ParentId = 3, Name = "����ˮ����ͳ�Ʊ���", LinkUrl = "~/App/Statistics/SerNumberStatistic",  Code = 204,  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },             
                new Module { Id = 6, ParentId = null, Name = "ϵͳ����", Code = 300,LinkUrl="#",  Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now, Icon = "fa-cogs"},
                new Module { Id = 7, ParentId = 6, Name = "��ɫ����", LinkUrl = "~/Member/Role/Index",  Code = 301,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 8, ParentId = 6, Name = "�û�����", LinkUrl = "~/Member/User/Index", Code = 302, Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now },
                new Module { Id = 9, ParentId = 6, Name = "ģ�����", LinkUrl = "~/Member/Module/Index",  Code = 303, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 10, ParentId = 6, Name = "Ȩ�޹���", LinkUrl = "~/Member/Permission/Index",  Code = 304, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 11, ParentId = 6, Name = "�û������", LinkUrl = "~/Member/UserGroup/Index",  Code = 305, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now },
                new Module { Id = 12, ParentId = null, Name = "�������ݹ���", LinkUrl = "#",Code = 400,Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now, Icon = "fa-cog"},
                new Module { Id = 13, ParentId = 12, Name = "���������ֵ��", LinkUrl = "~/BasicDataManagement/Reimbursement/Index",  Code = 401, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 14, ParentId = 12, Name = "ְ���ֵ��", LinkUrl = "~/BasicDataManagement/Professional/Index",  Code = 402, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 15, ParentId = 12, Name = "���������ֵ��", LinkUrl = "~/BasicDataManagement/BankAccount/Index",  Code = 403, Description = null, IsMenu = true, Enabled = true,  UpdateDate = DateTime.Now},
                new Module { Id = 16, ParentId = 12, Name = "���ʻ������ʼ��", LinkUrl = "~/BasicDataManagement/WageBaseTable/Index",  Code = 404,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 17, ParentId = 12, Name = "�������ֵ��", LinkUrl = "~/BasicDataManagement/AuditOption/Index",  Code = 405,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 18, ParentId = 12, Name = "�����ֵ��", LinkUrl = "~/BasicDataManagement/TaskManage/Index",  Code = 406,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 19, ParentId = 12, Name = "�����ֵ��", LinkUrl = "~/BasicDataManagement/Department/Index",  Code = 407,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 36, ParentId = 12, Name = "������˰��¼", LinkUrl = "~/BasicDataManagement/TaxPerOrder/Index",  Code = 408,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now},
                new Module { Id = 37, ParentId = 12, Name = "��ʷ��˰��¼", LinkUrl = "~/BasicDataManagement/TaxPerOrderHistory/Index",  Code = 409,Description = null, IsMenu = true, Enabled = true, UpdateDate = DateTime.Now}
        
            };
            DbSet<Module> moduleSet = context.Set<Module>();
            moduleSet.AddOrUpdate(t => new { t.Code }, modules.ToArray());
            context.SaveChanges();
            #endregion

            #region Ȩ�޹���
            List<Permission> permissions = new List<Permission>
            {
            #region �������뵥
                new Permission{Id=1, Name="����",Code=EnumPermissionCode.QuerySystemLog.ToString(), Description="���뵥����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[1]},
                new Permission{Id=39, Name="����",Code=EnumPermissionCode.QuerySystemLog.ToString(), Description="�����������Ϣ����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[1]},
            #endregion

            #region ����ͳ��
             new Permission{Id=2, Name="����ͳ��",Code=EnumPermissionCode.Input.ToString(), Description="��Ա������ͳ�Ʊ���" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[3]},
             new Permission{Id=3, Name="����ͳ��",Code=EnumPermissionCode.Input.ToString(), Description="��Ա������ͳ�Ʊ���" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[4]},
             new Permission{Id=35, Name="����ͳ��",Code=EnumPermissionCode.Input.ToString(), Description="������ͳ�Ʊ���" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[5]},
             new Permission{Id=38, Name="��ˮ��ͳ��",Code=EnumPermissionCode.Input.ToString(), Description="����ˮ����ͳ�Ʊ���" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
       
            #endregion

            #region ��ɫ
		       new Permission{Id=4, Name="��ѯ",Code=EnumPermissionCode.QueryRole.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
               new Permission{Id=5, Name="����",Code=EnumPermissionCode.AddRole.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
               new Permission{Id=6, Name="�޸�",Code=EnumPermissionCode.UpdateRole.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
               new Permission{Id=7, Name="ɾ��",Code=EnumPermissionCode.DeleteRole.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]},
               new Permission{Id=8, Name="��Ȩ",Code=EnumPermissionCode.AuthorizeRole.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[6]}, 
            #endregion

            #region �û�
		       new Permission{Id=9, Name="��ѯ",Code=EnumPermissionCode.QueryUser.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=10, Name="����",Code=EnumPermissionCode.AddUser.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=11, Name="�޸�",Code=EnumPermissionCode.UpdateUser.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=12, Name="ɾ��",Code=EnumPermissionCode.DeleteUser.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=13, Name="��������",Code=EnumPermissionCode.ResetPwdUser.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=34, Name="�����û�",Code=EnumPermissionCode.ImportUser.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},

               new Permission{Id=14, Name="�����û���",Code=EnumPermissionCode.SetGroupUser.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]},
               new Permission{Id=15, Name="���ý�ɫ",Code=EnumPermissionCode.SetRolesUser.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[7]}, 
            #endregion
             
            #region ģ��
		     new Permission{Id=16, Name="��ѯ",Code=EnumPermissionCode.QueryModule.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[8]},
             new Permission{Id=17, Name="����",Code=EnumPermissionCode.AddModule.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[8]},
             new Permission{Id=18, Name="�޸�",Code=EnumPermissionCode.UpdateModule.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[8]},
            #endregion

            #region Ȩ��
		     new Permission{Id=19, Name="��ѯ",Code=EnumPermissionCode.QueryPermission.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[9]},
             new Permission{Id=20, Name="����",Code=EnumPermissionCode.AddPermission.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[9]},
             new Permission{Id=21, Name="�޸�",Code=EnumPermissionCode.UpdatePermission.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[9]},
            #endregion

            #region �û���
		     new Permission{Id=22, Name="��ѯ",Code=EnumPermissionCode.QueryUserGroup.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
             new Permission{Id=23, Name="����",Code=EnumPermissionCode.AddUserGroup.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
             new Permission{Id=24, Name="�޸�",Code=EnumPermissionCode.UpdateUserGroup.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
             new Permission{Id=25, Name="ɾ��",Code=EnumPermissionCode.DeleteUserGroup.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
             new Permission{Id=26, Name="���ý�ɫ",Code=EnumPermissionCode.SetRolesUserGroup.ToString(), Description="����" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[10]},
            #endregion

            #region �������ݹ���
             new Permission{Id=27, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="���������ֵ��" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[12]},
             new Permission{Id=28, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="ְ���ֵ��" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[13]},
             new Permission{Id=29, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="���������ֵ��" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[14]},
             new Permission{Id=30, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="���ʻ������ʼ��" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[15]},
             new Permission{Id=31, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="�������ֵ��" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[16]},
             new Permission{Id=32, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="�����ֵ��" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[17]},           
             new Permission{Id=33, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="�����ֵ��" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[18]},                     
             new Permission{Id=36, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="������˰��¼" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[19]},     
             new Permission{Id=37, Name="����",Code=EnumPermissionCode.Audit.ToString(), Description="��ʷ��˰��¼" ,Enabled=true,UpdateDate=DateTime.Now,module=modules[20]},  
            #endregion
            };
            DbSet<Permission> permissionSet = context.Set<Permission>();
            permissionSet.AddOrUpdate(m => new { m.Id }, permissionSet.ToArray());
            context.SaveChanges();
            #endregion

            #region ��ɫ����
            List<Role> roles = new List<Role>
            {
                new Role { Id=1,  RoleName = "ϵͳ����Ա", Description="ϵͳ����Ա",Enabled=true,OrderSort=1,UpdateDate=DateTime.Now,Permissions=permissions},
                new Role { Id=2,  RoleName = "��ͨ�û�", Description="��ͨ�û�",Enabled=true,OrderSort=1,UpdateDate=DateTime.Now ,Permissions=permissions}
            };
            DbSet<Role> roleSet = context.Set<Role>();
            roleSet.AddOrUpdate(m => new { m.RoleName }, roles.ToArray());
            context.SaveChanges();
            #endregion

            #region �û�����
            List<User> members = new List<User>
            {
                new User { Id=1, UserName = "admin", Password = "e10adc3949ba59abbe56e057f20f883e", TrueName = "����Ա", Enabled=true,Roles=new List<Role>{roles[0]} },
                new User { Id=2, UserName = "zhangs", Password = "e10adc3949ba59abbe56e057f20f883e",TrueName = "��˶", Enabled=true,Roles=new List<Role>{roles[1]} }
            };
            DbSet<User> memberSet = context.Set<User>();
            memberSet.AddOrUpdate(m => new { m.UserName }, members.ToArray());
            context.SaveChanges();
            #endregion

            #region �û������
            List<UserGroup> userGroups = new List<UserGroup>
            {
                new UserGroup { Id=1, GroupName = "��ͨ�û���",Description = "��ͨ�û���",Enabled=true,Roles=new List<Role>{roles[1]},OrderSort = 1,Users = new List<User>(){members[1]}},
                new UserGroup { Id=2, GroupName = "ϵͳ������", Description = "ϵͳ������",Enabled=true,Roles=new List<Role>{roles[0]},OrderSort = 2,Users = new List<User>(){members[0]}}
            };
            DbSet<UserGroup> userGroupsSet = context.Set<UserGroup>();
            userGroupsSet.AddOrUpdate(m => new { m.GroupName }, userGroups.ToArray());
            context.SaveChanges();
            #endregion

            #region ְ��--title
            //List<Title> titles = new List<Title>
            //{
            //    new Title { TitleCode="1000", TitleName = "�о�Ա" },
            //    new Title { TitleCode="1001", TitleName = "���о�Ա" },
            //    new Title { TitleCode="1002", TitleName = "�߹�" },
            //    new Title { TitleCode="1003", TitleName = "����ʦ" },
            //    new Title { TitleCode="1004", TitleName = "������ʦ" },
            //    new Title { TitleCode="1005", TitleName = "ְԱ" },
            //    new Title { TitleCode="1006", TitleName = "����" },
            //    new Title { TitleCode="1007", TitleName = "������" },
            //    new Title { TitleCode="1008", TitleName = "��ʦ" },
            //    new Title { TitleCode="1009", TitleName = "��ʿ��" },
            //    new Title { TitleCode="1010", TitleName = "��ʿ" },
            //    new Title { TitleCode="1011", TitleName = "�о���" },
            //    new Title { TitleCode="1012", TitleName = "����" }
            //};
            //DbSet<Title> titlesSet = context.Set<Title>();
            //titlesSet.AddOrUpdate(m => new { m.TitleCode }, titles.ToArray());
            //context.SaveChanges();
            #endregion

            #region ά��ʱ��--SafeguardTime
            //List<SafeguardTime> safeguardTime = new List<SafeguardTime>
            //{
            //    new SafeguardTime { StartTime=new DateTime(), EndTime= new DateTime() }
            //};
            //DbSet<SafeguardTime> safeguardTimesSet = context.Set<SafeguardTime>();
            //safeguardTimesSet.AddOrUpdate(m => new { m.StartTime }, safeguardTime.ToArray());
            //context.SaveChanges();
            #endregion

            //#region ���--umrcover
            //List<Umrcover> umrcovers = new List<Umrcover>
            //{
            //    new Umrcover { UmrID="1000", Name = "ɽ����ַ", UserID = "00001" }
            //};
            //DbSet<Umrcover> umrcoverSet = context.Set<Umrcover>();
            //umrcoverSet.AddOrUpdate(m => new { m.Name }, umrcovers.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--basicproperty
            //List<BasicProperty> basicPropertys = new List<BasicProperty>
            //{
            //    new BasicProperty { UmrID="1000", Address = "����", Longitude = "107.21", Latitude="35.2" }
            //};
            //DbSet<BasicProperty> basicPropertySet = context.Set<BasicProperty>();
            //basicPropertySet.AddOrUpdate(m => new { m.UmrID }, basicPropertys.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--points
            //List<Points> points = new List<Points>
            //{
            //    new Points { UmrID="1000", Altitude = 1000.3, Longitude = "107.21", Latitude="35.2" }
            //};
            //DbSet<Points> pointsSet = context.Set<Points>();
            //pointsSet.AddOrUpdate(m => new { m.UmrID }, points.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--sammples
            //List<Samples> samples = new List<Samples>
            //{
            //    new Samples { UmrID="1000", Name = "ɽ������", UserID = "00001" }
            //};
            //DbSet<Samples> samplesSet = context.Set<Samples>();
            //samplesSet.AddOrUpdate(m => new { m.UmrID }, samples.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--others
            //List<Others> others = new List<Others>
            //{
            //    new Others { UmrID="1001", Name = "ɽ������", UserID = "00001" }
            //};
            //DbSet<Others> othersSet = context.Set<Others>();
            //othersSet.AddOrUpdate(m => new { m.UmrID }, others.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--photos
            //List<Photos> photos = new List<Photos>
            //{
            //    new Photos { UmrID="1000", Name = "ɽ������", UserID = "00001" }
            //};
            //DbSet<Photos> photosSet = context.Set<Photos>();
            //photosSet.AddOrUpdate(m => new { m.PhotoID }, photos.ToArray());
            //context.SaveChanges();
            //#endregion

         

            //#region ���--audit
            //List<Audit> audits = new List<Audit>
            //{
            //    new Audit { UmrID="1000", Operator = "Lucy", OperatorID = "00001" }
            //};
            //DbSet<Audit> auditSet = context.Set<Audit>();
            //auditSet.AddOrUpdate(m => new { m.UmrID }, audits.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--DigsituationBefore
            //List<DigsituationBefore> digsituationBefores = new List<DigsituationBefore>
            //{
            //    new DigsituationBefore { UmrID="1000", Name = "ɽ��ͼֽ" ,DigareaID="100"}
            //};
            //DbSet<DigsituationBefore> digsituationBeforesSet = context.Set<DigsituationBefore>();
            //digsituationBeforesSet.AddOrUpdate(m => new { m.DigareaID }, digsituationBefores.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--LayerDeposit
            //List<LayerDeposit> layerDeposits = new List<LayerDeposit>
            //{
            //    new LayerDeposit { UmrID="1000", Shape = "��Բ"}
            //};
            //DbSet<LayerDeposit> layerDepositsSet = context.Set<LayerDeposit>();
            //layerDepositsSet.AddOrUpdate(m => new { m.UmrID }, layerDeposits.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--ImportAntsites
            //List<ImportAntsites> importAntsitess = new List<ImportAntsites>
            //{
            //    new ImportAntsites { UmrID="1000", SiteID = "1001"}
            //};
            //DbSet<ImportAntsites> importAntsitessSet = context.Set<ImportAntsites>();
            //importAntsitessSet.AddOrUpdate(m => new { m.SiteID }, importAntsitess.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--Literature
            //List<Literature> literatures = new List<Literature>
            //{
            //    new Literature { UmrID="1000", LiteratureID = "1001"}
            //};
            //DbSet<Literature> literaturesSet = context.Set<Literature>();
            //literaturesSet.AddOrUpdate(m => new { m.LiteratureID }, literatures.ToArray());
            //context.SaveChanges();
            //#endregion

            //#region ���--ProtectUnits
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
