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
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Add(Plan plan)
        {
            dbContext.Plans.Add(plan);
            return dbContext.SaveChanges();
        }

        public int Delete(Plan plan)
        {
            dbContext.Plans.Remove(plan);
            return dbContext.SaveChanges();
        }

        public IEnumerable<Plan> GetAll() => dbContext.Plans.ToList();

        public Plan? GetById(int Id) => dbContext.Plans.Find(Id);

        public int Update(Plan plan)
        {
            dbContext.Plans.Update(plan);
            return dbContext.SaveChanges();
        }
    }
}
