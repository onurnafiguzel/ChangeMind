namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // Table
        builder.ToTable("Payments");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.PackageId)
            .IsRequired();

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.TransactionId)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(p => p.CompletedAt)
            .IsRequired(false);

        builder.Property(p => p.IdempotencyKey)
            .IsRequired(false)
            .HasMaxLength(36);

        // Indexes
        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("IX_Payments_UserId");

        builder.HasIndex(p => p.PackageId)
            .HasDatabaseName("IX_Payments_PackageId");

        builder.HasIndex(p => p.Status)
            .HasDatabaseName("IX_Payments_Status");

        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName("IX_Payments_CreatedAt");

        builder.HasIndex(p => new { p.UserId, p.IdempotencyKey })
            .IsUnique()
            .HasFilter("\"IdempotencyKey\" IS NOT NULL")
            .HasDatabaseName("UX_Payments_UserId_IdempotencyKey");

        // Relationships
        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Package)
            .WithMany(pkg => pkg.Payments)
            .HasForeignKey(p => p.PackageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
