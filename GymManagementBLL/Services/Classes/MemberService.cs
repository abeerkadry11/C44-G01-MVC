using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        #region Without UnitOfWork
        //private readonly IGenericRepository<Member> memberRepository;
        //private readonly IGenericRepository<MemberShip> memberShipRepository;
        //private readonly IPlanRepository planRepository;
        //private readonly IGenericRepository<HealthRecord> healthRecordRepo;
        //private readonly IGenericRepository<MemberSession> memberSessionRepository;

        //// Ask CLR For Creating Object From Service 
        //// CLR Will Inject Address Of Object In Constructor 

        //public MemberService(IGenericRepository<Member> _memberRepository
        //    , IGenericRepository<MemberShip> _memberShipRepository
        //    , IPlanRepository _planRepository,
        //    IGenericRepository<HealthRecord> _healthRecordRepo,
        //    IGenericRepository<MemberSession> _memberSessionRepository)
        //{
        //    memberRepository = _memberRepository;
        //    this.memberShipRepository = _memberShipRepository;
        //    planRepository = _planRepository;
        //    healthRecordRepo = _healthRecordRepo;
        //    memberSessionRepository = _memberSessionRepository;
        //}

        #endregion

        public MemberService(IUnitOfWork _unitOfWork , IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }


        public bool CreateMember(CreateMemberViewModel createMemberViewModel)
        {
            try
            {
                // Look At Business Rules First When Create Or Insert Or Update

                // Email And Phone Must Be Unique And Valid -- Valid Done
                //  Egyptian Phone Number -- Done
                // HealthRecord Are Required -- Done

                // Check If Email Or Phone Already Exist In Database
                // If One Of Them Exists Return False
                // If Not Create Member Object And Fill It From ViewModel return True If Added

                // Go Make GetAll Accept Func
                //var EmailExists = memberRepository.GetAll(m => m.Email == createMemberViewModel.Email).Any();
                //var PhoneExists = memberRepository.GetAll(m => m.Phone == createMemberViewModel.Phone).Any();
                //if (PhoneExists || EmailExists) return false;

                if (IsEmailExists(createMemberViewModel.Email , createMemberViewModel.Name) || IsPhoneExists(createMemberViewModel.Phone , createMemberViewModel.Name)) return false;


                var member = mapper.Map<Member>(createMemberViewModel);

                // Add Member To Database
                unitOfWork.GetRepository<Member>().Add(member); // Added Locally
                // return memberRepository.Add(member) > 0 ;
                return unitOfWork.SaveChanges() > 0;
            }
            catch   
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = unitOfWork.GetRepository<Member>().GetAll() ?? [];
            // Or 
            if (Members is null || !Members.Any()) return Enumerable.Empty<MemberViewModel>();

            // return Members   Error You Need MemberViewModel Not Member    


            var memberViewModels = mapper.Map<IEnumerable<MemberViewModel>>(Members);

            return memberViewModels;


        }

        public HealthRecordViewModel? GetHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;


            return mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return null;
            var MemberViewModel = mapper.Map<MemberViewModel>(member);

            // Active MemberShip
            // [EndDate - CreatedAt]
            // You Need To Use MemberShipRepository
            var memberShip = unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemberId == member.Id && X.Status == "Active")
                                                    .FirstOrDefault();
            if (memberShip is not null)
            {
                MemberViewModel.MemeberShipStartDate = memberShip.CreatedAt.ToShortDateString();
                MemberViewModel.MemeberShipEndDate = memberShip.EndDate.ToShortDateString();

                // Active Plan
                // [Plan Name]
                // You Need To Use PlanRepository
                var Plan = unitOfWork.GetRepository<Plan>().GetById(memberShip.PlanId);
                MemberViewModel.PlanName = Plan?.Name;

            }
            return MemberViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return null;

            return mapper.Map<MemberToUpdateViewModel>(member);
        }


        public bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel updatedMember)
        {
            try
            {
                //// There Is Logical Error Where He Will Search In The Members 
                //// And The Member It Self Has This Email And Phone
                //// So We Will Get All Members Except The Current Member ( X.Id != MemberId ) 
                //// Add It After
                //var EmailExists = memberRepository.GetAll(X=>X.Email == updatedMember.Email && X.Id != MemberId).Any();
                //var PhoneExists = memberRepository.GetAll(X=>X.Phone == updatedMember.Phone && X.Id != MemberId).Any();

                var EmailExists = unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Email == updatedMember.Email && X.Id != MemberId);
                var PhoneExists = unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Phone == updatedMember.Phone && X.Id != MemberId);

                if (EmailExists.Any() || PhoneExists.Any()) return false;

                //if (IsEmailExists(updatedMember.Email , updatedMember.Name) || IsPhoneExists(updatedMember.Phone , updatedMember.Name)) return false;

                var member = unitOfWork.GetRepository<Member>().GetById(MemberId);
                if (member is null) return false;

                mapper.Map(updatedMember, member);

                unitOfWork.GetRepository<Member>().Update(member);
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = unitOfWork.GetRepository<Member>();
            var Member = MemberRepo.GetById(MemberId);
            if (Member is null) return false;

            // Active Member Sessions
            // Look In MemberSession Table Where Member Id = MemberId
            // And The Start Date Of The Session <= Now 

            var SessionIds = unitOfWork.GetRepository<MemberSession>()
                .GetAll(ms => ms.MemberId == MemberId).Select(X=>X.SessionId); // 1 2 9

            var HasFutureSessions = unitOfWork.GetRepository<Session>()
                .GetAll(X => SessionIds.Contains(X.Id) && X.CreatedAt > DateTime.Now).Any();


            if (HasFutureSessions) return false;

            // Do Cascading Here
            var MemberShips = unitOfWork.GetRepository<MemberShip>().GetAll(ms => ms.MemberId == MemberId);
            try
            {
                if (MemberShips.Any())
                {
                    foreach (var memberShip in MemberShips)
                    {
                        unitOfWork.GetRepository<MemberShip>().Delete(memberShip);
                    }
                }
                MemberRepo.Delete(Member);
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }


        #region Helper Methods

        private bool IsEmailExists(string email , string name )
        {
            return unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email && m.Name != name).Any();
        }
        private bool IsPhoneExists(string phone , string name)
        {
            return unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone && m.Name != name).Any();
        }


        #endregion 

    }
}
