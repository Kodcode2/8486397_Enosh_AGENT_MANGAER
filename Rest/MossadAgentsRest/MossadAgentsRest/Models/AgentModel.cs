namespace MossadAgentsRest.Models
{
    public class AgentModel
    {
        public int Id { get; set; }

        public string? Image {  get; set; }
        public string? Name { get; set; }
        public int LocationX { get; set; } = -1;
        public int LocationY { get; set; } = -1;
        public enum Status
        {
            Sleep,
            Active
        }
    }
}
