using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService _memberService)
        {
            memberService = _memberService;
        }
        #region Get All Members

        public ActionResult Index()
        {
            var members = memberService.GetAllMembers();
            // Model Will Bind To Be Strongly Typed View
            return View(members);
        }

        #endregion

        #region Get Member Data

        // BaseUrl/Member/MemberDetails -> id = 0
        // BaseUrl/Member/MemberDetails/1 -> id = 1
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = memberService.GetMemberDetails(id);

            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found ";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);

        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var HealthRecord = memberService.GetHealthRecordDetails(id);
            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found ";
                return RedirectToAction(nameof(Index));
            }
            return View(HealthRecord);
        }
        #endregion

        #region Create Member

        //[HttpGet] // By Default
        // BaseUrl/Member/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createdMember)
        {
            // TO: Validate Created Member Data
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(Create), createdMember);
            }

            var result = memberService.CreateMember(createdMember);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Creation Failed , Check Phone And Email";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Member

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found ";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public ActionResult MemberEdit(  [FromRoute] int Memberid , MemberToUpdateViewModel UpdatedMember )
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Check The Data";
                //return View(nameof(MemberEdit) , UpdatedMember);
                return View( UpdatedMember);
            }

            var result = memberService.UpdateMemberDetails(Memberid , UpdatedMember);

            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Update Failed ";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Member
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = memberService.GetMemberDetails(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = member.Name;
            return View();

        }

        [HttpPost]
        public ActionResult DeleteConfirmed( [FromForm] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Check The Data";
                return View(nameof(Delete));
            }

            var result = memberService.RemoveMember(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Delete Member Failed ";
            }
            return View(nameof(Index));
        }

        #endregion

    }
}
