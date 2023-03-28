using HortaIn.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HortaIn.DAL.Data
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options) { }

        
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UsuarioContext>
    {
        public UsuarioContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory()
      + "/../HortaIn.API/appsettings.json").Build();

            var builder = new DbContextOptionsBuilder<UsuarioContext>();
            var connectionString = configuration.GetConnectionString("UsuariosDb");
            builder.UseSqlServer(connectionString);
            return new UsuarioContext(builder.Options);

        }
    }
}
