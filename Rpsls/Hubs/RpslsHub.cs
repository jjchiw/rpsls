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

		public void Join(string myName)
		{
			//using(var session = docu)

			if (FreeForAll.Clients.Where(x => x.Name.Equals(myName)).Count().Equals(0))
			{
				var client = new Client() { Name = myName, LastMoveResponse = null, LastMove = "", Waiting = false, Id = Context.ConnectionId };
				FreeForAll.Clients.Add(client);
				Caller.Name = myName;
			}
			else
				throw new Exception("This login is already in use");
		}

		public void SendMoveServer(string lastMove)
		{
			var client = FreeForAll.Clients.Where(x => x.Name == Caller.Name).FirstOrDefault();

			var clientToSendMessage = FreeForAll.Clients.Where(x => x.Waiting && x.LastMoveResponse.HasValue && x.Id != client.Id).
														OrderBy(x => x.LastMoveResponse).FirstOrDefault();

			if (clientToSendMessage != null)
			{
				client.LastMove = lastMove;
				var outcome = Engage(client, clientToSendMessage);

				if (outcome.Winner == null)
				{
					Caller.addMessage(outcome.WinnerLegend, clientToSendMessage.LastMove);
					Clients[clientToSendMessage.Id].addMessage(outcome.LoserLegend, client.LastMove);
				}
				else if (outcome.Winner.Id == client.Id)
				{
					Caller.addMessage(outcome.WinnerLegend, clientToSendMessage.LastMove);
					Clients[clientToSendMessage.Id].addMessage(outcome.LoserLegend, client.LastMove);
				}
				else
				{
					Caller.addMessage(outcome.LoserLegend, clientToSendMessage.LastMove);
					Clients[clientToSendMessage.Id].addMessage(outcome.WinnerLegend, client.LastMove);
				}

				client.Reset();
				clientToSendMessage.Reset();
				
				return;
			}

			client.LastMoveResponse = DateTime.UtcNow;
			client.LastMove = lastMove;
			client.Waiting = true;
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