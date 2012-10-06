using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Rpsls.Models;
using Xunit;

namespace Rpsls.Tests
{
	public class MatchEncountersByUserId : AbstractIndexCreationTask<MatchEncounter>
	{
		public MatchEncountersByUserId()
		{
			Map = encounters => from encounter in encounters
								select new { User = encounter.User.Id };
		}

	}

	public class UpdateUserDenormalizedTaskTest
	{
		[Fact]
		public void Update()
		{
			//using (var documentStore = new DocumentStore() { Url = "http://localhost:8080/databases/rpsls" })
			//{
			//	documentStore.Initialize();

			//	var user = new User
			//	{
			//		Id = "users/1",
			//		UserName = "yaya",
			//		Email = "zecta@hotmail.com"
			//	};

			//	using (var session = documentStore.OpenSession())
			//	{
			//		IndexCreation.CreateIndexes(typeof(MatchEncountersByUserId).Assembly, documentStore);

			//		documentStore.DatabaseCommands.UpdateByIndex("MatchEncountersByUserId",
			//		new IndexQuery
			//		{
			//			Query = String.Format("User:{0}", user.Id)
			//		}, new[]
			//		{
			//			new PatchRequest
			//			{
			//				Type = PatchCommandType.Modify,
			//				Name = "User",
			//				Nested = new[]
			//				{
			//					new PatchRequest
			//					{
			//						Type = PatchCommandType.Set,
			//						Name = "UserName",
			//						Value = user.UserName,
			//					},
			//					new PatchRequest
			//					{
			//						Type = PatchCommandType.Set,
			//						Name = "Email",
			//						Value = user.Email,
			//					},
			//				}
			//			}
			//		},
			//		allowStale: true);

			//		var usern = session.Query<MatchEncounter>().First().User;

			//		Assert.Equal("yaya", usern.UserName);
			//	}
			//}
		}
	}
}
