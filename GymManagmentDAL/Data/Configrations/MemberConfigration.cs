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
    internal class MemberConfigration:GymUserConfigration<Member>,IEntityTypeConfiguration<Member>
    {
        /*
        I used (new) because the Configure method in the base class is not virtual, so I cannot override it.

        new simply hides the parent method and defines a new one with the same name.

        override → replaces the base method (requires virtual)

        new → hides the base method and creates a new one

        In EF Core, this is used when you want a base configuration but also want a separate configuration for the derived class.
        */
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            // if You Want to Make Any Config and You Inherit from class you must put config before base to avoid override in base 

            builder.Property(x => x.CreatedAt).HasColumnName("JoinDate").HasDefaultValueSql("GETDATE()");

            base.Configure(builder); // Takes all Configration Of the GymUser


        }



    }
}
