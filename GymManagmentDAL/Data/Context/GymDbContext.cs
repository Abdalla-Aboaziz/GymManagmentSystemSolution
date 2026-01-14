using GymManagmentDAL.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.Context
{
    public class GymDbContext:IdentityDbContext<ApplicationUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=GymMangmentv1;Integrated Security=True;TrustServerCertificate=True");
        //}

        public GymDbContext(DbContextOptions<GymDbContext> option):base(option) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // read configration classes
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(E =>
            {
               E.Property(U => U.FirstName).HasColumnType("varchar").HasMaxLength(50).IsRequired();
                E.Property(U => U.LastName).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            });
        }

        #region Dbset
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category > Categories { get; set; }
        public DbSet <Trainer> Trainers { get; set; }
        public DbSet <Session> Sessions { get; set; }
        public DbSet <MemberShip > MemberShips { get; set; }
        public DbSet <MemberSession > MemberSessions { get; set; }
        #endregion



    }
}
