using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService _trainerService)
        {
            trainerService = _trainerService;
        }
        #region Get All Trainers
        public ActionResult Index()
        {
            var Trainers = trainerService.GetAllTrainers();
            return View(Trainers);
        }

        #endregion

        #region Get Trainer Data
        // Get : Trainer/Details/5
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can not Be 0 Or Negative";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = trainerService.GetTrainerDetails(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }

        #endregion

        #region Create Trainer

        // Get : Trainer/Create
        public ActionResult Create()
        {
            return View();
        }

        // Post : Trainer/Create
        [HttpPost]
        public ActionResult CreateConfirmed(CreateTrainerViewModel createdTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(Create), createdTrainer);
            }
            var result = trainerService.CreateTrainer(createdTrainer);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Creation Failed ";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit Trainer

        public ActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can not Be 0 Or Negative";
                return RedirectToAction(nameof(Index));
            }
            var trainer = trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public ActionResult TrainerEdit([FromRoute] int id, TrainerToUpdateViewModel updatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(TrainerEdit), updatedTrainer);
            }
            var result = trainerService.UpdateTrainerDetails(updatedTrainer, id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Update Failed ";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Trainer

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var trainer = trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed( [FromForm] int id)
        {
            var result = trainerService.RemoveTrainer(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Deletion Failed ";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

    }
}
