using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models
{
	public class Rock : Player
	{
		public Rock(PlayerNumber number)
		{
			Number = number;
			GType = GestureType.Rock;
			WinTo = new List<GestureType>
			{
				GestureType.Lizard, GestureType.Scissor
			};
		}
	}
}