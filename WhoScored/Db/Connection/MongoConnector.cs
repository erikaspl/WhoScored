namespace WhoScored.Db.Connection
{
    using System.Configuration;

    using MongoDB.Driver;

    public class MongoConnector
    {
        private MongoServer _server;
        public MongoServer Server
        {
            get
            {
                return _server;
            }
        }

        private MongoDatabase _database;

        public MongoDatabase Database
        {
            get
            {
                return _database;
            }
        }

        public void CreateConnection()
        {
            string connectionString = ConfigurationManager.AppSettings["MongoDbConnection"];
            _server = MongoServer.Create(connectionString);

            string databaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];
            _database = _server.GetDatabase(databaseName);
        }
    }
}