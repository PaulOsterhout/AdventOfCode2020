using System;
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
			Assert.Equal(231, FormulaEvaluator.EvaluateAdvanced(formulas[0]));
			Assert.Equal(51, FormulaEvaluator.EvaluateAdvanced(formulas[1]));
			Assert.Equal(46, FormulaEvaluator.EvaluateAdvanced(formulas[2]));
			Assert.Equal(1445, FormulaEvaluator.EvaluateAdvanced(formulas[3]));
			Assert.Equal(669060, FormulaEvaluator.EvaluateAdvanced(formulas[4]));
			Assert.Equal(23340, FormulaEvaluator.EvaluateAdvanced(formulas[5]));
		}

		[Fact]
		public void Part2()
		{
			Assert.Equal(9966990988262, StringListRetriever.Retreive("InputList18.txt").Select(x => FormulaEvaluator.EvaluateAdvanced(x)).Sum());
		}

		private static class FormulaEvaluator
		{
			internal static long Evaluate(string originalFormula)
			{
				return PerformEvaluate(EvaluateParenthesis(originalFormula, Evaluate));
			}

			internal static long EvaluateAdvanced(string originalFormula)
			{
				return PerformAdvancedEvaluate(EvaluateParenthesis(originalFormula, EvaluateAdvanced));
			}

            private static long PerformEvaluate(string originalFormula)
			{
				List<string> formulaParts = originalFormula.Split(' ').ToList();
				while (formulaParts.Count > 1)
				{
					string subFormula = $"{formulaParts[0]} {formulaParts[1]} {formulaParts[2]}";
					formulaParts.RemoveRange(1, 2);
					formulaParts[0] = $"{EvaluateFormula(subFormula)}";
				};
				return long.Parse(formulaParts[0]);
			}

            private static long PerformAdvancedEvaluate(string originalFormula)
			{
				List<string> formulaParts = originalFormula.Split(' ').ToList();
				while (formulaParts.Count > 1)
				{
					int delimiterIndex = formulaParts.IndexOf("+");
					if (delimiterIndex == -1)
					{
						delimiterIndex = 1;
					}
					string subFormula = $"{formulaParts[delimiterIndex - 1]} {formulaParts[delimiterIndex]} {formulaParts[delimiterIndex + 1]}";
					formulaParts.RemoveRange(delimiterIndex, 2);
					formulaParts[delimiterIndex - 1] = $"{EvaluateFormula(subFormula)}";
				};
				return long.Parse(formulaParts[0]);
			}

			private static long EvaluateFormula(string formula)
			{
				string[] formulaParts = formula.Split(' ');
				long value1 = long.Parse(formulaParts[0]);
				string operation = formulaParts[1];
				long value2 = long.Parse(formulaParts[2]);
				switch (operation)
				{
					case "+":
						return value1 + value2;
					case "*":
						return value1 * value2;
				}
				throw new Exception($"Bad formula: {formula}");
			}

			private static string EvaluateParenthesis(string originalFormula, Func<string, long> EvaluteFunction)
			{
				string formula = originalFormula;
				int startIndex = formula.LastIndexOf('(');
				while (startIndex > -1)
				{
					int length = formula.IndexOf(')', startIndex) - startIndex - 1;
					string subFormula = formula.Substring(startIndex + 1, length);
					long subResult = EvaluteFunction.Invoke(subFormula);
					formula = $"{formula.Substring(0, startIndex)}{subResult}{formula.Substring(startIndex + length + 2)}";
					startIndex = formula.LastIndexOf('(');
				}
				return formula;
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
