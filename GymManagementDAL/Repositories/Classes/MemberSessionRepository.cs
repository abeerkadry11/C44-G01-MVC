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
    public class MemberSessionRepository: IMemberSessionRepository
    {
        private readonly GymDbContext dbContext;

        public MemberSessionRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Add(MemberSession memberSession)
        {
            dbContext.MemberSessions.Add(memberSession);
            return dbContext.SaveChanges();
        }

        public int Delete(MemberSession memberSession)
        {
            dbContext.MemberSessions.Remove(memberSession);
            return dbContext.SaveChanges();
        }

        public IEnumerable<MemberSession> GetAll() => dbContext.MemberSessions.ToList();

        public MemberSession? GetById(int Id) => dbContext.MemberSessions.Find(Id);

        public int Update(MemberSession memberSession)
        {
            dbContext.MemberSessions.Update(memberSession);
            return dbContext.SaveChanges();
        }
    }
}
