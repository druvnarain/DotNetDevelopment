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


    }
}