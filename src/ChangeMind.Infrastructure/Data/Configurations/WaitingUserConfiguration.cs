namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WaitingUserConfiguration : IEntityTypeConfiguration<WaitingUser>
{
    public void Configure(EntityTypeBuilder<WaitingUser> builder)
    {
        // Table
        builder.ToTable("WaitingUsers");

        // Primary Key
        builder.HasKey(w => w.Id);

        // Properties
        builder.Property(w => w.Id)
            .ValueGeneratedNever();

        builder.Property(w => w.UserId)
            .IsRequired();

        builder.Property(w => w.IsWaitingForAssignment)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(w => w.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(w => w.UpdatedAt)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(w => w.UserId)
            .IsUnique()
            .HasDatabaseName("IX_WaitingUsers_UserId");

        builder.HasIndex(w => w.IsWaitingForAssignment)
            .HasDatabaseName("IX_WaitingUsers_IsWaitingForAssignment");

        // Relationships
        builder.HasOne(w => w.User)
            .WithOne(u => u.WaitingUserRecord)
            .HasForeignKey<WaitingUser>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
