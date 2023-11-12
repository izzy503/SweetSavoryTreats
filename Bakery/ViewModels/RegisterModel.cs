using System.ComponentModel.DataAnnotations;

namespace SweetSavoryTreats.ViewModels
{
  public class RegisterModels
  {

    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Your password must contain at least s, a capital letter, a lowercase letter, eight characters a number, and a special character.")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password is incorrect.")]
    public string ConfirmPassword { get; set; }

    [Required]
    [Display(Name = "First and Last Name")]
    public string DisplayName { get; set; }

    [Required]
    public string FullName { get; set; }
  }
}