using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberShipViewModel;
using GymManagmentDAL.Entites;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class MemberShipController : Controller
    {
        private readonly IMemberShipServices _memberShipService;

        public MemberShipController(IMemberShipServices memberShipService)
        {
            _memberShipService = memberShipService;
        }
        public IActionResult Index()
        {
            var memberShips = _memberShipService.GetActiveMembersAndPlans();
            return View(memberShips);
        }

        [HttpPost]
        public ActionResult Cancel(int memberId, int planId)
        {
            var result = _memberShipService.CancelMemberShip(memberId, planId);
            if (result)
            {
                TempData["Success"] = "Membership cancelled successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to cancel membership.";
            }

            return RedirectToAction(nameof(Index));


        }

        public IActionResult Create()
        {
            return View(new CreateMemberShipViewModel
            {
                Members = _memberShipService.GetAllMemberforSelect(),
                Plans = _memberShipService.GetplansForSelect()
            });
        }

        [HttpPost]

        public IActionResult Create(CreateMemberShipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Members = _memberShipService.GetAllMemberforSelect();
                model.Plans = _memberShipService.GetplansForSelect();
                return View(model);
            }

            var result = _memberShipService.CreateMemberShip(model);

            if (!result)
            {
                TempData["Error"] = "Failed to create membership.";
                return RedirectToAction(nameof(Create));
            }

            TempData["Success"] = "Membership created successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
