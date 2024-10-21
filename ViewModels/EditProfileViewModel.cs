using System;
using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Birthdate is required")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; } // Nullable for optional cases

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; } // Assuming Gender is an enum
    }
}
