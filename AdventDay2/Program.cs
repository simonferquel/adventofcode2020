using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            {
                var result = Lines(reader).Select(l => PasswordEntry.Parse(l)).Aggregate(new Result(), (previous, entry) => previous.Aggregate(entry));
                Console.WriteLine($"Valid (v1): {result.ValidV1}");
                Console.WriteLine($"Valid (v2): {result.ValidV2}");
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

    struct Result
    {
        public int ValidV1 { get; set; }
        public int ValidV2 { get; set; }

        public Result Aggregate(PasswordEntry entry)
        {
            return new Result
            {
                ValidV1 = ValidV1 + (entry.IsValid ? 1 : 0),
                ValidV2 = ValidV2 + (entry.IsValidV2 ? 1 : 0)
            };
        }
    }

    struct Constraint
    {
        public char Letter { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public static Constraint Parse(string s)
        {
            var parts = s.Split(" ");
            var rangeParts = parts[0].Split("-");
            return new Constraint
            {
                Letter = parts[1][0],
                Min = int.Parse(rangeParts[0]),
                Max = int.Parse(rangeParts[1])
            };
        }

        public bool Validate(string password)
        {
            var letter = Letter;
            var count = password.Count(l => l == letter);
            return count >= Min && count <= Max;
        }

        public bool ValidateV2(string password)
        {
            int matchCount = 0;
            if (IsInRange(password, Min) && password[Min - 1] == Letter)
            {
                matchCount++;
            }
            if (IsInRange(password, Max) && password[Max - 1] == Letter)
            {
                matchCount++;
            }
            return matchCount == 1;
        }

        static bool IsInRange(string password, int index)
        {
            return index > 0 && index <= password.Length;
        }
    }

    struct PasswordEntry
    {
        public Constraint Constraint { get; set; }
        public string Password { get; set; }
        public static PasswordEntry Parse(string s)
        {
            var parts = s.Split(":");
            return new PasswordEntry
            {
                Constraint = Constraint.Parse(parts[0]),
                Password = parts[1].Trim()
            };
        }

        public bool IsValid => Constraint.Validate(Password);
        public bool IsValidV2 => Constraint.ValidateV2(Password);
    }
}
