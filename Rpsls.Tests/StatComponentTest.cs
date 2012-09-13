using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Raven.Client;
using Raven.Client.Embedded;
using Rpsls.Models;
using Rpsls.Components;
using Rpsls.Modules;
using Rpsls.Hubs;

namespace Rpsls.Tests
{
	public class StatComponentTest
	{
		IDocumentStore _store;
		IEngageComponent _engageComponent;
		IStatComponent _statComponent;
		User _user1;
		User _user2;

		public StatComponentTest ()
		{
			_engageComponent = new EngageComponent();

			_store = new EmbeddableDocumentStore
			{
			  RunInMemory = true
			};
			_store.Initialize();

			_statComponent = new StatComponent(_store);

			_user1 = new User();
			_user2 = new User();

			using (var session = _store.OpenSession())
			{
				session.Store(_user1);
				session.Store(_user2);
				session.SaveChanges();
			}

		}

		[Fact]
		public void Save_Engage_Win_Count()
		{
			var clientOne = new Client { LastMove = "Rock", UserId = _user1.Id };
			var clientTwo = new Client { LastMove = "Scissors", UserId = _user2.Id };

			var p = new Player(PlayerNumber.PlayerOne, clientOne.Gesture);
			var p2 = new Player(PlayerNumber.PlayerTwo, clientTwo.Gesture);

			PlayerNumber winner;
			var fought = _engageComponent.TryEngage(p, p2, out winner);

			var results = new MatchOutcome();
			results.PlayerNumberWinner = winner;
			if (results.PlayerNumberWinner == PlayerNumber.PlayerOne)
			{
				results.Winner = clientOne;
				results.Loser = clientTwo;
			}
			else if (results.PlayerNumberWinner == PlayerNumber.PlayerTwo)
			{
				results.Winner = clientTwo;
				results.Loser = clientOne;
			}
			else if (results.PlayerNumberWinner == PlayerNumber.None)
			{
				results.Winner = clientOne;
				results.Loser = clientTwo;
			}

			_statComponent.SaveStats(results);

			using (var session = _store.OpenSession())
			{
				var u1 = session.Load<User>(results.Winner.UserId);
				var u2 = session.Load<User>(results.Loser.UserId);

				Assert.Equal(1, u1.WinCount);
				Assert.Equal(1, u2.LostCount);
			}

		}

		[Fact]
		public void Save_Engage_Tie_Count()
		{
			var clientOne = new Client { LastMove = "Rock", UserId = _user1.Id };
			var clientTwo = new Client { LastMove = "Rock", UserId = _user2.Id };

			var p = new Player(PlayerNumber.PlayerOne, clientOne.Gesture);
			var p2 = new Player(PlayerNumber.PlayerTwo, clientTwo.Gesture);

			PlayerNumber winner;
			var fought = _engageComponent.TryEngage(p, p2, out winner);

			var results = new MatchOutcome();
			results.PlayerNumberWinner = winner;

			if (results.PlayerNumberWinner == PlayerNumber.PlayerOne)
			{
				results.Winner = clientOne;
				results.Loser = clientTwo;
			}
			else if (results.PlayerNumberWinner == PlayerNumber.PlayerTwo)
			{
				results.Winner = clientTwo;
				results.Loser = clientOne;
			}
			else if (results.PlayerNumberWinner == PlayerNumber.None)
			{
				results.Winner = clientOne;
				results.Loser = clientTwo;
			}

			_statComponent.SaveStats(results);

			using (var session = _store.OpenSession())
			{
				var u1 = session.Load<User>(results.Winner.UserId);
				var u2 = session.Load<User>(results.Loser.UserId);

				Assert.Equal(1, u1.TieCount);
				Assert.Equal(1, u2.TieCount);
			}

		}
	}
}
