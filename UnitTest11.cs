using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest11
	{
		[Fact]
		public void Part1Sample()
		{
			Ferry ferry = new Ferry();
			Assert.Equal(37, ferry.GetSeatsAfterEquilibriumPart1(GetSampleLayout()));
		}

		[Fact]
		public void Part1()
		{
			Ferry ferry = new Ferry();
			Assert.Equal(2222, ferry.GetSeatsAfterEquilibriumPart1(StringListRetriever.Retreive("InputList11.txt").ToList()));
		}

		[Fact]
		public void Part2Sample()
		{
			Ferry ferry = new Ferry();
			Assert.Equal(26, ferry.GetSeatsAfterEquilibriumPart2(GetSampleLayout()));
		}

		[Fact]
		public void Part2()
		{
			Ferry ferry = new Ferry();
			Assert.Equal(2032, ferry.GetSeatsAfterEquilibriumPart2(StringListRetriever.Retreive("InputList11.txt").ToList()));
		}

		private class Ferry
		{
			internal int GetSeatsAfterEquilibriumPart1(List<string> layout)
			{
				List<string> current = new List<string>(layout);
				do
				{
					List<string> next = GetNextPart1Layout(current);
					if (LayoutsMatch(current, next)) break;
					current = new List<string>(next);
				} while (true);
				return current.Sum(x => x.Count(x => x == '#'));
			}

            internal int GetSeatsAfterEquilibriumPart2(List<string> layout)
            {
				List<string> current = new List<string>(layout);
				do
				{
					List<string> next = GetNextPart2Layout(current);
					if (LayoutsMatch(current, next)) break;
					current = new List<string>(next);
				} while (true);
				return current.Sum(x => x.Count(x => x == '#'));
            }

			private List<string> GetNextPart1Layout(List<string> layout)
			{
				List<string> next = new List<string>();
				for (int rowIndex = 0; rowIndex < layout.Count; rowIndex++)
				{
					System.Text.StringBuilder row = new System.Text.StringBuilder();
					for (int columnIndex = 0; columnIndex < layout[rowIndex].Length; columnIndex++)
					{
						char currentState = layout[rowIndex][columnIndex];
						if (currentState == '.')
						{
							row.Append('.');
						}
						else
						{
							int occupiedCount = GetOccupiedCountForPart1(layout, rowIndex, columnIndex);
							if (currentState == 'L')
							{
								row.Append((occupiedCount == 0) ? '#' : 'L');
							}
							else
							{
								row.Append((occupiedCount > 3) ? 'L' : '#');
							}
						}
					}
					next.Add(row.ToString());
				}
				return next;
			}

			private List<string> GetNextPart2Layout(List<string> layout)
			{
				List<string> next = new List<string>();
				for (int rowIndex = 0; rowIndex < layout.Count; rowIndex++)
				{
					System.Text.StringBuilder row = new System.Text.StringBuilder();
					for (int columnIndex = 0; columnIndex < layout[rowIndex].Length; columnIndex++)
					{
						char currentState = layout[rowIndex][columnIndex];
						if (currentState == '.')
						{
							row.Append('.');
						}
						else
						{
							int occupiedCount = GetOccupiedCountForPart2(layout, rowIndex, columnIndex);
							if (currentState == 'L')
							{
								row.Append((occupiedCount == 0) ? '#' : 'L');
							}
							else
							{
								row.Append((occupiedCount > 4) ? 'L' : '#');
							}
						}
					}
					next.Add(row.ToString());
				}
				return next;
			}

			private int GetOccupiedCountForPart1(List<string> layout, int rowIndex, int columnIndex)
			{
				int occupiedCount = 0
					+ ((GetLayoutValueForPart1(layout, rowIndex - 1, columnIndex - 1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart1(layout, rowIndex - 1, columnIndex) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart1(layout, rowIndex - 1, columnIndex + 1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart1(layout, rowIndex, columnIndex - 1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart1(layout, rowIndex, columnIndex + 1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart1(layout, rowIndex + 1, columnIndex - 1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart1(layout, rowIndex + 1, columnIndex) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart1(layout, rowIndex + 1, columnIndex + 1) == '#') ? 1 : 0);
				return occupiedCount;
			}

			private int GetOccupiedCountForPart2(List<string> layout, int rowIndex, int columnIndex)
			{
				int occupiedCount = 0
					+ ((GetLayoutValueForPart2(layout, rowIndex, columnIndex, -1, -1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart2(layout, rowIndex, columnIndex, -1, 0) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart2(layout, rowIndex, columnIndex, -1, 1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart2(layout, rowIndex, columnIndex, 0, -1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart2(layout, rowIndex, columnIndex, 0, 1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart2(layout, rowIndex, columnIndex, 1, -1) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart2(layout, rowIndex, columnIndex, 1, 0) == '#') ? 1 : 0)
					+ ((GetLayoutValueForPart2(layout, rowIndex, columnIndex, 1, 1) == '#') ? 1 : 0);
				return occupiedCount;
			}

            private char GetLayoutValueForPart1(List<string> layout, int rowIndex, int columnIndex, char offLayoutValue = '.')
			{
				if ((rowIndex < 0) || (rowIndex >= layout.Count) || (columnIndex < 0) || (columnIndex >= layout[0].Length)) return offLayoutValue;
				return layout[rowIndex][columnIndex];
			}

            private char GetLayoutValueForPart2(List<string> layout, int layoutRowIndex, int layoutColumnIndex, int rowOffset, int columnOffset)
            {
				int rowIndex = layoutRowIndex;
				int columnIndex = layoutColumnIndex;
				do
				{
					rowIndex += rowOffset;
					columnIndex += columnOffset;
					char value = GetLayoutValueForPart1(layout, rowIndex, columnIndex, '!');
					switch (value)
					{
						case '!': return '.';
						case '#':
						case 'L': return value;
					} 
				} while (true);
            }

			private bool LayoutsMatch(List<string> current, List<string> next)
			{
				bool matched = true;
				for (int rowIndex = 0; rowIndex < current.Count; rowIndex++)
				{
					if (current[rowIndex] != next[rowIndex])
					{
						matched = false;
						break;
					}
				}
				return matched;
			}
        }

		private List<string> GetSampleLayout()
		{
			return new List<string>
			{
				"L.LL.LL.LL",
				"LLLLLLL.LL",
				"L.L.L..L..",
				"LLLL.LL.LL",
				"L.LL.LL.LL",
				"L.LLLLL.LL",
				"..L.L.....",
				"LLLLLLLLLL",
				"L.LLLLLL.L",
				"L.LLLLL.LL"
			};
		}
	}
}
