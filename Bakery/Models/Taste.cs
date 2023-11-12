using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SweetSavoryTreats.Models
{
  public class Taste
  {
    public int TasteId { get; set; }
    [Required(ErrorMessage = "Please enter a Taste of Choice.")]
    public string Type { get; set; }
    public List<TasteTreat> JoinEntities { get; set; }
    public User User { get; set; }
  }
}