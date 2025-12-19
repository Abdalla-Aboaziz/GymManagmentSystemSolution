using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Data.DataSeeding;
using GymManagmentDAL.Reposotories.Classes;
using GymManagmentDAL.Reposotories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
        
            #region Database Configuration

            builder.Services.AddDbContext<GymDbContext>(options =>          // Enable DI for GymDbContext
            {
                //option.UseSqlServer(builder.Configuration.GetSection("ConnectionString")["DefaultConnection"]);
                //option.UseSqlServer(builder.Configuration["ConnectionString:DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion
            #region Services Configration


            #region Before UnitOfWork
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository, PlaneRepository>(); 
            #endregion

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository,SessionRepository>(); 


            #endregion

            var app = builder.Build();


            #region DataSeeding 
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            GymDbContextSeeding.SeedData(dbcontext, app.Environment.ContentRootPath);
            var PendingMigration = dbcontext.Database.GetPendingMigrations();

            if (PendingMigration?.Any() ?? false)
            {
                dbcontext.Database.Migrate();
            }

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
