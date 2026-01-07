using FCG_Payments.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG_Payments.Infrastructure.Data.Mapping;

public class PurchaseMap : IEntityTypeConfiguration<PurchaseEntity>
{
    public void Configure(EntityTypeBuilder<PurchaseEntity> builder)
    {
        builder.ToTable("Purchase");

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Value)
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired();
    }
}
