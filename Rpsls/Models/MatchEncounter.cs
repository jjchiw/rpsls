using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Hubs;
using Rpsls.Models.Helpers;

namespace Rpsls.Models
{
	public class MatchEncounter
	{
		public string Id { get; set; }
		public UserDenormalized<User> User { get; set; }
		public UserDenormalized<User>	 UserRival { get; set; }
		public GestureType UserGestureType { get; set; }
		public GestureType UserRivalGestureType { get; set; }
		public MatchResult Result { get; set; }
		public DateTime Date { get; set; }
	}
}