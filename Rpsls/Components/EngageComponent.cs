using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Models;

namespace Rpsls.Components
{
	public class EngageComponent : Rpsls.Components.IEngageComponent
	{
		static IDictionary<GestureType, IList<GestureType>> _gestureBeatGestures = null;

		static EngageComponent()
		{
			_gestureBeatGestures = new Dictionary<GestureType, IList<GestureType>>
			{
				{GestureType.Lizard , new List<GestureType>{GestureType.Spock, GestureType.Paper}},
				{GestureType.Paper , new List<GestureType>{GestureType.Rock, GestureType.Spock}},
				{GestureType.Rock , new List<GestureType>{GestureType.Lizard, GestureType.Scissors}},
				{GestureType.Scissors , new List<GestureType>{GestureType.Paper, GestureType.Lizard}},
				{GestureType.Spock , new List<GestureType>{GestureType.Scissors, GestureType.Rock}},
			};
		}

		public GestureType RandomGesture()
		{
			var r = new Random();
			var randomIndex = r.Next(5);

			return (GestureType) randomIndex;
		}

		public bool TryEngage(Player playerOne, Player playerTwo, out PlayerNumber outcome)
		{
			outcome = PlayerNumber.None;
			var result = false;

			if (playerOne.GType == playerTwo.GType)
			{
				result = false;
			}
			else if (!_gestureBeatGestures.ContainsKey(playerOne.GType) || !_gestureBeatGestures.ContainsKey(playerTwo.GType))
			{
				throw new NotImplementedException();
			}
			else if (_gestureBeatGestures[playerOne.GType].Any(x => x == playerTwo.GType))
			{
				outcome = playerOne.Number;
				result = true;
			}
			else if (_gestureBeatGestures[playerTwo.GType].Any(x => x == playerOne.GType))
			{
				outcome = playerTwo.Number;
				result = true;
			}

			return result;
		}
	}
}