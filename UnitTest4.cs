using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest4
	{
		[Fact]
		public void Part1()
		{
			List<Passport> passports = GetPassports();
			Assert.Equal(295, passports.Count);
			Assert.Equal(new KeyValuePair<string, string>("hcl", "#a97842"), passports.Last().Fields.Last());
			Assert.Equal(264, passports.Count(x => IsValid(x, GetFieldRequirements(true))));
		}

		[Fact]
		public void Part2()
		{
			List<Passport> passports = GetPassports();
			Assert.Equal(295, passports.Count);
			Assert.Equal(new KeyValuePair<string, string>("hcl", "#a97842"), passports.Last().Fields.Last());
			Assert.Equal(0, passports.Count(x => IsValid(x, GetFieldRequirements(false))));
		}

		private List<Passport> GetPassports()
		{
			List<Passport> passports = new List<Passport>();
			Passport passport = null;
			foreach (string line in StringListRetriever.Retreive("InputList4.txt"))
			{
				if (line.Length == 0)
				{
					if (passport != null)
					{
						passports.Add(passport);
						passport = null;
					}
				}
				else
				{
					if (passport == null)
					{
						passport = new Passport();
					}
					passport.AddLine(line);
				}
			}
			if (passport != null)
			{
				passports.Add(passport);
			}
			return passports;
		}

		private bool IsEyeColorValid(string value)
		{
			string[] validEyeColors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
			return validEyeColors.Any(x => x.Equals(value));
		}

		private bool IsHairColorValid(string value)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(value, "^#([a-fA-F0-9]{6})$");
		}

		private bool IsHeightValid(string value)
		{
			if (value.EndsWith("cm"))
			{
				return IsInRange(value.Substring(0, value.Length - 2), 150, 193);
			}
			if (value.EndsWith("in"))
			{
				return IsInRange(value.Substring(0, value.Length - 2), 59, 76);
			}
			return false;
		}

		private bool IsInRange(string value, int minValue, int maxValue)
		{
			int actualValue = int.Parse(value);
			return ((actualValue >= minValue) && (actualValue <= maxValue));
		}

		private bool IsPassportIdValid(string value)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(value, "^\\d{9}$");
		}

		private bool IsValid(Passport passport, List<FieldRequirement> fieldRequirements)
		{
			Dictionary<string, bool> requirementSummary = new Dictionary<string, bool>();
			foreach (FieldRequirement fieldRequirement in fieldRequirements)
			{
				requirementSummary.Add(fieldRequirement.FieldName, false);
			}
			foreach (KeyValuePair<string, string> field in passport.Fields)
			{
				if (requirementSummary.Any(x => x.Key.Equals(field.Key)))
				{
					FieldRequirement fieldRequirement = fieldRequirements.SingleOrDefault(x => x.FieldName.Equals(field.Key));
					requirementSummary[field.Key] = fieldRequirement?.IsValid(field.Value) ?? false;
				}
			}
			return requirementSummary.All(x => x.Value);
		}

		private List<FieldRequirement> GetFieldRequirements(bool isPart1)
		{
			return new List<FieldRequirement>
			{
				new FieldRequirement
				{
					FieldName = "byr",
					IsValid = (isPart1 ? (x) => true : (x) => IsInRange(x, 1920, 2002))
				},
				new FieldRequirement
				{
					FieldName = "iyr",
					IsValid = (isPart1 ? (x) => true : (x) => IsInRange(x, 2010, 2020))
				},
				new FieldRequirement
				{
					FieldName = "eyr",
					IsValid = (isPart1 ? (x) => true : (x) => IsInRange(x, 2020, 2030))
				},
				new FieldRequirement
				{
					FieldName = "hgt",
					IsValid = (isPart1 ? (x) => true : (x) => IsHeightValid(x))
				},
				new FieldRequirement
				{
					FieldName = "hcl",
					IsValid = (isPart1 ? (x) => true : (x) => IsHairColorValid(x))
				},
				new FieldRequirement
				{
					FieldName = "ecl",
					IsValid = (isPart1 ? (x) => true : (x) => IsEyeColorValid(x))
				},
				new FieldRequirement
				{
					FieldName = "pid",
					IsValid = (isPart1 ? (x) => true : (x) => IsPassportIdValid(x))
				}
			};
		}

		private class FieldRequirement
		{
			public string FieldName { get; set; }
			public Func<string, bool> IsValid { get; set; }
		}

		private class Passport
		{
			private readonly List<KeyValuePair<string, string>> fields;

			public Passport()
			{
				fields = new List<KeyValuePair<string, string>>();
			}

			public void AddLine(string line)
			{
				foreach (string pair in line.Split(' '))
				{
					string[] items = pair.Split(':');
					fields.Add(new KeyValuePair<string, string>(items[0], items[1]));
				}
			}

			public List<KeyValuePair<string, string>> Fields { get { return fields; } }
		}
	}
}
