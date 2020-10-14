using System.Linq;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class StoreContext : DbContext
    {

        //connection string goes in options
        //specify <type> if using multiple DbContext
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        //Represents a table in the database. When scaffolding, usings attribute name for table name
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        //Method responsible for creating migration (entity to table)
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            //From DbContext class (base)
            base.OnModelCreating(modelBuilder);

            //Applies configurations (ex. ProductConfigurations.cs) in Configurations Folder
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if(Database.ProviderName == "Microsoft.EntityFramework.Sqlite")
            {
                foreach(var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach(var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }

    }
}