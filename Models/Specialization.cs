namespace Project.Models
{
    public class Specialization
    {
        //[Key]
        public int SpecializationId { get; set; } // Primary Key
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<Doctor> Doctors { get; set; }

    }
}

public enum SpecializationName
{
General_Practice_Family_Medicine,
Internal_Medicine,
Pediatrics,
Cardiology,
Endocrinology,
Gastroenterology,
Dermatology,
Neurology,
Orthopedics,
Psychiatry,
Ophthalmology,
Otolaryngology_ENT,
Urology,
Nephrology,
Oncology,
Rheumatology,
Pulmonology,
Obstetrics_and_Gynecology_OB_GYN,
Anesthesiology,
Radiology,
Surgery,
Geriatrics,
Hematology
}
