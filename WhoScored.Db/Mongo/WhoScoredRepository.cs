using System;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using WhoScored.Db.Connection;
using WhoScored.Model;
using MongoDB.Driver.Linq;
using WhoScored.Model.Implementation;

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
                    if (property.Name != "IsMatchMigrated")
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
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(WORLD_DETAILS_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            return result;
        }

        public T GetWorldDetails<T>(int countryId) where T : class, IWorldDetails
        {
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
            var database = MongoConnector.GetDatabase();

            var collection = database.GetCollection(SETTINGS_COLLECTION_NAME);

            collection.Save(settings);
        }

        public T GetSettings<T>() where T : class, ISettings
        {
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
                var result = collection.Save(worldDetail);
            }
        }

        /// <summary>
        /// Saves provided leagueDetail to a database. 
        /// Updates records if they already exist.
        /// </summary>
        /// <param name="leagueDetail"></param>
        public void SaveSeriesDetails<T>(T leagueDetail) where T : class, ILeagueDetails
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(SERIES_DETAILS_COLLECTION_NAME);

            collection.Save(leagueDetail);
        }

        public List<T> GetSeriesDetails<T>() where T : class, ILeagueDetails
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(SERIES_DETAILS_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            return result;
        }

        public List<T> GetSeriesDetails<T>(string countryId) where T : class, ILeagueDetails
        {
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

        public void SaveSeriesFixtures<T>(List<T> seriesFixtures)
            where T : class, ISeriesFixtures
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(SERIES_FIXTURES_COLLECTION_NAME);

            foreach (var fixture in seriesFixtures)
            {
                collection.Save(fixture);
            }
        }

        public void SaveSeriesFixtures<T>(T seriesFixture)
            where T : class, ISeriesFixtures
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(SERIES_FIXTURES_COLLECTION_NAME);
            collection.Save(seriesFixture);
        }

        public List<T> GetSeriesFixturesSummary<T>()
            where T : class, ISeriesFixtures
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection<T>(SERIES_FIXTURES_COLLECTION_NAME);

            var result = collection.FindAll().ToList();

            return result;
        }

        public T GetSeriesFixturesSummary<T, Y>(int leagueId, int season)
            where T : class, ISeriesFixtures
            where Y : class, IMatch
        {
            var database = MongoConnector.GetDatabase();
            var matchSummaryCol = database.GetCollection<T>(SERIES_FIXTURES_COLLECTION_NAME);
            var query = Query.And(Query.EQ("LeagueLevelUnitID", leagueId), Query.EQ("Season", season));
            var result = matchSummaryCol.FindOne(query);

            var matchCollection = database.GetCollection(MATCH_DETAILS_COLLECTION_NAME);

            if (result != null)
            {
                foreach (var matchSummary in result.Matches)
                {
                    matchSummary.IsMatchMigrated =
                        matchCollection.AsQueryable<Y>().Count(m => m.MatchID == matchSummary.MatchID.ToString()) > 0;
                }
            }

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

        public void SaveMatchDetails<T>(List<T> matchDetails) where T : class, IMatch
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(MATCH_DETAILS_COLLECTION_NAME);

            foreach (var fixture in matchDetails)
            {
                collection.Save(fixture);
            }
        }

        public void SaveMatchDetails<T>(T matchDetails) where T : class, IMatch
        {
            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(MATCH_DETAILS_COLLECTION_NAME);

            collection.Save(matchDetails);
        }

        public List<T> GetMatchDetails<T>() where T : class, IMatch
        {
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

        #region Series Details

        private string SeriesStandingsMap()
        {
            return @"
                function Map() {
	                emit({SeriesId : this.LeagueLevelUnitID, Season: this.MatchSeason},
	                {	
		                'Season': this.MatchSeason,
		                'HomeTeamID': this.MatchHomeTeam.HomeTeamID,
		                'HomeTeamName': this.MatchHomeTeam.HomeTeamName,
		                'HomeGoals': this.MatchHomeTeam.HomeGoals,
		                'AwayTeamID': this.MatchAwayTeam.AwayTeamID,
		                'AwayTeamName': this.MatchAwayTeam.AwayTeamName,
		                'AwayGoals': this.MatchAwayTeam.AwayGoals
	                });
                }";
        }

        private string SeriesStandingsReduce()
        {
            return
                @"
                    function Reduce(key, values) {

	                    var teamStandings = new Object();
	                    values.forEach(function(value) {	
		                    var homeTeam;
		                    if (value.HomeTeamID in teamStandings){
			                    homeTeam = teamStandings[value.HomeTeamID];
		                    }else{
			                    homeTeam = {
					                    TeamId:value.HomeTeamID,
					                    TeamName:value.HomeTeamName,
					                    GoalsScored:0,
					                    GoalsConceded:0,
					                    GoalDifference:0,
					                    HomePoints:0,
					                    AwayPoints:0,
					                    TotalPoints:0,
					                    Won:0,
					                    Lost:0,
					                    Drawn:0,
					                    Played:0
			                    }
			                    teamStandings[value.HomeTeamID] = homeTeam;	
		                    }
		
		                    var awayTeam;
		                    if (value.AwayTeamID in teamStandings){
			                    awayTeam = teamStandings[value.AwayTeamID];
		                    }else{
			                    awayTeam = {
					                    TeamId:value.AwayTeamID,
					                    TeamName:value.AwayTeamName,
					                    GoalsScored:0,
					                    GoalsConceded:0,
					                    GoalDifference:0,
					                    HomePoints:0,
					                    AwayPoints:0,
					                    TotalPoints:0,
					                    Won:0,
					                    Lost:0,
					                    Drawn:0,
					                    Played:0
			                    }
			                    teamStandings[value.AwayTeamID] = awayTeam;	
		                    }
		
		                    homeTeam.Played += 1;
		                    awayTeam.Played += 1;
		
		                    homeTeam.GoalsScored += parseInt(value.HomeGoals);
		                    awayTeam.GoalsScored += parseInt(value.AwayGoals);
		                    homeTeam.GoalsConceded += parseInt(value.AwayGoals);
		                    awayTeam.GoalsConceded += parseInt(value.HomeGoals);
		
		                    homeTeam.GoalDifference = homeTeam.GoalsScored - homeTeam.GoalsConceded;
		                    awayTeam.GoalDifference = awayTeam.GoalsScored - awayTeam.GoalsConceded;
		
		                    if (value.HomeGoals === value.AwayGoals){
				                    homeTeam.TotalPoints += 1;
				                    homeTeam.HomePoints += 1;			
				                    awayTeam.TotalPoints += 1;
				                    awayTeam.AwayPoints += 1;
				
				                    homeTeam.Drawn += 1;
				                    awayTeam.Drawn += 1;
			                    }
			
			                    if (value.HomeGoals > value.AwayGoals){
				                    homeTeam.TotalPoints += 3;
				                    homeTeam.HomePoints += 3;
				                    homeTeam.Won += 1;
				                    awayTeam.Lost += 1;
			                    }
			
			                    if (value.HomeGoals < value.AwayGoals){
				                    awayTeam.TotalPoints += 3;
				                    awayTeam.AwayPoints += 3;
				                    awayTeam.Won += 1;
				                    homeTeam.Lost += 1;
			                    }	
		
	                    });
	
	                    var reduced = {teams :[]};
	                    reduced.teams = teamStandings;
	
	                    return reduced;
                    }";
        }

        private string SeriesStandingsFinalize()
        {
            return @"
                function Finalize(key, reduced) {
	                return reduced;
                }";
        }

        public List<ISeriesStandingsTeamEntity> GetSeriesStandingsWithResults(int seriesId, int season, int matchRound)
        {
            var standings = GetSeriesStandings(seriesId, season, matchRound);
            SetTeamResults(seriesId, season, matchRound, standings);
            return standings;
        }

        private List<ISeriesStandingsTeamEntity> GetSeriesStandings(int seriesId, int season, int matchRound)
        {
            string map = SeriesStandingsMap();
            string reduce = SeriesStandingsReduce();
            string finalize = SeriesStandingsFinalize();

            var query = Query.And(
                Query.EQ("LeagueLevelUnitID", seriesId.ToString()), 
                Query.EQ("MatchSeason", season.ToString()),
                Query.LTE("MatchRound", matchRound));

            var options = new MapReduceOptionsBuilder();
            options.SetOutput(MapReduceOutput.Inline);
            options.SetFinalize(finalize);
            options.SetQuery(query);

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(MATCH_DETAILS_COLLECTION_NAME);

            var results = collection.MapReduce(map, reduce, options);

            var tealList = new List<SeriesStandingsTeamEntity>();
            var result = results.InlineResults.FirstOrDefault();

            if (result != null)
            {
                tealList.AddRange(result["value"].AsBsonDocument["teams"].AsBsonDocument.Elements.ToList().
                    Select(element => new SeriesStandingsTeamEntity
                    {
                        TeamId = element.Value.AsBsonDocument["TeamId"].ToString(),
                        TeamName = element.Value.AsBsonDocument["TeamName"].ToString(),
                        GoalsScored = element.Value.AsBsonDocument["GoalsScored"].ToInt32(),
                        GoalsConceded = element.Value.AsBsonDocument["GoalsConceded"].ToInt32(),
                        GoalDifference = element.Value.AsBsonDocument["GoalDifference"].ToInt32(),
                        HomePoints = element.Value.AsBsonDocument["HomePoints"].ToInt32(),
                        AwayPoints = element.Value.AsBsonDocument["AwayPoints"].ToInt32(),
                        TotalPoints = element.Value.AsBsonDocument["TotalPoints"].ToInt32(),
                        Won = element.Value.AsBsonDocument["Won"].ToInt32(),
                        Lost = element.Value.AsBsonDocument["Lost"].ToInt32(),
                        Drawn = element.Value.AsBsonDocument["Drawn"].ToInt32(),
                        Played = element.Value.AsBsonDocument["Played"].ToInt32()
                    }));
            }
            
            var sortedList = tealList.OrderByDescending(t => t.TotalPoints).ThenByDescending(t => t.GoalDifference).ToList();

            int counter = 1;
            sortedList.ForEach(x => x.Position = counter++);
            return sortedList.Cast<ISeriesStandingsTeamEntity>().ToList();
        }

        private string TeamResultsMap()
        {
            return
                @"function Map() {
	                emit({SeriesId : this.LeagueLevelUnitID, Season: this.MatchSeason},
	                {	
		                'Season': this.MatchSeason,
		                'HomeTeamID': this.MatchHomeTeam.HomeTeamID,
		                'HomeTeamName': this.MatchHomeTeam.HomeTeamName,
		                'HomeGoals': this.MatchHomeTeam.HomeGoals,
		                'AwayTeamID': this.MatchAwayTeam.AwayTeamID,
		                'AwayTeamName': this.MatchAwayTeam.AwayTeamName,
		                'AwayGoals': this.MatchAwayTeam.AwayGoals,
		                'MatchRound': this.MatchRound
	                });
                }";
        }

        private string TeamResultReduce()
        {
            return
                @"function Reduce(key, values) {
	                var teamForm = new Object();
	
	                values.forEach(function(value) {	
		                var homeTeam;
		                if (value.HomeTeamID in teamForm){
			                homeTeam = teamForm[value.HomeTeamID];
		                }else{
			                homeTeam = {
					                TeamId:value.HomeTeamID,
					                TeamName:value.HomeTeamName,
					                TeamProgress: {}
			                }
			                teamForm[value.HomeTeamID] = homeTeam;	
		                }
		
		                var awayTeam;
		                if (value.AwayTeamID in teamForm){
			                awayTeam = teamForm[value.AwayTeamID];
		                }else{
			                awayTeam = {
					                TeamId:value.AwayTeamID,
					                TeamName:value.AwayTeamName,
					                TeamProgress: {}
			                }
			                teamForm[value.AwayTeamID] = awayTeam;	
		                }
		                var homeResult;
		                if (value.HomeGoals === value.AwayGoals){
			                homeTeam.TeamProgress[value.MatchRound] = {
                                MatchRound:value.MatchRound,
				                HomeTeamName: value.HomeTeamName,
				                HomeTeamGoals: value.HomeGoals,
				                AwayTeamName: value.AwayTeamName,
				                AwayTeamGoals: value.AwayGoals,
				                Result:'d'
			                };
			                awayTeam.TeamProgress[value.MatchRound] = {
                                MatchRound:value.MatchRound,
				                HomeTeamName: value.HomeTeamName,
				                HomeTeamGoals: value.HomeGoals,
				                AwayTeamName: value.AwayTeamName,
				                AwayTeamGoals: value.AwayGoals,
				                Result:'D'
			                };				
		                }
			
		                if (value.HomeGoals > value.AwayGoals){
			                homeTeam.TeamProgress[value.MatchRound] = {
                                MatchRound:value.MatchRound,
				                HomeTeamName: value.HomeTeamName,
				                HomeTeamGoals: value.HomeGoals,
				                AwayTeamName: value.AwayTeamName,
				                AwayTeamGoals: value.AwayGoals,
				                Result:'w'
			                };
			                awayTeam.TeamProgress[value.MatchRound] = {
                                MatchRound:value.MatchRound,
				                HomeTeamName: value.HomeTeamName,
				                HomeTeamGoals: value.HomeGoals,
				                AwayTeamName: value.AwayTeamName,
				                AwayTeamGoals: value.AwayGoals,
				                Result:'L'
			                };
		                }
		
		                if (value.HomeGoals < value.AwayGoals){
			                homeTeam.TeamProgress[value.MatchRound] = {
                                MatchRound:value.MatchRound,
				                HomeTeamName: value.HomeTeamName,
				                HomeTeamGoals: value.HomeGoals,
				                AwayTeamName: value.AwayTeamName,
				                AwayTeamGoals: value.AwayGoals,
				                Result:'l'
			                };
			                awayTeam.TeamProgress[value.MatchRound] = {
                                MatchRound:value.MatchRound,
				                HomeTeamName: value.HomeTeamName,
				                HomeTeamGoals: value.HomeGoals,
				                AwayTeamName: value.AwayTeamName,
				                AwayTeamGoals: value.AwayGoals,
				                Result:'W'
			                };
		                }		
	                });
	
	                var reduced = {teams :[]};
	                reduced.teams = teamForm;
	
	                return reduced;
                }";
        }

        private string TeamResultFinalize()
        {
            return @"
                function Finalize(key, reduced) {
	                return reduced;
                }";
        }

        private void SetTeamResults(int seriesId, int season, int matchRound, List<ISeriesStandingsTeamEntity> seriesStandings)
        {
            string map = TeamResultsMap();
            string reduce = TeamResultReduce();
            string finalize = TeamResultFinalize();

            var query = Query.And(
                Query.EQ("LeagueLevelUnitID", seriesId.ToString()),
                Query.EQ("MatchSeason", season.ToString()),
                Query.LTE("MatchRound", matchRound));

            var options = new MapReduceOptionsBuilder();
            options.SetOutput(MapReduceOutput.Inline);
            options.SetFinalize(finalize);
            options.SetQuery(query);

            var database = MongoConnector.GetDatabase();
            var collection = database.GetCollection(MATCH_DETAILS_COLLECTION_NAME);

            var results = collection.MapReduce(map, reduce, options);

            var result = results.InlineResults.FirstOrDefault();


            foreach (var teamEntity in seriesStandings)
            {
                ISeriesStandingsTeamEntity entity = teamEntity;
                var teamStandings =
                    result["value"].AsBsonDocument["teams"].AsBsonDocument.Elements.ToList().First(
                        e => e.Value.AsBsonDocument["TeamId"].ToString() == entity.TeamId).ToBsonDocument()["Value"].
                        AsBsonDocument["TeamProgress"].AsBsonDocument.Elements.ToList();

                entity.Results.AddRange(teamStandings.OrderBy(r => r.Value.AsBsonDocument["MatchRound"].ToInt32()).Select(element => new TeamMatchResultEntity
                                                                            {
                                                                                MatchRound = element.Value.AsBsonDocument["MatchRound"].ToInt32(),
                                                                                HomeTeamName = element.Value.AsBsonDocument["HomeTeamName"].ToString(),
                                                                                HomeTeamGoals = element.Value.AsBsonDocument["HomeTeamGoals"].ToString(),
                                                                                AwayTeamName = element.Value.AsBsonDocument["AwayTeamName"].ToString(),
                                                                                AwayTeamGoals = element.Value.AsBsonDocument["AwayTeamGoals"].ToString(),
                                                                                ResultSymbol = element.Value.AsBsonDocument["Result"].ToString()
                                                                            }));
            }
        }

        #endregion
    }
}