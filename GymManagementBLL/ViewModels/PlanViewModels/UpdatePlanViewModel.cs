using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        [Required(ErrorMessage = "Plan Name Is Required")]
        [StringLength(maximumLength: 50, ErrorMessage = "Plan Name Must Be Less Than 51 Characters")]
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description Name Is Required")]
        [StringLength(maximumLength: 200, MinimumLength = 5 ,ErrorMessage = "Description Must Be Between 5 and 200 Characters")]
        public string Description { get; set; } = null!;

        [Range(1,365 , ErrorMessage = "Duration Must Be Between 1 and 365 Days")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Is Required")]
        [Range(0.1, 10000, ErrorMessage = "Price Must Be Between 0.1 and 10000")]
        public decimal Price { get; set; }


    }
}
