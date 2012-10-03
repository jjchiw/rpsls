using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;
using Rpsls.Models;

namespace Rpsls.Helpers.Indexes
{
	public class MatchEncounterLoseIndex : AbstractIndexCreationTask<MatchEncounter, MatchEncounterIndexResult>
	{
		public MatchEncounterLoseIndex()
		{
			Map = encounters => from encounter in encounters
								where encounter.Result == Hubs.MatchResult.Lose
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