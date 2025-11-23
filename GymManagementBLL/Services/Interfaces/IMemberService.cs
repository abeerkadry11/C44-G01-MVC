using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

         bool CreateMember(CreateMemberViewModel createMemberViewModel);

        MemberViewModel? GetMemberDetails(int MemberId);

        HealthRecordViewModel? GetHealthRecordDetails(int MemberId);

        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);

        // Id For Fetch The Member From DataBase To Update 
        // And Ensure That This Id Exists And He Didn't Change Id When Updating The Data
        bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel updatedMember);

        bool RemoveMember(int MemberId);
    }
}
