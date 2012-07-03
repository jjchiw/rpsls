using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpsls.Models;

namespace Rpsls.Components
{
	public interface IEngageComponent
	{
		bool TryEngage(IPlayer playerOne, IPlayer playerTwo, out PlayerNumber number);
	}
}