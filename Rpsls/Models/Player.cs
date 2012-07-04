using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Components;

namespace Rpsls.Models
{
	public class Player
	{
		public Player(PlayerNumber number, GestureType gType)
		{
			GType = gType;
			Number = number;
		}

		public GestureType GType { get; set; }
		public PlayerNumber Number { get; set; }
		protected IList<GestureType> WinTo { get; set; }
	}
}