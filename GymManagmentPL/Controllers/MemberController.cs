using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public ActionResult Index()
        {
            var member = _memberService.GetAllMember();
            return View(member);
        }
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID.";

                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";

                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID.";

                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";

                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            // Validate incoming model based on data annotation rules
            if (!ModelState.IsValid)
            {
                // Add a general validation error for missing or invalid data
                ModelState.AddModelError("DataMissed", "Check Data  And Missing Field");
                // Return the Create view with the same model
                // to display validation messages and preserve user input
                return View(nameof(Create), createMember);
            }
            // Call the service layer to create a new member
            // Returns true if the operation succeeds, otherwise false
            bool result = _memberService.CreateMember(createMember);
            if (result)
            {
                // Store success message to be displayed after redirect
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                // Store error message to be displayed after redirect
                TempData["ErrorMessage"] = "An error occurred while creating the member";
            }
            // Redirect to Index action to prevent form resubmission
            return RedirectToAction(nameof(Index));
        }

        public ActionResult EditMember(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID.";

                return RedirectToAction(nameof(Index));
            }
            var memberToUpdate = _memberService.GetMemberToUpdate(id);
            if (memberToUpdate is null)
            {
                TempData["ErrorMessage"] = "Member Not Found To Update";
                return RedirectToAction(nameof(Index));
            }

            return View(memberToUpdate);


        }

        [HttpPost]
        public ActionResult EditMember([FromRoute] int id, MemberToUpdateViewModel memberToUpdate)
        {
            // Validate the incoming model using data annotation rules
            if (!ModelState.IsValid)
            {
                // Return the Edit view with the same model
                // to display validation errors and preserve user input
                return View(memberToUpdate);
            }
            var result = _memberService.UpdateMember(id, memberToUpdate);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "An Error Occurred While Updating The Member";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteMember(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID.";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "An Error Occurred While Deleting The Member";
            }
          ViewBag.MemberId= id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirm ([FromForm]int id)
        {
            var result = _memberService.RemoveMember(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "An Error Occurred While Deleting The Member";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
