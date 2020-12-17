using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest6
	{
		[Fact]
		public void MiscellaneousTests()
		{
			List<string> items = StringListRetriever.Retreive("InputList6.txt").ToList();
			int count = items.Count;
			Assert.Equal(2172, count);
			Assert.Equal("su", items[0]);
			Assert.Equal("lzdaftrjphco", items[2171]);
			List<PassengerGroup> passengerGroups = PassengerGroupsParser.Parse(StringListRetriever.Retreive("InputList6.txt").ToList());
			Assert.Equal(2, passengerGroups.First().Passengers.Count);
			Assert.Equal(3, passengerGroups.Last().Passengers.Count);
			Assert.Equal(("lzdaftrjphco").Length, passengerGroups.Last().Passengers.Last().YesAnswers.Count);
			Assert.Equal('a', passengerGroups.Last().Passengers.Last().YesAnswers[0]);
			Assert.Equal('z', passengerGroups.Last().Passengers.Last().YesAnswers[11]);
			Assert.Equal(4, passengerGroups.First().YesQuestionCount());
		}

		[Fact]
		public void Part1()
		{
			List<PassengerGroup> passengerGroups = PassengerGroupsParser.Parse(StringListRetriever.Retreive("InputList6.txt").ToList());
			Assert.Equal(6590, passengerGroups.Sum(x => x.YesQuestionCount()));
		}

		[Fact]
		public void Part2()
		{
			List<PassengerGroup> passengerGroups = PassengerGroupsParser.Parse(StringListRetriever.Retreive("InputList6.txt").ToList());
			Assert.Equal(3288, passengerGroups.Sum(x => x.EveryoneYesCount()));
		}

		private static class PassengerGroupsParser
		{
			public static List<PassengerGroup> Parse(List<string> yesAnswersList)
			{
				PassengerGroup currentGroup = null;
				List<PassengerGroup> passengerGroups = new List<PassengerGroup>();
				foreach (string yesAnswers in yesAnswersList)
				{
					if (yesAnswers.Length == 0)
					{
						if (currentGroup != null)
						{
							passengerGroups.Add(currentGroup);
						}
						currentGroup = null;
						continue;
					}
					if (currentGroup == null)
					{
						currentGroup = new PassengerGroup();
					}
					currentGroup.Passengers.Add(new Passenger(yesAnswers));
				}
				if (currentGroup != null)
				{
					passengerGroups.Add(currentGroup);
				}
				return passengerGroups;
			}
		}

		private class PassengerGroup
		{
			public readonly List<Passenger> Passengers;

			public PassengerGroup()
			{
				Passengers = new List<Passenger>();
			}

			public int EveryoneYesCount()
			{
				List<char> yesAnswers = new List<char>(Passengers.First().YesAnswers);
				for (int passengerIndex = 1; passengerIndex < Passengers.Count; passengerIndex++)
				{
					yesAnswers = yesAnswers.Where(x => Passengers[passengerIndex].YesAnswers.Any(y => y == x)).ToList();
				}
				return yesAnswers.OrderBy(x => x).Distinct().Count();
			}

			public int YesQuestionCount()
			{
				List<char> yesAnswers = new List<char>();
				Passengers.ForEach(x => yesAnswers.AddRange(x.YesAnswers));
				return yesAnswers.OrderBy(x => x).Distinct().Count();
			}
		}

		private class Passenger
		{
			public readonly List<char> YesAnswers;

			public Passenger(string yesAnswers)
			{
				YesAnswers = yesAnswers.OrderBy(x => x).ToList();
			}
		}
	}
}
