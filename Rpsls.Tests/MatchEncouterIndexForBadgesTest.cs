using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Rpsls.Models;

namespace Rpsls.Tests
{
	public class MatchEncouterIndexForBadgesTest
	{
		//public class MatchEncouterIndexForBadges : AbstractIndexCreationTask<MatchEncounter, MatchEncounterIndexResult>
		//{
		//	public MatchEncouterIndexForBadges()
		//	{
		//		Map = encounters => from encounter in encounters
		//							where !encounter.Counted
		//							select new { UserId = encounter.User.Id, Gesture = encounter.UserGestureType, Count = 1 };

		//		Reduce = results => from result in results
		//							group result by new { result.UserId, result.Gesture } into agg
		//							select new { UserId = agg.Key.UserId, Gesture = agg.Key.Gesture, Count = agg.Sum(x => x.Count) };
		//	}
		//}
	}
}
