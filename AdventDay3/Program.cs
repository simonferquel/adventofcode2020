using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay3
{
    class Program
    {
        static void Main(string[] args)
        {
            var analyzers = new List<SlopeAnalyzer>
            {
                new SlopeAnalyzer(1,1),
                new SlopeAnalyzer(3,1),
                new SlopeAnalyzer(5,1),
                new SlopeAnalyzer(7,1),
                new SlopeAnalyzer(1,2),
            };
            using (var reader = File.OpenText("input.txt"))
            {
                foreach (var row in Lines(reader).Select(l => new Row(l)))
                {
                    foreach (var analyzer in analyzers)
                    {
                        analyzer.Observe(row);
                    }
                }
            }
            Console.WriteLine(analyzers.Aggregate(1L, (x, a) => x * a.ObservedTrees));
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

    class SlopeAnalyzer
    {
        readonly int _rightStep;
        readonly int _downStep;
        int _x;
        int _observedRows;
        int _observedTrees;

        public int ObservedTrees => _observedTrees;

        public SlopeAnalyzer(int right, int down)
        {
            _rightStep = right;
            _downStep = down;
        }

        public void Observe(Row r)
        {
            if (_observedRows % _downStep == 0)
            {
                if (r.IsTree(_x))
                {
                    _observedTrees++;
                }

                _x += _rightStep;
            }

            _observedRows++;
        }
    }

    struct Row
    {
        private List<bool> _data;
        public List<bool> Data => _data;

        public Row(string value)
        {
            _data = new List<bool>();
            foreach (var c in value)
            {
                _data.Add(c == '#');
            }
        }

        public bool IsTree(int index)
        {
            return _data[index % _data.Count];
        }
    }
}
