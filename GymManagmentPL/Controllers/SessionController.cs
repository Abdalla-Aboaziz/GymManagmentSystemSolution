using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
          _sessionService = sessionService;
        }
        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();

            return View(sessions);
        }
        public ActionResult Details(int id )
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID.";

                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";

                return RedirectToAction(nameof(Index));
            }

            return View(session);

        }

        public ActionResult Create()
        {
            // Load Categories and Trainers for dropdown lists
            LoadDropdowns();
            return View();
        }


        // Handles form submission for creating a new session.
        // Performs model validation, calls the service layer,
        // and handles success or failure scenarios.

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            // Validate incoming data based on Data Annotations
            if (!ModelState.IsValid)
            {
                // Reload dropdown data because ViewBag
                // does not persist after a POST request
                LoadDropdowns();
                // Return the view with validation errors
                return View(createSession);

            }
            // Attempt to create a new session using the service layer
            var result = _sessionService.CreateSession(createSession);

            if (result)
            {
                // Store success message to be displayed after redirect
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Store error message to be displayed after redirect
                TempData["ErrorMessage"] = "Session Creation Failed";
                // Reload dropdowns before returning the view
                LoadDropdowns();
                return View(createSession);
            }
        }
        



        #region Helper Method
        // Helper method used to load dropdown list data
        // for Categories and Trainers.
        // Called in both GET and POST actions when needed.
        private void LoadDropdowns()
        {
            // Retrieve categories for selection
            var categories = _sessionService.GetAllCategoriesForSelect();
            // Map categories to SelectList (Id as Value, Name as Text)
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            // Retrieve trainers for selection
            var trainers = _sessionService.GetAllTrainersForSelect();
            // Map trainers to SelectList (Id as Value, Name as Text)
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }

        #endregion


    }
}
