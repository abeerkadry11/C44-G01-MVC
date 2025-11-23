using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.AnalyticsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public AnalyticsViewModel GetAnalyticsData()
        {
            // Only One Request to fetch all sessions
            var Sessions = unitOfWork.GetRepository<Session>().GetAll();
            return new AnalyticsViewModel
            {
                ActiveMembers = unitOfWork.GetRepository<MemberShip>().GetAll(X=>X.Status == "Active").Count(),
                TotalMembers = unitOfWork.GetRepository<MemberShip>().GetAll().Count(),
                TotalTrainers = unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                //UpcomingSessions = unitOfWork.sessionRepository.GetAll(s => s.CreatedAt > DateTime.Now).Count(),
                UpcomingSessions = Sessions.Where(s => s.CreatedAt > DateTime.Now).Count(),
                OngoingSessions = Sessions.Where(s => s.CreatedAt <= DateTime.Now && s.EndDate >= DateTime.Now).Count(),
                CompletedSessions = Sessions.Where(s => s.EndDate < DateTime.Now).Count()
            };
        }
    }
}
