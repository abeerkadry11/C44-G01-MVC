using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        // GetAll
        IEnumerable<Member> GetAll();

        // Get By Id
        Member? GetByID(int Id);

        // Add
        int Add(Member member);

        // Upadte
        int Update(Member member);  

        // Delete
        int Delete(int id);
    }
}
