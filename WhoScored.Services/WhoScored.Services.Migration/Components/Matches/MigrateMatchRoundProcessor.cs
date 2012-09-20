using System;
using NServiceBus;
using WhoScoredServices.InternalMessages.Matches;


namespace WhoScored.Services.Migration.Components.Matches
{
	public partial class MigrateMatchRoundProcessor
	{
		
        partial void HandleImplementation(MigrateMatchRound message)
        {
            // Implement your handler logic here.
            Console.WriteLine("Matches received " + message.GetType().Name);
        }

	}
}