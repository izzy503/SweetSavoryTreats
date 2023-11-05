namespace SweetSavoryTreats.Models
{
  public class TasteTreat
  {
    public int TasteTreatId { get; set; }
    public int TasteId { get; set; }
    public int TreatId { get; set; }
    public Taste Taste { get; set; }
    public Treat Treat { get; set; }
  }
}