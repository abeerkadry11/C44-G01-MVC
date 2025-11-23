using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : ISessionRepository
    {

        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Add(Session session)
        {
            dbContext.Sessions.Add(session);
            return dbContext.SaveChanges();
        }

        public int Delete(Session session)
        {
            dbContext.Sessions.Remove(session);
            return dbContext.SaveChanges();
        }

        public IEnumerable<Session> GetAll() => dbContext.Sessions.ToList();

        public Session? GetById(int Id) => dbContext.Sessions.Find(Id);

        public int Update(Session session)
        {
            dbContext.Sessions.Update(session);
            return dbContext.SaveChanges();
        }
    }
}
