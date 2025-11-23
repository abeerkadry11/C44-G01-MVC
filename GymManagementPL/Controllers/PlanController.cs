using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService planService;

        public PlanController(IPlanService _planService)
        {
            planService = _planService;
        }

        // Index
        public ActionResult Index()
        {
            var plans = planService.GetAllPlans();
            return View(plans);
        }

        // Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }

            var plan = planService.GetPlanId(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // GET : Edit
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = planService.GetPlanToUpdate(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        // POST : Edit
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatedplan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Please Correct All Errors");
                return View(updatedplan);
            }
            var result = planService.UpdatePlan(id, updatedplan);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Plan , Please Try Again";
            }
            return RedirectToAction(nameof(Index));
        }

        // Toggle
        [HttpPost]
        public ActionResult Toggle([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var result = planService.ToggleStatus(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Change Plan Status";
            }
            return RedirectToAction(nameof(Index));

        }
    }
}