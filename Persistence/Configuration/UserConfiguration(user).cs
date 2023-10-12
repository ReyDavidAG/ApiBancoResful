using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class UserConfiguration_user_ : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(p => p.IdUser);

            builder.Property(p => p.IdUser)
                .IsRequired()
                .HasColumnName("id_user")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.IdAccount)
                .IsRequired()
                .HasColumnName("id_account");

            builder.HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<User>(u => u.IdAccount)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Fk_user_account");
        }
    }
}
