using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;
using Rpsls.Models;

namespace Rpsls.Helpers.Indexes
{
	public class MatchEncounterIndex : AbstractIndexCreationTask<MatchEncounter, MatchEncounterIndexResult>
	{
		public MatchEncounterIndex()
		{
			Map = encounters => from encounter in encounters
								select new { UserId = encounter.User.Id, MatchResult = encounter.Result, Gesture = encounter.UserGestureType, Count = 1 };

			Reduce = results => from result in results
								group result by new { result.UserId, result.Gesture, result.MatchResult } into agg
								select new { UserId = agg.Key.UserId, MatchResult = agg.Key.MatchResult, Gesture = agg.Key.Gesture, Count = agg.Sum(x => x.Count) };


		}
	}
}