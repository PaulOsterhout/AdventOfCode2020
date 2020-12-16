using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest5
	{
		[Fact]
		public void Part1()
		{
			// Test samples from site
			Assert.Equal(567, (new BoardingPass("BFFFBBFRRR").GetSeatId()));
			Assert.Equal(119, (new BoardingPass("FFFBBBFRRR").GetSeatId()));
			Assert.Equal(820, (new BoardingPass("BBFFBBFRLL").GetSeatId()));
			// Test first and last
			Assert.Equal(0, (new BoardingPass("FFFFFFFLLL").GetSeatId()));
			Assert.Equal(128 * 8 - 1, (new BoardingPass("BBBBBBBRRR").GetSeatId()));
			// Load from puzzle input file
			List<BoardingPass> boardingPasses = StringListRetriever.Retreive("InputList5.txt").Select(x => new BoardingPass(x)).ToList();
			Assert.Equal(927, boardingPasses.Count);
			Assert.Equal(6, boardingPasses.Min(x => x.GetSeatId()));
			Assert.Equal(933, boardingPasses.Max(x => x.GetSeatId()));
		}

		[Fact]
		public void Part2()
		{
			List<BoardingPass> boardingPasses = StringListRetriever.Retreive("InputList5.txt").Select(x => new BoardingPass(x)).ToList();
			int minSeatId = 8;
			int maxSeatId = 127 * 8;
			Dictionary<int, bool> seatList = new Dictionary<int, bool>();
			for (int seatId = minSeatId; seatId <= maxSeatId; seatId++)
			{
				seatList.Add(seatId, false);
			}
			foreach (BoardingPass boardingPass in boardingPasses)
			{
				seatList[boardingPass.GetSeatId()] = true;
			}
			List<int> emptySeatIds = seatList.Where(x => !x.Value).Select(x => x.Key).ToList();
			Assert.Equal(84, emptySeatIds.Count);
			List<int> emptySeatIds2 = emptySeatIds.Where(x => !emptySeatIds.Any(y => y == x - 1)).ToList();
			Assert.Equal(2, emptySeatIds2.Count);
			List<int> emptySeatIds3 = emptySeatIds2.Where(x => !emptySeatIds.Any(y => y == x + 1)).ToList();
			Assert.Equal(1, emptySeatIds3.Count);
			Assert.Equal(711, emptySeatIds3[0]);
		}

		private class BoardingPass
		{
			public readonly string SeatDesignation;

			public BoardingPass(string seatDesignation)
			{
				SeatDesignation = seatDesignation;
			}

			public int GetColumn(int start, int end, string columnDesignation)
			{
				if (columnDesignation.Length == 1)
				{
					switch (columnDesignation)
					{
						case "L":
							return start;
						case "R":
							return end;
						default:
							throw new System.Exception($"Invalid Row Designation, {columnDesignation}.");
					}
				}
				else
				{
					string designation = columnDesignation.Substring(0, 1);
					string nextRowDesignation = columnDesignation.Substring(1);
					int halfLength = (end - start + 1) / 2;
					switch (designation)
					{
						case "L":
							end -= halfLength;
							return GetColumn(start, end, nextRowDesignation);
						case "R":
							start += halfLength;
							return GetColumn(start, end, nextRowDesignation);
						default:
							throw new System.Exception($"Invalid Row Designation, {designation}.");
					}
				}
			}

			public int GetRow(int start, int end, string rowDesignation)
			{
				if (rowDesignation.Length == 1)
				{
					switch (rowDesignation)
					{
						case "B":
							return end;
						case "F":
							return start;
						default:
							throw new System.Exception($"Invalid Row Designation, {rowDesignation}.");
					}
				}
				else
				{
					string designation = rowDesignation.Substring(0, 1);
					string nextRowDesignation = rowDesignation.Substring(1);
					int halfLength = (end - start + 1) / 2;
					switch (designation)
					{
						case "B":
							start += halfLength;
							return GetRow(start, end, nextRowDesignation);
						case "F":
							end -= halfLength;
							return GetRow(start, end, nextRowDesignation);
						default:
							throw new System.Exception($"Invalid Row Designation, {designation}.");
					}
				}
			}

			public int GetSeatId()
			{
				int row = GetRow(0, 127, SeatDesignation.Substring(0, 7));
				int column = GetColumn(0, 7, SeatDesignation.Substring(7));
				return row * 8 + column;
			}
		}
	}
}
