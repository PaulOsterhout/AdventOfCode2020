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
                for (int ruleIndex = rules.Select(x => x.Index).Max(); ruleIndex >= rules.Select(x => x.Index).Min(); ruleIndex--)
				{
                    SatelliteRule rule = rules.SingleOrDefault(x => x.Index == ruleIndex);
                    if ((rule != null) && (rule.RuleType == "SubRule"))
                    {
                        SatelliteSubRule subRule = rule as SatelliteSubRule;
                        foreach (List<int> ruleSet in subRule.RuleSets)
                        {
                            List<string> subMessages = new List<string>();
                            foreach(int ruleId in ruleSet)
                            {
                                SatelliteRule messageRule = rules.SingleOrDefault(x => x.Index == ruleId);
                                if (messageRule.RuleType == "SubRule")
                                {
                                    SatelliteSubRule valuesRule = messageRule as SatelliteSubRule;
                                    int valuesCount = valuesRule.Messages.Count;
                                    List<string> newMessages = new List<string>();
                                    subMessages.ForEach(x => 
                                    {
                                        if (valuesCount > 1)
                                        {
                                            for (int valueIndex = 1; valueIndex < valuesCount; valueIndex++)
                                            {
                                                newMessages.Add(x + valuesRule.Messages[valueIndex]);
                                            }
                                        }
                                        x += valuesRule.Messages[0];
                                    });
                                    subMessages.AddRange(newMessages);
                                }
                                else
                                {
                                    SatelliteCharacterRule characterRule = messageRule as SatelliteCharacterRule;
                                    subMessages.ForEach(x => x += characterRule.MessageCharacters);
                                }
                            }
                        }
                    }
				}
				return messages;
            }

			private static List<string> GetValidMessages(List<SatelliteRule> rules, SatelliteRule rule)
			{
				if (rule.RuleType == "MessageCharacters")
				{
					return null;
				}
				else
				{
					List<string> messages = new List<string>();
                    SatelliteSubRule satelliteSubRule = rule as SatelliteSubRule;
                    foreach (List<int> subRuleOrder in satelliteSubRule.RuleSets)
                    {
                        foreach (int subRuleId in subRuleOrder)
                        {
                            SatelliteRule subRule = rules[subRuleId];
                            if (rule.RuleType == "SubRule")
                            {
                                List<string> newMessages = GetValidMessages(rules, subRule);
                                if (newMessages != null)
                                {
                                    messages.AddRange(newMessages);
                                }
                            }
                            else
                            {
                                SatelliteCharacterRule satelliteCharacterRule = subRule as SatelliteCharacterRule;
                                messages.ForEach(x => x += satelliteCharacterRule.MessageCharacters);
                            }
                        }
                    }
					return messages;
				}
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
                            Rules.Add(SatelliteRule.CreateRuleFrom(line));
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