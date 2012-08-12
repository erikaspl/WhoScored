using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Db.Mongo
{
    public interface IFileReader
    {
        string ReadFile(string name, string mapName);
    }
}
