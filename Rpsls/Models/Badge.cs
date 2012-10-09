using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Hubs;
using Rpsls.Models.Helpers;

namespace Rpsls.Models
{
	public class Badge : IBadgeDenormalized
	{
		public string Id { get; set; }
		public MatchResult MathchResult { get; set; }
		public GestureType Gesture { get; set; }
		public bool IsStrike { get; set; }
		public int Limit { get; set; }
		public string Name { get; set; }
		public int Total { get; set; }
	}
}