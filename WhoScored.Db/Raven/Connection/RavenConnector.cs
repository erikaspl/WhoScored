using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Raven.Client.Document;

namespace WhoScored.Db.Connection
{
    public class RavenConnector
    {
        public static DocumentStore CreateConnection()
        {
            var documentStore = new DocumentStore {ConnectionStringName = "RavenDbConncetion"};
            documentStore.Initialize();
            return documentStore;
        }
    }
}
