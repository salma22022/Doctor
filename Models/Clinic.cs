namespace Project.Models
{
    public class Clinic
    {
        //[Key]
        public int ClinicId { get; set; } // Primary Key
        public string Name { get; set; }
        public double Price { get; set; }
        public Location? Location { get; set; }

        // Navigation Properties
        public Doctor doctor { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public ICollection<ClinicInsurance>? ClinicInsurances { get; set; }

    }
}
