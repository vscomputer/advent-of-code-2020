using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace advent_of_code_2020
{
    [TestFixture]
    public class Day06Tests
    {
        private string testOne = @"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day06-test-input-1.txt";
        private string testTwo = @"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day06-test-input-2.txt";
        private string realInput = @"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day06-input.txt";
        [Test]
        public void GetsYes_SimpleIndividual_GetsYesses()
        {
            var subject = new GetsYes();
            int result = subject.Get("abcx");
            result.Should().Be(4);
        }

        [Test]
        public void GetsYes_GroupOfIndividuals_DoesntCountDuplicates()
        {
            var subject = new GetsYes();
            int result = subject.GetGroupSum(File.ReadAllLines(testOne));
            result.Should().Be(6);
        }

        [Test]
        public void GetsYes_GroupOfGroups_GetsSumOfGroups()
        {
            var subject = new GetsYes();
            int result = subject.GetSumOfGroups(File.ReadAllLines(testTwo));
            result.Should().Be(11);
        }

        [Test]
        public void GetsYes_RealInput_GetsRealAnswer()
        {
            var subject = new GetsYes();
            int result = subject.GetSumOfGroups(File.ReadAllLines(realInput));
            result.Should().Be(6534);
        }

        [Test]
        public void GetsYes_PartTwo_UnanimousResponses()
        {
            var subject = new GetsYes();
            int result = subject.GetSumOfGroups(File.ReadAllLines(testTwo), true);
            result.Should().Be(6);
        }

        [Test]
        public void GetsYes_PartTwo_RealInput()
        {
            var subject = new GetsYes();
            int result = subject.GetSumOfGroups(File.ReadAllLines(realInput), true);
            result.Should().Be(3402);
        }
    }

    public class GetsYes
    {
        public int Get(string posResponses)
        {
            return posResponses.Length;
        }
        
        public int GetGroupSum(string[] lines)
        {
            StringBuilder aggregateResponse = new StringBuilder();
            foreach (var line in lines)
            {
                foreach (var letter in line)
                {
                    if (aggregateResponse.ToString().Contains(letter.ToString()) == false)
                    {
                        aggregateResponse.Append(letter);
                    }
                }
            }

            return aggregateResponse.Length;
        }
        
        private int GetUnanimousGroupSum(string[] lines)
        {
            var posResponses = new StringBuilder();
            posResponses.Append(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                foreach (var letter in posResponses.ToString())
                {
                    if (lines[i].Contains(letter.ToString()) == false)
                    {
                        posResponses.Replace(letter.ToString(), string.Empty);
                    }
                }
            }
            return posResponses.Length;
        }

        public int GetSumOfGroups(string[] lines, bool needsUnanimous = false)
        {
            var totalPosResponses = 0;
            var parsedLines = 0;
            while (parsedLines < lines.Length)
            {
                var group = new List<string>();
                while (parsedLines < lines.Length && string.IsNullOrWhiteSpace(lines[parsedLines]) == false)
                {
                    group.Add(lines[parsedLines]);
                    parsedLines++;
                }

                if (needsUnanimous)
                {
                    totalPosResponses += GetUnanimousGroupSum(group.ToArray());
                }
                else
                {
                    totalPosResponses += GetGroupSum(group.ToArray());    
                }
                parsedLines++;
            }

            return totalPosResponses;
        }
    }
}