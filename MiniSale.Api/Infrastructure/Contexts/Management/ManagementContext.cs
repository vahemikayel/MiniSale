using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniSale.Api.Models.Product.Entity;
using System;

namespace MiniSale.Api.Infrastructure.Contexts.Management
{
    public class ManagementContext : DbContext
    {
        internal static string Schema { get; private set; } = "Sale";

        public ManagementContext() : base()
        {
        }

        public ManagementContext(DbContextOptions<ManagementContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Schema);

            builder.Entity<ProductEModel>(Configure);

            base.OnModelCreating(builder);
        }

        private void Configure(EntityTypeBuilder<ProductEModel> builder)
        {
            builder.ToTable("Products")
                   .HasKey(x => x.Id);

            builder.Property(x=>x.Name)
                   .HasMaxLength(15)
                   .IsRequired()
                   .IsUnicode(true);

            builder.Property(x => x.BarCode)
                   .HasMaxLength(13);

            builder.Property(x => x.PLU)
                   .IsRequired()
                   .IsUnicode(true);

            builder.HasIndex(x => x.PLU)
                   .IsUnique();

            builder.Property(x=>x.Price).HasMaxLength(5000);
        }
    }
}
