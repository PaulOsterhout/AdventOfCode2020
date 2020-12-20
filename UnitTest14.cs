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
            int answer = 0;
            Assert.Equal(165, answer);
        }

        private class InstructionExecuter
        {
            internal Dictionary<int, BitArray> mem;
            
            internal InstructionExecuter(List<string> instructions)
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
                            BitArray andMask = new BitArray(mask.Replace('X', '1').Select(x => x == 1).ToArray());
                            BitArray orMask = new BitArray(mask.Replace('X', '0').Select(x => x == 1).ToArray());
                            BitArray bitValue = new BitArray(value.Select(x => x == '1').ToArray());
                            BitArray savedValue = bitValue.And(andMask).Or(orMask);
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
