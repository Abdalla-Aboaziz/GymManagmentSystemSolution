using GymManagmentDAL.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.Configrations
{
    internal class GymUserConfigration<T> : IEntityTypeConfiguration<T> where T : GymUser  // To Apply on Member and Trainer
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {


            builder.Property(X => X.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(X => X.Email).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(X => X.Phone).HasColumnType("varchar").HasMaxLength(11);
            // put constrain on table for validation  [Email - Phone]
            builder.ToTable(TP =>
            {
                TP.HasCheckConstraint("GymUserValidEmailCheck", "Email Like '_%@_%._%'"); // Email
                TP.HasCheckConstraint("GymUserValidPhoneCheck", "Phone Like '01%' and Phone Not Like '%[^0-9]%'"); // Phone


            });

            builder.HasIndex(x => x.Email).IsUnique();        //make Email Unique
            builder.HasIndex(x => x.Phone).IsUnique();        //make Phone Unique


            //Adress Config

            builder.OwnsOne(x => x.Address, AddressBuilder =>
            {
                AddressBuilder.Property(x => x.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(30); // street in Address
                AddressBuilder.Property(x => x.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(30); // City in Address
                AddressBuilder.Property(x => x.BuildingNumber).HasColumnName("BuildingNumber"); // BuildingNumber in Address

            });


          

        }
    }
}
