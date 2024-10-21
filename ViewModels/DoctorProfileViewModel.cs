using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class DoctorProfileViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public Gender Gender { get; set; }
        public string LicenseNumber { get; set; }
        public string SpecializationName { get; set; }
        public string ClinicName { get; set; }
        public double ClinicPrice { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string GpsLoc { get; set; }

    }

}
