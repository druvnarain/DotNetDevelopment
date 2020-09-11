using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    //Used to configure Entity (which results in configuring tables created from scaffolding), 
    //can be placed in StoreContext but shouldnt
    //must be referenced in StoreContext.cs
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(prop => prop.Id).IsRequired();
            builder.Property(prop => prop.Name).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.Description).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.Price).HasColumnType("decimal(18,2)");
            builder.Property(prop => prop.PictureUrl).IsRequired();
            //Reads product has one brand, brand has many products
            builder.HasOne(brand => brand.ProductBrand).WithMany()
                .HasForeignKey(prop => prop.ProductBrandId);
            builder.HasOne(type => type.ProductType).WithMany()
                .HasForeignKey(prop => prop.ProductTypeId);
        }
    }
}