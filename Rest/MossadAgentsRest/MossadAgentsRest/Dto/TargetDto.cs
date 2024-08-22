using System.Text.Json.Serialization;

namespace MossadAgentsRest.Dto
{
    public class TargetDto
    {
        public enum StatusTarget
        {
            Alive,
            dead
        }
        //public int Id { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? PhotoUrl { get; set; }
        public int Location_X { get; set; } = -1;
        public int Location_Y { get; set; } = -1;
        [JsonIgnore]
        public StatusTarget Status { get; set; } = StatusTarget.Alive;
    }
}
