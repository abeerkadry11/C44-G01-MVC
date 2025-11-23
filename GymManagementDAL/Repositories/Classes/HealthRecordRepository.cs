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
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext dbContext;

        public HealthRecordRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<HealthRecord> GetAll() => dbContext.HealthRecords.ToList();

        public HealthRecord? GetById(int Id) => dbContext.HealthRecords.Find(Id);

        public int Add(HealthRecord healthRecord)
        {
            dbContext.HealthRecords.Add(healthRecord);
            return dbContext.SaveChanges();
        }

        public int Update(HealthRecord healthRecord)
        {
            dbContext.HealthRecords.Update(healthRecord);
            return dbContext.SaveChanges();
        }

        public int Delete(HealthRecord healthRecord)
        {
            dbContext.HealthRecords.Remove(healthRecord);
            return dbContext.SaveChanges();
        }   
    }
}
