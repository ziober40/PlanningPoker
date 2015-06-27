namespace PlanningPoker.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using PlanningPoker.Models;

    public class TestData
    {
        public static IQueryable<Team> VotingPeopleCounts10
        {
            get
            {
                var votingPeopleCounts = new List<Team>();

                var vote = new Team
                {
                    Id = 0,
                    Name = "Default",
                    Amount = 10
                };
                votingPeopleCounts.Add(vote);
                return votingPeopleCounts.AsQueryable();
            }
        }

        public static IQueryable<Team> VotingPeopleCounts5
        {
            get
            {
                var votingPeopleCounts = new List<Team>();

                var vote = new Team
                {
                    Id = 0,
                    Name = "Default",
                    Amount = 5
                };

                votingPeopleCounts.Add(vote);
                return votingPeopleCounts.AsQueryable();
            }
        }

        public static IQueryable<Result> ResultsWithLowerAndHigher
        {
            get
            {
                var results = new List<Result>();
               
                    var result = new Result
                    {
                        Id = 0,
                        Estimate = Estimation.five,
                        Nickname = "Bartek" + 0,
                        TeamId = 0
                    };
                    results.Add(result);

                    var result2 = new Result
                    {
                        Id = 1,
                        Estimate = Estimation.infinite,
                        Nickname = "Bartek" + 1,
                        TeamId = 0
                    };
                    results.Add(result2);

                    var result3 = new Result
                    {
                        Id = 2,
                        Estimate = Estimation.fourty,
                        Nickname = "Bartek" + 2,
                        TeamId = 0
                    };
                    results.Add(result3);
               
                return results.AsQueryable();
            }
        }

        public static IQueryable<Result> Results7
        {
            get
            {
                var results = new List<Result>();
                for (int i = 1; i <= 7; i++)
                {
                    var result = new Result
                    {
                        Id = i - 1,
                        Estimate = (Estimation)(i % 5),
                        Nickname = "Bartek" + i,
                        TeamId = 0
                    };
                    results.Add(result);
                }

                return results.AsQueryable();
            }
        }

        public static IQueryable<Result> Results7c
        {
            get
            {
                var results = new List<Result>();
                for (int i = 1; i <= 7; i++)
                {
                    var result = new Result
                    {
                        Id = i - 1,
                        Estimate = Estimation.eight,
                        Nickname = "Bartek" + i,
                        TeamId = 0
                    };
                    results.Add(result);
                }

                return results.AsQueryable();
            }
        }

        public static IQueryable<Result> Results5
        {
            get
            {
                var results = new List<Result>();
                for (int i = 1; i <= 7; i++)
                {
                    var result = new Result
                    {
                        Id = i - 1,
                        Estimate = (Estimation)(i % 5),
                        Nickname = "Bartek" + i,
                        TeamId = 0
                    };
                    results.Add(result);
                }

                return results.AsQueryable();
            }
        }


        public static IQueryable<Result> Results
        {
            get
            {
                var results = new List<Result>();
                for (int i = 1; i <= 10; i++)
                {
                    var result = new Result
                    {
                        Id = i - 1,
                        Estimate = (Estimation)(i % 5),
                        Nickname = "Bartek" + i,
                        TeamId = 0
                    };
                    results.Add(result);
                }

                return results.AsQueryable();
            }
        }

        public static IQueryable<Result> ResultsWithConsensus
        {
            get
            {
                var results = new List<Result>();
                for (int i = 1; i <= 10; i++)
                {
                    var result = new Result
                    {
                        Id = i - 1,
                        Estimate = (Estimation)3,
                        Nickname = "Bartek" + i,
                        TeamId = 0
                    };
                    results.Add(result);
                }

                return results.AsQueryable();
            }
        }
    }
}
