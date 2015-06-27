namespace PlanningPoker.Models
{
    public class VotingNotFinishedModel
    {
        public int PeopleVoted { get; set; }

        public int PeopleConnected { get; set; }

        public Team Team { get; set; }

        public string Name { get; set; }

        public int Maximum { get; set; }
    }
}