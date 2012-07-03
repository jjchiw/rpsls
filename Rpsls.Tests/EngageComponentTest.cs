using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Rpsls.Components;
using Rpsls.Models;

namespace Rpsls.Tests
{
	public class EngageComponentTest
	{
		//Scissors cut paper
		[Fact]
		public void Scissors_Cut_Paper()
		{
			var playerOne = new Scissor(PlayerNumber.PlayerOne);
			var playerTwo = new Paper(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Paper covers rock
		[Fact]
		public void Paper_Covers_Rock()
		{
			var playerOne = new Paper(PlayerNumber.PlayerOne);
			var playerTwo = new Rock(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Rock crushes lizard
		[Fact]
		public void Rock_Crushes_Lizard()
		{
			var playerOne = new Rock(PlayerNumber.PlayerOne);
			var playerTwo = new Lizard(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Lizard poisons Spock
		[Fact]
		public void Lizard_Poisons_Spock()
		{
			var playerOne = new Lizard(PlayerNumber.PlayerOne);
			var playerTwo = new Spock(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Spock smashes scissors
		[Fact]
		public void Spock_Smashes_Scissors()
		{
			var playerOne = new Spock(PlayerNumber.PlayerOne);
			var playerTwo = new Scissor(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Scissors decapitate lizard
		[Fact]
		public void Scissors_Decapitate_Lizard()
		{
			var playerOne = new Scissor(PlayerNumber.PlayerOne);
			var playerTwo = new Lizard(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Lizard eats paper
		[Fact]
		public void Lizard_Eats_Paper()
		{
			var playerOne = new Lizard(PlayerNumber.PlayerOne);
			var playerTwo = new Paper(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Paper disproves Spock
		[Fact]
		public void Paper_Disaproves_Spock()
		{
			var playerOne = new Paper(PlayerNumber.PlayerOne);
			var playerTwo = new Spock(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Spock vaporizes rock
		[Fact]
		public void Spock_Vaporizes_Rock()
		{
			var playerOne = new Spock(PlayerNumber.PlayerOne);
			var playerTwo = new Rock(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}

		//Rock crushes scissors
		[Fact]
		public void Rock_Crushes_Scissors()
		{
			var playerOne = new Rock(PlayerNumber.PlayerOne);
			var playerTwo = new Scissor(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerOne);
		}


		//Player Two
		//Scissors cut paper
		[Fact]
		public void Paper_Cut_By_Scissor()
		{
			var playerOne = new Paper(PlayerNumber.PlayerOne);
			var playerTwo = new Scissor(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Paper covers rock
		[Fact]
		public void Rock_Covered_By_Paper()
		{
			var playerOne = new Rock(PlayerNumber.PlayerOne);
			var playerTwo = new Paper(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Rock crushes lizard
		[Fact]
		public void Lizard_Crushed_By_Rock()
		{
			var playerOne = new Lizard(PlayerNumber.PlayerOne);
			var playerTwo = new Rock(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Lizard poisons Spock
		[Fact]
		public void Spock_Poisoned_By_Lizard()
		{
			var playerOne = new Spock(PlayerNumber.PlayerOne);
			var playerTwo = new Lizard(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Spock smashes scissors
		[Fact]
		public void Scissors_Smashed_By_Spock()
		{
			var playerOne = new Scissor(PlayerNumber.PlayerOne);
			var playerTwo = new Spock(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Scissors decapitate lizard
		[Fact]
		public void Lizard_Decapitated_By_Scissors()
		{
			var playerOne = new Lizard(PlayerNumber.PlayerOne);
			var playerTwo = new Scissor(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Lizard eats paper
		[Fact]
		public void Paper_Ate_By_Lizard()
		{
			var playerOne = new Paper(PlayerNumber.PlayerOne);
			var playerTwo = new Lizard(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Paper disproves Spock
		[Fact]
		public void Spock_Disaproved_By_Paper()
		{
			var playerOne = new Spock(PlayerNumber.PlayerOne);
			var playerTwo = new Paper(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Spock vaporizes rock
		[Fact]
		public void Rock_Vaporized_By_Spock()
		{
			var playerOne = new Rock(PlayerNumber.PlayerOne);
			var playerTwo = new Spock(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Rock crushes scissors
		[Fact]
		public void Scissors_Crushed_By_Rock()
		{
			var playerOne = new Scissor(PlayerNumber.PlayerOne);
			var playerTwo = new Rock(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.True(result);
			Assert.Equal(outcome, PlayerNumber.PlayerTwo);
		}

		//Equals Ties
		[Fact]
		public void Tie()
		{
			var playerOne = new Scissor(PlayerNumber.PlayerOne);
			var playerTwo = new Scissor(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			var result = eng.TryEngage(playerOne, playerTwo, out outcome);

			Assert.False(result);
		}

		//Not_Implemented
		[Fact]
		public void Not_Implemented()
		{
			var playerOne = new Scissor(PlayerNumber.PlayerOne);
			playerOne.GType = GestureType.Empty;

			var playerTwo = new Scissor(PlayerNumber.PlayerTwo);

			PlayerNumber outcome;

			var eng = new EngageComponent();

			Assert.Throws<NotImplementedException>(delegate
			{
				eng.TryEngage(playerOne, playerTwo, out outcome);
			});
		}

		
	}
}
