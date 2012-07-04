using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Rpsls.Models;
using Rpsls.Components;

namespace Rpsls.Modules
{
	public class RootModule : NancyModule
	{
		public RootModule()
		{
			Get["/"] = Root;
			Post["/game"] = Game;
		}

		private Response Game(dynamic o)
		{
			GestureType outcome;
			Console.WriteLine((Request.Form.gesture as string));
			var parsed = Enum.TryParse<GestureType>(Request.Form.gesture.Value, out outcome);
			if (!parsed)
				return HttpStatusCode.NotImplemented;

			var eng = new EngageComponent();

			var p = new Player(PlayerNumber.PlayerOne, outcome);
			var p2 = new Player(PlayerNumber.PlayerTwo, eng.RandomGesture());

			PlayerNumber winner;
			var fought = eng.TryEngage(p, p2, out winner);

			var results = new Outcome();
			results.Winner = winner;
			results.PlayerTwoGesture = p2.GType.ToString();

			if (!fought)
				results.Legend = "Tie";

			if(winner == PlayerNumber.PlayerOne)
				results.Gesture= p.GType.ToString();
			else
				results.Gesture = p2.GType.ToString();

			results.Legend = "Winner " + results.Winner + " with " + results.Gesture;

			return Response.AsJson<Outcome>(results);
		}

		private Response Root(dynamic o)
		{
			return View["Index"];
		}
	}

	public class Outcome
	{
		public PlayerNumber Winner { get; set; }
		public string PlayerTwoGesture { get; set; }
		public string Gesture { get; set; }
		public string Legend { get; set; }
	}
}