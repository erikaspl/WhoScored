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
        public void MapWorldDetails<T>() where T : class, IWorldDetails
        {
            var map = BsonClassMap.RegisterClassMap<T>(cm => cm.MapIdProperty("LeagueID"));
            
            foreach(var property in typeof(IWorldDetails).GetProperties())
            {
                map.MapProperty(property.Name);
            }
        }

        private const string WORLD_DETAILS_COLLECTION_NAME = "WorldDetails";


        /// <summary>
        /// Saves provided worldDetails to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="worldDetails">List of world details.</param>
        public void SaveWorldDetails(List<IWorldDetails> worldDetails)
        {
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
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IWorldDetails> GetWorldDetails()
        {
            var connector = new MongoConnector();
            connector.CreateConnection();

            var database = connector.Database;
            var collection = database.GetCollection<IWorldDetails>(WORLD_DETAILS_COLLECTION_NAME);

            return collection.FindAll().ToList();
        }
    }
}
