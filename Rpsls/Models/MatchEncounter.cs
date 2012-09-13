using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Hubs;

namespace Rpsls.Models
{
	public class MatchEncounter
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public string UserIdRival { get; set; }
		public GestureType UserGestureType { get; set; }
		public GestureType UserRivalGestureType { get; set; }
		public MatchResult Result { get; set; }
		public DateTime Date { get; set; }
	}
}