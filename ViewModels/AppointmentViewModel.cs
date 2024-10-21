using System;
using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class AppointmentViewModel
    {
        [Required(ErrorMessage = "Patient name is required.")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime AppointmentDate { get; set; }
    }
}
