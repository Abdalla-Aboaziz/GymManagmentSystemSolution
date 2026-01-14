using GymManagmentBLL.ViewModels.AccountViewModel;
using GymManagmentDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces.Account
{
    public interface IAccountService
    {

        ApplicationUser? ValidateUser(LoginViewModel loginViewModel);
    }
}
