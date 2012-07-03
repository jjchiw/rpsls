using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Components;

namespace Rpsls.Models
{
	public abstract class Player
	{
		public GestureType GType { get; set; }
		public PlayerNumber Number { get; set; }
		protected IList<GestureType> WinTo { get; set; }

		public bool WinsOver(Player player)
		{
			return WinTo.Any(x => x == player.GType);
		}
	}
}