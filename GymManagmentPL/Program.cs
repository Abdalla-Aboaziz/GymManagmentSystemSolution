using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagmentBLL;
using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.Interfaces;
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
            // Create application builder
            var builder = WebApplication.CreateBuilder(args);

            // Register MVC services (Controllers + Views)
            builder.Services.AddControllersWithViews();

            #region Database Configuration

            // Configure SQL Server DbContext with Dependency Injection
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                //option.UseSqlServer(builder.Configuration.GetSection("ConnectionString")["DefaultConnection"]);
                //option.UseSqlServer(builder.Configuration["ConnectionString:DefaultConnection"]);
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                );
            });

            #endregion

            #region Services Configuration

            #region Before UnitOfWork
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository, PlaneRepository>(); 
            #endregion

            // Unit Of Work & Repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();

            // Business Services
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();


            #region AutoMapper Configuration

            // Register AutoMapper with mapping profile
            builder.Services.AddAutoMapper(
                x => x.AddProfile(new MappingProfile())
            );

            #endregion

            #endregion

            // Build application
            var app = builder.Build();

            #region Data Seeding & Migrations

            // Create scope for database operations at startup
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

            // Apply pending migrations automatically
            var pendingMigrations = dbcontext.Database.GetPendingMigrations();
            if (pendingMigrations?.Any() ?? false)
            {
                dbcontext.Database.Migrate();
            }
            // Seed initial data
            GymDbContextSeeding.SeedData(
                dbcontext,
                app.Environment.ContentRootPath
            );

          

            #endregion

            // Configure HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            // Static files & default routing
            app.MapStaticAssets();
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                )
                .WithStaticAssets();

            // Run application
            app.Run();
        }
    }
}
