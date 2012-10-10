using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using System.Dynamic;
using Nancy.Authentication.Forms;
using Rpsls.Models;
using Rpsls.Helpers.Indexes;
using Rpsls.Models.ViewModels;

namespace Rpsls.Helpers
{
	public static class ModuleExtensions
	{
		public static bool IsLoggedIn(this NancyContext context)
		{
			return !(context == null || context.CurrentUser == null ||
					 string.IsNullOrWhiteSpace(context.CurrentUser.UserName));
		}

		public static string Username(this NancyContext context)
		{
			return (context == null || context.CurrentUser == null || string.IsNullOrWhiteSpace(context.CurrentUser.UserName)) ? string.Empty : context.CurrentUser.UserName;
		}

		public static string UserEmail(this NancyContext context)
		{
			return (context == null || context.CurrentUser == null) ? string.Empty : (context.CurrentUser as User).Email;
		}

		public static string UserRavenIdString(this NancyContext context)
		{
			return (context == null || context.CurrentUser == null) ? string.Empty : (context.CurrentUser as User).Id;
		}

		public static string UserGravatarUrl(this NancyContext context)
		{
			return (context == null || context.CurrentUser == null) ? string.Empty : (context.CurrentUser as User).Email.ToGravatarUrl();
		}

		public static dynamic Model(this NancyContext context, string title)
		{
			dynamic model = new ExpandoObject();
			model.Title = title;
			model.IsLoggedIn = context.IsLoggedIn();
			model.UserName = context.Username();
			model.Email = context.UserEmail();
			model.GravatarUrl = context.UserGravatarUrl();
			model.HomeActive = "";
			model.NewPathActive = "";
			model.MyPathsActive = "";
			model.AboutActive = "";
			return model;
		}

		public static StatsViewModel CreateStatViewModel(this NancyModule module, List<MatchEncounterIndexResult> matchEncounters)
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