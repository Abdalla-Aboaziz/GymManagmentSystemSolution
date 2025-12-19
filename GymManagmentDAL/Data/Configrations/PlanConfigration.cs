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
    internal class PlanConfigration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(X => X.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(X => X.Description).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.Price).HasPrecision(10, 2);

            // Duration ==> constrain On Table 
            builder.ToTable(TP =>
            {
                TP.HasCheckConstraint("PlanDurationCheck", "DurationDays Between 1 and 365");

            });

        }
    }
}
