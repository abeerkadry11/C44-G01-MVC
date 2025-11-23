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
    public class MemberRepository : IMemberRepository
    {
        //private readonly GymDbContext dbContext = new GymDbContext();

        private readonly GymDbContext dbContext;

        //// Ask CLR To Inject Object From GymDbContext
        //public MemberRepository(GymDbContext _dbContext)
        //{
        //    dbContext = _dbContext;
        //}
        // Ask CLR To Inject Object From GymDbContext


        // Now Object Is Injected , Not Created Manually
        public MemberRepository(GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public int Add(Member member)
        {
            dbContext.Members.Add(member);
            return dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var member = dbContext.Members.Find(id);

            if (member is null) return 0;

            dbContext.Members.Remove(member);
            return dbContext.SaveChanges();
        }

        public IEnumerable<Member> GetAll() =>  dbContext.Members.ToList() ;

        public Member? GetByID(int Id)
        {
            return dbContext.Members.Find(Id);
        }

        public int Update(Member member)
        {

            dbContext.Members.Update(member);
            return dbContext.SaveChanges();
        }
    }
}
