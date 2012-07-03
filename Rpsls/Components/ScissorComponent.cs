using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Models;

namespace Rpsls.Components
{
	public class ScissorEngageComponent : IEngageComponent
	{
		#region IEngageComponent Members

		public bool TryEngage(IPlayer playerOne, IPlayer playerTwo, out PlayerNumber number)
		{
			number = PlayerNumber.None;
			if (playerOne.GType != GestureType.Scissor) return false;

			switch (playerTwo.GType)
			{
				case GestureType.Scissor:
					return true;
				case GestureType.Paper:
					number = PlayerNumber.PlayerOne;
					return true;
				case GestureType.Rock:
					number = PlayerNumber.PlayerTwo;
					return true;
				case GestureType.Lizard:
					number = PlayerNumber.PlayerOne;
					return true;
				case GestureType.Spock:
					number = PlayerNumber.PlayerTwo;
					return true;
				default:
					throw new NotImplementedException();
			}
		}

		#endregion
	}
}