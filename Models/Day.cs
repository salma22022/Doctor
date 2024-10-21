namespace Project.Models
{
    public class Day
    {
        //[Key]
        public int DayId { get; set; } // Primary Key
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int WaitingPeriod { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string DayName { get; set; }

        // Foreign Key to Doc

        public Doctor Doc { get; set; }

        // Navigation Properties
        public  ICollection<Booking> Bookings { get; set; }
        public ICollection<BlockOutHours> BlockOutHours { get; set; }

    }
}
