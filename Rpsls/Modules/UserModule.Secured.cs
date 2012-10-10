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
	public class UserModuleSecured : RavenModule
	{
		public UserModuleSecured() : base("users")
		{
			this.RequiresAuthentication();

			Post["/"] = parameter =>
			{
				var user = (Context.CurrentUser as User);

                user.UserName = Request.Form.Username;

				RavenSession.Store(user);

				Context.CurrentUser = user;

				TaskExecutor.ExecuteLater(new UpdateUserDenormalizedTask(user));

				return Response.AsRedirect("/users/");
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


				m.UserStats = this.CreateStatViewModel(matchEncounters);

				var encounters = RavenSession.Query<MatchEncounter>()
											 .Where(x => x.User.Id == user.Id)
											 .OrderByDescending(x => x.Date).ToList();

				m.Matches = Mapper.Map<List<MatchEncounter>, List<MatchEncounterView>>(encounters);
				m.Badges = user.Badges.ToList();

				return View["User/Edit", m];
			};

			Get["/{id}"] = parameters =>
			{
				var userId = String.Format("{0}/{1}", this.ModulePath, parameters.id.Value as string);

				var authUser = (Context.CurrentUser as User);
				if(userId == authUser.Id)
					return Response.AsRedirect("/users/");

				var user = RavenSession.Load<User>(userId);

				var m = Context.Model("View User");


				var matchEncounters = RavenSession.Query<MatchEncounterIndexResult, MatchEncounterIndex>()
										.Where(x => x.UserId == user.Id).ToList();


				m.UserStats = this.CreateStatViewModel(matchEncounters);

				var encounters = RavenSession.Query<MatchEncounter>()
											 .Where(x => x.User.Id == user.Id)
											 .OrderByDescending(x => x.Date).ToList();

				m.Matches = Mapper.Map<List<MatchEncounter>, List<MatchEncounterView>>(encounters);
				m.Badges = user.Badges.ToList();
				m.User = Mapper.Map<User, UserView>(user);

				return View["User/View", m];
			};

			Get[".json"] = parameter =>
			{
				var m = Context.Model("Edit User");
				return Response.AsJson(new { m });
			};
		}
	}
}