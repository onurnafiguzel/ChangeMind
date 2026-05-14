namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedNever();

        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);

        builder.Property(u => u.Role).IsRequired().HasConversion<string>();

        builder.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(u => u.IsCompletedProfile).IsRequired().HasDefaultValue(false);

        builder.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(u => u.UpdatedAt).IsRequired(false);

        // Ignore backward-compat read-only navigation accessors
        builder.Ignore(u => u.FirstName);
        builder.Ignore(u => u.LastName);

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_Users_IsActive");

        // Profile relationship is configured from UserProfileConfiguration.

        builder.HasMany(u => u.Photos)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.AssignedPrograms)
            .WithOne(tp => tp.AssignedTo)
            .HasForeignKey(tp => tp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.CoachRelationships)
            .WithOne(cu => cu.User)
            .HasForeignKey(cu => cu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
