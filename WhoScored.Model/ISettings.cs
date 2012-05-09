using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface ISettings
    {
        Guid Id { get; set; }
        int GlobalSeason { get; set; }
    }
}
