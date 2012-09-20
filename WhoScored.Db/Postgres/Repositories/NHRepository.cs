using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using WhoScored.Model.Repositories;

namespace WhoScored.Db.Postgres.Repositories
{
    public class NHRepository<T> : IRepository<T> where T : class
    {
        protected readonly ISession Session;

        public NHRepository(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");
            Session = session;
        }

        public IQueryable<T> GetAll()
        {
            return Session.Query<T>();
        }

        public T GetById(int id)
        {
            return Session.Get<T>(id);
        }

        public void Save(T entity)
        {
            Session.Save(entity);
        }

        public void Update(T entity)
        {
            Session.Update(entity);
        }

        public void SaveUpdate(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public void Delete(T entity)
        {
            Session.Delete(entity);
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        /// <summary>
        /// Returns the total number of entities that match the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public long Count(DetachedCriteria criteria)
        {
            return Convert.ToInt64(criteria.GetExecutableCriteria(Session)
                .SetProjection(Projections.RowCountInt64()).UniqueResult());
        }
    }
}
