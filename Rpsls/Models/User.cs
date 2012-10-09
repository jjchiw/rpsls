using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using Rpsls.Models.Helpers;

namespace Rpsls.Models
{
	public class User : IUserIdentity, IUserDenormalized
	{
		public string Id { get; set; }
		public Guid Guid { get; set; }
		public string Email { get; set; }

		public int WinCount { get; set; }
		public int LostCount { get; set; }
		public int TieCount { get; set; }

		public string Token { get; set; }

		public List<BadgeDenormalized<Badge>> Badges { get; set; }

		#region IUserIdentity Members

		public string UserName { get; set; }
		public IEnumerable<string> Claims { get; set; }

		#endregion

		public User()
		{
			Badges = new List<BadgeDenormalized<Badge>>();
		}
	}
}