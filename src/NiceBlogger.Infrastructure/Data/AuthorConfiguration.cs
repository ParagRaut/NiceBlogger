using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NiceBlogger.UseCases.AuthorUseCases.Entities;

namespace NiceBlogger.Infrastructure.Data;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(author => author.Value,
                value => new AuthorId(value));

        builder.Property(r => r.Name).HasMaxLength(100);

        builder.Property(r => r.Surname).HasMaxLength(250);
    }
}