using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay1
{
    class Program
    {
        static void Main(string[] args)
        {           
            using (var reader = File.OpenText("input.txt"))
            {
                var numbers = LinesToNumbers(reader).ToList();
                for (var i = 0; i<numbers.Count; i++)
                {
                    for(var j = i+1; j<numbers.Count; j++)
                    {
                        for (var k = j + 1; k < numbers.Count; k++)
                        {
                            if (numbers[i] + numbers[j] + numbers[k] == 2020)
                            {
                                Console.WriteLine(numbers[i] * numbers[j] * numbers[k]);
                                return;
                            }
                        }
                    }
                }
            }
        }

        static IEnumerable<int> LinesToNumbers(StreamReader reader)
        {
            string line;
            while(null != (line = reader.ReadLine()))
            {
                yield return int.Parse(line);
            }
        }
    }
}
