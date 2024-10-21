namespace Project.Models
{
    public class Insurance
    {
        //[Key]
        public int InsuranceId { get; set; } // Primary Key
        public string Name { get; set; }


        public ICollection<ClinicInsurance> ClinicInsurances { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
