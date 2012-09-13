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
				var u1 = session.Load<User>(results.Winner.UserId);
				var u2 = session.Load<User>(results.Loser.UserId);

				PrepareDataToSave(results, dateTime, u1, u2, out m1, out m2);

				session.Store(m1);
				session.Store(m2);
				session.SaveChanges();

			}
		}

		private static void PrepareDataToSave(MatchOutcome results, DateTime dateTime, User u1, User u2, out MatchEncounter m1, out MatchEncounter m2)
		{
			m1 = null;
			m2 = null;

			if (results.PlayerNumberWinner != PlayerNumber.None)
			{
				m1 = new MatchEncounter
				{
					UserId = results.Winner.UserId,
					UserIdRival = results.Loser.UserId,
					UserGestureType = results.Winner.Gesture,
					UserRivalGestureType = results.Loser.Gesture,
					Result = MatchResult.Win,
					Date = dateTime
				};

				m2 = new MatchEncounter
				{
					UserId = results.Loser.UserId,
					UserIdRival = results.Winner.UserId,
					UserGestureType = results.Loser.Gesture,
					UserRivalGestureType = results.Winner.Gesture,
					Result = MatchResult.Lose,
					Date = dateTime
				};

				u1.WinCount++;
				u2.LostCount++;
			}
			else
			{
				m1 = new MatchEncounter
				{
					UserId = results.Winner.UserId,
					UserIdRival = results.Loser.UserId,
					UserGestureType = results.Winner.Gesture,
					UserRivalGestureType = results.Loser.Gesture,
					Result = MatchResult.Tie,
					Date = dateTime
				};

				m2 = new MatchEncounter
				{
					UserId = results.Loser.UserId,
					UserIdRival = results.Winner.UserId,
					UserGestureType = results.Loser.Gesture,
					UserRivalGestureType = results.Winner.Gesture,
					Result = MatchResult.Tie,
					Date = dateTime
				};

				u1.TieCount++;
				u2.TieCount++;
			}
		}
	}
}