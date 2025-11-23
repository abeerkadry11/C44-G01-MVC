using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {

        [Required(ErrorMessage = "Height Is Required")]
        [Range(0.1,300 , ErrorMessage = "Height Must Between 0 and 300")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight Is Required")]
        [Range(0.1,500 , ErrorMessage = "Weight Must Between 0 and 500")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "BloodType Is Required")]
        [StringLength(3, ErrorMessage = "BloodType Must Between 3 Chars")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }

    }
}
