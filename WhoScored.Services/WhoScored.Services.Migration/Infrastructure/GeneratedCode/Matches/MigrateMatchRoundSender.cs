using System;
using NServiceBus;
using NServiceBus.Config;
using WhoScoredServices.InternalMessages.Matches;

namespace WhoScored.Services.Migration.Components.Matches
{
    public partial class MigrateMatchRoundSender: IMigrateMatchRoundSender, WhoScored.Services.Migration.Infrastructure.INServiceBusComponent
    {
        public void Send(MigrateMatchRound message)
		{
			Bus.Send(message);	
		}

        public IBus Bus { get; set; }
    }

    public interface IMigrateMatchRoundSender
    {
        void Send(MigrateMatchRound message);
    }
}