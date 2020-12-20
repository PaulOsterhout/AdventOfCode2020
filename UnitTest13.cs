using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest13
	{
		[Fact]
		public void Part1Sample()
		{
			BusScheduler busScheduler = new BusScheduler();
			BusSchedulerResult busSchedulerResult = busScheduler.FindBus(GetSampleInput());
			long answer = busSchedulerResult.BusId * busSchedulerResult.MinutesWaited;
			Assert.Equal(295, answer);
		}

		[Fact]
		public void Part1()
		{
			BusScheduler busScheduler = new BusScheduler();
			BusSchedulerResult busSchedulerResult = busScheduler.FindBus(StringListRetriever.Retreive("InputList13.txt").ToList());
			long answer = busSchedulerResult.BusId * busSchedulerResult.MinutesWaited;
			Assert.Equal(6568, answer);
		}

		[Fact(Skip="Runs too long")]
		public void Part2Sample()
		{
			BusScheduler busScheduler = new BusScheduler();
			Assert.Equal(1068781, busScheduler.FindSequentialDepartures(GetSampleInput()));
		}

		private List<string> GetSampleInput()
		{
			return new List<string>
			{
				"939",
				"7,13,x,x,59,x,31,19"
			};
		}

        private class BusScheduler
        {
			internal BusSchedulerResult FindBus(List<string> input)
			{
				long startTime = int.Parse(input[0]);
				long currentTime = startTime;
				List<int> availableBusIds = input[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToList();
				Dictionary<int, long> busSchedule = availableBusIds.ToDictionary(x => x, x => (long)-1);
				do
				{
					foreach (int busId in availableBusIds)
					{
						if ((busSchedule[busId] == -1) && (currentTime % busId == 0))
						{
							busSchedule[busId] = currentTime;
						}
					}
					currentTime++;
				} while (busSchedule.Any(x => x.Value == -1));
				long minTime = busSchedule.Min(x => x.Value);
				return new BusSchedulerResult
				{
					BusId = busSchedule.First(x => x.Value == minTime).Key,
					MinutesWaited = minTime - startTime
				};
			}

			internal long FindSequentialDepartures(List<string> input)
			{
				string[] busIds = input[1].Split(',');
				int difference = 0;
				Dictionary<int, int> availableBusIds = new Dictionary<int, int>();
				foreach (string busId in busIds)
				{
					if (busId != "x")
					{
						availableBusIds.Add(int.Parse(busId), difference);
					}
					difference++;
				}
				int minBusId = availableBusIds.Keys.Min();
				int timeIncrement = availableBusIds.Keys.Max(); 
				long currentTime = (timeIncrement - minBusId) * -1;
				bool sequenceFound;
				do
				{
					currentTime += timeIncrement;
					sequenceFound = true;
					foreach (int busId in availableBusIds.Keys)
					{
						if ((currentTime + availableBusIds[busId]) % busId > 0)
						{
							sequenceFound = false;
							break;
						}
					}
				} while (!sequenceFound);
				return currentTime;
			}
        }

		internal class BusSchedulerResult
		{
			internal int BusId;
			internal long MinutesWaited;
		}
    }
}
