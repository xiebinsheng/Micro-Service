using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthenticationServer.Host.EntityFrameworkCore
{
    public class AuthenticationServerHostDbContextFactory : IDesignTimeDbContextFactory<AuthenticationServerHostDbContext>
    {
        public AuthenticationServerHostDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AuthenticationServerHostDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new AuthenticationServerHostDbContext(builder.Options);
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
