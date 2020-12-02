using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace advent_of_code_2020
{
    [TestFixture]
    public class Day01Tests
    {
        [Test]
        public void CombinesToTwoThousand_TestInput_GetsExpectedProduct()
        {
            var subject = new CombinesToTwoThousand();
            int result =
                subject.FindCombinationInFile(
                    "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day01-test-input-1.txt", 2020);
            result.Should().Be(514579);
        }

        [Test]
        public void CombinesToTwentyTwenty_RealInput_GetsRealAnswer()
        {
            var subject = new CombinesToTwoThousand();
            int result =
                subject.FindCombinationInFile(
                    "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day01-input.txt", 2020);
            result.Should().Be(211899);
        }

        [Test]
        public void CombinesThreeToExpected_TestInput_GetsTheAnswer()
        {
            var subject = new CombinesThreeToExpected();
            int result =
                            subject.FindCombinationInFile(
                                "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day01-test-input-1.txt", 2020);
            result.Should().Be(241861950);
        }

        [Test]
        public void CombinesThreeToExpected_RealInput_GetsRealAnswer()
        {
            var subject = new CombinesThreeToExpected();
            int result =
                subject.FindCombinationInFile(
                    "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day01-input.txt", 2020);
            result.Should().Be(275765682);
        }
    }

    public class CombinesThreeToExpected
    {
        public int FindCombinationInFile(string filename, int expected)
        {
            var entries = File.ReadAllLines(filename).ToList();
            var parsedEntries = entries.Select(int.Parse).ToList();
            foreach (var i in parsedEntries)
            {
                foreach (var j in parsedEntries)
                {
                    foreach (var k in parsedEntries.Where(k => i + j + k == expected))
                    {
                        return i * j * k;
                    }
                }
            }
            throw new Exception("Reached the end of the file with no expected combinations");
        }
    }

    public class CombinesToTwoThousand
    {
        public int FindCombinationInFile(string filename, int expected)
        {
            int x = 0;
            int y = 0;
            var entries = File.ReadAllLines(filename).ToList();
            for (int i = 0; i < entries.Count; i++)
            {
                for (int j = 0; j < entries.Count; j++)
                {
                    if (int.Parse(entries[i]) + int.Parse(entries[j]) == expected)
                    {
                        x = int.Parse(entries[i]);
                        y = int.Parse(entries[j]);
                        return x * y;
                    }
                }
            }
            throw new Exception("Reached the end of the file with no expected combinations");
        }
    }
}