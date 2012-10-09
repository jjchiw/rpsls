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
		private List<Badge> _badges;
		private List<MatchEncounter> _lastMaxMatches;

		public AchieveBadgeTask(User user)
		{
			_user = user;
		}

		public override void Execute()
		{
			_badges = DocumentSession.Query<Badge>().ToList();
			_lastMaxMatches = DocumentSession.Query<MatchEncounter>()
											 .Where(x => x.User.Id == _user.Id)
											 .OrderByDescending(x => x.Date).Take(SeedBadges.MAX_LIMIT).ToList();

			var matchEncounters = DocumentSession.Query<MatchEncounterIndexResult, MatchEncounterIndex>()
										.Where(x => x.UserId == _user.Id).ToList();

			var count = 0;
			foreach (GestureType gesture in Enum.GetValues(typeof(GestureType)))
			{
				if (gesture == GestureType.Empty)
				{
					foreach (Hubs.MatchResult matchResult in Enum.GetValues(typeof(Hubs.MatchResult)))
					{
						count = matchEncounters.Where(x => x.MatchResult == matchResult).Sum(x => x.Count);
						UpdateUserBadges(gesture, count, matchResult);
						
					}
				}
				else
				{
					foreach (Hubs.MatchResult matchResult in Enum.GetValues(typeof(Hubs.MatchResult)))
					{
						var list = matchEncounters.FirstOrDefault(x => x.MatchResult == matchResult && x.Gesture == gesture);
						
						if (list != null)
						{
							count = list.Count;
							UpdateUserBadges(gesture, count, list.MatchResult);
						}
					}
				}
			}

			UpdateStrikeRecord();

			DocumentSession.Store(_user);
		}

		private void UpdateStrikeRecord()
		{
			foreach (var limit in SeedBadges.Limits)
			{
				var lastEncounters = _lastMaxMatches.Take(limit);

				if (lastEncounters.Count() != limit)
					return;

				foreach (Hubs.MatchResult matchResult in Enum.GetValues(typeof(Hubs.MatchResult)))
				{
					foreach (GestureType gesture in Enum.GetValues(typeof(GestureType)))
					{
						if (lastEncounters.All(x => x.Result == matchResult 
													&& x.UserGestureType == ( gesture == GestureType.Empty ? x.UserGestureType : gesture)))
						{
							var badge = _badges.FirstOrDefault(x => x.Gesture == gesture
																		   && x.Limit == limit
																		   && x.MathchResult == matchResult
																		   && x.IsStrike);

							if (badge != null)
							{
								var ownedBadge = _user.Badges.FirstOrDefault(x => x.Id == badge.Id);
								if (ownedBadge == null)
									_user.Badges.Add(new BadgeDenormalized<Badge> { Id = badge.Id, Name = badge.Name, Total = 1 });
								else
									ownedBadge.Total++;
							}
						}
					}
				}
			}							
		}

		private void UpdateUserBadges(GestureType gesture, int winCount, Hubs.MatchResult matchResult)
		{

			var badgesNotInUser = _badges.Where(x => x.Gesture == gesture 
													&& x.Limit <= winCount 
													&& x.MathchResult == matchResult
													&& !x.IsStrike)
										 .Where(x => !_user.Badges.Any(y => y.Id == x.Id));

			if(badgesNotInUser.Count() > 0)
				_user.Badges.AddRange(badgesNotInUser.Select(x => new BadgeDenormalized<Badge> {Id = x.Id, Name = x.Name, Total = 1}));
		}
	}
}