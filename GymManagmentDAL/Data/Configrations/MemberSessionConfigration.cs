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
    internal class MemberSessionConfigration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            // createdAt ==> BookingDate

            builder.Property(x => x.CreatedAt).HasColumnName("BookingDate").HasDefaultValueSql("GETDATE()");

            // Set Composite Primary Key

            builder.HasKey(Cp => new
            {
                Cp.MemberId,
                Cp.SessionId
            });

            // ignore id that inherit from base Entity
            builder.Ignore(x => x.Id);
        }
    }
}
