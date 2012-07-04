using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Rhino.Mocks;

namespace Rpsls.Tests.Hi
{
	public enum GestureType
	{
		Rock,Paper, Scissor, Lizard, Spock
	}

	public interface IGesture
	{
		GestureType GType { get; set; }
	}

	public interface IEngage
	{
		GestureType Combat(IGesture one, IGesture two);
	}

	public class Scissor : IGesture
	{


		public bool Engage(Paper p)
		{
			return true;
		}

		public bool Engage(Lizard p)
		{
			return true;
		}
	}

	public class Paper
	{
		
	}

	public class Lizard
	{

	}

	public class TestGame
	{
		[Fact]
		public void ScissorsCutPaper()
		{
			var s = new Scissor();
			var p = new Paper();
			
			var r = s.Engage(p);

			Assert.Equal(r, true);
		}

		[Fact]
		public void Scissors_Decapitate_Lizard()
		{
			var s = new Scissor();
			var p = new Lizard();

			var r = s.Engage(p);

			Assert.Equal(r, true);
		}

		//Scissors cut paper
		//Paper covers rock
		//Rock crushes lizard
		//Lizard poisons Spock
		//Spock smashes scissors
		//Scissors decapitate lizard
		//Lizard eats paper
		//Paper disproves Spock
		//Spock vaporizes rock
		//Rock crushes scissors
	}
}
