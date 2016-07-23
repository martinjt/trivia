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
			Assert.That(game.isPlayable(), Is.False);
			game.add("Keith");
			Assert.That(game.isPlayable(), Is.False);
			game.add("martin");
			Assert.That(game.isPlayable(), Is.True);
			game.add("Mark");
			Assert.That(game.isPlayable(), Is.True);
		}

		[Test]
		public void Game_can_be_won()
		{
			var game = new Game();
			game.add("Keith");
			game.add("martin");
			game.add("Mark");

			var hasbeenwon = false;

			game.roll(5);
			hasbeenwon = game.wasCorrectlyAnswered();

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