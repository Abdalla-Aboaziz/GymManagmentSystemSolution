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
    internal class SessionConfigration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            // 2 constrains 

            builder.ToTable(tp =>
            {
                tp.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25");
                tp.HasCheckConstraint("SessionEndDate", "EndDate > StartDate");

            });
          #region Session-Category 
           builder.HasOne(X=>X.SessionCategory).WithMany(x => x.Sessions).HasForeignKey(x => x.CategoryId);

          #endregion

          #region Trainer -Session
            builder.HasOne(X => X.SessionTrainer).WithMany(x => x.TrainerSession).HasForeignKey(x => x.TrainerId);

            #endregion


        }
    }
}
