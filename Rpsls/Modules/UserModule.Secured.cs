using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Security;
using Raven.Client;
using Rpsls.Models;
using Rpsls.Helpers;
using System.Web.Security;
using Rpsls.Helpers.Indexes;
using Rpsls.Models.ViewModels;
using Rpsls.Tasks;
using AutoMapper;
using Rpsls.Tasks.Infrastructure;
using System.Dynamic;

namespace Rpsls.Modules
{
	public class UserModule : RavenModule
	{
		public UserModule() : base("user")
		{
			this.RequiresAuthentication();

			Post["/"] = parameter =>
			{
				var user = (Context.CurrentUser as User);

                user.UserName = Request.Form.Username;

				RavenSession.Store(user);

				Context.CurrentUser = user;

				TaskExecutor.ExecuteLater(new UpdateUserDenormalizedTask(user));

				return Response.AsRedirect("/User/");
			};

            Post["/token"] = parameter =>
            {
                var user = (Context.CurrentUser as User);

				user.Token = Membership.GeneratePassword(6, 3);//new Random().Next(1, 10).ToString();

				RavenSession.Store(user);
                    
                return Response.AsJson(new {Token = user.Token}, HttpStatusCode.OK );
            };

			Get["/"] = parameter =>
			{
				var user = (Context.CurrentUser as User);

				var m = Context.Model("Edit User");
                m.Token = user.Token;



				var matchEncounters = RavenSession.Query<MatchEncounterIndexResult, MatchEncounterIndex>()
										.Where(x => x.UserId == user.Id).ToList();


				m.UserStats = CreateStatViewModel(matchEncounters);

				var encounters = RavenSession.Query<MatchEncounter>()
											 .Where(x => x.User.Id == user.Id)
											 .OrderByDescending(x => x.Date).ToList();

				m.Matches = Mapper.Map<List<MatchEncounter>, List<MatchEncounterView>>(encounters);
				m.Badges = user.Badges.ToList();

				return View["User/Edit", m];
			};

			Get[".json"] = parameter =>
			{
				var m = Context.Model("Edit User");
				return Response.AsJson(new { m });
			};
		}

		private  StatsViewModel CreateStatViewModel(List<MatchEncounterIndexResult> matchEncounters)
		{
			//var result = new StatsViewModel();

			dynamic expando = new ExpandoObject();
			var dict = expando as IDictionary<string, object>;

			var key = "";
			foreach (GestureType gestureType in Enum.GetValues(typeof(GestureType)))
			{
				if (gestureType == GestureType.Empty)
				{
					foreach (Hubs.MatchResult matchResult in Enum.GetValues(typeof(Hubs.MatchResult)))
					{
						key = String.Format("Total{0}Count", matchResult);
						dict.Add(key, 0);
						if (matchEncounters.Any(x => x.Gesture == gestureType))
							dict[key] = matchEncounters.Where(x => x.MatchResult == matchResult).Sum(x => x.Count);
					}

					continue;
				}

				foreach (Hubs.MatchResult matchResult in Enum.GetValues(typeof(Hubs.MatchResult)))
				{
					key = String.Format("{0}{1}Count", gestureType, matchResult);
					dict.Add(key, 0);
					if (matchEncounters.Any(x => x.Gesture == gestureType && x.MatchResult == matchResult))
						dict[key] = matchEncounters.First(x => x.Gesture == gestureType && x.MatchResult == matchResult).Count;
				}
			}

			var result = new StatsViewModel(expando);
		
			return result;
		}
	}
}