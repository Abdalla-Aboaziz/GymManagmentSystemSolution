using GymManagmentDAL.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.Configrations
{
    internal class HealthRecordConfigration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<HealthRecord> builder)
        {
           // for relation 1:1 total from both (Member-HealthRecord)
           builder.ToTable("Members").HasKey(t => t.Id);

           builder.HasOne<Member>().WithOne(t => t.HealthRecord).HasForeignKey<HealthRecord>(x => x.Id);
            // ignore CreatedAt from Base 
            builder.Ignore(x => x.CreatedAt);
        }
    }
}
