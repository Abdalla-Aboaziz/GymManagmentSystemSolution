using GymManagmentBLL.Service.Interfaces;
using GymManagmentDAL.Entites;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class HomeController : Controller
    {
        //  Action method  handle requests to the home page 


        private readonly IAnalyticsService _analyticsService;

        

        public HomeController(IAnalyticsService analyticsService)
        {
          _analyticsService = analyticsService;
        }
        public IActionResult Index()
        {
           var data = _analyticsService.GetAnalyticsData();
            return View(data);
        }

   


    }
}
