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

			Assert.Equal(s.WinnerLegend, p.Name + "'s Scissors Cuts " + p2.Name + "'s Paper. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Scissors Cuts " + p2.Name + "'s Paper. You Lost.");
		}

		//Paper covers rock
		[Fact]
		public void Paper_Covers_Rock()
		{
			var p = new Client { Name = "ukua", LastMove = "Paper" };
			var p2 = new Client { Name = "illya", LastMove = "Rock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Paper Covers " + p2.Name + "'s Rock. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Paper Covers " + p2.Name + "'s Rock. You Lost.");
		}
		
		//Rock crushes lizard
		[Fact]
		public void Rock_Crushes_Lizard()
		{
			var p = new Client { Name = "ukua", LastMove = "Rock" };
			var p2 = new Client { Name = "illya", LastMove = "Lizard" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Rock Crushes " + p2.Name + "'s Lizard. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Rock Crushes " + p2.Name + "'s Lizard. You Lost.");
		}

		//Lizard poisons Spock
		[Fact]
		public void Lizard_Poisons_Spock()
		{
			var p = new Client { Name = "ukua", LastMove = "Lizard" };
			var p2 = new Client { Name = "illya", LastMove = "Spock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Lizard Poisons " + p2.Name + "'s Spock. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Lizard Poisons " + p2.Name + "'s Spock. You Lost.");
		}

		//Spock smashes scissors
		[Fact]
		public void Spock_Smashes_Scissors()
		{
			var p = new Client { Name = "ukua", LastMove = "Spock" };
			var p2 = new Client { Name = "illya", LastMove = "Scissors" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Spock Smashes " + p2.Name + "'s Scissors. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Spock Smashes " + p2.Name + "'s Scissors. You Lost.");
		}

		//Scissors decapitates lizard
		[Fact]
		public void Scissors_Decapites_Lizard()
		{
			var p = new Client { Name = "ukua", LastMove = "Scissors" };
			var p2 = new Client { Name = "illya", LastMove = "Lizard" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Scissors Decapites " + p2.Name + "'s Lizard. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Scissors Decapites " + p2.Name + "'s Lizard. You Lost.");
		}

		//Lizard eats paper
		[Fact]
		public void Lizard_Eats_Paper()
		{
			var p = new Client { Name = "ukua", LastMove = "Lizard" };
			var p2 = new Client { Name = "illya", LastMove = "Paper" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Lizard Eats " + p2.Name + "'s Paper. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Lizard Eats " + p2.Name + "'s Paper. You Lost.");
		}

		//Paper disproves Spock
		[Fact]
		public void Paper_Disproves_Spock()
		{
			var p = new Client { Name = "ukua", LastMove = "Paper" };
			var p2 = new Client { Name = "illya", LastMove = "Spock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Paper Disproves " + p2.Name + "'s Spock. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Paper Disproves " + p2.Name + "'s Spock. You Lost.");
		}

		//Spock vaporizes rock
		[Fact]
		public void Spock_Vaporizes_Rock()
		{
			var p = new Client { Name = "ukua", LastMove = "Spock" };
			var p2 = new Client { Name = "illya", LastMove = "Rock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Spock Vaporizes " + p2.Name + "'s Rock. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Spock Vaporizes " + p2.Name + "'s Rock. You Lost.");
		}

		//Rock crushes scissors
		[Fact]
		public void Rock_Crushes_Scissors()
		{
			var p = new Client { Name = "ukua", LastMove = "Rock" };
			var p2 = new Client { Name = "illya", LastMove = "Scissors" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Rock Crushes " + p2.Name + "'s Scissors. You Win.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Rock Crushes " + p2.Name + "'s Scissors. You Lost.");
		}

		//Rock Ties Rock
		[Fact]
		public void Rock_Ties_Rock()
		{
			var p = new Client { Name = "ukua", LastMove = "Rock" };
			var p2 = new Client { Name = "illya", LastMove = "Rock" };

			var s = WinnerLoserLengendGenerator.GenerateLegend(p, p2);

			Assert.Equal(s.WinnerLegend, p.Name + "'s Rock Ties " + p2.Name + "'s Rock.");
			Assert.Equal(s.LoserLegend, p.Name + "'s Rock Ties " + p2.Name + "'s Rock.");
		}

		
	}
}
