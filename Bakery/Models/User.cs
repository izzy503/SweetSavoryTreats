using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SweetSavoryTreats.Models
{
  public class User : IdentityUser
  {
    [Required(ErrorMessage = "Please enter your first and last name.")]
    public string DisplayName { get; set; }
  }
}