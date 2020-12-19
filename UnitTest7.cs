using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest7
	{
		[Fact]
		public void TestMiscellaneous()
		{
			List<string> rulesTexts = StringListRetriever.Retreive("InputList7.txt").ToList();
			int rulesCount = rulesTexts.Count;
			Assert.Equal(594, rulesCount);
			Assert.Equal("vibrant purple bags contain 3 shiny lavender bags, 1 mirrored gray bag, 4 muted bronze bags.", rulesTexts.First());
			Assert.Equal("shiny violet bags contain 5 shiny aqua bags, 1 striped brown bag, 1 dark blue bag, 5 wavy white bags.", rulesTexts.Last());
			Rule firstRule = new Rule(rulesTexts.First());
			Assert.Equal(14, firstRule.ContainerColor.Length);
			Assert.Equal("vibrant purple", firstRule.ContainerColor);
			Assert.Equal(3, firstRule.Contents.Count);
			Assert.Equal(8, firstRule.Contents.Sum(x => x.Value));
			Rule lastRule = new Rule(rulesTexts.Last());
			Assert.Equal(12, lastRule.ContainerColor.Length);
			Assert.Equal("shiny violet", lastRule.ContainerColor);
			Assert.Equal(4, lastRule.Contents.Count);
			Assert.Equal(12, lastRule.Contents.Sum(x => x.Value));
			Rule noContentsRule = new Rule(rulesTexts[16]);
			Assert.Equal(13, noContentsRule.ContainerColor.Length);
			Assert.Equal("vibrant brown", noContentsRule.ContainerColor);
			int actual = noContentsRule.Contents.Count;
			Assert.Equal(0, actual);
			Assert.Equal(0, noContentsRule.Contents.Sum(x => x.Value));
		}

		[Fact]
		public void Part1()
		{
			List<Rule> rules = StringListRetriever.Retreive("InputList7.txt").Select(x => new Rule(x)).ToList();
			int rulesCount = rules.Count;
			Assert.Equal(594, rulesCount);
			Assert.Equal(119, GetContainerBagCount(rules, "shiny gold"));
		}

		[Fact]
		public void Part2()
		{
			List<Rule> rules = StringListRetriever.Retreive("InputList7.txt").Select(x => new Rule(x)).ToList();
			int rulesCount = rules.Count;
			Assert.Equal(594, rulesCount);
			Assert.Equal(155802, GetContainingBagCount(rules, "shiny gold"));
		}

		[Fact]
		public void Part2Sample()
		{
			List<Rule> rules = new List<Rule>
			{
				new Rule("shiny gold bags contain 2 dark red bags."),
				new Rule("dark red bags contain 2 dark orange bags."),
				new Rule("dark orange bags contain 2 dark yellow bags."),
				new Rule("dark yellow bags contain 2 dark green bags."),
				new Rule("dark green bags contain 2 dark blue bags."),
				new Rule("dark blue bags contain 2 dark violet bags."),
				new Rule("dark violet bags contain no other bags.")
			};
			int rulesCount = rules.Count;
			Assert.Equal(7, rulesCount);
			Assert.Equal(126, GetContainingBagCount(rules, "shiny gold"));
		}

		private int GetContainerBagCount(List<Rule> rules, string bagType)
		{
			List<string> containerList = new List<string>();
			return GetContainersFor(rules, bagType, containerList).Count;
		}

		private List<string> GetContainersFor(List<Rule> rules, string bagType, List<string> containerList)
		{
			List<string> newContainers = rules.Where(x => x.Contents.Keys.Any(y => y == bagType)).Select(x => x.ContainerColor).ToList();
			List<string> commonContainers = newContainers.Where(x => containerList.Any(y => y == x)).ToList();
			newContainers.RemoveAll(x => commonContainers.Any(y => y == x));
			containerList.AddRange(newContainers);
			foreach (string containerBagType in newContainers)
			{
				GetContainersFor(rules, containerBagType, containerList);
			}
			return containerList;
		}

		private int GetContainingBagCount(List<Rule> rules, string bagType)
		{
			Rule bagRule = rules.Single(x => x.ContainerColor == bagType);
			int bags = bagRule.Contents.Sum(x => x.Value);
			foreach (string contentBagType in bagRule.Contents.Keys)
			{
				bags += (bagRule.Contents[contentBagType] * GetContainingBagCount(rules, contentBagType));
			}
			return bags;
		}

		private class Rule
		{
			public Rule(string text)
			{
				Contents = new Dictionary<string, int>();
				string containerSearchText = " bags contain";
				int containerLength = text.IndexOf(containerSearchText);
				ContainerColor = text.Substring(0, containerLength);
				string contentsText = text.Substring(containerLength + containerSearchText.Length + 1);
				string[] contentsItems = contentsText.Substring(0, contentsText.Length - 1).Split(',');
				foreach (string item in contentsItems)
				{
					string contentText = item.Trim();
					int spacePosition = contentText.IndexOf(' ');
					string countValue = contentText.Substring(0, spacePosition);
					if (countValue != "no")
					{
						int count = int.Parse(countValue);
						string theRest = contentText.Substring(spacePosition + 1);
						int bagPosition = theRest.IndexOf("bag");
						string color = theRest.Substring(0, bagPosition - 1);
						Contents.Add(color, count);
					}
				}
			}

			public string ContainerColor { get; set; }
			public Dictionary<string, int> Contents { get; set; }
		}
	}
}
