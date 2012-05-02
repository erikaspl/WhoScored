using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using WhoScored.Db.Connection;
using WhoScored.Model;

namespace WhoScored.Db.Mongo
{  
    public class MongoService : IWhoScoredDbService
    {
        public MongoService()
        {
        }

        public void MapWorldDetails<T>() where T : class, IWorldDetails
        {
            BsonClassMap.RegisterClassMap<T>
                (cm =>
                     {
                         cm.AutoMap();
                         cm.MapIdProperty("LeagueID");
                         //cm.GetMemberMap(c => c.LeagueName).SetIsRequired(true);
                     }
                );
        }

        private const string WORLD_DETAILS_COLLECTION_NAME = "WorldDetails";

        public void SaveWorldDetails(List<IWorldDetails> worldDetails)
        {
            var connector = new MongoConnector();
            connector.CreateConnection();

            var database = connector.Database;

            var collection = database.GetCollection(WORLD_DETAILS_COLLECTION_NAME);

            collection.Save(worldDetails);
        }
    }
}
