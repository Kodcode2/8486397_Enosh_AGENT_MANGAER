namespace MossadAgentsRest.Models
{
    public class TargetModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Gob { get; set; }
        public int LocationX { get; set; } = -1;
        public int LocationY { get; set; } = -1;
        public enum Status
        {
            Alive,
            dead
        }
    }
}
