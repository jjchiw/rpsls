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

namespace Rpsls.Hubs
{
	[HubName("rpslsHub")]
	public class RpslsHub : Hub
	{
		private readonly int TimeoutInSeconds = 30;
		private readonly FreeForAll _freeForAll;

		public RpslsHub() : this(FreeForAll.Instance) { }

		public RpslsHub(FreeForAll freeForAll)
		{
			_freeForAll = freeForAll;
			_freeForAll.Hub = this;

			//var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
			//parser.Parse();

			//var documentStore = new DocumentStore
			//{
			//    ApiKey = parser.ConnectionStringOptions.ApiKey,
			//    Url = parser.ConnectionStringOptions.Url
			//};

			//documentStore.Initialize();
		}

		public void Join(string playerName)
		{
			//using(var session = docu)

			if (FreeForAll.Clients.Where(x => x.Name.Equals(playerName)).Count().Equals(0))
			{
				var client = new Client() { Name = playerName, LastMoveResponse = null, LastMove = "", Waiting = false, Id = Context.ConnectionId };
				FreeForAll.Clients.Add(client);
				Caller.Name = playerName;
			}
			else
			{
				var clientIndex = FreeForAll.Clients.Select((x, i) => new { Name = x.Name, Index = i }).Where(x => x.Name == playerName).FirstOrDefault();
				if (clientIndex != null)
				{
					FreeForAll.Clients.RemoveAt(clientIndex.Index);
					var client = new Client() { Name = playerName, LastMoveResponse = null, LastMove = "", Waiting = false, Id = Context.ConnectionId };
					FreeForAll.Clients.Add(client);
					Caller.Name = playerName;
				}
				
				//throw new Exception("This login is already in use");
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
					Caller.play(outcome.WinnerLegend, clientToSendMessage.LastMove);
					Clients[clientToSendMessage.Id].play(outcome.LoserLegend, client.LastMove);
				}
				else if (outcome.Winner.Id == client.Id)
				{
					Caller.play(outcome.WinnerLegend, clientToSendMessage.LastMove);
					Clients[clientToSendMessage.Id].play(outcome.LoserLegend, client.LastMove);
				}
				else
				{
					Caller.play(outcome.LoserLegend, clientToSendMessage.LastMove);
					Clients[clientToSendMessage.Id].play(outcome.WinnerLegend, client.LastMove);
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

		}

		public void Disconnect()
		{
			Clients.leave(Context.ConnectionId, DateTime.Now.ToString());

			var client = FreeForAll.Clients.Where(x => x.Id == Context.ConnectionId).FirstOrDefault();
			FreeForAll.Clients.Remove(client);

			Clients.totalPlayers(FreeForAll.Clients.Count);
		}

		private Outcome Engage(Client clientOne, Client clientTwo)
		{

			var eng = new EngageComponent();

			var p = new Player(PlayerNumber.PlayerOne, clientOne.Gesture);
			var p2 = new Player(PlayerNumber.PlayerTwo, clientTwo.Gesture);

			PlayerNumber winner;
			var fought = eng.TryEngage(p, p2, out winner);

			var results = new Outcome();
			if (winner == PlayerNumber.PlayerOne)
			{
				results.Winner = clientOne;
				results.Loser = clientTwo;
			}
			else if (winner == PlayerNumber.PlayerTwo)
			{
				results.Winner = clientTwo;
				results.Loser = clientOne;
			}
			else if (winner == PlayerNumber.None)
			{
				results.Winner = clientOne;
				results.Loser = clientTwo;
			}

			var legends = WinnerLoserLengendGenerator.GenerateLegend(results.Winner, results.Loser);

			results.WinnerLegend = legends.WinnerLegend;
			results.LoserLegend = legends.LoserLegend;

			return results;
		}

		public class Outcome
		{
			public Client Winner { get; set; }
			public Client Loser { get; set; }
			public string WinnerLegend { get; set; }
			public string LoserLegend { get; set; }
		}
	}
}