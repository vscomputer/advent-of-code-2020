using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace advent_of_code_2020
{
    [TestFixture]
    public class Day03Tests
    {
        private string testFile =
            "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day03-test-input-1.txt";

        private string realFile = @"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day03-input.txt";
        
        [Test]
        public void GetsRowsFromFile_TestFile_Gets11Rows()
        {
            var subject = new GetsRowsFromFile();
            List<Row> result = subject.Get(testFile);
            result.Count.Should().Be(11);
        }
        
        //..##.......
        //#...#...#..
        //.#....#..#.
        //..#.#...#.#
        //.#...##..#.
        //..#.##.....
        //.#.#.#....#
        //.#........#
        //#.##...#...
        //#...##....#
        //.#..#...#.#

        [Test]
        public void ChecksForTrees_TestFile_FindsTreesAndSpacesInFirstLine()
        {
            var subject = new ChecksForTrees();
            var getsRows = new GetsRowsFromFile();
            var rows = getsRows.Get(testFile);
            subject.Check(rows, 1, 1).Should().BeFalse();
            subject.Check(rows, 1, 2).Should().BeFalse();
            subject.Check(rows, 1, 3).Should().BeTrue();
        }

        [Test]
        public void ChecksForTrees_TestFile_FindsTreesandSpacesInSecondLine()
        {
            var subject = new ChecksForTrees();
            var getsRows = new GetsRowsFromFile();
            var rows = getsRows.Get(testFile);
            subject.Check(rows, 2, 1).Should().BeTrue();
            subject.Check(rows, 2, 2).Should().BeFalse();
            subject.Check(rows, 2, 3).Should().BeFalse();
        }

        [Test]
        public void ChecksForTrees_TestFile_FindsTreesAndSpacesInFirstLinePastEndOfPattern()
        {
            var subject = new ChecksForTrees();
            var getsRows = new GetsRowsFromFile();
            var rows = getsRows.Get(testFile);
            subject.Check(rows, 1, 12).Should().BeFalse();
            subject.Check(rows, 1, 13).Should().BeFalse();
            subject.Check(rows, 1, 14).Should().BeTrue();
        }

        [Test]
        public void ChecksForTrees_TestFile_TripDownRowsGetsTheRightNumberOfTrees()
        {
            var subject = new ChecksForTrees();
            var getsRows = new GetsRowsFromFile();
            var rows = getsRows.Get(testFile);
            int treesHit = 0;
            int currentRow = 1;
            int currentColumn = 1;
            while (currentRow <= rows.Count)
            {
                if (subject.Check(rows, currentRow, currentColumn))
                {
                    treesHit++;
                }

                currentRow++;
                currentColumn += 3;
            }

            treesHit.Should().Be(7);
        }
        
        [Test]
        public void ChecksForTrees_RealFile_TripDownRowsGetsTheRightNumberOfTrees()
        {
            var subject = new ChecksForTrees();
            var getsRows = new GetsRowsFromFile();
            var rows = getsRows.Get(realFile);
            int treesHit = 0;
            int currentRow = 1;
            int currentColumn = 1;
            while (currentRow <= rows.Count)
            {
                if (subject.Check(rows, currentRow, currentColumn))
                {
                    treesHit++;
                }

                currentRow++;
                currentColumn += 3;
            }

            treesHit.Should().Be(276);
        }
    }
    
    public class Row
    {
        private readonly List<bool> _treePositions;

        public Row(string line)
        {
            _treePositions = DetermineTreePositions(line);
        }

        public List<bool> GetTreePositions()
        {
            return _treePositions;
        }

        private List<bool> DetermineTreePositions(string positions)
        {
            var result = new List<bool>();
            foreach (char position in positions)
            {
                switch (position)
                {
                    case '.':
                        result.Add(false);
                        break;
                    //tree
                    case '#':
                        result.Add(true);
                        break;
                    default:
                        throw new ArgumentException("neither a tree nor not a tree?");
                }
            }
            return result;
        }
    }

    public class ChecksForTrees
    {
        public bool Check(List<Row> rows, int row, int column)
        {
            var currentRow = rows[(row - 1)];
            var patternLength = currentRow.GetTreePositions().Count;
            return currentRow.GetTreePositions()[(column - 1) % patternLength];
        }
    }

    public class GetsRowsFromFile
    {
        public List<Row> Get(string filename)
        {
            var lines = File.ReadAllLines(filename);
            return lines.Select(line => new Row(line)).ToList();
        }
    }
}