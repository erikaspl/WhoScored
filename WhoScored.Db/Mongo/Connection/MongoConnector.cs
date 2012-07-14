namespace WhoScored.Db.Connection
{
    using System.Configuration;

    using MongoDB.Driver;

    public class MongoConnector
    {
        public static MongoDatabase GetDatabase()
        {
            var server = CreateServer();
            string databaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];

            return server.GetDatabase(databaseName);
        }

        public static MongoServer CreateServer()
        {
            string connectionString = ConfigurationManager.AppSettings["MongoDbConnection"];
            return MongoServer.Create(connectionString);
        }
    }
}