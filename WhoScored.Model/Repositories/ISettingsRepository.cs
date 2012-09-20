using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model.Repositories
{
    public interface ISettingsRepository : IRepository<Settings>
    {
        void ResetDatabase();
    }
}
