using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public abstract class BaseEnitiy
    {
        public int Id { get; set; }
        // Udit Cols
        public DateTime CreatedAt { get; set; }

        // Must Put Date For Creation
        // But Not Must Put Update 
        public DateTime? UpdatedAt { get; set; }
    }
}
