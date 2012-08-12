using System.IO;
using System.Reflection;


namespace WhoScored.Db.Mongo
{
    public class EmbededFileReader : IFileReader
    {
        public string ReadFile(string name, string mapName)
        {
            string result = string.Empty;
            var assembly = Assembly.GetAssembly(typeof(EmbededFileReader));

            using (var stream = new StreamReader(assembly.
                GetManifestResourceStream(string.Format("{0}.{1}.{2}.{3}", assembly.GetName().Name, "Mongo.MapReduce", name, mapName))))
            {
                result = stream.ReadToEnd();
            }
            return result;
        }
    }
}
