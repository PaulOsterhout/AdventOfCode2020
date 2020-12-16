using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
    public class UnitTest3
    {
        private class Map
        {
            private readonly List<string> mapLines;
            private int xPosition = 0;
            private int yPosition = 0;

            public Map(IEnumerable<string> mapLines)
            {
                this.mapLines = mapLines.ToList();
            }

            public int CountTrees(int xIncrement, int yIncrement)
            {
                int treeCount = 0;
                xPosition = 0;
                yPosition = 0;
                do
                {
                    MoveDown(xIncrement, yIncrement);
                    if (IsTree())
                    {
                        treeCount++;
                    }
                } while (!AtBottom());
                return treeCount;
            }

            private bool AtBottom()
            {
                return yPosition == mapLines.Count - 1;
            }

            private bool IsOpen()
            {
                char position = mapLines[yPosition][xPosition];
                return position.Equals('.');
            }

            private bool IsTree()
            {
                char position = mapLines[yPosition][xPosition];
                return position.Equals('#');
            }

            private void MoveDown(int xIncrement, int yIncrement)
            {
                yPosition = yPosition + yIncrement;
                xPosition = xPosition + xIncrement;
                int mapWidth = mapLines[yPosition].Length;
                if (xPosition >= mapWidth)
                {
                    xPosition = xPosition - mapWidth;
                }
            }
        }

        [Fact]
        public void CountTreesPart1()
        {
            Map map = new Map(StringListRetriever.Retreive("InputList3.txt"));
            Assert.Equal(195, map.CountTrees(3, 1));
        }

        [Fact]
        public void CountTreesPart2()
        {
            Map map = new Map(StringListRetriever.Retreive("InputList3.txt"));
            int count1 = map.CountTrees(1, 1);
            Assert.Equal(84, count1);
            int count2 = map.CountTrees(3, 1);
            Assert.Equal(195, count2);
            int count3 = map.CountTrees(5, 1);
            Assert.Equal(70, count3);
            int count4 = map.CountTrees(7, 1);
            Assert.Equal(70, count4);
            int count5 = map.CountTrees(1, 2);
            Assert.Equal(47, count5);
            long multipled = (long)count1 * (long)count2 * (long)count3 * (long)count4 * (long)count5;
            Assert.Equal(3772314000, multipled);
        }
    }
}
