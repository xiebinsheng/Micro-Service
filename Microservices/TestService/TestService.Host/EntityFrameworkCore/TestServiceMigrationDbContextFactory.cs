using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TestService.EntityFrameworkCore
{
    public class TestServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<TestServiceMigrationDbContext>
    {
        public TestServiceMigrationDbContext CreateDbContext(string[] args)
        {
            TestServiceEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            //确认执行迁移到TestService数据库
            var builder = new DbContextOptionsBuilder<TestServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("TestService"));

            return new TestServiceMigrationDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
