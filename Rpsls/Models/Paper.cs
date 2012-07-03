using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models
{
	public class Paper : Player
	{
		public Paper(PlayerNumber number)
		{
			Number = number;
			GType = GestureType.Paper;
			WinTo = new List<GestureType>
			{
				GestureType.Rock, GestureType.Spock
			};
		}
	}
}