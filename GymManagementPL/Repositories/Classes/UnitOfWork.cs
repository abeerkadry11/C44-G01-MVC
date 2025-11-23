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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> repositories = new();
        private readonly GymDbContext dbContext;

        public UnitOfWork(GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // To Avoid Creating Object EveryTime You Can Implement Caching Here
            // Key -> Type [Member]
            // Object -> New GenericRepository<Member>()     

            var EntityType = typeof(TEntity);
            //if(repositories.ContainsKey(EntityType))                             
            //{
            //    return (IGenericRepository<TEntity>)repositories[EntityType];
            //}

            // If Found Return It
            if (repositories.TryGetValue(EntityType, out var repo))
            {
                return (IGenericRepository<TEntity>)repo;
            }

            // Not Found Create It
            var newRepo = new GenericRepository<TEntity>(dbContext);
            repositories[EntityType] = newRepo;
            return newRepo;
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges() ;
        }
    }
}
