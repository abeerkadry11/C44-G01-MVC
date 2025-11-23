using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // JoinDate == CreatedAt Of BaseEntity
        public string Photo { get; set; } = null!;

        #region Member - HealthRecord 

        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion

        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        public ICollection<MemberSession> MemberSessions   { get; set; } = null!;


    }
}
