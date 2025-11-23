using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }


        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return dbContext.Sessions.Include(S => S.SessionTrainer)
                                     .Include(S => S.SessionCategory)
                                     .ToList();
        }

        public int GetCountOfBookingSlots(int SessionId)
        {
            return dbContext.MemberSessions.Count(X => X.SessionId == SessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return dbContext.Sessions
                            .Include(X => X.SessionTrainer)
                            .Include(X => X.SessionCategory)
                            .FirstOrDefault(X => X.Id == sessionId);
        }
    }
}
