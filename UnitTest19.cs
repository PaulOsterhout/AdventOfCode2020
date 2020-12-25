using System;
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
			List<string> validMessages = ValidSatelliteMessageCreator.CreateList(satelliteData.Rules);
            Assert.Equal("", string.Join(',', validMessages));
            Assert.Equal(0, validMessages.Count);
			Assert.Equal(2, satelliteData.Messages.Count(x => validMessages.Any(y => y == x)));
			// Assert.Equal(2, satelliteData.Messages
			// 	.Where(x => SatelliteMessageValidator
			// 	.IsValid(x, satelliteData.Rules)).Count());
        }

        private class ValidSatelliteMessageCreator
        {
            internal static List<string> CreateList(List<SatelliteRule> rules)
            {
				List<string> messages = new List<string>();
				foreach (SatelliteRule rule in rules)
				{
					messages.AddRange(GetRuleMessage(rules, rule));
				}
				return messages;
            }

			private static List<string> GetRuleMessage(List<SatelliteRule> rules, SatelliteRule rule)
			{
				if (rule.Data.StartsWith('"'))
				{
					return new List<string> { rule.Data.Substring(1, rule.Data.Length - 2) };
				}
				else
				{
					List<string> messages = new List<string>();
					foreach (string subRules in rule.Data.Split(" | "))
					{
						foreach (int ruleId in subRules.Split(' ').Select(x => int.Parse(x)))
						{
							SatelliteRule subRule = rules.Single(x => x.Index == ruleId);
							messages.AddRange(GetRuleMessage(rules, subRule));
						}
					}
					return messages;
				}
			}
        }

		private class SatelliteRule
		{
			internal SatelliteRule(string ruleText)
			{
				int colonIndex = ruleText.IndexOf(':');
				Index = int.Parse(ruleText.Substring(0, colonIndex));
				Data = ruleText.Substring(colonIndex + 2);
			}
			internal int Index { get; }
			internal string Data { get; }
		}

        private class SatelliteData
        {
            internal SatelliteData(List<string> data)
            {
                Rules = new List<SatelliteRule>();
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
                            Rules.Add(new SatelliteRule(line));
                        }
                        else
                        {
                            Messages.Add(line);
                        }
                    }
                }
            }
            public List<SatelliteRule> Rules { get; }
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