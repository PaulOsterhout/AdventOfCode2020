using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
    public class UnitTest9
    {
        private List<long> GetSampleList()
        {
            return new List<long>
            {
                35,
                20,
                15,
                25,
                47,
                40,
                62,
                55,
                65,
                95,
                102,
                117,
                150,
                182,
                127,
                219,
                299,
                277,
                309,
                576
            };
        }

        [Fact]
        public void Part1Sample()
        {
            List<long> numbers = GetSampleList();
            int preamble = 5;
            XmasDecoder xmasDecoder = new XmasDecoder();
            xmasDecoder.Decode(numbers, preamble);
            Assert.Equal(127, xmasDecoder.BadNumber);
        }

        [Fact]
        public void Part1()
        {
            List<long> numbers = StringListRetriever.Retreive("InputList9.txt").Select(x => long.Parse(x)).ToList();
            int preamble = 25;
            XmasDecoder xmasDecoder = new XmasDecoder();
            xmasDecoder.Decode(numbers, preamble);
            Assert.Equal(552655238, xmasDecoder.BadNumber);
        }

        [Fact]
        public void Part2Sample()
        {
            List<long> numbers = GetSampleList();
            int preamble = 5;
            XmasDecoder xmasDecoder = new XmasDecoder();
            xmasDecoder.Decode(numbers, preamble);
            Assert.Equal(14, xmasDecoder.BadIndex);
            Assert.Equal(127, xmasDecoder.BadNumber);
            List<long> badSet = xmasDecoder.FindSetFor(numbers, xmasDecoder.BadIndex);
            Assert.Equal(15, badSet.Min());
            Assert.Equal(47, badSet.Max());
            Assert.Equal(62, badSet.Min() + badSet.Max());
        }

        [Fact]
        public void Part2()
        {
            List<long> numbers = StringListRetriever.Retreive("InputList9.txt").Select(x => long.Parse(x)).ToList();
            int preamble = 25;
            XmasDecoder xmasDecoder = new XmasDecoder();
            xmasDecoder.Decode(numbers, preamble);
            Assert.Equal(552655238, xmasDecoder.BadNumber);
            Assert.Equal(610, xmasDecoder.BadIndex);
            List<long> badSet = xmasDecoder.FindSetFor(numbers, xmasDecoder.BadIndex);
            Assert.Equal(70672245, badSet.Min() + badSet.Max());
        }

        private class XmasDecoder
        {
            public int BadIndex { get; private set;}
            public long BadNumber { get; private set; }

            internal void Decode(List<long> numbers, int preamble)
            {
                BadNumber = -1;
                for (int index = preamble; index < numbers.Count; index++)
                {
                    int startIndex = index - preamble;
                    int endIndex = index - 1;
                    bool found = false;
                    for (int firstIndex = startIndex; firstIndex < endIndex; firstIndex++)
                    {
                        for (int secondIndex = firstIndex + 1; secondIndex <= endIndex; secondIndex++)
                        {
                            if (numbers[index] == numbers[firstIndex] + numbers[secondIndex])
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                    if (!found)
                    {
                        BadIndex = index;
                        BadNumber = numbers[index];
                        break;
                    }
                }
            }

            internal List<long> FindSetFor(List<long> numbers, int totalIndex)
            {
                List<long> foundSet = new List<long>();
                long targetTotal = numbers[totalIndex];
                for (int startIndex = 0; startIndex < totalIndex; startIndex++)
                {
                    foundSet.Add(numbers[startIndex]);
                    long total = numbers[startIndex];
                    for (int nextIndex = startIndex + 1; nextIndex < totalIndex; nextIndex++)
                    {
                        foundSet.Add(numbers[nextIndex]);
                        total += numbers[nextIndex];
                        if (total >= targetTotal)
                        {
                            break;
                        }
                    }
                    if (total == targetTotal)
                    {
                        break;
                    }
                    else
                    {
                        foundSet = new List<long>();
                    }
                }
                return foundSet;
            }
        }
    }
}
