using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using WhoScored.Db.Connection;
using WhoScored.Model;

namespace WhoScored.Db.Mongo
{
    public class MongoService : IWhoScoredDbService
    {
        private void MapWorldDetails<T>() where T : class, IWorldDetails
        {
            BsonClassMap map;
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                map = BsonClassMap.RegisterClassMap<T>(cm => cm.MapIdProperty("LeagueID"));
                foreach (var property in typeof(IWorldDetails).GetProperties())
                {
                    map.MapProperty(property.Name);
                }
            }
            else
            {
                map = BsonClassMap.LookupClassMap(typeof(T));
            }
        }

        private const string WORLD_DETAILS_COLLECTION_NAME = "WorldDetails";


        /// <summary>
        /// Saves provided worldDetails to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="worldDetails">List of world details.</param>
        public void SaveWorldDetails<T>(List<T> worldDetails)  where T : class, IWorldDetails
        {
            MapWorldDetails<T>();
            var connector = new MongoConnector();
            connector.CreateConnection();

            var database = connector.Database;
            var collection = database.GetCollection(WORLD_DETAILS_COLLECTION_NAME);

            foreach (var worldDetail in worldDetails)
            {
                collection.Save(worldDetail);
            }
            connector.Server.Disconnect();
        }

        /// <summary>
        /// Saves provided worldDetails to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="worldDetail"></param>
        public void SaveWorldDetails<T>(T worldDetail) where T : class, IWorldDetails
        {
            MapWorldDetails<T>();
            var connector = new MongoConnector();
            connector.CreateConnection();

            var database = connector.Database;
            var collection = database.GetCollection(WORLD_DETAILS_COLLECTION_NAME);

            collection.Save(worldDetail);

            connector.Server.Disconnect();
        }


        /// <summary>
        /// Gets all world details records
        /// </summary>
        /// <returns></returns>
        public List<T> GetWorldDetails<T>() where T : class, IWorldDetails
        {
            MapWorldDetails<T>();
            var connector = new MongoConnector();
            connector.CreateConnection();

            var database = connector.Database;
            var collection = database.GetCollection<T>(WORLD_DETAILS_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            connector.Server.Disconnect();

            return result;
        }


        /// <summary>
        /// Drops world details from the database
        /// </summary>
        public void DropWorldDetails()
        {
            var connector = new MongoConnector();
            connector.CreateConnection();

            var database = connector.Database;
            var collection = database.GetCollection<IWorldDetails>(WORLD_DETAILS_COLLECTION_NAME);

            collection.Drop();

            connector.Server.Disconnect();
        }
    }
}