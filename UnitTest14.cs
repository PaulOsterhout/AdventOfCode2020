using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
	public class UnitTest14
	{
		[Fact]
		public void Part1Sample()
		{
			InstructionExecuter instructionExecuter = new InstructionExecuter(new Part1MaskApplier());
			instructionExecuter.Execute(GetSampleDataForPart1());
			long answer = instructionExecuter.GetSumOfAllValues();
			Assert.Equal(165, answer);
		}

		[Fact]
		public void Part1()
		{
			InstructionExecuter instructionExecuter = new InstructionExecuter(new Part1MaskApplier());
			instructionExecuter.Execute(StringListRetriever.Retreive("InputList14.txt").ToList());
			long answer = instructionExecuter.GetSumOfAllValues();
			Assert.Equal(17481577045893, answer);
		}

		[Fact]
		public void Part2Sample()
		{
			InstructionExecuter instructionExecuter = new InstructionExecuter(new Part2MaskApplier());
			instructionExecuter.Execute(GetSampleDataForPart2());
			long answer = instructionExecuter.GetSumOfAllValues();
			Assert.Equal(208, answer);
		}

		[Fact]
		public void Part2()
		{
			InstructionExecuter instructionExecuter = new InstructionExecuter(new Part2MaskApplier());
			instructionExecuter.Execute(StringListRetriever.Retreive("InputList14.txt").ToList());
			long answer = instructionExecuter.GetSumOfAllValues();
			Assert.Equal(4160009892257, answer);
		}

		private static class BitArrayHelper
		{
			internal static BitArray GetBitArrayFor(int value, int length)
			{
				BitArray bitValue = new BitArray(new int[] { value });
				if (bitValue.Length < length)
				{
					bitValue.Length = length;
				}
				return bitValue;
			}

			internal static long GetBitArrayValue(BitArray bitArray)
			{
				long value = 0;
				int bitLength = bitArray.Length;
				for (int bitIndex = 0; bitIndex < bitLength; bitIndex++)
				{
					value += bitArray[bitIndex] ? Convert.ToInt64(Math.Pow(2, bitIndex)) : 0;
				}
				return value;
			}
		}

		private interface IMaskApplier
		{
			Dictionary<long, long> ApplyMask(string mask, int address, string value);
		}

        private class Part1MaskApplier : IMaskApplier
        {
            public Dictionary<long, long> ApplyMask(string mask, int address, string value)
            {
				string reverseMask = new string(mask.Reverse().ToArray());
				BitArray andMask = new BitArray(reverseMask.Replace('X', '1').Select(x => x == '1').ToArray());
				BitArray orMask = new BitArray(reverseMask.Replace('X', '0').Select(x => x == '1').ToArray());
				BitArray bitValue = BitArrayHelper.GetBitArrayFor(int.Parse(value), andMask.Length);
				Dictionary<long, long> result = new Dictionary<long, long>();
				result.Add(address, BitArrayHelper.GetBitArrayValue(bitValue.Or(orMask).And(andMask)));
				return result;
            }
        }

        private class Part2MaskApplier : IMaskApplier
        {
            public Dictionary<long, long> ApplyMask(string mask, int address, string value)
            {
				string reverseMask = new string(mask.Reverse().ToArray());
				BitArray orMask = new BitArray(reverseMask.Replace('X', '0').Select(x => x == '1').ToArray());
				BitArray bitValue = BitArrayHelper.GetBitArrayFor(address, orMask.Length);
				BitArray orValue = new BitArray(bitValue).Or(orMask);
				BitArray xMask = new BitArray(reverseMask.Replace('1', '0').Replace('X', '1').Select(x => x == '1').ToArray());
				List<BitArray> addresses = new List<BitArray> { orValue };
				for (int index = 0; index < xMask.Length; index++)
				{
					if (xMask[index])
					{
						List<BitArray> testList = addresses.ToList();
						foreach (BitArray currentAddress in testList)
						{
							currentAddress.Set(index, false);
							BitArray newAddress = new BitArray(currentAddress);
							newAddress.Set(index, true);
							addresses.Add(newAddress);
						}
					}
				}
				long newValue = long.Parse(value);
				Dictionary<long, long> result = new Dictionary<long, long>();
				addresses.ForEach(x => result.Add(
					key: BitArrayHelper.GetBitArrayValue(x),
					value: newValue
				));
				return result;
            }
        }

        private class InstructionExecuter
		{
			private readonly IMaskApplier maskApplier;
			private Dictionary<long, long> memValues;

			internal InstructionExecuter(IMaskApplier maskApplier)
			{
				this.maskApplier = maskApplier;
			}

			public void Execute(List<string> instructions)
			{
				memValues = new Dictionary<long, long>();
				string mask = "";
				foreach (string instruction in instructions)
				{
					int equals = instruction.IndexOf(" = ");
					string assignment = instruction.Substring(0, equals);
					string value = instruction.Substring(instruction.IndexOf(" = ") + 3);
					switch (assignment)
					{
						case "mask":
							mask = value;
							break;
						default:
							int address = int.Parse(assignment.Substring(4, assignment.Length - 5));
							Dictionary<long, long> newValues = maskApplier.ApplyMask(mask, address, value);
							foreach (long key in newValues.Keys)
							{
								if (memValues.ContainsKey(key))
								{
									memValues[key] = newValues[key];
								}
								else
								{
									memValues.Add(key, newValues[key]);
								}
							}
							break;
					}
				}
			}

			internal long GetSumOfAllValues()
			{
				return memValues.Select(x => x.Value).Sum();
			}
		}

		public List<string> GetSampleDataForPart1()
		{
			return new List<string>
			{
				"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
				"mem[8] = 11",
				"mem[7] = 101",
				"mem[8] = 0"
			};
		}

		public List<string> GetSampleDataForPart2()
		{
			return new List<string>
			{
				"mask = 000000000000000000000000000000X1001X",
				"mem[42] = 100",
				"mask = 00000000000000000000000000000000X0XX",
				"mem[26] = 1"
			};
		}	
	}
}
