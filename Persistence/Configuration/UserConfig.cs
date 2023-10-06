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
    public class UserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Usersss");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nombre)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(p => p.Apellido)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(p => p.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(100)
                .IsRequired();

        }
    }
}
