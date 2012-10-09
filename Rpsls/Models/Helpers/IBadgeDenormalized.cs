using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models.Helpers
{
	public interface IBadgeDenormalized
	{
		string Id { get; set; }
		string Name { get; set; }
		int Total { get; set; }
	}
}