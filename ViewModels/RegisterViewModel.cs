using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage ="Name is required")] // name
    public string Name { get; set; }

    [Required(ErrorMessage ="Email is required")] // email
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage ="Phone number is required")] // phone
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Birthdate is required")] // birthdate
    [DataType(DataType.Date)]
    public DateTime Birthdate { get; set; }

    [Required(ErrorMessage = "Gender is required")] // gender
    public Gender Gender { get; set; }
    
    
    [Required(ErrorMessage ="Password is required")] // password
    [DataType(DataType.Password)]
    [StringLength(40,MinimumLength = 8,ErrorMessage ="Password must be between 8 and 40 character")]
    [Compare("ConfirmPassword",ErrorMessage ="Password doesn't match")]
    public string Password { get; set; }

    [Required(ErrorMessage ="Password confirmation is required")] //confirm password
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage ="Password doesn't match")]
    [Display(Name ="Confirm password")]
    public string ConfirmPassword { get; set; }

}
