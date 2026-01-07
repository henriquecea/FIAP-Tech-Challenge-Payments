using FCG_Payments.Domain.Entity;
using FCG_Payments.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace FCG_Payments.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PurchaseEntity> Purchases { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PurchaseMap());
    }
}
