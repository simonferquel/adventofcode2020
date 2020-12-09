using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay8
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            {
                var instructionParsers = Lines(reader).Select(InstructionParser.Parse).ToList();
                for(int i = 0; i< instructionParsers.Count; i++)
                {
                    var instructions = instructionParsers.Select((p, ix) => ix == i ? p.Alternate : p.Normal).ToList();
                    try
                    {
                        Console.WriteLine(ExecuteProgram(instructions));
                        return;
                    }
                    catch(Exception e)
                    {
                        //
                    }
                }
                
            }

        }

        static int ExecuteProgram(List<Instruction> instructions)
        {
            var visitedInstructions = new HashSet<int>();
            var state = new MachineState();
            for (; ; )
            {
                if(state.InstructionPointer == instructions.Count)
                {
                    return state.Accumulator;
                }
                if (!visitedInstructions.Add(state.InstructionPointer))
                {
                    throw new InvalidOperationException("infinite loop detected, accumulator is: " + state.Accumulator);
                }
                if(state.InstructionPointer > instructions.Count)
                {
                    throw new InvalidOperationException("Segmentation fault");
                }
                var instruction = instructions[state.InstructionPointer];
                instruction(ref state);
            }
        }



        static IEnumerable<string> Lines(StreamReader reader)
        {
            string? line;
            while ((line = reader.ReadLine()) is not null)
            {
                yield return line;
            }
        }
    }

    struct InstructionParser
    {
        public Instruction Normal { get; set; }
        public Instruction Alternate { get; set; }

        public static InstructionParser Parse(string text)
        {
            var parts = text.Split(' ');
            var i = parts[0];
            var arg = int.Parse(parts[1]);
            return i switch
            {
                "nop" => new InstructionParser
                {
                    Normal = (ref MachineState state) => state.InstructionPointer++,
                    Alternate = (ref MachineState state) => state.InstructionPointer += arg,
                },
                "acc" => new InstructionParser
                {
                    Normal = (ref MachineState state) => { state.Accumulator += arg; state.InstructionPointer++; },
                    Alternate = (ref MachineState state) => { state.Accumulator += arg; state.InstructionPointer++; },
                }
                ,
                "jmp" => new InstructionParser
                {
                    Normal = (ref MachineState state) => state.InstructionPointer += arg,
                    Alternate = (ref MachineState state) => state.InstructionPointer++,
                },
                _ => throw new InvalidOperationException("unknown operation " + i)
            };
        }
    }

    delegate void Instruction(ref MachineState state);

    struct MachineState
    {
        public int Accumulator { get; set; }
        public int InstructionPointer { get; set; }
    }
}
