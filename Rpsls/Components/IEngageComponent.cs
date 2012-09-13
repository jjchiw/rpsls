using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Models;

namespace Rpsls.Components
{
	public interface IEngageComponent
	{
		GestureType RandomGesture();
		bool TryEngage(Player playerOne, Player playerTwo, out PlayerNumber outcome);
	}
}