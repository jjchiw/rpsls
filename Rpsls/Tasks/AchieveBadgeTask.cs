using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Raven.Abstractions.Data;
using Rpsls.Helpers.Indexes;
using Rpsls.Models;
using Rpsls.Models.Helpers;
using Rpsls.Tasks.Infrastructure;

namespace Rpsls.Tasks
{
	public class AchieveBadgeTask : BackgroundTask
	{
		private class InternalMatchEncounter
		{
			public Hubs.MatchResult MatchResult { get; set; }
			public List<MatchEncounterIndexResult> Matches {get;set;}
		}

		private User _user;

		public AchieveBadgeTask(User user)
		{
			_user = user;
		}

		public override void Execute()
		{
			var winEncounters = DocumentSession.Query<MatchEncounterIndexResult, MatchEncounterWinIndex>()
										.Where(x => x.UserId == _user.Id).ToList();

			var loseEncounters = DocumentSession.Query<MatchEncounterIndexResult, MatchEncounterLoseIndex>()
									.Where(x => x.UserId == _user.Id).ToList();

			var tieEncounters = DocumentSession.Query<MatchEncounterIndexResult, MatchEncounterTieIndex>()
									.Where(x => x.UserId == _user.Id).ToList();

			var encounters = new List<InternalMatchEncounter> 
			{
				new InternalMatchEncounter
				{
					MatchResult = Hubs.MatchResult.Win,
					Matches = winEncounters
				},
				new InternalMatchEncounter
				{
					MatchResult = Hubs.MatchResult.Lose,
					Matches = loseEncounters
				},
				new InternalMatchEncounter
				{
					MatchResult = Hubs.MatchResult.Tie,
					Matches = tieEncounters
				}
			};

			foreach (GestureType gesture in Enum.GetValues(typeof(GestureType)))
			{
				if (gesture == GestureType.Empty)
				{
					var count = winEncounters.Sum(x => x.Count);

					UpdateUserBadges(gesture, count, Hubs.MatchResult.Win);

					count = loseEncounters.Sum(x => x.Count);

					UpdateUserBadges(gesture, count, Hubs.MatchResult.Lose);

					count = tieEncounters.Sum(x => x.Count);

					UpdateUserBadges(gesture, count, Hubs.MatchResult.Tie);
				}
				else
				{
					foreach (var item in encounters)
					{
						var list = item.Matches.FirstOrDefault(x => x.Gesture == gesture);
						if (list != null)
						{
							var count = list.Count;
							UpdateUserBadges(gesture, count, item.MatchResult);
						}
					}

				}
			}

			DocumentSession.SaveChanges();

		}

		private void UpdateUserBadges(GestureType gesture, int winCount, Hubs.MatchResult matchResult)
		{
			var badges = DocumentSession.Query<Badge>()
										.Where(x => x.Gesture == gesture
													   && x.Limit < winCount
													   && x.MathchResult == matchResult
													   && !x.IsStrike)
										   .ToList();

			//var badgesNotInUser = badges.Cast<IBadgeDenormalized>().Except(_user.Badges);

			var badgesNotInUser = badges.Where(x => !_user.Badges.Any(y => y.Id == x.Id));

			_user.Badges.AddRange(badgesNotInUser.Select(x => new BadgeDenormalized<Badge> {Id = x.Id, Name = x.Name, Total = 1}));

			DocumentSession.Store(_user);
		}
	}
}