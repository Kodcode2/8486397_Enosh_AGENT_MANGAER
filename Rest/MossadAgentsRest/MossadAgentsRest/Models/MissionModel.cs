namespace MossadAgentsRest.Models
{
    public class MissionModel
    {
        public enum MissionStatus
        {
            Suggest,
            Active,
            finish
        }

        public int Id { get; set; }
        public int AgentId { get; set; }
        public AgentModel? Agent { get; set; }
        public int TargetId { get; set; }
        public TargetModel? Target { get; set; }
        public string? LeftTime { get; set; }
        public string? TimeFromStartToEnd { get; set; }
        public DateTime KillTime { get; set; }
        public MissionStatus Status { get; set; } = MissionStatus.Suggest;
    }
}
