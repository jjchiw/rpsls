using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Rpsls.Models;

namespace Rpsls.Tasks
{
	public class SeedBadges
	{

		public const int MAX_LIMIT = 100;

		private static readonly int[] _limits = new int[]{10,20,30,40,50,60,70,80,90,100};

		public static int[] Limits
		{
			get
			{
				return _limits;
			}
		}

		public static void Execute(IDocumentStore documentStore)
		{
			using (var session = documentStore.OpenSession())
			{
				if (session.Query<Badge>().Count() != 0)
					return;


				foreach (var limit in Limits)
				{
					foreach (GestureType gesture in Enum.GetValues(typeof(GestureType)))
					{
						foreach (Hubs.MatchResult matchResult in Enum.GetValues(typeof(Hubs.MatchResult)))
						{
							foreach (var strike in new bool[] { true, false })
							{
								var badge = new Badge
								{
									Limit = limit,
									MathchResult = matchResult,
									Gesture = gesture,
									IsStrike = strike,
									Name = String.Format("{0}{1}-Strike{2}-{3}", limit, gesture, strike, matchResult)
								};
								session.Store(badge);

							}
						}
						
					}
					session.SaveChanges();
				}
				
			}
		}
	}
}