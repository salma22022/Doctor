using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Booking
    {
        public int BookingId { get; set; } // Primary Key
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        public string Status { get; set; } = "pending"; // Default value = 'pending'
        public string CancelReasons { get; set; } // Optional cancellation reasons
        public string CanceledBy { get; set; } // Optional who canceled


        // Foreign Key to User
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
        // Foreign Key to Day
        [ForeignKey(nameof(Day))]
        public int? DayId { get; set; }
        public Day Day { get; set; }

        // Foreign Key to Insurance
        [ForeignKey(nameof(Insurance))]
        public int? InsuranceId { get; set; }
        public Insurance? Insurance { get; set; }

    }
}
