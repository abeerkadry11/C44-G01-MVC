using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = " Name Must Between 2 And 50")]
        // ^ Start
        // $ End
        // a-z A-Z Space
        // + One Or More 
        // * Zero Or More
        // ? Zero Or One
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = " Name Contains Letters And Spaces Only")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")] // Validation Attribute
        [DataType(DataType.EmailAddress)] // UI Hint
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = " Email Must Between 5 And 100")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Format")] // Validation Attribute
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Invalid Egyptian Phone Number")]
        [DataType(DataType.PhoneNumber)] // UI Hint
        [StringLength(maximumLength: 15, MinimumLength = 10, ErrorMessage = " Phone Must Between 10 And 15")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Date Of Birth Is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }


        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "BuildingNumber Is Required")]
        [Range(1,9000 , ErrorMessage = " BuildingNumber Must Be Between 1 and 9000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30 , MinimumLength = 2 , ErrorMessage = " Street Must Be Between 2 and 30")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30 , MinimumLength = 2 , ErrorMessage = " City Must Be Between 2 and 30")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = " City Contains Letters And Spaces Only")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "HealthRecordViewModel is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;

    }
}
