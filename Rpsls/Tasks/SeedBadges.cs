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
		public static void Execute(IDocumentStore documentStore)
		{
			using (var session = documentStore.OpenSession())
			{

				foreach (var limit in new int[]{10,20,30,40,50,60,70,80,90,100})
				{
					foreach (GestureType gesture in Enum.GetValues(typeof(GestureType)))
					{
						foreach (Hubs.MatchResult matchResult in Enum.GetValues(typeof(Hubs.MatchResult)))
						{
							foreach (var strike in new bool[] { true, false })
							{
								var badge = new Badge
								{
									IsStrike = strike,
									Limit = limit,
									MathchResult = matchResult,
									Gesture = gesture,
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