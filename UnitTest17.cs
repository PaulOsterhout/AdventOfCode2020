using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest17
	{
		[Fact]
		public void Part1Sample()
		{
			List<Cube> cubeState = CubeCoordinateParser.ParseFrom(GetPart1SampleData());
			List<Cube> activeCubes = cubeState.Where(c => c.active).OrderBy(c => c.location.z).OrderBy(c => c.location.x).OrderBy(c => c.location.y).ToList();
			Assert.Equal(5, cubeState.Count(c => c.active));
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			Assert.Equal(11, cubeState.Count(c => c.active));
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			Assert.Equal(21, cubeState.Count(c => c.active));
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			Assert.Equal(38, cubeState.Count(c => c.active));
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			Assert.Equal(112, cubeState.Count(c => c.active));
		}

		[Fact]
		public void Part1()
		{
			List<Cube> cubeState = CubeCoordinateParser.ParseFrom(StringListRetriever.Retreive("InputList17.txt").ToList());
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState);
			Assert.Equal(289, cubeState.Count(c => c.active));
		}

		[Fact]
		public void Part2Sample()
		{
			List<Cube> cubeState = CubeCoordinateParser.ParseFrom(GetPart1SampleData());
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			Assert.Equal(848, cubeState.Count(c => c.active));
		}

		[Fact(Skip = "Runs too long")]
		public void Part2()
		{
			List<Cube> cubeState = CubeCoordinateParser.ParseFrom(StringListRetriever.Retreive("InputList17.txt").ToList());
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			cubeState = CubeBootProcessor.ProcessCycle(cubeState, 1);
			Assert.Equal(2084, cubeState.Count(c => c.active));
		}

		private static class CubeBootProcessor
		{
			internal static List<Cube> ProcessCycle(List<Cube> initialState, int wMultiplier = 0)
			{
				List<Cube> newState = new List<Cube>();
				int minW = initialState.Select(x => x.location.w).Min() - (1 * wMultiplier);
				int maxW = initialState.Select(x => x.location.w).Max() + (1 * wMultiplier);
				int minX = initialState.Select(x => x.location.x).Min() - 1;
				int maxX = initialState.Select(x => x.location.x).Max() + 1;
				int minY = initialState.Select(x => x.location.y).Min() - 1;
				int maxY = initialState.Select(x => x.location.y).Max() + 1;
				int minZ = initialState.Select(x => x.location.z).Min() - 1;
				int maxZ = initialState.Select(x => x.location.z).Max() + 1;
				for (int w = minW; w <= maxW; w++)
				{
					for (int z = minZ; z <= maxZ; z++)
					{
						for (int y = minY; y <= maxY; y++)
						{
							for (int x = minX; x <= maxX; x++)
							{
								Cube initialCube = initialState.SingleOrDefault(c =>
									(c.location.w == w) &&(c.location.x == x) &&
									(c.location.y == y) && (c.location.z == z));
								int activeCubeCount = initialState
									.Where(cube =>
										(cube.location.w >= w - (1 * wMultiplier)) &&
										(cube.location.w <= w + (1 * wMultiplier)) &&
										(cube.location.x >= x - 1) &&
										(cube.location.x <= x + 1) &&
										(cube.location.y >= y - 1) &&
										(cube.location.y <= y + 1) &&
										(cube.location.z >= z - 1) &&
										(cube.location.z <= z + 1) &&
										cube.active)
									.Count();
								bool active = (initialCube?.active ?? false)
									? ((activeCubeCount == 3) || (activeCubeCount == 4))
									: (activeCubeCount == 3);
								
								if (active)
								{
									Cube newCube = new Cube(new Coordinate(x, y, z, w), active);
									newState.Add(newCube);
								}
							}
						}
					}
				}
				return newState;
			}
		}

		private static class CubeCoordinateParser
		{
			internal static List<Cube> ParseFrom(List<string> cubeDataInput)
			{
				List<Cube> cubes = new List<Cube>();
				int y = -1;
				int z = 0;
				foreach (string cubeDataLine in cubeDataInput)
				{
					int x = -1;
					y++;
					foreach (char cubeDataItem in cubeDataLine)
					{
						x++;
						cubes.Add(new Cube(new Coordinate(x, y, z), cubeDataItem == '#'));
					}
				}
				return cubes;
			}
		}

		private class Coordinate
		{
			internal Coordinate(int x, int y, int z, int w = 0)
			{
				this.w = w;
				this.x = x;
				this.y = y;
				this.z = z;
			}

			internal int w { get; }
			internal int x { get; }
			internal int y { get; }
			internal int z { get; }
		}

		private class Cube
		{
			public Cube(Coordinate location, bool active)
			{
				this.location = location;
				this.active = active;
			}
			internal Coordinate location { get; }
			internal bool active { get; }
		}

		private List<string> GetPart1SampleData()
		{
			return new List<string>
			{
				".#.",
				"..#",
				"###"
			};
		}
	}
}
