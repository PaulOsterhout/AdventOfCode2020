using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
    public class UnitTest8
    {
        [Fact]
        public void Part1Sample()
        {
            List<string> instructions = new List<string>
            {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };
            Emulator emulator = new Emulator(instructions);
            emulator.Run();
            Assert.Equal(5, emulator.State.Accumulator);
			Assert.True(emulator.State.IsInfiniteLoop());
        }

		[Fact]
		public void Part1()
		{
            Emulator emulator = new Emulator(StringListRetriever.Retreive("InputList8.txt").ToList());
            emulator.Run();
            Assert.Equal(1584, emulator.State.Accumulator);
			Assert.True(emulator.State.IsInfiniteLoop());
		}

        [Fact]
        public void Part2Sample()
        {
			int attemptLimit = 10;
			List<int> changeHistory = new List<int>();
            List<string> originalInstructions = new List<string>
            {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };
			List<string> instructions = new List<string>(originalInstructions);
			int accumulator = 0;
			int attempts = 0;
			bool isFixed = false;
			do
			{
				attempts++;
				Emulator emulator = new Emulator(instructions);
				emulator.Run();
				if (!emulator.State.IsInfiniteLoop())
				{
					accumulator = emulator.State.Accumulator;
					isFixed = true;
					break;
				}
				instructions = FixInstructions(originalInstructions, emulator.State.ExecutionPath, changeHistory);
			} while ((!isFixed) && (attempts < attemptLimit));
            Assert.Equal(8, accumulator);
			Assert.Equal(3, attempts);
        }

		[Fact]
		public void Part2()
		{
			int attemptLimit = 100;
			List<int> changeHistory = new List<int>();
			List<string> originalInstructions = StringListRetriever.Retreive("InputList8.txt").ToList();
			List<string> instructions = new List<string>(originalInstructions);
			int accumulator = 0;
			int attempts = 0;
			bool isFixed = false;
			do
			{
				attempts++;
				Emulator emulator = new Emulator(instructions);
				emulator.Run();
				if (!emulator.State.IsInfiniteLoop())
				{
					accumulator = emulator.State.Accumulator;
					isFixed = true;
					break;
				}
				instructions = FixInstructions(originalInstructions, emulator.State.ExecutionPath, changeHistory);
			} while ((!isFixed) && (attempts < attemptLimit));
            Assert.Equal(920, accumulator);
			Assert.Equal(81, attempts);
		}

		private List<string> FixInstructions(List<string> instructions, List<int> executionPath, List<int> changeHistory)
		{
			List<string> newInstructions = new List<string>(instructions);
			for (int pathIndex = executionPath.Count - 2; pathIndex >= 0; pathIndex--)
			{
				int instructionIndex = executionPath[pathIndex];
				if (changeHistory.Contains(instructionIndex))
				{
					continue;
				}
				if (newInstructions[instructionIndex].Contains("acc"))
				{
					continue;
				}
				changeHistory.Add(instructionIndex);
				newInstructions[instructionIndex] = newInstructions[instructionIndex].Contains("nop")
					? newInstructions[instructionIndex].Replace("nop", "jmp")
					: newInstructions[instructionIndex].Replace("jmp", "nop");
				break;
			}
			return newInstructions;
		}

        private class Emulator
        {
            private readonly List<string> instructions;

            public Emulator(List<string> instructions)
            {
                this.instructions = instructions;
            }

            public EmulatorState State { get; private set; }

            public void Run()
            {
                State = new EmulatorState();
                State.Initialize(instructions);
                do
                {
                    Instruction currentInstruction = InstructionFactory.CreateInstruction(State.GetCurrentInstruction());
                    State = currentInstruction.Run(State);
                } while (!State.TerminateRun);
            }
        }

        private class EmulatorState
        {
            public int Accumulator;
            private int CurrentIndex;
			public List<int> ExecutionPath { get; private set; }
            private Dictionary<int, int> InstructionCounts;
            private List<string> Instructions;
            public bool TerminateRun;

            public EmulatorState Clone()
            {
                return new EmulatorState
                {
                    Accumulator = this.Accumulator,
                    CurrentIndex = this.CurrentIndex,
					ExecutionPath = new List<int>(this.ExecutionPath),
					InstructionCounts = new Dictionary<int, int>(this.InstructionCounts),
					Instructions = new List<string>(this.Instructions),
                    TerminateRun = this.TerminateRun
                };
            }

            public string GetCurrentInstruction()
            {
				ExecutionPath.Add(CurrentIndex);
                return Instructions[CurrentIndex];
            }

            public void IncrementCurrentIndex(int increment)
            {
                if (!TerminateRun)
                {
                    CurrentIndex += increment;
                    if (CurrentIndex >= Instructions.Count)
                    {
                        TerminateRun = true;
                    }
                }
            }

            public void IncrementInstructionCount()
            {
                int count = InstructionCounts[CurrentIndex] + 1;
                InstructionCounts[CurrentIndex] = count;
                if (count > 1) TerminateRun = true;
            }

            public void Initialize(List<string> instructions)
            {
                Accumulator = 0;
                CurrentIndex = 0;
				ExecutionPath = new List<int>();
                InstructionCounts = new Dictionary<int, int>();
                Instructions = instructions;
                TerminateRun = false;
                for (int index = 0; index < instructions.Count; index++)
                {
                    InstructionCounts.Add(index, 0);
                }
            }

			public bool IsInfiniteLoop()
			{
				return InstructionCounts.Any(x => x.Value > 1);
			}
        }

        private static class InstructionFactory
        {
            public static Instruction CreateInstruction(string instruction)
            {
                string[] parts = instruction.Split(' ');
                string instructionType = parts[0];
                int argument = int.Parse(parts[1]);
                switch (instructionType)
                {
					case "acc": return new Acc(argument);
					case "jmp": return new Jmp(argument);
                    case "nop": return new Nop(argument);
                    default: return null;
                }
            }
        }

        private abstract class Instruction
        {
            protected readonly int argument;
            public Instruction(int argument)
            {
                this.argument = argument;
            }
            public abstract EmulatorState Run(EmulatorState currentState);
        }

        private class Acc : Instruction
        {
            public Acc(int argument) : base(argument) { }
            public override EmulatorState Run(EmulatorState currentState)
            {
                EmulatorState newState = currentState.Clone();
                newState.IncrementInstructionCount();
                if (!newState.TerminateRun)
                {
                    newState.Accumulator += argument;
					newState.IncrementCurrentIndex(1);
                }
                return newState;
            }
        }

        private class Jmp : Instruction
        {
            public Jmp(int argument) : base(argument) { }
            public override EmulatorState Run(EmulatorState currentState)
            {
                EmulatorState newState = currentState.Clone();
                newState.IncrementInstructionCount();
                if (!newState.TerminateRun)
                {
					newState.IncrementCurrentIndex(argument);
                }
                return newState;
            }
        }

        private class Nop : Instruction
        {
            public Nop(int argument) : base(argument) { }
            public override EmulatorState Run(EmulatorState currentState)
            {
                EmulatorState newState = currentState.Clone();
                newState.IncrementInstructionCount();
				newState.IncrementCurrentIndex(1);
                return newState;
            }
        }
    }
}
