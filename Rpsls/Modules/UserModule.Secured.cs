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

				//UpdateUserDenormalizedTask.Execute(user);

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



				var winEncounters = RavenSession.Query<MatchEncounterIndexResult, MatchEncounterWinIndex>()
										.Where(x => x.UserId == user.Id).ToList();

				var loseEncounters = RavenSession.Query<MatchEncounterIndexResult, MatchEncounterLoseIndex>()
										.Where(x => x.UserId == user.Id).ToList();

				var tieEncounters = RavenSession.Query<MatchEncounterIndexResult, MatchEncounterTieIndex>()
										.Where(x => x.UserId == user.Id).ToList();


				m.UserStats = CreateStatViewModel(winEncounters, loseEncounters, tieEncounters);

				var encounters = RavenSession.Query<MatchEncounter>()
											 .Where(x => x.User.Id == user.Id)
											 .OrderByDescending(x => x.Date).ToList();

				m.Matches = Mapper.Map<List<MatchEncounter>, List<MatchEncounterView>>(encounters);

				return View["User/Edit", m];
			};

			Get[".json"] = parameter =>
			{
				var m = Context.Model("Edit User");
				return Response.AsJson(new { m });
			};
		}

		private  StatsViewModel CreateStatViewModel(List<MatchEncounterIndexResult> winEncounters, List<MatchEncounterIndexResult> loseEncounters, List<MatchEncounterIndexResult> tieEncounters)
		{
			var result = new StatsViewModel();

			if(winEncounters.Any(x => x.Gesture == GestureType.Rock))
				result.RockWinCount = winEncounters.First(x => x.Gesture == GestureType.Rock).Count;

			if (winEncounters.Any(x => x.Gesture == GestureType.Scissors))
				result.ScissorsWinCount = winEncounters.First(x => x.Gesture == GestureType.Scissors).Count;

			if (winEncounters.Any(x => x.Gesture == GestureType.Paper))
				result.PaperWinCount = winEncounters.First(x => x.Gesture == GestureType.Paper).Count;

			if (winEncounters.Any(x => x.Gesture == GestureType.Lizard))
				result.LizardWinCount = winEncounters.First(x => x.Gesture == GestureType.Lizard).Count;

			if (winEncounters.Any(x => x.Gesture == GestureType.Spock))
				result.SpockWinCount = winEncounters.First(x => x.Gesture == GestureType.Spock).Count;


			if (loseEncounters.Any(x => x.Gesture == GestureType.Rock))
				result.RockLoseCount = loseEncounters.First(x => x.Gesture == GestureType.Rock).Count;

			if (loseEncounters.Any(x => x.Gesture == GestureType.Scissors))
				result.ScissorsLoseCount = loseEncounters.First(x => x.Gesture == GestureType.Scissors).Count;

			if (loseEncounters.Any(x => x.Gesture == GestureType.Paper))
				result.PaperLoseCount = loseEncounters.First(x => x.Gesture == GestureType.Paper).Count;

			if (loseEncounters.Any(x => x.Gesture == GestureType.Lizard))
				result.LizardLoseCount = loseEncounters.First(x => x.Gesture == GestureType.Lizard).Count;

			if (loseEncounters.Any(x => x.Gesture == GestureType.Spock))
				result.SpockLoseCount = loseEncounters.First(x => x.Gesture == GestureType.Spock).Count;


			if (tieEncounters.Any(x => x.Gesture == GestureType.Rock))
				result.RockTieCount = tieEncounters.First(x => x.Gesture == GestureType.Rock).Count;

			if (tieEncounters.Any(x => x.Gesture == GestureType.Scissors))
				result.ScissorsTieCount = tieEncounters.First(x => x.Gesture == GestureType.Scissors).Count;

			if (tieEncounters.Any(x => x.Gesture == GestureType.Paper))
				result.PaperTieCount = tieEncounters.First(x => x.Gesture == GestureType.Paper).Count;

			if (tieEncounters.Any(x => x.Gesture == GestureType.Lizard))
				result.LizardTieCount = tieEncounters.First(x => x.Gesture == GestureType.Lizard).Count;

			if (tieEncounters.Any(x => x.Gesture == GestureType.Spock))
				result.SpockTieCount = tieEncounters.First(x => x.Gesture == GestureType.Spock).Count;

			result.TotalWinCount = winEncounters.Sum(x => x.Count);
			result.TotalLoseCount = loseEncounters.Sum(x => x.Count);
			result.TotalTieCount = tieEncounters.Sum(x => x.Count);

			result.RockTotalCount = result.RockWinCount + result.RockLoseCount + result.RockTieCount;

			if (result.RockTotalCount > 0)
			{
				result.RockWinRate = Math.Round((((double)result.RockWinCount / (double)result.RockTotalCount) * 100d), 2);
				result.RockLoseRate = Math.Round((((double)result.RockLoseCount / (double)result.RockTotalCount) * 100d), 2);
				result.RockTieRate = Math.Round((((double)result.RockTieCount / (double)result.RockTotalCount) * 100d), 2); 
			}


			result.PaperTotalCount = result.PaperWinCount + result.PaperLoseCount + result.PaperTieCount;
			if (result.PaperTotalCount > 0)
			{
				result.PaperWinRate = Math.Round((( (double)result.PaperWinCount /  (double)result.PaperTotalCount) * 100d), 2);
				result.PaperLoseRate = Math.Round((((double)result.PaperLoseCount / (double)result.PaperTotalCount) * 100d), 2);
				result.PaperTieRate = Math.Round((( (double)result.PaperTieCount /  (double)result.PaperTotalCount) * 100d), 2); 
			}

			result.ScissorsTotalCount = result.ScissorsWinCount + result.ScissorsLoseCount + result.ScissorsTieCount;

			if (result.ScissorsTotalCount > 0)
			{
				result.ScissorsWinRate = Math.Round((((double)result.ScissorsWinCount / (double)result.ScissorsTotalCount) * 100d), 2);
				result.ScissorsLoseRate = Math.Round((((double)result.ScissorsLoseCount / (double)result.ScissorsTotalCount) * 100d), 2);
				result.ScissorsTieRate = Math.Round((((double)result.ScissorsTieCount / (double)result.ScissorsTotalCount) * 100d), 2); 
			}

			result.LizardTotalCount = result.LizardWinCount + result.LizardLoseCount + result.LizardTieCount;

			if (result.LizardTotalCount > 0)
			{
				result.LizardWinRate = Math.Round((((double)result.LizardWinCount / (double)result.LizardTotalCount) * 100d), 2);
				result.LizardLoseRate = Math.Round((((double)result.LizardLoseCount / (double)result.LizardTotalCount) * 100d), 2);
				result.LizardTieRate = Math.Round((((double)result.LizardTieCount / (double)result.LizardTotalCount) * 100d), 2); 
			}

			result.SpockTotalCount = result.SpockWinCount + result.SpockLoseCount + result.SpockTieCount;

			if (result.SpockTotalCount > 0)
			{
				result.SpockWinRate = Math.Round((((double)result.SpockWinCount / (double)result.SpockTotalCount) * 100d), 2);
				result.SpockLoseRate = Math.Round((((double)result.SpockLoseCount / (double)result.SpockTotalCount) * 100d), 2);
				result.SpockTieRate = Math.Round((((double)result.SpockTieCount / (double)result.SpockTotalCount) * 100d), 2); 
			}

			result.TotalTotalCount = result.TotalWinCount + result.TotalLoseCount + result.TotalTieCount;
			if (result.TotalTotalCount > 0)
			{
				result.TotalWinRate = Math.Round((((double)result.TotalWinCount / (double)result.TotalTotalCount) * 100d), 2);
				result.TotalLoseRate = Math.Round((((double)result.TotalLoseCount / (double)result.TotalTotalCount) * 100d), 2);
				result.TotalTieRate = Math.Round((((double)result.TotalTieCount / (double)result.TotalTotalCount) * 100d), 2); 
			}

		
			return result;
		}
	}
}