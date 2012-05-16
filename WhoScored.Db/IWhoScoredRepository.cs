using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoScored.Model;

namespace WhoScored.Db
{
    public interface IWhoScoredRepository
    {
        void SaveWorldDetails<T>(List<T> worldDetails) where T : class, IWorldDetails;

        void SaveWorldDetails<T>(T worldDetail) where T : class, IWorldDetails;

        List<T> GetWorldDetails<T>() where T : class, IWorldDetails;

        T GetWorldDetails<T>(int countryId) where T : class, IWorldDetails;

        void DropWorldDetails();

        void SaveSettings<T>(T settings) where T : class, ISettings;

        T GetSettings<T>() where T : class, ISettings;

        void SaveLeagueDetails<T>(List<T> leagueDetails) where T : class, ILeagueDetails;

        void SaveLeagueDetails<T>(T leagueDetail) where T : class, ILeagueDetails;

        List<T> GetLeagueDetails<T>() where T : class, ILeagueDetails;

        List<T> GetLeagueDetails<T>(string countryId) where T : class, ILeagueDetails;

        void DropLeagueDetails();
    }
}
