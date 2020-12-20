using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest12
	{
		[Fact]
		public void Part1Sample()
		{
			Ferry ferry = new Ferry();
			Position ferryPosition = new Position
			{
				Direction = 90,
				EastPosition = 0,
				NorthPosition = 0
			};
			ferry.MoveMany(new Part1FerryMover(ferryPosition), GetSampleNavigationInstructions());
			Assert.Equal(25, ferryPosition.GetManhattanDistance());
		}

		[Fact]
		public void Part1()
		{
			Ferry ferry = new Ferry();
			Position ferryPosition = new Position
			{
				Direction = 90,
				EastPosition = 0,
				NorthPosition = 0
			};
			ferry.MoveMany(new Part1FerryMover(ferryPosition), StringListRetriever.Retreive("InputList12.txt").Select(x => new NavigationInstruction(x)).ToList());
			Assert.Equal(420, ferryPosition.GetManhattanDistance());
		}

		[Fact]
		public void Part2Sample()
		{
			Ferry ferry = new Ferry();
			Position ferryPosition = new Position
			{
				Direction = 90,
				EastPosition = 0,
				NorthPosition = 0
			};
			Position waypointPosition = new Position
			{
				Direction = 90,
				EastPosition = 10,
				NorthPosition = 1
			};
			ferry.MoveMany(new Part2FerryMover(ferryPosition, waypointPosition), GetSampleNavigationInstructions());
			Assert.Equal(286, ferryPosition.GetManhattanDistance());
		}

		[Fact]
		public void Part2()
		{
			Ferry ferry = new Ferry();
			Position ferryPosition = new Position
			{
				Direction = 90,
				EastPosition = 0,
				NorthPosition = 0
			};
			Position waypointPosition = new Position
			{
				Direction = 90,
				EastPosition = 10,
				NorthPosition = 1
			};
			ferry.MoveMany(new Part2FerryMover(ferryPosition, waypointPosition), StringListRetriever.Retreive("InputList12.txt").Select(x => new NavigationInstruction(x)).ToList());
			Assert.Equal(108013, ferryPosition.GetManhattanDistance());
		}

		private List<NavigationInstruction> GetSampleNavigationInstructions()
		{
			return new List<NavigationInstruction>
			{
				new NavigationInstruction("F10"),
				new NavigationInstruction("N3"),
				new NavigationInstruction("F7"),
				new NavigationInstruction("R90"),
				new NavigationInstruction("F11")
			};
		}

		private class NavigationInstruction
		{
			internal NavigationInstruction(string instruction)
			{
				Direction = instruction.Substring(0, 1);
				Units = int.Parse(instruction.Substring(1));
			}

			public string Direction { get; }
			public int Units { get; }
		}

		private class Ferry
		{
			internal Position position;

			internal Ferry()
			{
				position = new Position();
			}

			internal void MoveMany(IFerryMover ferryMover, List<NavigationInstruction> navigationInstructions)
			{
				foreach (NavigationInstruction navigationInstruction in navigationInstructions)
				{
					ferryMover.Move(navigationInstruction);
				}
			}
		}

		private class Position
		{
			internal int Direction;
			internal int EastPosition;
			internal int NorthPosition;

			internal int GetManhattanDistance()
			{
				return Math.Abs(EastPosition) + Math.Abs(NorthPosition);
			}
		}

		private interface IFerryMover
		{
			Position GetFerryPosition();
			void Move(NavigationInstruction navigationInstruction);
		}

		private class Part1FerryMover : IFerryMover
		{
			internal Position position;

			internal Part1FerryMover(Position initialPosition)
			{
				position = initialPosition;
			}

			public Position GetFerryPosition()
			{
				return position;
			}

			public void Move(NavigationInstruction navigationInstruction)
			{
				switch (navigationInstruction.Direction)
				{
					case "E":
						position.EastPosition += navigationInstruction.Units;
						break;
					case "F":
						switch (position.Direction)
						{
							case 0:
								position.NorthPosition += navigationInstruction.Units;
								break;
							case 90:
								position.EastPosition += navigationInstruction.Units;
								break;
							case 180:
								position.NorthPosition -= navigationInstruction.Units;
								break;
							case 270:
								position.EastPosition -= navigationInstruction.Units;
								break;
						}
						break;
					case "L":
						position.Direction -= navigationInstruction.Units;
						if (position.Direction < 0)
						{
							position.Direction += 360;
						}
						break;
					case "N":
						position.NorthPosition += navigationInstruction.Units;
						break;
					case "R":
						position.Direction += navigationInstruction.Units;
						if (position.Direction >= 360)
						{
							position.Direction -= 360;
						}
						break;
					case "S":
						position.NorthPosition -= navigationInstruction.Units;
						break;
					case "W":
						position.EastPosition -= navigationInstruction.Units;
						break;
				}
			}
		}

		private class Part2FerryMover : IFerryMover
		{
			internal Position ferryPosition;
			internal Position waypointPosition;

			internal Part2FerryMover(Position ferryPosition, Position waypointPosition)
			{
				this.ferryPosition = ferryPosition;
				this.waypointPosition = waypointPosition;
			}

			public Position GetFerryPosition()
			{
				return ferryPosition;
			}

			public void Move(NavigationInstruction navigationInstruction)
			{
				switch (navigationInstruction.Direction)
				{
					case "E":
						waypointPosition.EastPosition += navigationInstruction.Units;
						break;
					case "F":
						ferryPosition.EastPosition += waypointPosition.EastPosition * navigationInstruction.Units;
						ferryPosition.NorthPosition += waypointPosition.NorthPosition * navigationInstruction.Units;
						break;
					case "L":
						RotateWaypointLeft();
						break;
					case "N":
						waypointPosition.NorthPosition += navigationInstruction.Units;
						break;
					case "R":
						RotateWaypointRight();
						break;
					case "S":
						waypointPosition.NorthPosition -= navigationInstruction.Units;
						break;
					case "W":
						waypointPosition.EastPosition -= navigationInstruction.Units;
						break;
				}
			}

			private void RotateWaypointLeft()
			{
				int eastPosition = waypointPosition.EastPosition;
				int northPosition = waypointPosition.NorthPosition;
				waypointPosition.EastPosition = northPosition * -1;
				waypointPosition.NorthPosition = eastPosition;
			}

			private void RotateWaypointRight()
			{
				int eastPosition = waypointPosition.EastPosition;
				int northPosition = waypointPosition.NorthPosition;
				waypointPosition.EastPosition = northPosition;
				waypointPosition.NorthPosition = eastPosition * -1;
			}
		}
	}
}
