using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay6
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            {
                Console.WriteLine(Groups(Lines(reader)).Sum());
            }
        }

        static IEnumerable<int> Groups(IEnumerable<string> lines)
        {
            var current = new Dictionary<char, int>();
            int answerCount = 0;
            foreach (var line in lines)
            {
                if (line.Length == 0)
                {
                    yield return current.Where(kvp => kvp.Value == answerCount).Count();
                    current.Clear();
                    answerCount = 0;
                }
                else
                {
                    foreach (var c in line)
                    {
                        Increment(current, c);
                    }
                    answerCount++;
                }
            }
            if (current.Count != 0)
            {
                yield return current.Where(kvp => kvp.Value == answerCount).Count();
            }
        }

        static void Increment(Dictionary<char, int> current, char c)
        {
            if(current.TryGetValue(c, out var i))
            {
                current[c] = i + 1;
            } else
            {
                current[c] = 1;
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
}