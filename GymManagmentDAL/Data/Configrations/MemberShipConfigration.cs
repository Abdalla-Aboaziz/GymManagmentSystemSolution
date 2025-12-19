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
    internal class MemberShipConfigration : IEntityTypeConfiguration<MemberShip>
    {
        //this config  class for relation m:m between Member ,plan
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            // createdAt ==> startDate 

            builder.Property(x => x.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");

            // Set Composite Primary Key

            builder.HasKey(Cp => new
            {
                Cp.MemberId,
                Cp.PlanId
            });

            // ignore id that inherit from base Entity
            builder.Ignore(x => x.Id);
        }
    }
}
