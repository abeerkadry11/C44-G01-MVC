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
    public class CategoryRepository: ICategoryRepository
    {
                private readonly GymDbContext dbContext;

        public CategoryRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Category> GetAll() => dbContext.Categories.ToList();

        public Category? GetById(int Id) => dbContext.Categories.Find(Id);

        public int Add(Category category)
        {
            dbContext.Categories.Add(category);
            return dbContext.SaveChanges();
        }

        public int Update(Category category)
        {
            dbContext.Categories.Update(category);
            return dbContext.SaveChanges();
        }

        public int Delete(Category category)
        {
            dbContext.Categories.Remove(category);
            return dbContext.SaveChanges();
        }
    }
}
