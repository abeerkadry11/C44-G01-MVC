using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService _sessionService)
        {
            sessionService = _sessionService;
        }

        #region Get All Sessions
        public ActionResult Index()
        {
            var sessions = sessionService.GetAllSessions();
            return View(sessions);
        }


        #endregion

        #region Get Session Data

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }
            var session = sessionService.GetSessionById(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        #endregion

        #region Create Session

        public ActionResult Create()
        {
            LoadDropDownListCategories();
            LoadDropDownListTrainers();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createdSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownListCategories();
                LoadDropDownListTrainers();
                return View(createdSession);
            }
            var isCreated = sessionService.CreateSession(createdSession);
            if (isCreated)
            {
                TempData["SuccessMessage"] = "Session Created Successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Create Session Failed.";
                LoadDropDownListCategories();
                LoadDropDownListTrainers();
                return View(createdSession);
            }
        }
        #endregion

        // ${"form}.removeData("validator) => To Clear The Validation Errors From The Form [Client Side Validations]

        #region Edit Session

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }
            var sessionToUpdate = sessionService.GetSessionToUpdate(id);
            if (sessionToUpdate == null)
            {
                TempData["ErrorMessage"] = "Session Is Not Found Or You Can't Update It Now.";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownListTrainers();
            return View(sessionToUpdate);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel updatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownListTrainers();
                return View(updatedSession);
            }
            var isUpdated = sessionService.UpdateSession(updatedSession, id);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Update Session Failed.";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }
            var session = sessionService.GetSessionById(id);
            if (session is  null)
            {
                TempData["ErrorMessage"] = "Session Not Found.";
            return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = session.Id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed( int id)
        {
            var isDeleted = sessionService.RemoveSession(id);
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Delete Session Failed.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helper

        private void LoadDropDownListTrainers()
        {
            var Trainers = sessionService.GetTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadDropDownListCategories()
        {
            var Categories = sessionService.GetCategoriesForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        #endregion

    }
}
