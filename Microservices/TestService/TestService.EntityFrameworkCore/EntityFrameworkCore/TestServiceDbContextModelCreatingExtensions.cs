using TestService.Domain.Base.Faults;
using TestService.Domain.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using TestService.Domain.EFTest.FluntApi;

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
                b.Property(x => x.FaultGradeColor).HasDefaultValue(0);

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
        }
    }
}