namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NutritionPlanConfiguration : IEntityTypeConfiguration<NutritionPlan>
{
    public void Configure(EntityTypeBuilder<NutritionPlan> builder)
    {
        builder.ToTable("NutritionPlans");
        builder.HasKey(np => np.Id);

        builder.Property(np => np.Id).ValueGeneratedNever();

        builder.Property(np => np.CoachId).IsRequired(false);
        builder.Property(np => np.UserId).IsRequired();
        builder.Property(np => np.CreatedByType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(np => np.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(np => np.Description)
            .HasMaxLength(1000);

        builder.Property(np => np.DailyPlanJson)
            .IsRequired()
            .HasColumnType("json");

        builder.Property(np => np.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(np => np.VersionNumber).IsRequired().HasDefaultValue(1);

        builder.Property(np => np.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(np => np.UpdatedAt);

        builder.HasIndex(np => np.UserId).HasDatabaseName("IX_NutritionPlans_UserId");
        builder.HasIndex(np => np.CoachId).HasDatabaseName("IX_NutritionPlans_CoachId");
        builder.HasIndex(np => np.IsActive).HasDatabaseName("IX_NutritionPlans_IsActive");
        builder.HasIndex(np => np.CreatedAt).HasDatabaseName("IX_NutritionPlans_CreatedAt");

        builder.HasOne(np => np.User)
            .WithMany()
            .HasForeignKey(np => np.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(np => np.Coach)
            .WithMany()
            .HasForeignKey(np => np.CoachId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
