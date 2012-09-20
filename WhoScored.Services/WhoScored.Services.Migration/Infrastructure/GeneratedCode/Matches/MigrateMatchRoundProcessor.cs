using System;
using NServiceBus;
using WhoScoredServices.InternalMessages.Matches;


namespace WhoScored.Services.Migration.Components.Matches
{
    public partial class MigrateMatchRoundProcessor : IHandleMessages<MigrateMatchRound>
    {
		
		public void Handle(MigrateMatchRound message)
		{
			this.HandleImplementation(message);
		}

		partial void HandleImplementation(MigrateMatchRound message);

		public IBus Bus { get; set; }

    }
}