using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagmentBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
         _trainerService = trainerService;
        }
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainer();
            return View(trainers);
        }
        public IActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid trainer ID.";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            // Validate incoming model based on data annotation rules
            if (!ModelState.IsValid)
            {
                // Add a general validation error for missing or invalid data
                ModelState.AddModelError("DataMissed", "Check Data  And Missing Field");
                // Return the Create view with the same model
                // to display validation messages and preserve user input
                return View(nameof(Create), createTrainer);
            }
            // Call the service layer to create a new Trainer
            // Returns true if the operation succeeds, otherwise false
            bool result = _trainerService.CreateTrainer(createTrainer);
            if (result)
            {
                // Store success message to be displayed after redirect
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                // Store error message to be displayed after redirect
                TempData["ErrorMessage"] = "An error occurred while creating the Trainer";
            }
            // Redirect to Index action to prevent form resubmission
            return RedirectToAction(nameof(Index));
        }

        public ActionResult EditTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID.";

                return RedirectToAction(nameof(Index));
            }
            var trainerToUpdate = _trainerService.GetTrainerToUpdate(id);
            if (trainerToUpdate is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found To Update";
                return RedirectToAction(nameof(Index));
            }

            return View(trainerToUpdate);


        }

        [HttpPost]
        public ActionResult EditTrainer([FromRoute] int id, UpdateTrainerViewModel updateTrainer)
        {
            // Validate the incoming model using data annotation rules
            if (!ModelState.IsValid)
            {
                // Return the Edit view with the same model
                // to display validation errors and preserve user input
                return View(updateTrainer);
            }
            var result = _trainerService.UpdateTrainerDetails(updateTrainer, id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "An Error Occurred While Updating The Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID.";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "An Error Occurred While Deleting The Trainer";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId =trainer.Id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirm([FromForm] int id)
        {
            var result = _trainerService.RemoveTrainer(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "An Error Occurred While Deleting The Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
