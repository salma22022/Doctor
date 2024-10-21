namespace Project.Models
{
    public class Phone
    {
        public int PhoneId { get; set; } // Primary Key
        public string Number { get; set; }

        // Foreign Key to Clinic
        public Clinic Clinic { get; set; }

    }
}
