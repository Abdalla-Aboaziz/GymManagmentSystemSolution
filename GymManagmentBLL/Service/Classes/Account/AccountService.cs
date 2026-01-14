using GymManagmentBLL.Service.Interfaces.Account;
using GymManagmentBLL.ViewModels.AccountViewModel;
using GymManagmentDAL.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            // Make User Manger  Check on Email and Password
            var user = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if (user is null) return null;
            var isPasswordValid = _userManager.CheckPasswordAsync(user, loginViewModel.Password).Result;
            return isPasswordValid ? user : null;
        }
    }
}
