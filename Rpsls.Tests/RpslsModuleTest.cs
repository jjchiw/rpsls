using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Nancy.Testing;
using Nancy;
using Rpsls.Models;
using Rpsls.Components;

namespace Rpsls.Tests
{
	public class RpslsModuleTest
	{
		[Fact]
		public void IndexTest()
		{
			//var result = new Browser(new DefaultNancyBootstrapper()).Get(
			//"/",
			//with =>
			//{
			//    with.HttpRequest();
			//});

			//Assert.Equal(HttpStatusCode.OK, result.StatusCode);
		}

		[Fact]
		public void GameNotImplemetedTest()
		{
			//var result = new Browser(new DefaultNancyBootstrapper()).Post(
			//    "/game",
			//    with =>
			//    {
			//        with.FormValue("gesture", "Picard"); //Scissor
			//    }
			//    );

			//Assert.Equal(HttpStatusCode.NotImplemented, result.StatusCode);
		}

		[Fact]
		public void GameWinnerPlayerOneTest()
		{
			var result = new Browser(new DefaultNancyBootstrapper()).Post(
				"/gameWinnerPlayerOne",
				with =>
				{
					with.FormValue("gesture", "Scissors"); //Scissor
				}
				);

			Assert.Equal(result.Body.AsString(), "Winner PlayerOne");
			Assert.Equal(HttpStatusCode.OK, result.StatusCode);
		}

		[Fact]
		public void GameWinnerPlayerTwoTest()
		{
			var result = new Browser(new DefaultNancyBootstrapper()).Post(
				"/gameWinnerPlayerTwo",
				with =>
				{
					with.FormValue("gesture", "Scissors"); //Scissor
				}
				);

			Assert.Equal(result.Body.AsString(), "Winner PlayerTwo");
			Assert.Equal(HttpStatusCode.OK, result.StatusCode);
		}

		public class RootModule : NancyModule
		{
			public RootModule()
			{
				Post["/gameWinnerPlayerOne"] = GameWinnerPlayerOne;
				Post["/gameWinnerPlayerTwo"] = GameWinnerPlayerTwo;
			}

			private Response GameWinnerPlayerOne(dynamic o)
			{
				GestureType outcome;
				Console.WriteLine((Request.Form.gesture as string));
				var parsed = Enum.TryParse<GestureType>(Request.Form.gesture.Value, out outcome);
				if (!parsed)
					return HttpStatusCode.NotImplemented;

				var eng = new EngageComponent();

				var p = new Player(PlayerNumber.PlayerOne, outcome);
				var p2 = new Player(PlayerNumber.PlayerTwo, GestureType.Paper);

				PlayerNumber winner;
				var fought = eng.TryEngage(p, p2, out winner);

				if (!fought)
					return Response.AsText("Tie");

				return Response.AsText("Winner " + winner);
			}

			private Response GameWinnerPlayerTwo(dynamic o)
			{
				GestureType outcome;
				Console.WriteLine((Request.Form.gesture as string));
				var parsed = Enum.TryParse<GestureType>(Request.Form.gesture.Value, out outcome);
				if (!parsed)
					return HttpStatusCode.NotImplemented;

				var eng = new EngageComponent();

				var p = new Player(PlayerNumber.PlayerOne, outcome);
				var p2 = new Player(PlayerNumber.PlayerTwo, GestureType.Rock);

				PlayerNumber winner;
				var fought = eng.TryEngage(p, p2, out winner);

				if (!fought)
					return Response.AsText("Tie");

				return Response.AsText("Winner " + winner);
			}

			private Response Root(dynamic o)
			{
				return HttpStatusCode.OK;
			}
		}
	}
}
