using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models.ViewModels
{
	public class MatchEncounterView
	{
		public string User { get; set; }
		public string UserRival { get; set; }
		public string UserGesture { get; set; }
		public string UserRivalGesture { get; set; }
		public string Result { get; set; }
		public string Date { get; set; }
	}
}