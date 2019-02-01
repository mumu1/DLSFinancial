﻿/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/5/15 16:23:12  
*************************************/

using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BEYON.Component.Data.Configurations.Member;
using BEYON.Component.Data.Configurations.App;
using BEYON.Domain.Model.Member;
using BEYON.Domain.Model.App;

namespace BEYON.Component.Data.EF
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base(nameOrConnectionString: "BeyonDBGuMu") { }

        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString) { }

        public EFDbContext(DbConnection existingConnection)
            : base(existingConnection, true) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Module> Modules { get; set; }


        //地理所财务
        public DbSet<ApplicationForm> ApplicationForms { get; set; }

        public DbSet<PersonalRecord> PersonalRecords { get; set; }

        public DbSet<TopContacts> TopContactss { get; set; }

        public DbSet<TaxPerOrder> TaxPerOrders { get; set; }

        public DbSet<TaxPerOrderHistory> TaxPerOrderHistorys { get; set; }

        public DbSet<PersonalSalary> PersonalSalarys { get; set; }

        public DbSet<PersonalLabour> PersonalLabours { get; set; }

        public DbSet<ProjectCost> ProjectCosts { get; set; }

        public DbSet<Title> Titles { get; set; }

        public DbSet<RefundType> RefundTypes { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<SafeguardTime> SafeguardTimes { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<TaskManage> TaskManages { get; set; }

        public DbSet<AuditStatus> AuditStatuss { get; set; }

        public DbSet<AuditOpinion> AuditOpinions { get; set; }

        public DbSet<TaxBaseByMonth> TaxBaseByMonths { get; set; }

        public DbSet<TaxBaseEveryMonth> TaxBaseEveryMonths { get; set; }


        //[ImportMany(typeof(IEntityMapper))]
        //public IEnumerable<IEntityMapper> EntityMappers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除一对多的级联删除约定，【想要级联删除可以在 EntityTypeConfiguration<TEntity>的实现类中进行控制,级联删除是在WithMany返回的对象中设定的。】
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //移除一对一的级联删除约定
            //modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
            //移除多对多的级联删除约定
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new ModuleConfiguration());
            modelBuilder.Configurations.Add(new UserGroupConfiguration());

            //地理所财务
            modelBuilder.Configurations.Add(new ApplicationFormConfiguration());
            modelBuilder.Configurations.Add(new PersonalRecordConfiguration());
            modelBuilder.Configurations.Add(new TopContactsConfiguration());
            modelBuilder.Configurations.Add(new TaxPerOrderConfiguration());
            modelBuilder.Configurations.Add(new TaxPerOrderHistoryConfiguration());
            modelBuilder.Configurations.Add(new PersonalSalaryConfiguration());
            modelBuilder.Configurations.Add(new PersonalLabourConfiguration());
            modelBuilder.Configurations.Add(new ProjectCostConfiguration());
            modelBuilder.Configurations.Add(new TitleConfiguration());
            modelBuilder.Configurations.Add(new RefundTypeConfiguration());
            modelBuilder.Configurations.Add(new BankConfiguration());
            modelBuilder.Configurations.Add(new SafeguardTimeConfiguration());
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new TaskManageConfiguration());
            modelBuilder.Configurations.Add(new TaxBaseByMonthConfiguration());
            modelBuilder.Configurations.Add(new TaxBaseEveryMonthConfiguration());
            modelBuilder.Configurations.Add(new AuditStatusConfiguration());
            modelBuilder.Configurations.Add(new AuditOpinionConfiguration());
            //if (EntityMappers == null)
            //{
            //    return;
            //}

            //foreach (var mapper in EntityMappers)
            //{
            //    mapper.RegistTo(modelBuilder.Configurations);
            //}
        }
    }
}