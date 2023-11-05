using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SweetSavoryTreats.Models
{
  public class Treat
  {


    public int TreatId { get; set; }
    [Required(ErrorMessage = "Please submit a treat name.")]
    public string Name { get; set; }
    public List<TasteTreat> JoinEntities { get; set; }
    public User User { get; set; }
  }
}