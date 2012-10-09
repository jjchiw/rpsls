using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using System.Threading;
using Rpsls.Models;
using Rpsls.Tasks.Infrastructure;

namespace Rpsls.Tasks
{
	public class UpdateUserDenormalizedTask : BackgroundTask
	{
		private User _user;

		public UpdateUserDenormalizedTask(User user)
		{
			_user = user;
		}

		public override void Execute()
		{
			DocumentStore.DatabaseCommands.UpdateByIndex("MatchEncountersByUserId",
				new IndexQuery
				{
					Query = String.Format("User:{0}", _user.Id)
				}, new[]
				{
					new PatchRequest
					{
						Type = PatchCommandType.Modify,
						Name = "User",
						Nested = new[]
						{
							new PatchRequest
							{
								Type = PatchCommandType.Set,
								Name = "UserName",
								Value = _user.UserName,
							},
							new PatchRequest
							{
								Type = PatchCommandType.Set,
								Name = "Email",
								Value = _user.Email,
							},
						}
					}
				},
				allowStale: true);


			DocumentStore.DatabaseCommands.UpdateByIndex("MatchEncountersByUserId",
				new IndexQuery
				{
					Query = String.Format("UserRival:{0}", _user.Id)
				}, new[]
				{
					new PatchRequest
					{
						Type = PatchCommandType.Modify,
						Name = "UserRival",
						Nested = new[]
						{
							new PatchRequest
							{
								Type = PatchCommandType.Set,
								Name = "UserName",
								Value = _user.UserName,
							},
							new PatchRequest
							{
								Type = PatchCommandType.Set,
								Name = "Email",
								Value = _user.Email,
							},
						}
					}
				},
				allowStale: true);
		

		}
	}
}