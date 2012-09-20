using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace WhoScored.Services.Migration
{
    public class MessageConventions : IWantToRunBeforeConfiguration
    {
        public void Init()
        {
            Configure.Instance
            .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("WhoScored.Services.InternalMessages"))
            .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("WhoScored.Services.Contract"));
        }
    }
}

