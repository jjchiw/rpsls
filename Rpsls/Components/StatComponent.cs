using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Rpsls.Models;
using Rpsls.Hubs;

namespace Rpsls.Components
{
	public class StatComponent : IStatComponent
	{
		IDocumentStore _store;

		public StatComponent(IDocumentStore store)
		{
			_store = store;
		}

		public void SaveStats(MatchOutcome results)
		{
			using (var session = _store.OpenSession())
			{
				var dateTime = DateTime.UtcNow;
				MatchEncounter m1;
				MatchEncounter m2;
				var winner = session.Load<User>(results.Winner.UserId);
				var loser = session.Load<User>(results.Loser.UserId);

				PrepareDataToSave(results, dateTime, winner, loser, out m1, out m2);

				session.Store(m1);
				session.Store(m2);
				session.SaveChanges();

			}
		}

		private static void PrepareDataToSave(MatchOutcome results, DateTime dateTime, User winner, User loser, out MatchEncounter m1, out MatchEncounter m2)
		{
			m1 = null;
			m2 = null;

			if (results.PlayerNumberWinner != PlayerNumber.None)
			{
				m1 = new MatchEncounter
				{
					User = winner,
					UserRival = loser,
					UserGestureType = results.Winner.Gesture,
					UserRivalGestureType = results.Loser.Gesture,
					Result = MatchResult.Win,
					Date = dateTime
				};

				m2 = new MatchEncounter
				{
					User = loser,
					UserRival = winner,
					UserGestureType = results.Loser.Gesture,
					UserRivalGestureType = results.Winner.Gesture,
					Result = MatchResult.Lose,
					Date = dateTime
				};

				winner.WinCount++;
				loser.LostCount++;
			}
			else
			{
				m1 = new MatchEncounter
				{
					User = winner,
					UserRival = loser,
					UserGestureType = results.Winner.Gesture,
					UserRivalGestureType = results.Loser.Gesture,
					Result = MatchResult.Tie,
					Date = dateTime
				};

				m2 = new MatchEncounter
				{
					User = loser,
					UserRival = winner,
					UserGestureType = results.Loser.Gesture,
					UserRivalGestureType = results.Winner.Gesture,
					Result = MatchResult.Tie,
					Date = dateTime
				};

				winner.TieCount++;
				loser.TieCount++;
			}
		}
	}
}