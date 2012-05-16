﻿using System;
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
    public class WhoScoredRepository : IWhoScoredRepository
    {
        #region Mongo Mappings

        public static void MapWorldDetails<T>() where T : class, IWorldDetails
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<T>(cm => cm.MapIdProperty("LeagueID"));
                foreach (var property in typeof(IWorldDetails).GetProperties())
                {
                    map.MapProperty(property.Name);
                }
            }
        }

        public static void MapSettings<T>() where T : class, ISettings
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<T>();
                foreach (var property in typeof(ISettings).GetProperties())
                {
                    map.MapProperty(property.Name);
                }
            }
        }

        public static void MapLeagueDetails<T>() where T : class, ILeagueDetails
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof (T)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<T>(cm => cm.MapIdProperty("LeagueLevelUnitID"));
                foreach (var property in typeof (ILeagueDetails).GetProperties())
                {
                    map.MapProperty(property.Name);
                }
            }
        }

        #endregion

        #region WorldDetails CRUID

        public const string WORLD_DETAILS_COLLECTION_NAME = "WorldDetails";

        /// <summary>
        /// Saves provided worldDetails to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="worldDetails">List of world details.</param>
        public void SaveWorldDetails<T>(List<T> worldDetails)  where T : class, IWorldDetails
        {
            MapWorldDetails<T>();

            var database = MongoConnector.GetDatabase();
            
            var collection = database.GetCollection(WORLD_DETAILS_COLLECTION_NAME);

            foreach (var worldDetail in worldDetails)
            {
                collection.Save(worldDetail);
            }
        }

        /// <summary>
        /// Saves provided worldDetails to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="worldDetail"></param>
        public void SaveWorldDetails<T>(T worldDetail) where T : class, IWorldDetails
        {
            MapWorldDetails<T>();
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(WORLD_DETAILS_COLLECTION_NAME);

            collection.Save(worldDetail);
        }


        /// <summary>
        /// Gets all world details records
        /// </summary>
        /// <returns></returns>
        public List<T> GetWorldDetails<T>() where T : class, IWorldDetails
        {
            MapWorldDetails<T>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(WORLD_DETAILS_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            return result;
        }

        public T GetWorldDetails<T>(int countryId) where T : class, IWorldDetails
        {
            MapWorldDetails<T>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(WORLD_DETAILS_COLLECTION_NAME);
            var query = new QueryDocument("_id", countryId);
            var result = collection.FindOneAs<T>(query);

            return result;
        }


        /// <summary>
        /// Drops world details from the database
        /// </summary>
        public void DropWorldDetails()
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<IWorldDetails>(WORLD_DETAILS_COLLECTION_NAME);

            collection.Drop();
        }

        #endregion

        #region Settings CRUID

        public const string SETTINGS_COLLECTION_NAME = "Settings";

        public void SaveSettings<T>(T settings) where T : class, ISettings
        {
            MapSettings<T>();

            var database = MongoConnector.GetDatabase();

            var collection = database.GetCollection(SETTINGS_COLLECTION_NAME);

            collection.Save(settings);
        }

        public T GetSettings<T>() where T : class, ISettings
        {
            MapSettings<T>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(SETTINGS_COLLECTION_NAME);
            return collection.FindAll().ToList().First();
        }

        #endregion

        #region LeagueDetails CRUID

        private const string LEAGUE_DETAILS_COLLECTION_NAME = "SeriesDetails";

        /// <summary>
        /// Saves provided leagueDetails to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="leagueDetails">List of league details.</param>
        public void SaveLeagueDetails<T>(List<T> leagueDetails) where T : class, ILeagueDetails
        {
            MapLeagueDetails<T>();

            var database = MongoConnector.GetDatabase();

            var collection = database.GetCollection(LEAGUE_DETAILS_COLLECTION_NAME);

            foreach (var worldDetail in leagueDetails)
            {
                collection.Save(worldDetail);
            }
        }

        /// <summary>
        /// Saves provided leagueDetail to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="leagueDetail"></param>
        public void SaveLeagueDetails<T>(T leagueDetail) where T : class, ILeagueDetails
        {
            MapLeagueDetails<T>();
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(LEAGUE_DETAILS_COLLECTION_NAME);

            collection.Save(leagueDetail);
        }

        public List<T> GetLeagueDetails<T>() where T : class, ILeagueDetails
        {
            MapLeagueDetails<T>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(LEAGUE_DETAILS_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            return result;
        }

        public List<T> GetLeagueDetails<T>(string countryId) where T : class, ILeagueDetails
        {
            MapLeagueDetails<T>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(LEAGUE_DETAILS_COLLECTION_NAME);

            var query = new QueryDocument("LeagueID", countryId);
            var result = collection.Find(query).ToList();

            return result;
        }

        /// <summary>
        /// Drops world details from the database
        /// </summary>
        public void DropLeagueDetails()
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<ILeagueDetails>(LEAGUE_DETAILS_COLLECTION_NAME);

            collection.Drop();
        }

        #endregion
    }
}