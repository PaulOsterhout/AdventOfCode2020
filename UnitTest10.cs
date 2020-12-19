using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020
{
    public class UnitTest10
    {
        private readonly ITestOutputHelper outputHelper;

        public UnitTest10(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void Part1Sample()
        {
            AdapterTester smallDataSetTester = new AdapterTester(GetSmallSampleDataSet());
            Dictionary<int, int> smallDataSetResults = smallDataSetTester.ComputeJoltageDifferences();
            Assert.Equal(7, smallDataSetResults[1]);
            Assert.Equal(4, smallDataSetResults[3]);
            AdapterTester largeDataSetTester = new AdapterTester(GetLargeSampleDataSet());
            Dictionary<int, int> largeDataSetResults = largeDataSetTester.ComputeJoltageDifferences();
            Assert.Equal(22, largeDataSetResults[1]);
            Assert.Equal(9, largeDataSetResults[3]);
        }

        [Fact]
        public void Part1()
        {
            AdapterTester adapterTester = new AdapterTester(StringListRetriever.Retreive("InputList10.txt").Select(x => int.Parse(x)).ToList());
            Dictionary<int, int> results = adapterTester.ComputeJoltageDifferences();
            int answer = results[1] * (results[3] + 1);
            Assert.Equal(1914, answer);
        }

        [Fact]
        public void Part2SmallSample()
        {
            AdapterTester smallDataSetTester = new AdapterTester(GetSmallSampleDataSet());
            Assert.Equal(8, smallDataSetTester.GetJoltageArrangementCount());
        }

        [Fact]
        public void Part2LargeSample()
        {
            AdapterTester largeDataSetTester = new AdapterTester(GetLargeSampleDataSet());
            Assert.Equal(19208, largeDataSetTester.GetJoltageArrangementCount());
        }

        [Fact]// (Skip="Runs too long")]
        public void Part2()
        {
            AdapterTester adapterTester = new AdapterTester(StringListRetriever.Retreive("InputList10.txt").Select(x => int.Parse(x)).ToList());
            Assert.Equal(9256148959232, adapterTester.GetJoltageArrangementCount());
        }

        private void WriteArrangementListToOutputHelper(List<AdapterArrangement> arrangements, ITestOutputHelper outputHelper)
        {
            foreach (AdapterArrangement arrangement in arrangements.OrderBy(x => x.Key))
            {
                arrangement.WriteToOutputHelper(outputHelper);
            }
        }

        private class AdapterArrangement
        {
            private readonly List<int> arrangement;

            internal AdapterArrangement(List<int> initialArrangement)
            {
                arrangement = new List<int>(initialArrangement);
                Key = ToString();
            }

            internal void Append(int joltage)
            {
                arrangement.Add(joltage);
                Key = ToString();
            }

            protected int ArrangementCount()
            {
                return arrangement.Count;
            }

            internal bool CanAppend(int joltage)
            {
                int lastJoltage = arrangement.Last();
                return ((joltage - lastJoltage > 0) && (joltage - lastJoltage < 4));
            }

            public string Key { get; private set; }

			internal int MaxJoltage { get { return arrangement.Max(); } }

            internal List<AdapterArrangement> NewArrangements(int joltage)
            {
                List<AdapterArrangement> newArrangements = new List<AdapterArrangement>();
                AdapterArrangement testArrangement = new AdapterArrangement(arrangement);
                testArrangement.RemoveLast();
                while (testArrangement.ArrangementCount() > 0)
                {
                    if (!testArrangement.CanAppend(joltage)) break;
                    AdapterArrangement newArrangement = new AdapterArrangement(testArrangement.arrangement);
                    newArrangement.Append(joltage);
                    newArrangements.Add(newArrangement);
                    testArrangement.RemoveLast();
                }
                return newArrangements;
            }

            protected void RemoveLast()
            {
                arrangement.RemoveAt(arrangement.Count - 1);
            }

            internal void WriteToOutputHelper(ITestOutputHelper outputHelper)
            {
                outputHelper.WriteLine(Key);
            }

            public override string ToString()
            {
                return string.Join(',', arrangement);
            }

            internal AdapterArrangement Clone()
            {
                return new AdapterArrangement(arrangement);
            }
        }

        private class AdapterTester
        {
            private readonly List<int> joltages;

            internal AdapterTester(List<int> joltages)
            {
                this.joltages = joltages.OrderBy(x => x).ToList();
            }

            internal List<AdapterArrangement> ComputeArrangements()
            {
                int previousJoltage = 0;
                AdapterArrangement first = new AdapterArrangement(new List<int> { previousJoltage });
                Dictionary<string, AdapterArrangement> arrangements = new Dictionary<string, AdapterArrangement> { { first.Key, first } };
                foreach (int joltage in joltages)
                {
                    List<AdapterArrangement> newArrangements = new List<AdapterArrangement>();
                    foreach (string key in arrangements.Keys)
                    {
                        AdapterArrangement arrangement = arrangements[key];
                        newArrangements.AddRange(arrangement.NewArrangements(joltage));
                        if (arrangement.CanAppend(joltage))
                        {
                            arrangement.Append(joltage);
                        }
                    }
                    foreach (AdapterArrangement newArrangement in newArrangements)
                    {
                        if (!arrangements.ContainsKey(newArrangement.Key))
                        {
                            arrangements.Add(newArrangement.Key, newArrangement);
                        }
                    }
                    previousJoltage = joltage;
                }
                return arrangements.Select(x => x.Value).ToList();
            }

            internal List<AdapterArrangement> ComputeArrangements2()
            {
				joltages.Insert(0, 0);
                int minJoltage = joltages.Min();
                int maxJoltage = joltages.Max();
                AdapterArrangement first = new AdapterArrangement(new List<int> { minJoltage });
                Dictionary<string, AdapterArrangement> arrangements = new Dictionary<string, AdapterArrangement> { { first.Key, first } };
                for (int joltage = minJoltage; joltage < maxJoltage; joltage++)
                {
					if (joltages.Contains(joltage))
					{
						Dictionary<int, bool> nextJoltages = new Dictionary<int, bool>
						{
							{ joltage + 1, joltages.Contains(joltage + 1) },
							{ joltage + 2, joltages.Contains(joltage + 2) },
							{ joltage + 3, joltages.Contains(joltage + 3) }
						};
						Dictionary<string, AdapterArrangement> nextArrangements = new Dictionary<string, AdapterArrangement>();
						foreach (AdapterArrangement arrangement in arrangements.Select(x => x.Value))
						{
							if (arrangement.MaxJoltage == joltage)
							{
								foreach (int nextJoltage in nextJoltages.Where(x => x.Value).Select(x => x.Key))
								{
									AdapterArrangement newArrangement = arrangement.Clone();
									newArrangement.Append(nextJoltage);
									nextArrangements.Add(newArrangement.Key, newArrangement);
								}
							}
							else
							{
								nextArrangements.Add(arrangement.Key, arrangement);
							}
						}
						arrangements = nextArrangements;
					}
                }
                return arrangements.Select(x => x.Value).ToList();
            }

            internal Dictionary<int, int> ComputeJoltageDifferences()
            {
                Dictionary<int, int> joltageDifferences = new Dictionary<int, int>();
                int previousJoltage = 0;
                foreach (int joltage in joltages)
                {
                    int joltageDifference = joltage - previousJoltage;
                    if (joltageDifferences.Keys.Any(x => x == joltageDifference))
                    {
                        joltageDifferences[joltageDifference] = joltageDifferences[joltageDifference] + 1;
                    }
                    else
                    {
                        joltageDifferences.Add(joltageDifference, 1);
                    }
                    previousJoltage = joltage;
                }
                return joltageDifferences;
            }

            internal long GetJoltageArrangementCount()
            {
				joltages.Insert(0, 0);
                int minJoltage = joltages.Min();
                int maxJoltage = joltages.Max();
                Dictionary<int, long> joltageCounts = new Dictionary<int, long> { { 0, 1 } };
                for (int joltage = minJoltage; joltage < maxJoltage; joltage++)
                {
					if (joltages.Contains(joltage) && joltageCounts.ContainsKey(joltage))
					{
                        long currentCount = joltageCounts[joltage];
                        joltageCounts.Remove(joltage);
						Dictionary<int, bool> nextJoltages = new Dictionary<int, bool>
						{
							{ joltage + 1, joltages.Contains(joltage + 1) },
							{ joltage + 2, joltages.Contains(joltage + 2) },
							{ joltage + 3, joltages.Contains(joltage + 3) }
						};
                        foreach (int nextJoltage in nextJoltages.Where(x => x.Value).Select(x => x.Key))
                        {
                            if (joltageCounts.ContainsKey(nextJoltage))
                            {
                                joltageCounts[nextJoltage] = joltageCounts[nextJoltage] + currentCount;
                            }
                            else
                            {
                                joltageCounts.Add(nextJoltage, currentCount);
                            }
                        }
					}
                }
                return joltageCounts.Select(x => x.Value).Sum();
            }
        }

        private List<int> GetSmallSampleDataSet()
        {
            return new List<int>
            {
                16,
                10,
                15,
                5,
                1,
                11,
                7,
                19,
                6,
                12,
                4
            };
        }

        private List<int> GetLargeSampleDataSet()
        {
            return new List<int>
            {
                28,
                33,
                18,
                42,
                31,
                14,
                46,
                20,
                48,
                47,
                24,
                23,
                49,
                45,
                19,
                38,
                39,
                11,
                1,
                32,
                25,
                35,
                8,
                17,
                7,
                9,
                4,
                2,
                34,
                10,
                3
            };
        }
    }
}