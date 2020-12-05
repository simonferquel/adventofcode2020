using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay5
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            {
                var lst = new SortedSet<int>(Lines(reader).Select(s => new Seat(s)).Select(s => s.ID));
                for(var i = lst.Min+1; i<lst.Max; i++)
                {
                    if (!lst.Contains(i))
                    {
                        Console.WriteLine(i);
                    }
                }
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

    struct Seat
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int ID => Row * 8 + Column;

        static int SearchInRange(IEnumerable<bool> highLow, Range range)
        {
            foreach(var high in highLow)
            {
                range = high ? range.HigherHalf : range.LowerHalf;
            }
            return range.LowerBoundInclusive;
        }

        public Seat(string str)
        {
            Row = SearchInRange(str.Take(7).Select(c => c == 'B'), new Range { LowerBoundInclusive = 0, HigherBoundExclusive = 128 });
            Column = SearchInRange(str.Skip(7).Select(c => c == 'R'), new Range { LowerBoundInclusive = 0, HigherBoundExclusive = 8 });
        }
    }

    struct Range
    {
        public int LowerBoundInclusive { get; set; }
        public int HigherBoundExclusive { get; set; }
        public int Middle => LowerBoundInclusive + (HigherBoundExclusive - LowerBoundInclusive) / 2;
        public Range LowerHalf => new Range { LowerBoundInclusive = LowerBoundInclusive, HigherBoundExclusive = Middle };
        public Range HigherHalf => new Range { LowerBoundInclusive = Middle, HigherBoundExclusive = HigherBoundExclusive };
    }
}
