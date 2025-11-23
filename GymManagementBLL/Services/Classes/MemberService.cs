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

        public MemberService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
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

                if (IsEmailExists(createMemberViewModel.Email) || IsPhoneExists(createMemberViewModel.Phone)) return false;


                // Create Member Object
                var member = new Member
                {
                    Name = createMemberViewModel.Name,
                    Email = createMemberViewModel.Email,
                    Phone = createMemberViewModel.Phone,
                    Gender = createMemberViewModel.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createMemberViewModel.BuildingNumber,
                        City = createMemberViewModel.City,
                        Street = createMemberViewModel.Street
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Weight = createMemberViewModel.HealthRecordViewModel.Weight,
                        Height = createMemberViewModel.HealthRecordViewModel.Height,
                        BloodType = createMemberViewModel.HealthRecordViewModel.BloodType,
                        Note = createMemberViewModel.HealthRecordViewModel.Note
                    }
                };

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

            #region Manual Mapping

            //// [1]
            //var memberViewModels = new List<MemberViewModel>();
            //foreach (var m in Members)
            //{
            //    var memberViewModel = new MemberViewModel
            //    {
            //        Id = m.Id,
            //        Photo = m.Photo,
            //        Name = m.Name,
            //        Email = m.Email,
            //        Phone = m.Phone,
            //        Gender = m.Gender.ToString()
            //    };
            //    memberViewModels.Add(memberViewModel);
            //}

            // [2] -- Projection
            var memberViewModels = Members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Photo = m.Photo,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Gender = m.Gender.ToString()
            });
            #endregion

            return memberViewModels;


        }

        public HealthRecordViewModel? GetHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;

            var HealthRecordViewModel = new HealthRecordViewModel
            {
                Weight = MemberHealthRecord.Weight,
                Height = MemberHealthRecord.Height,
                BloodType = MemberHealthRecord.BloodType,
                Note = MemberHealthRecord.Note
            };
            return HealthRecordViewModel;
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return null;
            var MemberViewModel = new MemberViewModel
            {
                Id = member.Id,
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",

            };

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

            var memberToUpdateViewModel = new MemberToUpdateViewModel
            {
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street
            };

            return memberToUpdateViewModel;
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

                if (IsEmailExists(updatedMember.Email) || IsPhoneExists(updatedMember.Phone)) return false;

                var member = unitOfWork.GetRepository<Member>().GetById(MemberId);
                if (member is null) return false;

                member.Name = updatedMember.Name;
                member.Email = updatedMember.Email;
                member.Phone = updatedMember.Phone;
                member.Photo = updatedMember.Photo;
                member.Address.BuildingNumber = updatedMember.BuildingNumber;
                member.Address.City = updatedMember.City;
                member.Address.Street = updatedMember.Street;
                // Auditing
                member.UpdatedAt = DateTime.Now;

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

            var HasActiveMemberSessions = unitOfWork.GetRepository<MemberSession>().GetAll(ms => ms.MemberId == MemberId && ms.Session.CreatedAt > DateTime.Now)
                                                                   .Any();
            if (HasActiveMemberSessions) return false;

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

        private bool IsEmailExists(string email)
        {
            return unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExists(string phone)
        {
            return unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }


        #endregion 

    }
}
