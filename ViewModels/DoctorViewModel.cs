namespace Project.ViewModels
{
    public class DoctorViewModel
    {
        public string Bio { get; set; }
        public string DoctorName { get; set; }
        public string ClinicName { get; set; }
        public decimal ClinicPrice { get; set; }
        public string ClinicCity { get; set; }
        public string SpecializationName { get; set; }
        public List<string> InsuranceAccepted { get; set; }
    }
}