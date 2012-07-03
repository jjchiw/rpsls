using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Models;

namespace Rpsls.Components
{
	public class EngageComponent
	{
		public bool TryEngage(Player playerOne, Player playerTwo, out PlayerNumber outcome)
		{
			outcome = PlayerNumber.None;

			if (playerOne.GType == playerTwo.GType)
				return false;

			if (playerOne.WinsOver(playerTwo))
			{
				outcome = playerOne.Number;
				return true;
			}

			if (playerTwo.WinsOver(playerOne))
			{
				outcome = playerTwo.Number;
				return true;
			}

			throw new NotImplementedException();
		}
	}
}