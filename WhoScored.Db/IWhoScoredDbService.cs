using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoScored.Model;

namespace WhoScored.Db
{
    public interface IWhoScoredDbService
    {
        void SaveWorldDetails(List<IWorldDetails> worldDetails);

        List<IWorldDetails> GetWorldDetails();
    }
}
