using Blog.Domain.Constants;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        _ = builder.Property(t => t.Name)
            .HasMaxLength(DataConfigs.ShortString)
            .IsRequired();

        _ = builder.Property(t => t.Description)
            .HasMaxLength(DataConfigs.LongString)
            .IsRequired();
    }
}
