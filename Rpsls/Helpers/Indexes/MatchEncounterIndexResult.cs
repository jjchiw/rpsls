using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpsls.Models;

namespace Rpsls.Helpers.Indexes
{
	public class MatchEncounterIndexResult
	{
		public string UserId { get; set; }
		public Hubs.MatchResult MatchResult { get; set; }
		public GestureType Gesture { get; set; }
		public int Count { get; set; }
	}
}
