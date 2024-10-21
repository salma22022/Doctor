using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Review
    {
        //  [Key]
        public int ReviewId { get; set; } // Primary Key
        public int Rate { get; set; }
        public string Context { get; set; }

        // Foreign Key to User
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public User User { get; set; }

        // Foreign Key to Doc
        [ForeignKey(nameof(Doctor))]
        public string? DoctorId { get; set; }
        public Doctor Doc { get; set; }

    }
}
