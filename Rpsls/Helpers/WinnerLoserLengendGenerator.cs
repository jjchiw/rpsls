using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Hubs;

namespace Rpsls.Helpers
{
	public class WinnerLoserLengendGenerator
	{

		private static Dictionary<string, string> verbs = new Dictionary<string, string> { { "Scissors+Paper" , "Cuts" },
																					{ "Paper+Rock", "Covers"},
																					{ "Rock+Lizard", "Crushes"},
																					{ "Lizard+Spock", "Poisons"},
																					{ "Spock+Scissors", "Smashes"},
																					{ "Scissors+Lizard", "Decapites"},
																					{ "Lizard+Paper", "Eats"},
																					{ "Paper+Spock", "Disproves"},
																					{ "Spock+Rock", "Vaporizes"},
																					{ "Rock+Scissors", "Crushes"},
																				  };

		public static WinnerLoserLegend GenerateLegend(Client winner, Client loser)
		{
			var legend = "Tie";
			var key = winner.LastMove + "+" + loser.LastMove;
			var tie = !verbs.ContainsKey(key);
			if (tie)
			{
				legend = String.Format("<a href='{0}' target='_blank'>{1}</a>'s {2} {3} <a href='{4}' target='_blank'>{5}</a>'s {6}.", winner.UserId, winner.Name, winner.LastMove, "Ties", loser.UserId, loser.Name, loser.LastMove);
			}
			else
			{
				legend = String.Format("<a href='{0}' target='_blank'>{1}</a>'s {2} {3} <a href='{4}' target='_blank'>{5}</a>'s {6}.", winner.UserId, winner.Name, winner.LastMove, verbs[key], loser.UserId, loser.Name, loser.LastMove);
			}

			return new WinnerLoserLegend(legend, tie);
		}
	}

	public struct WinnerLoserLegend
	{
		private string _winnerLegend;
		private string _loserLegend;

		public string WinnerLegend { get { return _winnerLegend; } }
		public string LoserLegend { get { return _loserLegend; } }

		public WinnerLoserLegend(string legend, bool tie)
		{
			_winnerLegend = legend;
			_loserLegend = legend;

			if (!tie)
			{
				_winnerLegend = String.Format("{0} You Win.", legend);
				_loserLegend = String.Format("{0} You Lost.", legend);
			}
			
		}
	}
}