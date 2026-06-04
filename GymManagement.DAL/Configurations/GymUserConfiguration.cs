using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
            .HasColumnType( "varchar")
﻿﻿             .HasMaxLength(50);

            builder.Property(x => x.Email)
            .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email like '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck",
            "[Phone] LIKE '010%' OR [Phone] LIKE '011%' " +
            "OR [Phone] LIKE '012%' OR [Phone] LIKE '015%'");
            });

            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").HasColumnType("Varchar").HasMaxLength(30);
                address.Property(a => a.City).HasColumnName("City").HasColumnType("Varchar").HasMaxLength(30);
            });

        }
    }
}
