﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using System.Threading;
using Rpsls.Models;

namespace Rpsls.Tasks
{
	public class UpdateUserDenormalizedTask
	{
		public static void Execute(User user)
		{
			Task.Factory.StartNew(() => {

				var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
				parser.Parse();

				var documentStore = new DocumentStore
				{
					ApiKey = parser.ConnectionStringOptions.ApiKey,
					Url = parser.ConnectionStringOptions.Url
				};

				documentStore.Initialize();


				documentStore.DatabaseCommands.UpdateByIndex("MatchEncountersByUserId",
					new IndexQuery
					{
						Query = String.Format("User:{0}", user.Id)
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
									Value = user.UserName,
								},
								new PatchRequest
								{
									Type = PatchCommandType.Set,
									Name = "Email",
									Value = user.Email,
								},
							}
						}
					},
					allowStale: true);


				documentStore.DatabaseCommands.UpdateByIndex("MatchEncountersByUserId",
					new IndexQuery
					{
						Query = String.Format("UserRival:{0}", user.Id)
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
									Value = user.UserName,
								},
								new PatchRequest
								{
									Type = PatchCommandType.Set,
									Name = "Email",
									Value = user.Email,
								},
							}
						}
					},
					allowStale: true);
			});

			Thread.Sleep(100);
		}
	}
}