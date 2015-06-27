namespace PlanningPoker.Models
{
    using System;
    using System.Linq;

    public interface IPlanningPokerDb : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;

        void Save();

        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;
    }


}