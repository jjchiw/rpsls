using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Rpsls.Hubs;
using Rpsls.Helpers;

namespace Rpsls.Tests
{
	public class LegendGeneratorTest
	{
		//Scissors cuts paper
		[Fact]
		public void Scissors_Cuts_Paper()
		{
			var p = new Client { Name = "ukua", LastMove = "Scissors" }; 
			var p2 = new Client { Name = "illya", LastMove = "Paper" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Paper covers rock
		[Fact]
		public void Paper_Covers_Rock()
		{
			var p = new Client { Name = "ukua", LastMove = "Paper" };
			var p2 = new Client { Name = "illya", LastMove = "Rock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}
		
		//Rock crushes lizard
		[Fact]
		public void Rock_Crushes_Lizard()
		{
			var p = new Client { Name = "ukua", LastMove = "Rock" };
			var p2 = new Client { Name = "illya", LastMove = "Lizard" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Lizard poisons Spock
		[Fact]
		public void Lizard_Poisons_Spock()
		{
			var p = new Client { Name = "ukua", LastMove = "Lizard" };
			var p2 = new Client { Name = "illya", LastMove = "Spock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Spock smashes scissors
		[Fact]
		public void Spock_Smashes_Scissors()
		{
			var p = new Client { Name = "ukua", LastMove = "Spock" };
			var p2 = new Client { Name = "illya", LastMove = "Scissors" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Scissors decapitates lizard
		[Fact]
		public void Scissors_Decapites_Lizard()
		{
			var p = new Client { Name = "ukua", LastMove = "Scissors" };
			var p2 = new Client { Name = "illya", LastMove = "Lizard" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Lizard eats paper
		[Fact]
		public void Lizard_Eats_Paper()
		{
			var p = new Client { Name = "ukua", LastMove = "Lizard" };
			var p2 = new Client { Name = "illya", LastMove = "Paper" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);
			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Paper disproves Spock
		[Fact]
		public void Paper_Disproves_Spock()
		{
			var p = new Client { Name = "ukua", LastMove = "Paper" };
			var p2 = new Client { Name = "illya", LastMove = "Spock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Spock vaporizes rock
		[Fact]
		public void Spock_Vaporizes_Rock()
		{
			var p = new Client { Name = "ukua", LastMove = "Spock" };
			var p2 = new Client { Name = "illya", LastMove = "Rock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Rock crushes scissors
		[Fact]
		public void Rock_Crushes_Scissors()
		{
			var p = new Client { UserId = "/users/1",  Name = "ukua", LastMove = "Rock" };
			var p2 = new Client { UserId = "/users/2",  Name = "illya", LastMove = "Scissors" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var result = GenerateWinnerLegend(p, p2);

			Assert.Equal(s.WinnerLegend, result[0]);
			Assert.Equal(s.LoserLegend, result[1]);
		}

		//Rock Ties Rock
		[Fact]
		public void Rock_Ties_Rock()
		{
			var p = new Client {  UserId = "/users/1", Name = "ukua", LastMove = "Rock" };
			var p2 = new Client { UserId = "/users/2", Name = "illya", LastMove = "Rock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			var strings = GenerateTiesLegend(p, p2);

			Assert.Equal(s.WinnerLegend, strings[0]);
			Assert.Equal(s.LoserLegend, strings[1]);
		}

		public static string[] GenerateTiesLegend(Client one, Client two)
		{
			return new string[]
			{
				String.Format("<a href='{0}' target='_blank'>{1}</a>'s {2} {3} <a href='{4}' target='_blank'>{5}</a>'s {6}.", one.UserId, one.Name, one.LastMove, "Ties", two.UserId, two.Name, two.LastMove),
				String.Format("<a href='{0}' target='_blank'>{1}</a>'s {2} {3} <a href='{4}' target='_blank'>{5}</a>'s {6}.", one.UserId, one.Name, one.LastMove, "Ties", two.UserId, two.Name, two.LastMove)
			};
		}

		public static string[] GenerateWinnerLegend(Client winner, Client loser)
		{
			var key = winner.LastMove + "+" + loser.LastMove;
			var legend = String.Format("<a href='{0}' target='_blank'>{1}</a>'s {2} {3} <a href='{4}' target='_blank'>{5}</a>'s {6}.", winner.UserId, winner.Name, winner.LastMove, verbs[key], loser.UserId, loser.Name, loser.LastMove);

			return new string[]
			{
				String.Format("{0} You Win.", legend),
				String.Format("{0} You Lost.", legend)
			};
		}

		private static Dictionary<string, string> verbs = new Dictionary<string, string> { { "Scissors+Paper" , "Cuts" },
																					{ "Paper+Rock", "Covers"},
																					{ "Rock+Lizard", "Crushes"},
																					{ "Lizard+Spock", "Poisons"},
																					{ "Spock+Scissors", "Smashes"},
																					{ "Scissors+Lizard", "Decapites"},
																					{ "Lizard+Paper", "Eats"},
																					{ "Paper+Spock", "Disproves"},
																					{ "Spock+Rock", "Vaporizes"},
																					{ "Rock+Scissors", "Crushes"},
																				  };

		
	}
}
