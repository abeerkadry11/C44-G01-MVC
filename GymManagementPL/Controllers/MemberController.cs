using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        public ActionResult Index(int id)
        {
            //return RedirectToAction("GetMembers")
            return RedirectToAction(nameof(GetMembers));
            return RedirectToRoute("Trainers" , new {action = "GetTrainer"});
        }
        public ActionResult GetMembers()
        {
            return View();
        }
        public ActionResult CreateMember()
        {
            return View();
        }

    }
}
