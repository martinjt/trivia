using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyTrivia
{
	public class Game
	{


		List<string> playerNames = new List<string>();

		int[] placeFor = new int[6];
		int[] amountOfCoinsFor = new int[6];

		bool[] penaltyBoxStateFor = new bool[6];

		LinkedList<string> popQuestions = new LinkedList<string>();
		LinkedList<string> scienceQuestions = new LinkedList<string>();
		LinkedList<string> sportsQuestions = new LinkedList<string>();
		LinkedList<string> rockQuestions = new LinkedList<string>();

		int currentPlayerIndex = 0;
		bool CurrentPlayCouldGetPlayerOutOfPenaltyBox;

		public Game()
		{
			for (int i = 0; i < 50; i++)
			{
				popQuestions.AddLast("Pop Question " + i);
				scienceQuestions.AddLast(("Science Question " + i));
				sportsQuestions.AddLast(("Sports Question " + i));
				rockQuestions.AddLast(createRockQuestion(i));
			}
		}

		public String createRockQuestion(int index)
		{
			return "Rock Question " + index;
		}

		public bool CanStartGame()
		{
			return (LastPlayerAdded() >= 2);
		}

		public bool AddPlayer(String playerName)
		{


			playerNames.Add(playerName);
			placeFor[LastPlayerAdded()] = 0;
			amountOfCoinsFor[LastPlayerAdded()] = 0;
			penaltyBoxStateFor[LastPlayerAdded()] = false;

			Console.WriteLine(playerName + " was added");
			Console.WriteLine("They are player number " + playerNames.Count);
			return true;
		}

		public int LastPlayerAdded()
		{
			return playerNames.Count;
		}

		public void Play(int roll)
		{
			Console.WriteLine(playerNames[currentPlayerIndex] + " is the current player");
			Console.WriteLine("They have rolled a " + roll);

			if (penaltyBoxStateFor[currentPlayerIndex])
			{
				if (roll % 2 != 0)
				{
					CurrentPlayCouldGetPlayerOutOfPenaltyBox = true;

					Console.WriteLine(playerNames[currentPlayerIndex] + " is getting out of the penalty box");
					placeFor[currentPlayerIndex] = placeFor[currentPlayerIndex] + roll;
					if (placeFor[currentPlayerIndex] > 11) placeFor[currentPlayerIndex] = placeFor[currentPlayerIndex] - 12;

					Console.WriteLine(playerNames[currentPlayerIndex]
							+ "'s new location is "
							+ placeFor[currentPlayerIndex]);
					Console.WriteLine("The category is " + CategoryForCurrentPlayerPosition());
					ConsumeNextQuestionForCurrentPosition();
				}
				else
				{
					Console.WriteLine(playerNames[currentPlayerIndex] + " is not getting out of the penalty box");
					CurrentPlayCouldGetPlayerOutOfPenaltyBox = false;
				}

			}
			else
			{

				placeFor[currentPlayerIndex] = placeFor[currentPlayerIndex] + roll;
				if (placeFor[currentPlayerIndex] > 11) placeFor[currentPlayerIndex] = placeFor[currentPlayerIndex] - 12;

				Console.WriteLine(playerNames[currentPlayerIndex]
						+ "'s new location is "
						+ placeFor[currentPlayerIndex]);
				Console.WriteLine("The category is " + CategoryForCurrentPlayerPosition());
				ConsumeNextQuestionForCurrentPosition();
			}

		}

		private void ConsumeNextQuestionForCurrentPosition()
		{
			if (CategoryForCurrentPlayerPosition() == "Pop")
			{
				Console.WriteLine(popQuestions.First());
				popQuestions.RemoveFirst();
			}
			if (CategoryForCurrentPlayerPosition() == "Science")
			{
				Console.WriteLine(scienceQuestions.First());
				scienceQuestions.RemoveFirst();
			}
			if (CategoryForCurrentPlayerPosition() == "Sports")
			{
				Console.WriteLine(sportsQuestions.First());
				sportsQuestions.RemoveFirst();
			}
			if (CategoryForCurrentPlayerPosition() == "Rock")
			{
				Console.WriteLine(rockQuestions.First());
				rockQuestions.RemoveFirst();
			}
		}


		private String CategoryForCurrentPlayerPosition()
		{
			if (placeFor[currentPlayerIndex] == 0) return "Pop";
			if (placeFor[currentPlayerIndex] == 4) return "Pop";
			if (placeFor[currentPlayerIndex] == 8) return "Pop";
			if (placeFor[currentPlayerIndex] == 1) return "Science";
			if (placeFor[currentPlayerIndex] == 5) return "Science";
			if (placeFor[currentPlayerIndex] == 9) return "Science";
			if (placeFor[currentPlayerIndex] == 2) return "Sports";
			if (placeFor[currentPlayerIndex] == 6) return "Sports";
			if (placeFor[currentPlayerIndex] == 10) return "Sports";
			return "Rock";
		}

		public bool UpdatePlayerStateOnCorrectAnswer()
		{
			if (penaltyBoxStateFor[currentPlayerIndex])
			{
				if (CurrentPlayCouldGetPlayerOutOfPenaltyBox)
				{
					Console.WriteLine("Answer was correct!!!!");
					amountOfCoinsFor[currentPlayerIndex]++;
					Console.WriteLine(playerNames[currentPlayerIndex]
							+ " now has "
							+ amountOfCoinsFor[currentPlayerIndex]
							+ " Gold Coins.");

					bool playResultedInWin = CurrentPlayerIsWinner();
					currentPlayerIndex++;
					if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;

					return playResultedInWin;
				}
				else
				{
					currentPlayerIndex++;
					if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;
					return true;
				}



			}
			else
			{

				Console.WriteLine("Answer was corrent!!!!");
				amountOfCoinsFor[currentPlayerIndex]++;
				Console.WriteLine(playerNames[currentPlayerIndex]
						+ " now has "
						+ amountOfCoinsFor[currentPlayerIndex]
						+ " Gold Coins.");

				bool playResultedInWin = CurrentPlayerIsWinner();
				currentPlayerIndex++;
				if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;

				return playResultedInWin;
			}
		}

		public bool UpdatePlayerStateOnWrongAnswer()
		{
			Console.WriteLine("Question was incorrectly answered");
			Console.WriteLine(playerNames[currentPlayerIndex] + " was sent to the penalty box");
			penaltyBoxStateFor[currentPlayerIndex] = true;

			currentPlayerIndex++;
			if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;
			return true;
		}


		private bool CurrentPlayerIsWinner()
		{
			return !(amountOfCoinsFor[currentPlayerIndex] == 6);
		}
	}

}
