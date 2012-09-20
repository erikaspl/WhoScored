using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using WhoScored.Db.Model.Mappings;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using WhoScored.Db.NHibernate.Extensions;

namespace WhoScored.Db.Postgres
{
    public class SessionFactory
    {

        public static ISessionFactory CreateSessionFactory(bool resetDb)
        {
            AutoPersistenceModel model = CreateMappings();
            NHibernateProfiler.Initialize();

            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82.Driver<NpgsqlDriverExtended>()
                .ConnectionString(c => c.FromConnectionStringWithKey("postgreWhoScored")))
                //.Mappings(m => m.FluentMappings.AddFromAssemblyOf<CountryMap>().ExportTo(@"c:\Dropbox\dev"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CountryMap>())
                .ExposeConfiguration(c => BuildSchema(c, resetDb))
                .BuildSessionFactory();
        }


        private static AutoPersistenceModel CreateMappings()
        {
            return AutoMap
                .Assembly(System.Reflection.Assembly.GetCallingAssembly())
                .Where(t => t.Namespace == "WhoScored.Db.Model.Mappings");
        }

        private static void BuildSchema(Configuration config, bool resetDb)
        {
            if (resetDb)
                new SchemaExport(config).Create(false, true);
        }
    }
}
