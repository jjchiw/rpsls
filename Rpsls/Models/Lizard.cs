using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models
{
	public class Lizard : Player
	{
		public Lizard(PlayerNumber number)
		{
			Number = number;
			GType = GestureType.Lizard;
			WinTo = new List<GestureType>
			{
				GestureType.Spock, GestureType.Paper
			};
		}
	}
}