using System.ComponentModel.DataAnnotations;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage ="Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage ="Password is required")]
    [DataType(DataType.Password)]
    [StringLength(40,MinimumLength = 8,ErrorMessage ="Password must be between 8 and 40 character")]
    [Compare("ConfirmNewPassword",ErrorMessage ="Password doesn't match")]
    [Display(Name ="New password")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage ="Password confirmation is required")]
    [DataType(DataType.Password)]
    [Compare("NewPassword",ErrorMessage ="Password doesn't match")]
    [Display(Name ="Confirm new password")]
    public string ConfirmNewPassword { get; set; }
}