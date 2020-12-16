using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest2
	{
		private abstract class BasePasswordEntry
		{
			protected readonly int[] indexes;

			public BasePasswordEntry(string entryLine)
			{
				string[] entryValues = entryLine.Split(' ');
				indexes = entryValues[0].Split('-').Select(x => int.Parse(x)).ToArray();
				LetterToValidate = entryValues[1][0];
				PasswordToTest = entryValues[2];
			}

			public char LetterToValidate { get; private set; }
			public string PasswordToTest { get; private set; }
			public abstract bool IsValid();
		}

		private class PasswordEntryPart1 : BasePasswordEntry
		{
			public PasswordEntryPart1(string entryLine) : base(entryLine)
			{
				MinCount = indexes[0];
				MaxCount = indexes[1];
			}

			public int MinCount { get; set; }
			public int MaxCount { get; set; }
			public override bool IsValid()
			{
				int count = PasswordToTest.Count(x => x.Equals(LetterToValidate));
				return ((count >= MinCount) && (count <= MaxCount));
			}
        }

		private class PasswordEntryPart2 : BasePasswordEntry
		{
			public PasswordEntryPart2(string entryLine) : base(entryLine)
			{
				Index1 = indexes[0] - 1;
				Index2 = indexes[1] - 1;
			}

			public int Index1 { get; set; }
			public int Index2 { get; set; }
			public override bool IsValid()
			{
				char position1 = PasswordToTest[Index1];
				char position2 = PasswordToTest[Index2];
				bool valid1 = position1.Equals(LetterToValidate);
				bool valid2 = position2.Equals(LetterToValidate);
				bool valid3 = !(valid1 && valid2);
				return (valid1 || valid2) && valid3;
			}
		}

		private List<BasePasswordEntry> GetPasswordEntries(Func<string, BasePasswordEntry> createEntry)
		{
			return StringListRetriever.Retreive("InputList2.txt").Select(x => createEntry.Invoke(x)).ToList();
		}

		[Fact]
		public void TestFirstPart1()
		{
			List<BasePasswordEntry> passwordEntries = GetPasswordEntries((entryLine) => new PasswordEntryPart1(entryLine));
			PasswordEntryPart1 entry = passwordEntries[0] as PasswordEntryPart1;
			Assert.Equal(1000, passwordEntries.Count);
			Assert.Equal(7, entry.MinCount);
			Assert.Equal(9, entry.MaxCount);
			Assert.Equal('l', entry.LetterToValidate);
			Assert.Equal("vslmtglbc", entry.PasswordToTest);
			Assert.False(entry.IsValid());
		}

		[Fact]
		public void TestAllPart1()
		{
			List<BasePasswordEntry> passwordEntries = GetPasswordEntries((entryLine) => new PasswordEntryPart1(entryLine));
			int validCount = passwordEntries.Count(x => x.IsValid());
			Assert.Equal(614, validCount);
		}

		[Fact]
		public void TestFirstPart2()
		{
			List<BasePasswordEntry> passwordEntries = GetPasswordEntries((entryLine) => new PasswordEntryPart2(entryLine));
			PasswordEntryPart2 entry = passwordEntries[0] as PasswordEntryPart2;
			Assert.Equal(1000, passwordEntries.Count);
			Assert.Equal(6, entry.Index1);
			Assert.Equal(8, entry.Index2);
			Assert.Equal('l', entry.LetterToValidate);
			Assert.Equal("vslmtglbc", entry.PasswordToTest);
			Assert.True(entry.IsValid());
		}

		[Fact]
		public void TestAllPart2()
		{
			List<BasePasswordEntry> passwordEntries = GetPasswordEntries((entryLine) => new PasswordEntryPart2(entryLine));
			Assert.Equal(1000, passwordEntries.Count);
			int validCount = passwordEntries.Count(x => x.IsValid());
			Assert.Equal(354, validCount);
		}
	}
}
