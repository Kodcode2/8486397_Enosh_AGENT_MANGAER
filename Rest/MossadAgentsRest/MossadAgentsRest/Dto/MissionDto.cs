using MossadAgentsRest.Models;

namespace MossadAgentsRest.Dto
{
    public class MissionDto
    {
        public enum MissionStatus
        {
            Suggest,
            Active,
            finish
        }

        public int Id { get; set; }
        public int AgentId { get; set; }
        public AgentDto? Agent { get; set; }
        public int TargetId { get; set; }
        public TargetDto? Target { get; set; }
        public DateTime LeftTime { get; set; }
        public DateTime KillTime { get; set; }
        public MissionStatus status { get; set; } = MissionStatus.Suggest;
    }
}
