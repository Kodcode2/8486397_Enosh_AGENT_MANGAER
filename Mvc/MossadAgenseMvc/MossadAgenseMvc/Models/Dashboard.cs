using Humanizer;

namespace MossadAgenseMvc.Models
{
    public class Dashboard
    {
        public int AgentNum { get; set; }
        public int AgentActiveNum { get; set; }
        public int TargetNum { get; set; }
        public int TargetDaedNum { get; set; }
        public int MissionsNum { get; set; }
        public int MissionsActiveNum { get; set; }
        public int RelationAgentsToTargets {  get; set; }

    }
}
