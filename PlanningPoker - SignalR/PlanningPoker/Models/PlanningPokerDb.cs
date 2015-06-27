namespace PlanningPoker.Models
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;

    public class PlanningPokerDb : DbContext, IPlanningPokerDb
    {
        public PlanningPokerDb()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Log> Logs { get; set; }

        IQueryable<T> IPlanningPokerDb.Query<T>()
        {
            return Set<T>();
        }

        void IPlanningPokerDb.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void IPlanningPokerDb.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        void IPlanningPokerDb.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        void IPlanningPokerDb.Save()
        {
            SaveChanges();
        }
    }
}