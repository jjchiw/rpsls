using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Models;

namespace Rpsls.Hubs
{
	public enum MatchResult
	{
		Win, Lose, Tie
	}

	public class MatchOutcome
	{
		public Client Winner { get; set; }
		public Client Loser { get; set; }
		public string WinnerLegend { get; set; }
		public string LoserLegend { get; set; }
		public PlayerNumber PlayerNumberWinner { get; set; }
	}
}