using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models
{
	public class Scissor : Player
	{
		public Scissor(PlayerNumber number)
		{
			Number = number;
			GType = GestureType.Scissor;
			WinTo = new List<GestureType>
			{
				GestureType.Paper, GestureType.Lizard
			};
		}
	}
}