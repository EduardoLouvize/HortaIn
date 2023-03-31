using HortaIn.BLL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HortaIn.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }      

        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {            

            // Configure the Post entity
            builder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(e => e.Conteudo).HasMaxLength(500).IsRequired();
                entity.Property(e => e.ApplicationUserId).IsRequired();
            });

            builder.Entity<Image>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(e => e.Uri).IsRequired();
                entity.Property(e => e.ApplicationUserId).IsRequired();
            });

            base.OnModelCreating(builder);
        }

    }

    public class IdentityDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory()
      + "/../HortaIn.API/appsettings.json").Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("AppIdentityConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);

        }
    }
}
