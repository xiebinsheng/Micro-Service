using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestService.Domain.Base.Faults;
using TestService.Domain.EFTest.FluntApi;
using TestService.TestEntities.BillManagement;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.EntityHistory;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueConverters;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace TestService.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See TestServiceMigrationsDbContext for migrations.
     */
    [ConnectionStringName("TestService")]
    public class TestServiceDbContext : AbpDbContext<TestServiceDbContext>
    {
        //public DbSet<AppUser> Users { get; set; }

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside TestServiceDbContextModelCreatingExtensions.ConfigureTestService
         */

        public DbSet<FaultGrade> FaultGrades { get; set; }

        public DbSet<TestEntityProperties> TestEntityProperties { get; set; }

        public DbSet<BillMembers> BillMembers { get; set; }

        public TestServiceDbContext(DbContextOptions<TestServiceDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            //builder.Entity<AppUser>(b =>
            //{
            //    b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser

            //    b.ConfigureByConvention();
            //    b.ConfigureAbpUser();

            //    /* Configure mappings for your additional properties
            //     * Also see the TestServiceEfCoreEntityExtensionMappings class
            //     */
            //});

            /* Configure your own tables/entities inside the ConfigureTestService method */

            builder.ConfigureTestService();
        }

    }
}
