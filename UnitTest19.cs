using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest19
	{
		[Fact]
		public void Part1Sample()
		{
			SatelliteData satelliteData = new SatelliteData(GetPart1SampleData());
			List<string> validMessages = ValidSatelliteMessageCreator.CreateList(satelliteData);
			Assert.Equal(8, validMessages.Count);
			Assert.Equal(2, satelliteData.Messages.Count(x => validMessages.Any(y => y == x)));
		}

		[Fact]
		public void Part1()
		{
			SatelliteData satelliteData = new SatelliteData(StringListRetriever.Retreive("InputList19.txt").ToList());
			List<string> validMessages = ValidSatelliteMessageCreator.CreateList(satelliteData);
			Assert.Equal(142, satelliteData.Messages.Count(x => validMessages.Any(y => y == x)));
		}

		private static class ValidSatelliteMessageCreator
		{
			internal static List<string> CreateList(SatelliteData satelliteData)
			{
				List<int> ruleKeys = satelliteData.SubRules.Keys.OrderByDescending(x => x).ToList();
				foreach (int key in ruleKeys)
				{
					GetMessagesFor(satelliteData.SubRules[key], satelliteData);
				}
				return satelliteData.SubRules[ruleKeys.Min()].Messages; //.OrderBy(x => x).ToList();
			}

			private static void GetMessagesFor(SatelliteSubRule rule, SatelliteData satelliteData)
			{
				if (rule.Messages.Count > 0) return;
				List<string> messages = new List<string>();
				foreach (var ruleSet in rule.RuleSets)
				{
					List<int> subRuleKeys = ruleSet.Where(x => satelliteData.SubRules.Keys.Any(y => x == y)).OrderByDescending(x => x).ToList();
					foreach (int subRuleKey in subRuleKeys)
					{
						GetMessagesFor(satelliteData.SubRules[subRuleKey], satelliteData);
					}
					List<string> ruleSetMessages = new List<string>{ "" };
					foreach (int ruleKey in ruleSet)
					{
						if (satelliteData.SubRules.Keys.Contains(ruleKey))
						{
							List<string> newMessages = new List<string>();
							ruleSetMessages.ForEach(x =>
							{
								satelliteData.SubRules[ruleKey].Messages.ForEach(y =>
								{
									newMessages.Add(x + y);
								});
							});
							ruleSetMessages = newMessages;
						}
						else
						{
							for (int index = 0; index < ruleSetMessages.Count; index++)
							{
								ruleSetMessages[index] += satelliteData.CharacterRules[ruleKey].MessageCharacters;
							}
						}
					}
					messages.AddRange(ruleSetMessages);
				}
				rule.Messages.AddRange(messages.Distinct());
			}
		}

		private abstract class SatelliteRule
		{
			internal SatelliteRule(string ruleText)
			{
				int colonIndex = ruleText.IndexOf(':');
				Index = int.Parse(ruleText.Substring(0, colonIndex));
				RuleData = ruleText.Substring(colonIndex + 2);
			}
			internal int Index { get; private set; }
			internal abstract string RuleType { get; }
			protected string RuleData { get; }

			internal static SatelliteRule CreateRuleFrom(string ruleText)
			{
				if (!ruleText.Contains(':')) throw new System.Exception("Not a rule");
				return ruleText.Contains('"') ? new SatelliteCharacterRule(ruleText) : new SatelliteSubRule(ruleText);
			}
		}

		private class SatelliteCharacterRule : SatelliteRule
		{
			internal SatelliteCharacterRule(string ruleText) : base(ruleText)
			{
				MessageCharacters = RuleData.Substring(1, RuleData.Length - 2);
			}
			internal override string RuleType => "MessageCharacters";
			public string MessageCharacters { get; }
		}

		private class SatelliteSubRule : SatelliteRule
		{
			internal SatelliteSubRule(string ruleText) : base(ruleText)
			{
				RuleSets = RuleData.Split(" | ").Select(x => x.Split(' ').Select(x => int.Parse(x)).ToList()).ToList();
				Messages = new List<string>();
			}
			internal override string RuleType => "SubRule";
			internal List<List<int>> RuleSets { get; }
			internal List<string> Messages { get; }
		}

		private class SatelliteData
		{
			internal SatelliteData(List<string> data)
			{
				CharacterRules = new Dictionary<int, SatelliteCharacterRule>();
				SubRules = new Dictionary<int, SatelliteSubRule>();
				Messages = new List<string>();
				bool isRule = true;
				foreach (string line in data)
				{
					if (line.Trim().Length == 0)
					{
						isRule = false;
					}
					else
					{
						if (isRule)
						{
							SatelliteRule rule = SatelliteRule.CreateRuleFrom(line);
							if (rule.RuleType == "SubRule")
							{
								SubRules.Add(rule.Index, rule as SatelliteSubRule);
							}
							else
							{
								CharacterRules.Add(rule.Index, rule as SatelliteCharacterRule);
							}
						}
						else
						{
							Messages.Add(line);
						}
					}
				}
			}
			public Dictionary<int, SatelliteCharacterRule> CharacterRules { get; }
			public Dictionary<int, SatelliteSubRule> SubRules { get; }
			public List<string> Messages { get; }
		}

		private List<string> GetPart1SampleData()
		{
			return new List<string>
			{
				"0: 4 1 5",
				"1: 2 3 | 3 2",
				"2: 4 4 | 5 5",
				"3: 4 5 | 5 4",
				"4: \"a\"",
				"5: \"b\"",
				"",
				"ababbb",
				"bababa",
				"abbbab",
				"aaabbb",
				"aaaabbb"
			};
		}
	}
}