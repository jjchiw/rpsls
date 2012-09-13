using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Hubs;

namespace Rpsls.Components
{
	public interface IStatComponent
	{
		void SaveStats(MatchOutcome results);
	}
}