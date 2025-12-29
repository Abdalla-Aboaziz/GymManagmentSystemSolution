using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
          _planService = planService;
        }
        public ActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan ID.";

                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";

                return RedirectToAction(nameof(Index));
            }

            return View(plan);


        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan ID.";

                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanToUpdate(id);
            if (Plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found To Update";
                return RedirectToAction(nameof(Index));
            }

            return View(Plan);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id,UpdatePlanViewModel updatePlan) { 
        
            if (!ModelState.IsValid)
            {
               ModelState.AddModelError("WrongData", "Check Data  And Missing Field");
                return View( updatePlan);
            }

            var result = _planService.UpdatePlan( id, updatePlan);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating the Plan";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Activate (int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan ID.";
                return RedirectToAction(nameof(Index));
            }
            var result = _planService.ToggleState(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Status  Changed Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while Change Status of the Plan";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
