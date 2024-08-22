namespace MossadAgenseMvc.Models
{
    public class AgentModel
    {
        public enum StatusAgent
        {
            Sleep,
            Active
        }
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public int Location_X { get; set; } = -1;
        public int Location_Y { get; set; } = -1;
        public List<MissionModel>? Missions { get; set; }
        public StatusAgent Status { get; set; } = StatusAgent.Sleep;
    }
}
