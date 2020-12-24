using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest16
	{
		[Fact]
		public void Part1Sample()
		{
			TicketData ticketData = TicketDataParser.Parse(GetPart1SampleData());
			Assert.Equal(3, ticketData.ValidationRules.Count);
			Assert.Equal(5, ticketData.Tickets.Count);
			TicketValidationResult ticketValidationResult = TicketValidator.ValidateNearby(ticketData);
			Assert.Equal(71, ticketValidationResult.InvalidValues.Sum());
		}

		[Fact]
		public void Part1()
		{
			TicketValidationResult result = TicketValidator.ValidateNearby(
				TicketDataParser.Parse(StringListRetriever.Retreive("InputList15.txt").ToList()));
			Assert.Equal(29759, result.InvalidValues.Sum());
		}

		[Fact]
		public void Part2Sample()
		{
			TicketData ticketData = TicketDataParser.Parse(GetPart2SampleData());
			Assert.Equal(3, ticketData.ValidationRules.Count);
			Assert.Equal(4, ticketData.Tickets.Count);
			TicketValidationResult ticketValidationResult = TicketValidator.ValidateNearby(ticketData);
			Assert.Equal(3, ticketValidationResult.Tickets.Count);
			FieldOrderResult fieldOrderResult = FieldOrderFinder.FindFieldOrderFor(ticketValidationResult.Tickets, ticketData.ValidationRules);
		}

		[Fact]
		public void Part2()
		{
			TicketData ticketData = TicketDataParser.Parse(StringListRetriever.Retreive("InputList15.txt").ToList());
			TicketValidationResult ticketValidationResult = TicketValidator.ValidateNearby(ticketData);
			FieldOrderResult fieldOrderResult = FieldOrderFinder.FindFieldOrderFor(ticketValidationResult.Tickets, ticketData.ValidationRules);
			Assert.Equal(1307550234719, fieldOrderResult.FieldOrder.Keys.Where(x => x.StartsWith("departure"))
			 	.Select(x => System.Convert.ToInt64(ticketData.Tickets[0].DataItems[fieldOrderResult.FieldOrder[x]]))
				.Aggregate<long, long>(1, (x, y) => x * y));
		}

		private class Ticket
		{
			internal Ticket(string ticketData)
			{
				DataItems = ticketData.Split(',').Select(x => int.Parse(x)).ToList();
			}

			internal List<int> DataItems { get; }
		}

		private class TicketValidationRule
		{
			internal TicketValidationRule(string ruleData)
			{
				Ranges = new List<Range>();
				List<string> ruleItems = ruleData.Split(':').ToList();
				FieldName = ruleItems[0];
				foreach (string rangeText in ruleItems[1].Trim().Split(' '))
				{
					if (rangeText == "or") continue;
					int[] rangeItems = rangeText.Split('-').Select(x => int.Parse(x)).ToArray();
					Ranges.Add(new Range { Min = rangeItems[0], Max = rangeItems[1] });
				}
			}

			internal string FieldName { get; private set; }
			private List<Range> Ranges;

			internal bool IsValidFor(int value)
			{
				return Ranges.Any(x => (value >= x.Min) && (value <= x.Max));
			}

			private class Range
			{
				internal int Min;
				internal int Max;
			}
		}

		private class TicketData
		{
			internal TicketData()
			{
				ValidationRules = new List<TicketValidationRule>();
				Tickets = new List<Ticket>();
			}
			internal List<TicketValidationRule> ValidationRules { get; }
			internal List<Ticket> Tickets { get; }
		}

		private static class TicketDataParser
		{
			internal static TicketData Parse(List<string> data)
			{
				TicketData ticketData = new TicketData();
				foreach (string line in data)
				{
					if (line.Trim().Length > 0)
					{
						if (line.Contains(" or "))
						{
							ticketData.ValidationRules.Add(new TicketValidationRule(line));
						}
						else
						{
							if (line.Contains(','))
							{
								ticketData.Tickets.Add(new Ticket(line));
							}
						}
					}
				}
				return ticketData;
			}
		}

		private static class TicketValidator
		{
			private static TicketValidationResult Validate(Ticket ticket, List<TicketValidationRule> validationRules)
			{
				TicketValidationResult result = new TicketValidationResult();
				foreach (int value in ticket.DataItems)
				{
					if (!validationRules.Any(x => x.IsValidFor(value)))
					{
						result.InvalidValues.Add(value);
						result.IsValid = false;
					}
				}
				if (result.IsValid) result.Tickets.Add(ticket);
				return result;
			}

			internal static TicketValidationResult ValidateNearby(TicketData ticketData)
			{
				TicketValidationResult result = new TicketValidationResult();
				List<Ticket> nearbyTickets = new List<Ticket>(ticketData.Tickets);
				nearbyTickets.RemoveAt(0);
				foreach (Ticket ticket in nearbyTickets)
				{
					result.Merge(Validate(ticket, ticketData.ValidationRules));
				}
				return result;
			}
		}

		private class TicketValidationResult
		{
			internal TicketValidationResult()
			{
				InvalidValues = new List<int>();
				IsValid = true;
				Tickets = new List<Ticket>();
			}

			internal List<int> InvalidValues { get; private set; }
			internal bool IsValid { get; set; }
			internal List<Ticket> Tickets { get; set; }

			internal void Merge(TicketValidationResult result)
			{
				InvalidValues.AddRange(result.InvalidValues);
				IsValid &= result.IsValid;
				Tickets.AddRange(result.Tickets);
			}
		}

		private class FieldOrderResult
		{
			internal FieldOrderResult()
			{
				FieldOrder = new Dictionary<string, int>();
			}

			internal Dictionary<string, int> FieldOrder { get; }
		}

		private static class FieldOrderFinder
		{
			internal static FieldOrderResult FindFieldOrderFor(List<Ticket> tickets, List<TicketValidationRule> validationRules)
			{
				FieldOrderResult result = new FieldOrderResult();
				Dictionary<string, List<int>> ruleOrderMatrix = ComputeRuleOrderMatrixFor(tickets, validationRules);
				while (ruleOrderMatrix.Any(x => x.Value.Count > 0))
				{
					foreach (string key in ruleOrderMatrix.Keys.Where(x => ruleOrderMatrix[x].Count == 1).ToList())
					{
						int dataItemIndex = ruleOrderMatrix[key][0];
						result.FieldOrder.Add(key, dataItemIndex);
						ruleOrderMatrix.Remove(key);
						foreach (string otherKey in ruleOrderMatrix.Keys.Where(x => ruleOrderMatrix[x].Contains(dataItemIndex)).ToList())
						{
							ruleOrderMatrix[otherKey].Remove(dataItemIndex);
						}
					}
				}
				return result;
			}

			private static Dictionary<string, List<int>> ComputeRuleOrderMatrixFor(List<Ticket> tickets, List<TicketValidationRule> validationRules)
			{
				Dictionary<string, List<int>> matrix = new Dictionary<string, List<int>>();
				int dataItemCount = tickets[0].DataItems.Count;
				int ticketCount = tickets.Count;
				foreach (TicketValidationRule validationRule in validationRules)
				{
					List<int> dataIndexes = new List<int>();
					for (int dataItemIndex = 0; dataItemIndex < dataItemCount; dataItemIndex++)
					{
						bool isValid = true;
						foreach (Ticket ticket in tickets)
						{
							if (!validationRule.IsValidFor(ticket.DataItems[dataItemIndex]))
							{
								isValid = false;
								break;
							}
						}
						if (isValid)
						{
							dataIndexes.Add(dataItemIndex);
						}
					}
					matrix.Add(validationRule.FieldName, dataIndexes);
				}
				return matrix;
			}
		}

		private List<string> GetPart1SampleData()
		{
			return new List<string>
			{
				"class: 1-3 or 5-7",
				"row: 6-11 or 33-44",
				"seat: 13-40 or 45-50",
				"",
				"your ticket:",
				"7,1,14",
				"",
				"nearby tickets:",
				"7,3,47",
				"40,4,50",
				"55,2,20",
				"38,6,12"
			};
		}

		private List<string> GetPart2SampleData()
		{
			return new List<string>
			{
				"class: 0-1 or 4-19",
				"row: 0-5 or 8-19",
				"seat: 0-13 or 16-19",
				"",
				"your ticket:",
				"11,12,13",
				"",
				"nearby tickets:",
				"3,9,18",
				"15,1,5",
				"5,14,9"
			};
		}
	}
}
