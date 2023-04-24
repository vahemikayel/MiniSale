using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniSale.Api.Models.Account.Entity;

namespace MiniSale.Api.Infrastructure.Contexts.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static string Schema { get; private set; } = "Identity";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Schema);

            builder.Entity<ApplicationUser>()
                  .Property(x => x.Id)
                  .HasMaxLength(36);

            builder.Entity<ApplicationUser>()
                   .Property(x => x.FirstName)
                   .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                   .Property(x => x.LastName)
                   .HasMaxLength(100);

            base.OnModelCreating(builder);
        }
    }
}
