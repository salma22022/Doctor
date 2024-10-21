namespace Project.Models
{
    public class BlockOutHours
    {
        //[Key]
        public int BlockOutHoursId { get; set; } // Primary Key
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Foreign Key to Day
        public  Day Day { get; set; }

    }
}
