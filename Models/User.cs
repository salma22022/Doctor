using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        // public string Password { get; set; } //// using the password property of IdentityUser
        // public string Email { get; set; } //// using the email property of IdentityUser
        // public string Phone { get; set; } //// using the phone property of IdentityUser
        // public string Type { get; set; } // e.g., Doctor, Patient
        public Gender Gender { get; set; }
        public byte[]? Picture { get; set; }
        public DateTime Db { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public Doctor Doc { get; set; }

    }

}
public enum Gender
{
    Male,
    Female
}