using System;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
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

        public static void MapSeriesFixtures<T, Y>() 
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<T>(cm => cm.MapIdProperty("Id"));
                foreach (var property in typeof(ISeriesFixtures).GetProperties())
                {
                    map.MapProperty(property.Name);
                }
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Y)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<Y>();
                foreach (var property in typeof(IMatchSummary).GetProperties())
                {
                    map.MapProperty(property.Name);
                }
            }
        }

        public static void MapMatchDetails<T, TY, TZ, TA, TB, TC, TD>()
            where T : class, IMatch
            where TY : class, IMatchArena
            where TZ : class, IMatchTeam
            where TA : class, IMatchScorers
            where TB : class, IMatchBookings
            where TC : class, IMatchInjuries
            where TD : class, IMatchEventList
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<T>(cm => cm.MapIdProperty("MatchID"));
                MapTypeProperties<IMatch>(map);
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TY)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<TY>();
                MapTypeProperties<IMatchArena>(map);
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TZ)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<TZ>();
                MapTypeProperties<IMatchTeam>(map);
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TA)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<TA>();
                MapTypeProperties<IMatchScorers>(map);
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TB)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<TB>();
                MapTypeProperties<IMatchBookings>(map);
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TC)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<TC>();
                MapTypeProperties<IMatchInjuries>(map);
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TD)))
            {
                BsonClassMap map = BsonClassMap.RegisterClassMap<TD>();
                MapTypeProperties<IMatchEventList>(map);
            }
        }

        private static void MapTypeProperties<T>(BsonClassMap map)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                map.MapProperty(property.Name);
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

        private const string SERIES_DETAILS_COLLECTION_NAME = "SeriesDetails";

        /// <summary>
        /// Saves provided leagueDetails to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="leagueDetails">List of league details.</param>
        public void SaveSeriesDetails<T>(List<T> leagueDetails) where T : class, ILeagueDetails
        {
            MapLeagueDetails<T>();

            var database = MongoConnector.GetDatabase();

            var collection = database.GetCollection(SERIES_DETAILS_COLLECTION_NAME);

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
        public void SaveSeriesDetails<T>(T leagueDetail) where T : class, ILeagueDetails
        {
            MapLeagueDetails<T>();
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(SERIES_DETAILS_COLLECTION_NAME);

            collection.Save(leagueDetail);
        }

        public List<T> GetSeriesDetails<T>() where T : class, ILeagueDetails
        {
            MapLeagueDetails<T>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(SERIES_DETAILS_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            return result;
        }

        public List<T> GetSeriesDetails<T>(string countryId) where T : class, ILeagueDetails
        {
            MapLeagueDetails<T>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(SERIES_DETAILS_COLLECTION_NAME);

            var query = new QueryDocument("LeagueID", int.Parse(countryId));
            var result = collection.Find(query).ToList();

            return result;
        }

        /// <summary>
        /// Drops world details from the database
        /// </summary>
        public void DropSeriesDetails()
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<ILeagueDetails>(SERIES_DETAILS_COLLECTION_NAME);

            collection.Drop();
        }

        #endregion

        #region Series fixtures CRUID

        private const string SERIES_FIXTURES_COLLECTION_NAME = "SeriesFixtureSummary";

        public void SaveSeriesFixtures<T, Y>(List<T> seriesFixtures)
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary
        {
            MapSeriesFixtures<T, Y>();
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(SERIES_FIXTURES_COLLECTION_NAME);

            foreach (var fixture in seriesFixtures)
            {
                collection.Save(fixture);
            }
        }

        public void SaveSeriesFixtures<T, Y>(T seriesFixture)
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary
        {
            MapSeriesFixtures<T, Y>();
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(SERIES_FIXTURES_COLLECTION_NAME);
            collection.Save(seriesFixture);
        }

        public List<T> GetSeriesFixturesSummary<T, Y>()
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary
        {
            MapSeriesFixtures<T, Y>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(SERIES_FIXTURES_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            return result;
        }

        public T GetSeriesFixturesSummary<T, Y>(int leagueId, int season)
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary
        {
            MapSeriesFixtures<T, Y>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(SERIES_FIXTURES_COLLECTION_NAME);
            var query = Query.And(Query.EQ("LeagueLevelUnitID", leagueId), Query.EQ("Season", season));
            var result = collection.FindOne(query);

            return result;
        }

        public void DropSeriesFixtures()
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<ISeriesFixtures>(SERIES_FIXTURES_COLLECTION_NAME);

            collection.Drop();
        }

        #endregion

        #region MatchDetails CRUID

        private const string MATCH_DETAILS_COLLECTION_NAME = "MatchDetails";

        public void SaveMatchDetails<T, TY, TZ, TA, TB, TC, TD>(List<T> matchDetails)
            where T : class, IMatch
            where TY : class, IMatchArena
            where TZ : class, IMatchTeam
            where TA : class, IMatchScorers
            where TB : class, IMatchBookings
            where TC : class, IMatchInjuries
            where TD : class, IMatchEventList
        {
            MapMatchDetails<T, TY, TZ, TA, TB, TC, TD>();
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(MATCH_DETAILS_COLLECTION_NAME);

            foreach (var fixture in matchDetails)
            {
                collection.Save(fixture);
            }
        }

        public void SaveMatchDetails<T, TY, TZ, TA, TB, TC, TD>(T matchDetails)
            where T : class, IMatch
            where TY : class, IMatchArena
            where TZ : class, IMatchTeam
            where TA : class, IMatchScorers
            where TB : class, IMatchBookings
            where TC : class, IMatchInjuries
            where TD : class, IMatchEventList
        {
            MapMatchDetails<T, TY, TZ, TA, TB, TC, TD>();
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(MATCH_DETAILS_COLLECTION_NAME);

            collection.Save(matchDetails);
        }

        public List<T> GetMatchDetails<T, TY, TZ, TA, TB, TC, TD>()
            where T : class, IMatch
            where TY : class, IMatchArena
            where TZ : class, IMatchTeam
            where TA : class, IMatchScorers
            where TB : class, IMatchBookings
            where TC : class, IMatchInjuries
            where TD : class, IMatchEventList
        {
            MapMatchDetails<T, TY, TZ, TA, TB, TC, TD>();

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(MATCH_DETAILS_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            return result;
        }

        public void DropMatchDetails()
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<IMatch>(MATCH_DETAILS_COLLECTION_NAME);

            collection.Drop();
        }

        #endregion
    }
}