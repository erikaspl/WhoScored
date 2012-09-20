using System.Linq;
using NHibernate;
using WhoScored.Model;
using WhoScored.Model.Repositories;

namespace WhoScored.Db.Postgres.Repositories
{
    public class SettingsRepository : NHRepository<Settings>, ISettingsRepository
    {
        public SettingsRepository(ISession session) : base(session)
        {
        }

        public void ResetDatabase()
        {
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            sessionFactory.OpenSession();
        }
    }
}
