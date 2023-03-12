using HortaIn.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HortaIn.DAL.Data
{
    public class PasswordChangeContext : DbContext
    {
        public PasswordChangeContext(DbContextOptions<PasswordChangeContext> options) : base(options) { }

        public DbSet<PasswordRecovery> PasswordChange { get; set; }
    }

    public class DesignTimeDbContextFactory2 : IDesignTimeDbContextFactory<PasswordChangeContext>
    {
        public PasswordChangeContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory()
      + "/../HortaIn.API/appsettings.json").Build();

            var builder = new DbContextOptionsBuilder<PasswordChangeContext>();
            var connectionString = configuration.GetConnectionString("UsuariosDb");
            builder.UseSqlServer(connectionString);
            return new PasswordChangeContext(builder.Options);

        }
    }
}
