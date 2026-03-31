namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserPhotoConfiguration : IEntityTypeConfiguration<UserPhoto>
{
    public void Configure(EntityTypeBuilder<UserPhoto> builder)
    {
        // Table
        builder.ToTable("UserPhotos");

        // Primary Key
        builder.HasKey(up => up.Id);

        // Properties
        builder.Property(up => up.Id)
            .ValueGeneratedNever();

        builder.Property(up => up.UserId)
            .IsRequired();

        builder.Property(up => up.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(up => up.ViewType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(up => up.Notes)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(up => up.MeasurementsJson)
            .IsRequired(false)
            .HasColumnType("text");

        builder.Property(up => up.UploadedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(up => up.UserId)
            .HasDatabaseName("IX_UserPhotos_UserId");

        builder.HasIndex(up => up.ViewType)
            .HasDatabaseName("IX_UserPhotos_ViewType");

        // Relationships
        builder.HasOne(up => up.User)
            .WithMany(u => u.Photos)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
