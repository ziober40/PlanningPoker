namespace PlanningPoker.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Result
    {
        public int Id { get; set; }

        public Estimation Estimate { get; set; }

        [Required]
        public string Nickname { get; set; }

        public bool IsLowest { get; set; }

        public bool IsHighest { get; set; }

        public int TeamId { get; set; }
    }
}