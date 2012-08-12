using System.IO;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace WhoScored.Db.Mongo
{
    public class WhoScoredMapReduce
    {
        private const string FINALIZE_FILE_NAME = "finalize.js";
        private const string MAP_FILE_NAME = "map.js";
        private const string REDUCE_FILE_NAME = "reduce.js";
        
        public MapReduceResult MapReduce(MongoCollection collection, MapReduceOptionsBuilder options, string name, IFileReader fileReader)
        {
            options.SetFinalize(ReadFinalizeFile(name, fileReader));
            return collection.MapReduce(ReadMapFile(name, fileReader), ReadReduceFile(name, fileReader), options);
        }

        private BsonJavaScript ReadReduceFile(string name, IFileReader fileReader)
        {
            return fileReader.ReadFile(name, REDUCE_FILE_NAME);
        }

        private BsonJavaScript ReadMapFile(string name, IFileReader fileReader)
        {
            return fileReader.ReadFile(name, MAP_FILE_NAME);
        }

        private BsonJavaScript ReadFinalizeFile(string name, IFileReader fileReader)
        {
            return fileReader.ReadFile(name, FINALIZE_FILE_NAME);
        }
    }
}