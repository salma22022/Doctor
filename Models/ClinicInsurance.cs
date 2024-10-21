namespace Project.Models
{
    public class ClinicInsurance
    {
        public int ClinicInsuranceId { get; set; }
        public Clinic Clinic { get; set; }
        public Insurance Insurance { get; set; }

    }
}
