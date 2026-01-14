using GymManagmentBLL.Service.Interfaces.Account;
using GymManagmentBLL.ViewModels.AccountViewModel;
using GymManagmentDAL.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    // Handles Login, Logout and Access Denied actions
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Login -Logout - AccessDenied

        public AccountController(IAccountService accountService,SignInManager<ApplicationUser> signInManager)
        {
          _accountService = accountService;
           _signInManager = signInManager;
        }

        // Display Login page
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        // Handle Login form submission
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            // Validate incoming model
            if (!ModelState.IsValid) return View(model);

            // Check if user credentials are valid
            var User = _accountService.ValidateUser(model);
            if (User is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid username or password");
                return View(model);

            }
            // Attempt to sign in the user
            var Result = _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false).Result;
            // Handle different sign-in failure scenarios
            if (Result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Email Is not Allowed");
            if (Result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Email Is LockedOut");
            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(model);


        }

        // Logout
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }
        // Access Denied
        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}
