namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.UserId).IsRequired();

        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Age).IsRequired(false);
        builder.Property(p => p.Height).HasPrecision(5, 2).IsRequired(false);
        builder.Property(p => p.Weight).HasPrecision(5, 2).IsRequired(false);
        builder.Property(p => p.Gender).IsRequired(false).HasConversion<string>().HasMaxLength(20);

        builder.Property(p => p.FitnessGoalId).IsRequired(false);
        builder.Property(p => p.FitnessLevel).IsRequired(false).HasConversion<string>().HasMaxLength(20);

        builder.Property(p => p.DailyWorkLifestyle).HasMaxLength(2000);
        builder.Property(p => p.GymDaysPerWeek).IsRequired(false);
        builder.Property(p => p.HealthConditions).HasMaxLength(2000);
        builder.Property(p => p.FoodAllergies).HasMaxLength(2000);
        builder.Property(p => p.SupplementInterest).HasMaxLength(2000);
        builder.Property(p => p.WantsSupplementSupport).IsRequired().HasDefaultValue(false);

        builder.Property(p => p.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt).IsRequired(false);

        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasDatabaseName("UX_UserProfiles_UserId");

        builder.HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.FitnessGoal)
            .WithMany()
            .HasForeignKey(p => p.FitnessGoalId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
