namespace MossadAgenseMvc.Models
{
    public class TargetModel
    {
        public enum StatusTarget
        {
            Alive,
            Active,
            dead
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Gob { get; set; }
        public int Location_X { get; set; } = -1;
        public int Location_Y { get; set; } = -1;
        public StatusTarget Status { get; set; } = StatusTarget.Alive;
    }
}
