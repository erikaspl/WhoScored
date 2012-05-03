using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using WhoScored.Db.Connection;
using WhoScored.Model;

namespace WhoScored.Db.Mongo
{  
    public class MongoService : IWhoScoredDbService
    {
        public void MapWorldDetails<T>() where T : class, IWorldDetails
        {
            BsonClassMap.RegisterClassMap<T>
                (cm =>
                     {
                         cm.AutoMap();
                         cm.MapIdProperty("LeagueID");
                         cm.GetMemberMap(c => c.LeagueName).SetIsRequired(true);
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

            foreach (var worldDetail in worldDetails)
            {
                dynamic expandoWorldDetails = AutoMapper.Mapper.DynamicMap<IWorldDetails>(worldDetail);

                BsonClassMap map = BsonClassMap.LookupClassMap(expandoWorldDetails.GetType());
                map.SetIdMember(map.GetMemberMap("LeagueID"));
                

                //var bsonObject = BsonExtensionMethods.ToBsonDocument(expandoWorldDetails);
                
                collection.Save(expandoWorldDetails);
            }
            connector.Server.Disconnect();
        }
    }
}
