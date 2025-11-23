using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    // ISessionRepository Has 7 Signatures
    // 5 From IGenericRepository 
    // 2 From ISessionRepository
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        int GetCountOfBookingSlots(int SessionId);

        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
