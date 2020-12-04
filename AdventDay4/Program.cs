using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay4
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            {
                var passports = Passports(Lines(reader));
                Console.WriteLine(passports.Count(p => p.IsValid));
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

        static IEnumerable<PassportEntry> Passports(IEnumerable<string> lines)
        {
            var fields = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (fields.Count > 0)
                    {
                        yield return new PassportEntry(fields);
                        fields = new Dictionary<string, string>();
                    }
                }
                else
                {
                    var pairs = line.Split(' ');
                    foreach (var p in pairs)
                    {
                        var kv = p.Split(':');
                        fields.Add(kv[0], kv[1]);
                    }
                }
            }
            if (fields.Count > 0)
            {
                yield return new PassportEntry(fields);
            }
        }
    }

    struct PassportEntry
    {
        Dictionary<string, string> _fields;
        public PassportEntry(Dictionary<string, string> fields)
        {
            _fields = fields;
        }

        public bool IsValid => _fields.ContainsKey("byr") && ValidateIntRange(_fields["byr"], 1920,2002) &&
            _fields.ContainsKey("iyr") && ValidateIntRange(_fields["iyr"], 2010, 2020) &&
            _fields.ContainsKey("eyr") && ValidateIntRange(_fields["eyr"], 2020, 2030) &&
            _fields.ContainsKey("hgt") && ValidateHeight(_fields["hgt"]) &&
            _fields.ContainsKey("hcl") && ValidateColor(_fields["hcl"]) &&
            _fields.ContainsKey("ecl") && ValidateEyeColor(_fields["ecl"]) &&
            _fields.ContainsKey("pid") && ValidatePID(_fields["pid"]);

        private bool ValidateIntRange(string str, int min, int max)
        {
            if(int.TryParse(str, out var v))
            {
                return v >= min && v <= max;
            }
            return false;
        }

        private bool ValidateHeight(string str)
        {
            if (str.EndsWith("cm")) return ValidateIntRange(str.Substring(0, str.Length - 2), 150, 193);
            if (str.EndsWith("in")) return ValidateIntRange(str.Substring(0, str.Length - 2), 59, 76);
            return false;
        }

        private bool ValidateColor(string str)
        {
            if (str.Length != 7)
            {
                return false;
            }
            if(str[0] != '#')
            {
                return false;
            }
            return str.Skip(1).All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'));
        }

        private bool ValidateEyeColor(string str)
        {
            return str switch
            {
                "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth" => true,
                _ => false
            };
        }

        private bool ValidatePID(string str)
        {
            return str switch
            {
                { Length: 9 } => int.TryParse(str, out _),
                _ => false
            };
        }
    }
}
