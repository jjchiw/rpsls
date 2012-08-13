using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Rpsls.Models;
using Nancy.Authentication.Forms;

namespace Rpsls.Helpers
{
	public class UserMapper : IUserMapper
	{
		private IDocumentStore _documentStore;

		public UserMapper(IDocumentStore documentStore)
		{
			_documentStore = documentStore;
		}
		#region IUserMapper Members

		public Nancy.Security.IUserIdentity GetUserFromIdentifier(Guid identifier)
		{
			using (IDocumentSession session = _documentStore.OpenSession())
			{
				var user = session.Query<User>().FirstOrDefault(x => x.Guid == identifier);

				return user;
			}
		}

		#endregion

		#region IUserMapper Members

		public Nancy.Security.IUserIdentity GetUserFromIdentifier(Guid identifier, Nancy.NancyContext context)
		{
			using (IDocumentSession session = _documentStore.OpenSession())
			{
				var user = session.Query<User>().FirstOrDefault(x => x.Guid == identifier);

				return user;
			}
		}

		#endregion
	}
}