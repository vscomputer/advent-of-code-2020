using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using FluentAssertions;
using NUnit.Framework;

namespace advent_of_code_2020
{
    [TestFixture]
    public class Day02Tests
    {
        [Test]
        public void ChecksPasswordPolicy_OneA_PassesMuster()
        {
            var subject = new ChecksPasswordPolicy();
            var result = subject.CheckPassword("1-3 a: abcde");
            result.Should().BeTrue();
        }

        [Test]
        public void ChecksPasswordPolicy_NoBs_Fails()
        {
            var subject = new ChecksPasswordPolicy();
            subject.CheckPassword("1-3 b: cdefg").Should().BeFalse();
        }

        [Test]
        public void ChecksPasswordPolicyByPosition_OneA_Passes()
        {
            var subject = new ChecksPasswordPolicyByPosition();
            subject.CheckPassword("1-3 a: abcde").Should().BeTrue();
        }

        [Test]
        public void ParsesPasswordPolicy_GetsMinimumCharactersFromPasswordString()
        {
            var subject = new ParsesPasswordPolicy("1-3 a: abcde");
            subject.GetMinimum().Should().Be(1);
        }

        [Test]
        public void ParsesPasswordPolicy_GetsMaximumCharactersFromPasswordString()
        {
            var subject = new ParsesPasswordPolicy("1-3 a: abcde");
            subject.GetMaximum().Should().Be(3);
        }

        [Test]
        public void ChecksPasswordPolicyByPosition_NoBs_Fails()
        {
            var subject = new ChecksPasswordPolicyByPosition();
            subject.CheckPassword("1-3 b: cdefg").Should().BeFalse();
        }

        [Test]
        public void ChecksPasswordPolicyByPosition_BothMatch_Fails()
        {
            var subject = new ChecksPasswordPolicyByPosition();
            subject.CheckPassword("2-9 c: ccccccccc").Should().BeFalse();
        }

        [Test]
        public void ParsesPasswordPolicy_GetsCharacter()
        {
            var subject = new ParsesPasswordPolicy("1-3 a: abcde");
            subject.GetCharacter().Should().Be("a");
        }

        [Test]
        public void ParsesPasswordPolicy_GetsPassword()
        {
            var subject = new ParsesPasswordPolicy("1-3 a: abcde");
            subject.GetPassword().Should().Be("abcde");
        }

        [Test]
        public void ChecksPasswordPolicy_OpenFile_CorrectNumberOfEntries()
        {
            var subject = new ChecksPasswordPolicy();
            subject.GetNumberOfPassingEntriesInFile(
                @"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day02-test-input-1.txt").Should().Be(2);
        }

        [Test]
        public void ChecksPasswordPolicy_GetNumberOfPassingEntriesInFile_GetsRealNumberOfEntries()
        {
            var subject = new ChecksPasswordPolicy();
            subject.GetNumberOfPassingEntriesInFile(
                @"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day02-input.txt").Should().Be(467);
        }
        
        [Test]
        public void ChecksPasswordPolicyByPosition_OpenFile_CorrectNumberOfEntries()
        {
            var subject = new ChecksPasswordPolicyByPosition();
            subject.GetNumberOfPassingEntriesInFile(
                @"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day02-test-input-1.txt").Should().Be(1);
        }

        [Test]
        public void ChecksPasswordPolicyByPosition_GetNumberOfPassingEntriesInFile_GetsRealNumberOfEntries()
        {
            var subject = new ChecksPasswordPolicyByPosition();
            subject.GetNumberOfPassingEntriesInFile(
                @"C:\Projects\Homework\advent-of-code-2020-puzzle-input\aoc-day02-input.txt").Should().Be(467);
        }
    }

    public class ChecksPasswordPolicyByPosition : IChecksPasswordPolicy
    {
        public bool CheckPassword(string policy)
        {
            var parsesPasswordPolicy = new ParsesPasswordPolicy(policy);
            if (
                (parsesPasswordPolicy.GetCharacter().ToCharArray()[0] ==
                 parsesPasswordPolicy.GetPassword()[parsesPasswordPolicy.GetMinimum() - 1])
                &&
                (parsesPasswordPolicy.GetCharacter().ToCharArray()[0] ==
                 parsesPasswordPolicy.GetPassword()[parsesPasswordPolicy.GetMaximum() - 1])
            )
                return false;
            
            if ((parsesPasswordPolicy.GetCharacter().ToCharArray()[0] ==
                 parsesPasswordPolicy.GetPassword()[parsesPasswordPolicy.GetMinimum() - 1]))
                return true;
            if ((parsesPasswordPolicy.GetCharacter().ToCharArray()[0] ==
                 parsesPasswordPolicy.GetPassword()[parsesPasswordPolicy.GetMaximum() - 1]))
                return true;

            return false;
        }
        
        public int GetNumberOfPassingEntriesInFile(string filename)
        {
            var policies = File.ReadAllLines(filename).ToList();
            int numberOfPassingPasswords = 0;
            foreach (var policy in policies)
            {
                var checksPasswordPolicy = new ChecksPasswordPolicyByPosition();
                if (checksPasswordPolicy.CheckPassword(policy))
                {
                    numberOfPassingPasswords++;
                }
            }

            return numberOfPassingPasswords;
        }
    }

    public interface IChecksPasswordPolicy
    {
        bool CheckPassword(string password);
    }
    public class ChecksPasswordPolicy : IChecksPasswordPolicy
    {
        public bool CheckPassword(string password)
        {
            var parsesPasswordPolicy = new ParsesPasswordPolicy(password);
            var charCount = parsesPasswordPolicy
                .GetPassword()
                .Count(c => c == parsesPasswordPolicy.GetCharacter().ToCharArray()[0]);
            return charCount >= parsesPasswordPolicy.GetMinimum() && charCount <= parsesPasswordPolicy.GetMaximum();
        }

        public int GetNumberOfPassingEntriesInFile(string filename)
        {
            var policies = File.ReadAllLines(filename).ToList();
            int numberOfPassingPasswords = 0;
            foreach (var policy in policies)
            {
                var checksPasswordPolicy = new ChecksPasswordPolicy();
                if (checksPasswordPolicy.CheckPassword(policy))
                {
                    numberOfPassingPasswords++;
                }
            }

            return numberOfPassingPasswords;
        }
    }

    public class ParsesPasswordPolicy
    {
        private int _minimum;
        private int _maximum;
        private string _character;
        private string _password;
        public ParsesPasswordPolicy(string policy)
        {
            Parse(policy);
        }

        private void Parse(string policy)
        {
            var sections = policy.Split(' ');
            var minMax = sections[0];
            var minAndMax = minMax.Split('-');
            _minimum = int.Parse(minAndMax[0]);
            _maximum = int.Parse(minAndMax[1]);
            _character = sections[1].Replace(':', ' ').Trim();
            _password = sections[2];
        }

        public int GetMinimum()
        {
            return _minimum;
        }

        public int GetMaximum()
        {
            return _maximum;
        }

        public string GetCharacter()
        {
            return _character;
        }

        public string GetPassword()
        {
            return _password;
        }
    }
}