using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UglyTrivia;

namespace Trivia
{
	[TestFixture]
	public class GameTest
	{
		[Test]
		public void Game_not_playable_for_fewer_than_two_players()
		{
			var game = new Game();
			Assert.That(game.CanStartGame(), Is.False);
			game.AddPlayer("Keith");
			Assert.That(game.CanStartGame(), Is.False);
			game.AddPlayer("martin");
			Assert.That(game.CanStartGame(), Is.True);
			game.AddPlayer("Mark");
			Assert.That(game.CanStartGame(), Is.True);
		}

		[Test]
		public void Game_can_be_won()
		{
			var game = new Game();
			game.AddPlayer("Keith");
			game.AddPlayer("martin");
			game.AddPlayer("Mark");

			var hasbeenwon = false;

			game.Play(5);
			hasbeenwon = game.UpdatePlayerStateOnCorrectAnswer();

			Assert.That(hasbeenwon, Is.True);
		}

		[Test]
		public void GoldenMasterTest()
		{
			var filename = $"golden-{DateTime.Now.ToFileTime()}.txt";

			using (var ms = new MemoryStream())
			using (var writer = new StreamWriter(ms))
			using (var fileStream = File.Create(filename))
			{
				Console.SetOut(writer);
				for (var i = 0; i < 1000; i++)
				{
					GameRunner.Main(new[] { i.ToString() });
				}
				ms.WriteTo(fileStream);
				ms.Flush();
			}

			var golden = File.ReadAllLines("goldenmaster.txt");
			var runFile = File.ReadAllLines(filename);

			for (var i = 0; i < golden.Length - 1; i++)
			{
				Assert.That(golden[i], Is.EqualTo(runFile[i]));
			}
		}
	}
}