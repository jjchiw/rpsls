using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models
{
	public class Spock : Player
	{
		public Spock(PlayerNumber number)
		{
			Number = number;
			GType = GestureType.Spock;
			WinTo = new List<GestureType>
			{
				GestureType.Scissor, GestureType.Rock
			};
		}
	}
}