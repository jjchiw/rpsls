using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;
using Rpsls.Models;

namespace Rpsls.Helpers.Indexes
{
	public class MatchEncounterTieIndex : AbstractIndexCreationTask<MatchEncounter, MatchEncounterIndexResult>
	{
		public MatchEncounterTieIndex()
		{
			Map = encounters => from encounter in encounters
								where encounter.Result == Hubs.MatchResult.Tie
								select new { UserId = encounter.User.Id, Gesture = encounter.UserGestureType, Count = 1 };
			//select new { UserId = encounter.UserId,  Count = 0 };

			Reduce = results => from result in results
								group result by new { result.UserId, result.Gesture } into agg
								//group result by result.Key into g
								//group result by result.UserId into g
								select new { UserId = agg.Key.UserId, Gesture = agg.Key.Gesture, Count = agg.Sum(x => x.Count) };
			//select new { UserId = g.Key, Count = g.Count() };


		}
	}
}