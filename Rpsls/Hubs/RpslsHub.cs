using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using Rpsls.Models;
using Rpsls.Components;
using Rpsls.Modules;
using Rpsls.Helpers;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using System.Threading.Tasks;
using Raven.Client;

namespace Rpsls.Hubs
{
	[HubName("rpslsHub")]
	public class RpslsHub : Hub
	{
		private readonly int TimeoutInSeconds = 30;
		private readonly FreeForAll _freeForAll;
		private readonly IEngageComponent _engageComponent;
		private readonly IStatComponent _statComponent;
		private readonly IDocumentStore _store;

		public RpslsHub() : this(FreeForAll.Instance) 
		{
		}

		public RpslsHub(FreeForAll freeForAll)
		{
			_freeForAll = freeForAll;
			_freeForAll.Hub = this;

			_engageComponent = new EngageComponent();

			var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
			parser.Parse();

			_store = new DocumentStore
			{
				ApiKey = parser.ConnectionStringOptions.ApiKey,
				Url = parser.ConnectionStringOptions.Url
			};

			_store.Initialize();

			_statComponent = new StatComponent(_store);
		}

		public void Join(string playerName, string email)
		{
			using (var session = _store.OpenSession())
			{
				var user = session.Query<User>().FirstOrDefault(x => x.Email == email);

				if (user == null)
				{
					session.Store(user);
					session.SaveChanges();
				}

				if (FreeForAll.Clients.Where(x => x.Name.Equals(playerName)).Count().Equals(0))
				{
					var client = new Client() { Name = playerName, LastMoveResponse = null, LastMove = "", Waiting = false, Id = Context.ConnectionId, UserId = user.Id };
					FreeForAll.Clients.Add(client);
					Caller.Name = playerName;
				}
				else
				{
					var clientIndex = FreeForAll.Clients.Select((x, i) => new { Name = x.Name, Index = i }).Where(x => x.Name == playerName).FirstOrDefault();
					if (clientIndex != null)
					{
						FreeForAll.Clients.RemoveAt(clientIndex.Index);
						var client = new Client() { Name = playerName, LastMoveResponse = null, LastMove = "", Waiting = false, Id = Context.ConnectionId, UserId = user.Id };
						FreeForAll.Clients.Add(client);
						Caller.Name = playerName;
					}

					//throw new Exception("This login is already in use");
				}
			}

			
			var message = string.Format("{0} entered to play", playerName);
			Clients.addMessage(message);

			var clientToSendMessage = FreeForAll.Clients.Where(x => x.Waiting && x.LastMoveResponse.HasValue && x.Id != Context.ConnectionId).
														OrderBy(x => x.LastMoveResponse).FirstOrDefault();

			if (clientToSendMessage != null)
			{
				message = string.Format("{0} is waiting for a move.", clientToSendMessage.Name);
				Caller.addMessage(message);
			}

			Clients.totalPlayers(FreeForAll.Clients.Count);
		}

		public void DismissMove()
		{
			var client = FreeForAll.Clients.Where(x => x.Id == Context.ConnectionId).FirstOrDefault();

			if (client == null)
				return;

			client.Reset();
		}

		public void SendMoveServer(string lastMove)
		{
			var client = FreeForAll.Clients.Where(x => x.Id == Context.ConnectionId).FirstOrDefault();

			if (client == null)
				return;

			if (client.Waiting)
			{
				Caller.addWarning(string.Format("You've a move waiting to be resolved."));
				return;
			}

			var clientToSendMessage = FreeForAll.Clients.Where(x => x.Waiting && x.LastMoveResponse.HasValue && x.Id != client.Id).
														OrderBy(x => x.LastMoveResponse).FirstOrDefault();

			if (clientToSendMessage != null)
			{
				Caller.moveAcceptedToPlay(lastMove);
				client.LastMove = lastMove;
				var outcome = Engage(client, clientToSendMessage);

				if (outcome.Winner == null)
				{
					Caller.play(outcome.WinnerLegend, client.LastMove, clientToSendMessage.LastMove, "tie");
					Clients[clientToSendMessage.Id].play(outcome.LoserLegend, clientToSendMessage.LastMove, client.LastMove, "tie");
				}
				else if (outcome.Winner.Id == client.Id)
				{
					Caller.play(outcome.WinnerLegend, client.LastMove, clientToSendMessage.LastMove, "win");
					Clients[clientToSendMessage.Id].play(outcome.LoserLegend, clientToSendMessage.LastMove, client.LastMove, "lose");
				}
				else
				{
					Caller.play(outcome.LoserLegend, client.LastMove, clientToSendMessage.LastMove, "lose");
					Clients[clientToSendMessage.Id].play(outcome.WinnerLegend, clientToSendMessage.LastMove, client.LastMove, "win");
				}

				client.Reset();
				clientToSendMessage.Reset();
				
				return;
			}

			client.LastMoveResponse = DateTime.UtcNow;
			client.LastMove = lastMove;
			client.Waiting = true;

			Caller.moveAccepted(lastMove);
			Caller.addMessage(string.Format("your move: {0}. Waiting for player", lastMove));
			Clients.addMessage(string.Format("{0}, made a move", client.Name));

		}

		public void Disconnect()
		{
			Clients.leave(Context.ConnectionId, DateTime.Now.ToString());

			var client = FreeForAll.Clients.Where(x => x.Id == Context.ConnectionId).FirstOrDefault();
			FreeForAll.Clients.Remove(client);

			Clients.totalPlayers(FreeForAll.Clients.Count);
		}

		private MatchOutcome Engage(Client clientOne, Client clientTwo)
		{
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
			else if (winner == PlayerNumber.None)
			{
				results.Winner = clientOne;
				results.Loser = clientTwo;
			}

			_statComponent.SaveStats(results);

			var legends = WinnerLoserLengendGenerator.GenerateLegend(results.Winner, results.Loser);

			results.WinnerLegend = legends.WinnerLegend;
			results.LoserLegend = legends.LoserLegend;

			if (winner == PlayerNumber.None)
			{
				results.Winner = null;
				results.Loser = null;
			}
				

			return results;
		}
	}
}