using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;
using Rpsls.Models;

namespace Rpsls.Helpers.Indexes
{
	public class MatchEncountersByUserId : AbstractIndexCreationTask<MatchEncounter>
	{
		public MatchEncountersByUserId()
		{
			Map = encounters => from encounter in encounters
								select new { User = encounter.User.Id, UserRival = encounter.UserRival.Id };
		}

	}
}