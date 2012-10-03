using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpsls.Models;
using Raven.Client.Indexes;
using Xunit;
using Raven.Client.Document;

namespace Rpsls.Tests
{

	public class MatchEncounterWinIndex : AbstractIndexCreationTask<MatchEncounter, MatchEncounterIndexResult>
	{
		public MatchEncounterWinIndex()
		{
			Map = encounters => from encounter in encounters 
								where encounter.Result == Hubs.MatchResult.Win
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

	public class MatchEncounterIndexResult
	{
		public string UserId { get; set; }
		public GestureType Gesture { get; set; }
		public int Count { get; set; }
	}

	

	public class RavenIndexesTest
	{
		[Fact]
		public void Test_Match_Encounter_Win_Index()
		{
			//using (var documentStore = new DocumentStore() { Url = "http://localhost:8080/databases/rpsls" })
			//{
			//    documentStore.Initialize();

			//    using (var session = documentStore.OpenSession())
			//    {
			//        IndexCreation.CreateIndexes(typeof(MatchEncounterWinIndex).Assembly, documentStore);

			//        //var encounters = session.Query<MatchEncounter>().Where(x => x.Result == "Win");
			//        var encounters = session.Query<MatchEncounterIndexResult, MatchEncounterWinIndex>()
			//                                .Select(x => x).ToList();

			//        Assert.True(encounters.Count() > 0);

			//        foreach (var item in encounters)
			//        {
			//            Console.WriteLine(item.UserId + " - " + item.Gesture + " - " + item.Count);
			//        }

			//    }
			//}
		}

		[Fact]
		public void Test_Match_Encounter_Lose_Index()
		{
			//using (var documentStore = new DocumentStore() { Url = "http://localhost:8080/databases/rpsls" })
			//{
			//    documentStore.Initialize();

			//    using (var session = documentStore.OpenSession())
			//    {
			//        IndexCreation.CreateIndexes(typeof(MatchEncounterLoseIndex).Assembly, documentStore);

			//        var encounters = session.Query<MatchEncounter>().Where(x => x.Result == "Win");
			//        var encounters = session.Query<MatchEncounterIndexResult, MatchEncounterLoseIndex>()
			//                                .Select(x => x).ToList();

			//        Assert.True(encounters.Count() > 0);

			//        foreach (var item in encounters)
			//        {
			//            Console.WriteLine(item.UserId + " - " + item.Gesture + " - " + item.Count);
			//        }

			//    }
			//}
		}

		[Fact]
		public void Test_Match_Encounter_Tie_Index()
		{
			//using (var documentStore = new DocumentStore() { Url = "http://localhost:8080/databases/rpsls" })
			//{
			//    documentStore.Initialize();

			//    using (var session = documentStore.OpenSession())
			//    {
			//        IndexCreation.CreateIndexes(typeof(MatchEncounterLoseIndex).Assembly, documentStore);

			//        //var encounters = session.Query<MatchEncounter>().Where(x => x.Result == "Win");
			//        var encounters = session.Query<MatchEncounterIndexResult, MatchEncounterTieIndex>()
			//                                .Select(x => x).ToList();

			//        Assert.True(encounters.Count() > 0);

			//        foreach (var item in encounters)
			//        {
			//            Console.WriteLine(item.UserId + " - " + item.Gesture + " - " + item.Count);
			//        }

			//    }
			//}
		}
	}
}
