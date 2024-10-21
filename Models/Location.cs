namespace Project.Models
{
    public class Location
    {
        //  [Key]
        public int LocationId { get; set; } // Primary Key
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string GpsLoc { get; set; }

    }
}
