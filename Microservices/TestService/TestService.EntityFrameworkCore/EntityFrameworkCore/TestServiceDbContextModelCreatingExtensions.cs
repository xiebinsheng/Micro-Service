using TestService.Domain.Base.Faults;
using TestService.Domain.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using TestService.Domain.EFTest.FluntApi;
using TestService.TestEntities.BillManagement;

namespace TestService.EntityFrameworkCore
{
    public static class TestServiceDbContextModelCreatingExtensions
    {
        public static void ConfigureTestService(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(TestServiceConsts.DbTablePrefix + "YourEntities", TestServiceConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});

            builder.Entity<FaultGrade>(b =>
            {
                b.ToTable($"{DbTablePrefix.Base}FAULT_GRADE","BAS");

                b.ConfigureByConvention();

                b.Property(x => x.FaultGradeNo)
                    .HasColumnName("FAULT_GRADE_NO")
                    .HasComment("警报级别编号")
                    .HasMaxLength(StringPropertyLength.MaxNoOrCodeLength)
                    .IsRequired();
                b.Property(x => x.FaultGradeName).IsRequired().HasMaxLength(StringPropertyLength.MaxNameLength);
                b.Property(x => x.FaultGradeColor).HasDefaultValue(1).ValueGeneratedNever();
                b.Property(x => x.CreationTime).HasColumnName("CREATE_TIME");
                b.Property(x => x.CreatorId).HasColumnName("CREATE_USERID");
                b.Property(x => x.LastModificationTime).HasColumnName("UPDATE_TIME");
                b.Property(x => x.LastModifierId).HasColumnName("UPDATE_USERID");

                //b.HasIndex(q => q.FaultGradeNo);
            });

            builder.Entity<TestEntityProperties>(b =>
            {
                b.ToTable($"{DbTablePrefix.Test}ENTITY_PROPERTY");

                b.ConfigureByConvention();

                b.Property(x => x.Code)
                    .HasColumnName("CODE")
                    .HasComment("示例测试编号")
                    .HasDefaultValue("test")
                    .HasMaxLength(StringPropertyLength.MaxNoOrCodeLength)
                    .IsRequired();
                b.Property(x => x.Name).HasColumnName("NAME").HasMaxLength(StringPropertyLength.MaxNameLength).IsRequired();
                b.Property(x => x.Desc).HasColumnName("DESC").HasMaxLength(StringPropertyLength.MaxDescLength).IsRequired();
                b.Property(x => x.Price).HasColumnName("PRICE").HasPrecision(14, 2).IsRequired();
                //b.Ignore(x => x.Price);
                b.Property(x => x.RecordTime).HasDefaultValueSql("getdate()").IsRequired();
            });


            builder.Entity<BillMembers>(b =>
            {
                b.ToTable($"BillMembers");

                b.Property(x => x.MemberName)
                    .HasComment("成员名称")
                    .HasMaxLength(StringPropertyLength.MaxNameLength)
                    .IsRequired();
                b.Property(x => x.MemberEName)
                    .HasComment("成员英文名称")
                    .HasMaxLength(StringPropertyLength.MaxNameLength)
                    .IsRequired();
                b.Property(x => x.MemberType)
                    .HasComment("成员类型")
                    .HasDefaultValue(1)
                    .IsRequired();
                b.Property(x => x.Comments)
                    .HasComment("备注")
                    .HasMaxLength(StringPropertyLength.MaxDescLength);
            });
                
        }
    }
}