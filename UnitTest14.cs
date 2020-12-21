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
            InstructionExecuter instructionExecuter = new InstructionExecuter();
            instructionExecuter.Execute(GetSampleData());
            long answer = 0;
            foreach (int key in instructionExecuter.mem.Keys)
            {
                answer += instructionExecuter.GetMemValue(key);
            }
            Assert.Equal(165, answer);
        }

        [Fact]
        public void Part1()
        {
            InstructionExecuter instructionExecuter = new InstructionExecuter();
            instructionExecuter.Execute(StringListRetriever.Retreive("INputList14.txt").ToList());
            long answer = 0;
            foreach (int key in instructionExecuter.mem.Keys)
            {
                answer += instructionExecuter.GetMemValue(key);
            }
            Assert.Equal(17481577045893, answer);
        }

        private class InstructionExecuter
        {
            internal Dictionary<int, BitArray> mem;
            
            internal void Execute(List<string> instructions)
            {
                mem = new Dictionary<int, BitArray>();
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
                            string reverseMask = new string(mask.Reverse().ToArray());
                            BitArray andMask = new BitArray(reverseMask.Replace('X', '1').Select(x => x == '1').ToArray());
                            BitArray orMask = new BitArray(reverseMask.Replace('X', '0').Select(x => x == '1').ToArray());
                            BitArray bitValue = new BitArray(new int[] { int.Parse(value) });
                            if (bitValue.Length < andMask.Length)
                            {
                                bitValue.Length = andMask.Length;
                            }
                            BitArray savedValue = bitValue.Or(orMask).And(andMask);
                            if (mem.ContainsKey(address))
                            {
                                mem[address] = savedValue;
                            }
                            else
                            {
                                mem.Add(address, savedValue);
                            }
                            break;
                    }
                }
            }

            internal long GetMemValue(int key)
            {
                if (!mem.ContainsKey(key)) return 0;
                long value = 0;
                int length = mem[key].Length;
                for (int index = 0; index < length; index++)
                {
                    value += mem[key][index] ? Convert.ToInt64(Math.Pow(2, index)) : 0;
                }
                return value;
            }
        }

        public List<string> GetSampleData()
        {
            return new List<string>
            {
                "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
                "mem[8] = 11",
                "mem[7] = 101",
                "mem[8] = 0"
            };
        }
    }
}
