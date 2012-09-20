using System;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using WhoScored.Db.Model.Mappings;
using WhoScored.Db.NHibernate.Extensions;

namespace WhoScored.Db.Postgres
{
    public class SessionManager
    {
        private static ISessionFactory Factory { get; set; }

        private static ISessionFactory GetFactory<T>() where T : ICurrentSessionContext
        {
            NHibernateProfiler.Initialize();
            return Fluently.Configure()
                    .Database(PostgreSQLConfiguration.PostgreSQL82.Driver<NpgsqlDriverExtended>()           
                    #if DEBUG
                             .ShowSql()
                    #endif
                .ConnectionString(c => c.FromConnectionStringWithKey("postgreWhoScored")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CountryMap>())
                .CurrentSessionContext<T>()
                //.ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }


        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config).Create(false, true);
        }

        public static ISession CurrentSession
        {
            get
            {
                if (Factory == null)
                    Factory = HttpContext.Current != null
                                    ? GetFactory<WebSessionContext>()
                                    : GetFactory<ThreadStaticSessionContext>();
                if (CurrentSessionContext.HasBind(Factory))
                {
                    var currentSession = Factory.GetCurrentSession();
                    currentSession.Clear();
                    return currentSession;
                }
                ISession session = Factory.OpenSession();
                CurrentSessionContext.Bind(session);
                return session;
            }
        }

        public static void CloseSession()
        {
            if (Factory == null)
                return;
            if (CurrentSessionContext.HasBind(Factory))
            {
                ISession session = CurrentSessionContext.Unbind(Factory);
                session.Close();
            }
        }

        public static void CommitSession(ISession session)
        {
            try
            {
                session.Transaction.Commit();
            }
            catch (Exception)
            {
                session.Transaction.Rollback();
                throw;
            }
        }
    }
}
