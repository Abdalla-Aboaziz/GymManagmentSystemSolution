using GymManagmentDAL.Entites;
using Microsoft.AspNetCore.Identity;

public static class IdentityDbContextSeeding
{
    public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        try
        {
            // Check if any users or roles already exist
            var HasUser = userManager.Users.Any();
            var HasRole = roleManager.Roles.Any();
            if (HasRole && HasUser) return false;
            if (!HasRole)
            {
                // Define roles to be created
                var Roles = new List<IdentityRole>()
                    {
                        new () {Name="SuperAdmin"},
                        new () {Name="Admin"},


                    };
                // Create roles if they do not exist
                foreach (var role in Roles)
                {
                    if (!roleManager.RoleExistsAsync(role.Name!).Result)
                    {
                        roleManager.CreateAsync(role).Wait();
                    }
                }
            }

            if (!HasUser)
            {
                //Create Default Users
                var MainAdmin = new ApplicationUser()
                {
                    FirstName = "Abdalla",
                    LastName = "Aboaziz",
                    UserName = "AbdallaAboaziz",
                    Email = "abdallaaboaziz@gmail.com",
                    PhoneNumber = "01277353904"

                };
                // Create Main Admin User
                userManager.CreateAsync(MainAdmin, "Admin@123").Wait();
                // Add Main Admin User to SuperAdmin Role
                userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();
                var Admin = new ApplicationUser()
                {
                    FirstName = "Mohamed",
                    LastName = "amr",
                    UserName = "Mohamedamr",
                    Email = "Mohamedamr@gmail.com",
                    PhoneNumber = "01092694545"

                };
                userManager.CreateAsync(Admin, "Admin@123").Wait();
                userManager.AddToRoleAsync(Admin, "Admin").Wait();
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seed Faild {ex}");
            return false;
        }
    }
}