using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                // Check Trainer Exists
                // Check Category Exists
                // Check If StartDate Is Before EndDate
                if (!IsTrainerExists(createdSession.TrainerId) || !IsCategoryExists(createdSession.CategoryId) || !IsValidTime(createdSession.StartDate, createdSession.EndDate))
                    return false;

                // Capacity 0 - 25
                if (createdSession.Capacity > 25) return false;

                //mapper.Map<CreateSessionViewModel, Session>(createdSession); 
                // Or You Can Specify The Destination , He Will Detect The Source From The Passing Value
                var SessionEntity = mapper.Map<Session>(createdSession);
                unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Session Failed: {ex}");
                return false;
            }

        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            //var Sessions = unitOfWork.GetRepository<Session>().GetAll();
            var Sessions = unitOfWork.sessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!Sessions.Any()) return Enumerable.Empty<SessionViewModel>();

            //return Sessions.Select(S => new SessionViewModel()
            //{
            //    Id = S.Id,
            //    Description = S.Description,
            //    StartDate = S.CreatedAt,
            //    EndDate = S.EndDate,
            //    Capacity = S.Capacity,
            //    TrainerName = S.SessionTrainer.Name, // Related Data [Now Loaded]
            //    CategoryName = S.SessionCategory.CategoryName,// Related Data [Now Loaded]
            //    //AvailableSlots = Computed [Capacity - Count Of Bookings For Session]
            //    AvailableSlots = S.Capacity - unitOfWork.sessionRepository.GetCountOfBookingSlots(S.Id)
            //});

            var MappedSession = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            // Don't Forget AvailableSlots That You Ignore It In Profile
            foreach (var session in MappedSession)
            {
                session.AvailableSlots = session.Capacity - unitOfWork.sessionRepository.GetCountOfBookingSlots(session.Id);
            }

            return MappedSession;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var Session = unitOfWork.sessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (Session is null) return null;

            //// Manual Mapping
            //return new SessionViewModel()
            //{
            //    Description = Session.Description,
            //    TrainerName = Session.SessionTrainer.Name, // Loaded [ Include() ]
            //    CategoryName = Session.SessionCategory.CategoryName, // Loaded [ Include() ]
            //    Capacity = Session.Capacity, 
            //    StartDate = Session.CreatedAt,
            //    EndDate = Session.EndDate,
            //    AvailableSlots = Session.Capacity - unitOfWork.sessionRepository.GetCountOfBookingSlots( Session.Id),
            //};

            var MappedSession = mapper.Map<Session, SessionViewModel>(Session);

            MappedSession.AvailableSlots = MappedSession.Capacity - unitOfWork.sessionRepository.GetCountOfBookingSlots(MappedSession.Id);

            return MappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = unitOfWork.GetRepository<Session>().GetById(sessionId);
            // Completed || Started || HasActiveBokkings => Not To Update
            if (!IsSessionAvailableToUpdate(Session!)) return null;

            return mapper.Map<UpdateSessionViewModel>(Session);

        }

        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try
            {

                var Session = unitOfWork.GetRepository<Session>().GetById(sessionId);
                if (!IsSessionAvailableToUpdate(Session!)) return false;
                if (!IsTrainerExists(updatedSession.TrainerId) || !IsValidTime(updatedSession.StartDate, updatedSession.EndDate))
                    return false;

                //mapper.Map<Session>(updatedSession);
                mapper.Map(updatedSession, Session);
                // Audit
                Session!.UpdatedAt = DateTime.Now;

                unitOfWork.sessionRepository.Update(Session);
                return unitOfWork.SaveChanges() > 0;



            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool RemoveSession(int sessionId)
        {
            var Session = unitOfWork.sessionRepository.GetById(sessionId);
            if (Session is null) return false;

            if (IsSessionAvailableToRemove(Session))
                return false;

            unitOfWork.sessionRepository.Delete(Session);
            return unitOfWork.SaveChanges() > 0;

        }




        #region Helper Method

        private bool IsTrainerExists(int trainerId)
        {
            return unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }
        private bool IsCategoryExists(int CategoryId)
        {
            return unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }
        private bool IsValidTime(DateTime Start, DateTime End)
        {
            return Start < End;
        }

        private bool IsSessionAvailableToUpdate(Session session)
        {
            // Completed
            if (session.CreatedAt < DateTime.Now) return false;
            // Started
            if (session.CreatedAt <= DateTime.Now) return false;
            // Active Bookings
            // Use Count Of Bookings
            var HasActiveBookings = unitOfWork.sessionRepository.GetCountOfBookingSlots(session.Id) > 0;
            if (HasActiveBookings) return false;

            return true;

        }
        private bool IsSessionAvailableToRemove(Session session)
        {
            // Started
            if (session.CreatedAt <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            // UpComing
            if (session.CreatedAt > DateTime.Now) return false;

            // Active Bookings
            // Use Count Of Bookings
            var HasActiveBookings = unitOfWork.sessionRepository.GetCountOfBookingSlots(session.Id) > 0;
            if (HasActiveBookings) return false;

            return true;

        }


        #endregion


    }
}