using System.Text.Json.Serialization;

namespace MossadAgentsRest.Dto
{
    public class AgentDto
    {
        public enum StatusAgent
        {
            Sleep,
            Active
        }
        public string? PhotoUrl { get; set; }
        public string? Nickname { get; set; }
        public int Location_X { get; set; } = -1;
        public int Location_Y { get; set; } = -1;
        [JsonIgnore]
        public StatusAgent Status { get; set; } = StatusAgent.Sleep;
    }
}
