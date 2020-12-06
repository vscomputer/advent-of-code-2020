using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace advent_of_code_2020
{
    [TestFixture]
    public class Day05Tests
    {
        [Test]
        public void Seats_InitialState_0to127()
        {
            var subject = new Seats();
            subject.Front.Should().Be(0);
            subject.Back.Should().Be(127);
        }
        
        [Test]
        public void Reduce_F_TakesFrontHalf()
        {
            var subject = new Seats();
            subject.Reduce("F");
            subject.Front.Should().Be(0);
            subject.Back.Should().Be(63);
        }

        [Test]
        public void Reduce_B_TakesBackHalf()
        {
            var subject = new Seats();
            subject.Reduce("B");
            subject.Front.Should().Be(64);
            subject.Back.Should().Be(127);
        }
        
        [Test]
        public void Reduce_FBFBBFF_ReturnsRow44()
        {
            var subject = new Seats();
            var input = "FBFBBFF";
            foreach (var half in input)
            {
                subject.Reduce(half.ToString());
            }

            subject.Back.Should().Be(44);
            subject.Front.Should().Be(44);
        }

        [Test]
        public void Reduce_RLR_ReturnsColumn5()
        {
            var subject = new Seats();
            var input = "RLR";
            foreach (var half in input)
            {
                subject.Reduce(half.ToString());
            }

            subject.Left.Should().Be(5);
            subject.Right.Should().Be(5);
        }

        [TestCase("FBFBBFFRLR", 357)]
        [TestCase("BFFFBBFRRR", 567)]
        [TestCase("FFFBBBFRRR", 119)]
        [TestCase("BBFFBBFRLL", 820)]
        public void GetSeatId_FBFBBFFRLR_Gets357(string input, int expected)
        {
            var subject = new Seats();
            foreach (var half in input)
            {
                subject.Reduce(half.ToString());
            }
            subject.GetSeatId().Should().Be(expected);
        }

        [Test]
        public void GetSeatId_IterateOverInput_GetHighestSeatId()
        {
            var lines = File.ReadAllLines(@"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day05-input.txt");
            var largestId = -1;
            foreach (var line in lines)
            {
                var seats = new Seats();
                foreach (var half in line)
                {
                    seats.Reduce(half.ToString());
                }

                if (seats.GetSeatId() > largestId)
                    largestId = seats.GetSeatId();
            }

            largestId.Should().Be(855);
        }

        [Test]
        public void PrintMissingSeatIds()
        {
            var ids = new List<int>();
            for (int i = 0; i < 855; i++)
            {
                ids.Add(i);
            }
            
            var lines = File.ReadAllLines(@"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day05-input.txt");
            var largestId = -1;
            foreach (var line in lines)
            {
                var seats = new Seats();
                foreach (var half in line)
                {
                    seats.Reduce(half.ToString());
                }

                ids.Remove(seats.GetSeatId());
            }
            var builder = new StringBuilder();
            foreach (var id in ids)
            {
                builder.Append(id);
                builder.Append(Env.NewLine);
            }
            File.WriteAllText(@"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day05-missing-ids.txt", builder.ToString());
            
        }
    }

    public class Seats
    {
        public Seats()
        {
            Front = 0;
            Back = 127;
            Left = 0;
            Right = 7;
        }
        public int Front { get; set; }
        public int Back { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }

        public void Reduce(string half)
        {
            switch (half)
            {
                case "F":
                    Back = Back - ((Depth() + 1) / 2);
                    break;
                case "B":
                    Front = Front + ((Depth() + 1) / 2);
                    break;
                case "L":
                    Right = Right - ((Width() + 1) / 2);
                    break;
                case "R":
                    Left = Left + ((Width() + 1) / 2);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private int Width()
        {
            return Right - Left;
        }

        private int Depth()
        {
            return Back - Front;
        }

        public int GetSeatId()
        {
            if((Back != Front) || (Left != Right))
            {
                throw new Exception("Seats didn't match, you don't understand this yet.");
            }

            return (Back * 8) + Left;
        }
    }
}