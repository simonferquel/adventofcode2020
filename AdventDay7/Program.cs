using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay7
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Rule> _rulesByOutter = new Dictionary<string, Rule>();
            Dictionary<string, List<Rule>> _rulesByInner = new Dictionary<string, List<Rule>>();
            using (var reader = File.OpenText("input.txt"))
            {
                foreach(var rule in Lines(reader).Select(l=>new Rule(l))){
                    _rulesByOutter.Add(rule.OutterColor, rule);
                    if (rule.Content == null)
                    {
                        continue;
                    }
                    foreach(var inner in rule.Content.Keys)
                    {
                        if(_rulesByInner.TryGetValue(inner, out var rules))
                        {
                            rules.Add(rule);
                        } else
                        {
                            _rulesByInner.Add(inner, new List<Rule> { rule });
                        }
                    }
                }
            }
            var outters = new HashSet<string>();
            SolvePossibleContainers(_rulesByInner, "shiny gold", outters);
            Console.WriteLine(outters.Count);
            Console.WriteLine(SolveNumberOfContainedBagFor(_rulesByOutter, "shiny gold", new Dictionary<string, int>()));
        }

        static void SolvePossibleContainers(Dictionary<string, List<Rule>> rulesByInner, string innerColor, HashSet<string> accumulatedResults)
        {
            if(rulesByInner.TryGetValue(innerColor, out var rules))
            {
                foreach(var r in rules)
                {
                    if (accumulatedResults.Add(r.OutterColor))
                    {
                        SolvePossibleContainers(rulesByInner, r.OutterColor, accumulatedResults);
                    }
                }               
            }
        }

        static int SolveNumberOfContainedBagFor(Dictionary<string, Rule> rulesByOutter, string outterColor, Dictionary<string, int> cache)
        {
            if(cache.TryGetValue(outterColor, out var result))
            {
                return result;
            }

            int totalCount = 0;
            var rule = rulesByOutter[outterColor];
            if(rule.Content == null)
            {
                cache[outterColor] = 0;
                return 0;
            }
            foreach(var b in rule.Content)
            {
                totalCount += b.Value + b.Value * SolveNumberOfContainedBagFor(rulesByOutter, b.Key, cache);
            }
            cache[outterColor] = totalCount;
            return totalCount;
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

    struct Rule
    {
        public string OutterColor { get; set; }
        public Dictionary<string, int> Content { get; set; }
        public Rule(string text)
        {
            var outterInnerSplit = text.Split(" contain ");
            var outterColor = ParseBagColor(outterInnerSplit[0].Split(' '));
            var content = ParseContent(outterInnerSplit[1]);
            OutterColor = outterColor;
            Content = content;
        }

        private static Dictionary<string, int> ParseContent(string v)
        {
            if(v == "no other bags.")
            {
                return null;
            }
            var result = new Dictionary<string, int>();
            var entries = v.Split(", ");
            foreach (var entry in entries)
            {
                var words = entry.Split(' ');
                var quantity = int.Parse(words[0]);
                var color = ParseBagColor(words.Skip(1).ToList());
                result.Add(color, quantity);
            }
            return result;
        }

        static string ParseBagColor(IList<string> words)
        {
            return string.Join(" ", words.Take(words.Count - 1));
        }
    }
}
