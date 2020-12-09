using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay9
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            {
                var data = Lines(reader).Select(long.Parse).ToList();
                var invalid = FindInvalidNumber(data);
                for(var ixBegin = 0; ixBegin<data.Count; ixBegin++)
                {
                    var currentSum = data[ixBegin];
                    var ixNext = ixBegin + 1;
                    while(currentSum < invalid && ixNext < data.Count)
                    {
                        currentSum += data[ixNext];
                        if (currentSum == invalid)
                        {
                            var range = data.Skip(ixBegin).Take(ixNext + 1 - ixBegin);
                            Console.WriteLine(range.Min() + range.Max());
                            return;
                        }
                        ixNext++;
                    }
                }
            }
        }

        static long FindInvalidNumber(IEnumerable<long> source)
        {
            Queue<long> buffer = new Queue<long>();
            foreach (var n in source)
            {
                if (buffer.Count < 25)
                {
                    buffer.Enqueue(n);
                    continue;
                }

                if (!HasMatchingSumPair(buffer, n))
                {
                    return n;
                }

                buffer.Enqueue(n);
                buffer.Dequeue();
            }
            return -1;
        }

        static bool HasMatchingSumPair(IEnumerable<long> buffer, long candidateSum)
        {
            var s = buffer.Where(b => b <= candidateSum).OrderBy(b=>b).ToList();
            for(int i=0; i<s.Count; i++)
            {
                for(int j=i+1; j < s.Count; j++)
                {
                    var sum = s[i] + s[j];
                    if (sum == candidateSum)
                    {
                        return true;
                    }
                    if (sum > candidateSum)
                    {
                        break;
                    }
                }
            }
            return false;
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
