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
            LoadDropdownsCategory();
            LoadDropdownsTrainer();
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
                 LoadDropdownsCategory();
                LoadDropdownsTrainer();
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
                LoadDropdownsCategory();
                LoadDropdownsTrainer();
                return View(createSession);
            }
        }
        public ActionResult Edit (int id)
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

            LoadDropdownsTrainer();


            return View(session);
        }

        [HttpPost]
        public ActionResult Edit ([FromRoute]int id, UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdownsTrainer();
                return View(updateSession);
            }
            var result = _sessionService.UpdateSession( id, updateSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
               
            }
            else
            {
                TempData["ErrorMessage"] = "Session Update Failed";
              
            }

            return RedirectToAction(nameof(Index));
        }
        
        public ActionResult Delete(int id)
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
            ViewBag.SessionId = session.Id;
            return View(session);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Deletion Failed";
            }
            return RedirectToAction(nameof(Index));
        }


        #region Helper Method
        // Two Helper methods used to load dropdown list data
        // for Categories and Trainers.
        // Called in both GET and POST actions when needed.
        private void LoadDropdownsCategory()
        {
            // Retrieve categories for selection
            var categories = _sessionService.GetAllCategoriesForSelect();
            // Map categories to SelectList (Id as Value, Name as Text)
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
           
        }

        private void LoadDropdownsTrainer()
        {
            // Retrieve trainers for selection
            var trainers = _sessionService.GetAllTrainersForSelect();
            // Map trainers to SelectList (Id as Value, Name as Text)
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }

        #endregion


    }
}
