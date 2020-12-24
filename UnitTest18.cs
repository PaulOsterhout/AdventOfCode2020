using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest18
	{
		[Fact]
		public void Part1Sample()
		{
			List<string> formulas = GetPart1SampleData();
			Assert.Equal(71, FormulaEvaluator.Evaluate(formulas[0]));
			Assert.Equal(51, FormulaEvaluator.Evaluate(formulas[1]));
			Assert.Equal(26, FormulaEvaluator.Evaluate(formulas[2]));
			Assert.Equal(437, FormulaEvaluator.Evaluate(formulas[3]));
			Assert.Equal(12240, FormulaEvaluator.Evaluate(formulas[4]));
			Assert.Equal(13632, FormulaEvaluator.Evaluate(formulas[5]));
			Assert.Equal(71 + 51 + 26 + 437 + 12240 + 13632, GetPart1SampleData().Select(x => FormulaEvaluator.Evaluate(x)).Sum());
			Assert.Equal(72077, FormulaEvaluator.Evaluate("9 + 2 * ((7 + 9 + 5 + 7) * 3 + (5 * 3 * 6 * 5 + 6) + (9 * 4 + 9 * 6 + 9)) * 8 + 5"));
		}

		[Fact]
		public void Part1()
		{
			Assert.Equal(280014646144, StringListRetriever.Retreive("InputList18.txt").Select(x => FormulaEvaluator.Evaluate(x)).Sum());
		}

		[Fact]
		public void Part2Sample()
		{
			List<string> formulas = GetPart1SampleData();
			Assert.Equal(231, FormulaEvaluator.Evaluate(formulas[0]));
			Assert.Equal(51, FormulaEvaluator.Evaluate(formulas[1]));
			Assert.Equal(46, FormulaEvaluator.Evaluate(formulas[2]));
			Assert.Equal(1445, FormulaEvaluator.Evaluate(formulas[3]));
			Assert.Equal(669060, FormulaEvaluator.Evaluate(formulas[4]));
			Assert.Equal(23340, FormulaEvaluator.Evaluate(formulas[5]));
		}

		private static class FormulaEvaluator
		{
			internal static long Evaluate(string originalFormula)
			{
				string formula = originalFormula;
				string lastOperation = "+";
				long result = 0;
				int startIndex = formula.LastIndexOf('(');
				while (startIndex > -1)
				{
					int length = formula.IndexOf(')', startIndex) - startIndex - 1;
					string subFormula = formula.Substring(startIndex + 1, length);
					long subResult = Evaluate(subFormula);
					formula = $"{formula.Substring(0, startIndex)}{subResult}{formula.Substring(startIndex + length + 2)}";
					startIndex = formula.LastIndexOf('(');
				}
				string[] formulaItems = formula.Split(' ');
				foreach (string forumlaItem in formulaItems)
				{
					long value = 0;
					if (long.TryParse(forumlaItem, out value))
					{
						switch (lastOperation)
						{
							case "+":
								result += value;
								break;
							case "*":
								result *= value;
								break;
						}
					}
					else
					{
						lastOperation = forumlaItem;
					}
				}
				return result;
			}
		}

		private List<string> GetPart1SampleData()
		{
			return new List<string>
			{
				"1 + 2 * 3 + 4 * 5 + 6",
				"1 + (2 * 3) + (4 * (5 + 6))",
				"2 * 3 + (4 * 5)",
				"5 + (8 * 3 + 9 + 3 * 4 * 3)",
				"5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
				"((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
			};
		}
	}
}
