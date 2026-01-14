using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.AccountViewModel
{
    public  class LoginViewModel
    {
        [Required(ErrorMessage ="Email is Required")]
        
        public string Email { get; set; } = null!;
        [Required (ErrorMessage ="Password is Required")]
        public string Password { get; set; }= null!;

        public bool RememberMe { get; set; }
    }
}
