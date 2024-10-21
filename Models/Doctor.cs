using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class Doctor
    {
        // Primary Key
        public string DoctorId { get; set; }
        public string Bio { get; set; }
        public string LicenseNumber { get; set; }

        [ForeignKey(nameof(Specialization))]
        public int SpecializationId { get; set; } // Add this line
        public virtual Specialization Specialization { get; set; }

        // Foreign Key to Clinic
        [ForeignKey(nameof(Clinic))]
        public int ClinicId { get; set; } // Foreign key to Clinic

        public virtual Clinic Clinic { get; set; }

        // Foreign Key to User
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } // Foreign key to User

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual ICollection<Day> Days { get; set; } = new List<Day>(); // Initialize to avoid null reference
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>(); // Initialize to avoid null reference
    }
}
