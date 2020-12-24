using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest15
	{
		[Theory]
		[ClassData(typeof(Part1SampleData))]
		public void Part1Sample(int expected, int number1, int number2, int number3)
		{
			int[] numbers = new int[] { number1, number2, number3 };
			NumbersGame game = new NumbersGame();
			game.Play(numbers, 2020);
			Assert.Equal(expected, game.CurrentNumber);
		}

		[Fact]
		public void Part1()
		{
			int[] numbers = new int[] { 9, 12, 1, 4, 17, 0, 18 };
			NumbersGame game = new NumbersGame();
			game.Play(numbers, 2020);
			Assert.Equal(610, game.CurrentNumber);
		}

		[Theory(Skip="Runs Too long")]
		[ClassData(typeof(Part2SampleData))]
		public void Part2Sample(int expected, int number1, int number2, int number3)
		{
			int[] numbers = new int[] { number1, number2, number3 };
			NumbersGame game = new NumbersGame();
			game.Play(numbers, 30000000);
			Assert.Equal(expected, game.CurrentNumber);
		}

		[Fact(Skip="Runs Too long")]
		public void Part2()
		{
			int[] numbers = new int[] { 9, 12, 1, 4, 17, 0, 18 };
			NumbersGame game = new NumbersGame();
			game.Play(numbers, 30000000);
			Assert.Equal(1407, game.CurrentNumber);
		}

        private class NumbersGame
        {
            public int CurrentNumber { get; private set; }

            public void Play(int[] initialNumbers, int turns)
            {
				Dictionary<int, int> previousTurns = initialNumbers.Select((value, index) => new { value, index }).ToDictionary(pair => pair.value, pair => pair.index + 1);
				Dictionary<int, int> spokenCounts = initialNumbers.ToDictionary(x => x, y => 1 );
				int turn = initialNumbers.Length;
				CurrentNumber = initialNumbers[turn - 1];
				int previousNumber;
				while (turn < turns)
				{
					previousNumber = CurrentNumber;
					if (previousTurns.ContainsKey(previousNumber))
					{						
						CurrentNumber = turn - previousTurns[previousNumber];
						previousTurns[previousNumber] = turn;
					}
					else
					{
						CurrentNumber = 0;
						previousTurns.Add(previousNumber, turn);
					}
					if (spokenCounts.ContainsKey(CurrentNumber))
					{
						spokenCounts[CurrentNumber] = spokenCounts[CurrentNumber] + 1;
					}
					else
					{
						spokenCounts[CurrentNumber] = 1;
					}
					turn++;
				}
            }
        }

		private class Part1SampleData : IEnumerable<object[]>
		{
            public IEnumerator<object[]> GetEnumerator()
            {
				yield return new object [] { 436, 0, 3, 6 };
				yield return new object [] { 1, 1, 3, 2 };
				yield return new object [] { 10, 2, 1, 3 };
				yield return new object [] { 27, 1, 2, 3 };
				yield return new object [] { 78, 2, 3, 1 };
				yield return new object [] { 438, 3, 2, 1 };
				yield return new object [] { 1836, 3, 1, 2 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		private class Part2SampleData : IEnumerable<object[]>
		{
            public IEnumerator<object[]> GetEnumerator()
            {
				yield return new object [] { 175594, 0, 3, 6 };
				yield return new object [] { 2578, 1, 3, 2 };
				yield return new object [] { 3544142, 2, 1, 3 };
				yield return new object [] { 261214, 1, 2, 3 };
				yield return new object [] { 6895259, 2, 3, 1 };
				yield return new object [] { 18, 3, 2, 1 };
				yield return new object [] { 362, 3, 1, 2 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
    }
}
