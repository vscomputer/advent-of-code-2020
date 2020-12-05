using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace advent_of_code_2020
{
    [TestFixture]
    public class Day04Tests
    {
        private string testFile =
            "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day04-test-input-1.txt";
        private string realFile =
            "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day04-input.txt";
        
        [Test]
        public void GetsPassportsFromFile_TestFile_Gets4Passports()
        {
            var subject = new GetsPassportsFromFile();
            List<Passport> result = subject.Get(testFile);
            result.Count.Should().Be(4);
        }

        [Test]
        public void GetsPassportsFromFle_TestFile_FirstPassportHas8Keys()
        {
            var subject = new GetsPassportsFromFile();
            List<Passport> result = subject.Get(testFile);
            result[0].GetFields().Count.Should().Be(8);
        }

        [Test]
        public void ValidatesPassports_TestFile_RejectsOnlyBadPassports()
        {
            var getsPassportsFromFile = new GetsPassportsFromFile();
            var passports = getsPassportsFromFile.Get(testFile);
            var subject = new ValidatesPassports();

            var validatedFields = subject.Validate(passports);
            validatedFields.Should().Contain(p => p.ContainsField("pid", "860033327"));
            validatedFields.Should().NotContain(p => p.ContainsField("pid", "028048884"));
            validatedFields.Should().Contain(p => p.ContainsField("pid", "760753108"));
            validatedFields.Should().NotContain(p => p.ContainsField("pid", "166559648"));
        }

        [Test]
        public void ValidatesPassports_RealFile_RejectsOnlyBadPassports()
        {
            var getsPassportsFromFile = new GetsPassportsFromFile();
            var passports = getsPassportsFromFile.Get(realFile);
            var subject = new ValidatesPassports();

            var validatedFields = subject.Validate(passports); 
            var csvWriter = new WritesCsv();
            //csvWriter.Write(validatedFields);
            validatedFields.Count.Should().Be(156);
        }
        
        private string invalidValues =
            "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day04-test-input-2.txt";
        private string validValues =
            "C:\\Projects\\Homework\\advent-of-code-2020-puzzle-input\\aoc-day04-test-input-3.txt";

        [Test]
        public void ValidatesPassports_InvalidAndValidPassports_ReturnsCorrectNumbers()
        {
            //getting lazy here, this problem is kind of dumb
            var getsPassportsFromFile = new GetsPassportsFromFile();
            var invalidPassports = getsPassportsFromFile.Get(invalidValues);
            var validPassports = getsPassportsFromFile.Get(validValues);
            var subject = new ValidatesPassports();

            subject.Validate(invalidPassports).Count.Should().Be(0);
            subject.Validate(validPassports).Count.Should().Be(4);
        }

        [Test]
        public void IsValidHeight_162in_ReturnsFalse() //bugfix
        {
            var subject = new ValidatesPassports();
            subject.IsValidHeight("162in").Should().BeFalse();
        }

    }

    public class WritesCsv
    {
        public void Write(List<Passport> validatedFields)
        {
            var builder = new StringBuilder();
            builder.Append("byr,iyr,eyr,hgt,hcl,ecl,pid,cid");
            builder.Append(Environment.NewLine);
            foreach (var passport in validatedFields)
            {
                string byr, iyr, eyr, hgt, hcl, ecl, pid, cid;
                passport.GetFields().TryGetValue("byr", out byr);
                passport.GetFields().TryGetValue("iyr", out iyr);
                passport.GetFields().TryGetValue("eyr", out eyr);
                passport.GetFields().TryGetValue("hgt", out hgt);
                passport.GetFields().TryGetValue("hcl", out hcl);
                passport.GetFields().TryGetValue("ecl", out ecl);
                passport.GetFields().TryGetValue("pid", out pid);
                passport.GetFields().TryGetValue("cid", out cid);
                builder.Append(byr + ',' + iyr + ',' + eyr + ',' + hgt + ',' + hcl + ',' + ecl + ',' + pid + ',' + cid);
                builder.Append(Environment.NewLine);
            }
            File.WriteAllText(@"C:\Projects\Homework\advent-of-code-2020-puzzle-input\what.csv", builder.ToString());
        }
    }

    public class ValidatesPassports
    {
        public List<Passport> Validate(List<Passport> passports)
        {
            return passports.Where(passport => ContainsRequiredKeys(passport) && PassportValuesAreValid(passport)).ToList();
        }

        private static bool ContainsRequiredKeys(Passport passport)
        {
            return passport.GetFields().ContainsKey("ecl") &&
                   passport.GetFields().ContainsKey("pid") &&
                   passport.GetFields().ContainsKey("eyr") &&
                   passport.GetFields().ContainsKey("hcl") &&
                   passport.GetFields().ContainsKey("byr") &&
                   passport.GetFields().ContainsKey("iyr") &&
                   passport.GetFields().ContainsKey("hgt");
        }
        
        
        // byr (Birth Year) - four digits; at least 1920 and at most 2002.
        // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
        // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
        // hgt (Height) - a number followed by either cm or in:
        // If cm, the number must be at least 150 and at most 193.
        // If in, the number must be at least 59 and at most 76.
        // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
        // ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
        // pid (Passport ID) - a nine-digit number, including leading zeroes.
        // cid (Country ID) - ignored, missing or not.


        private bool PassportValuesAreValid(Passport passport)
        {
            string actualValue = string.Empty;
            passport.GetFields().TryGetValue("byr", out actualValue);
            if (int.Parse(actualValue) < 1920 || int.Parse(actualValue) > 2002)
                return false;
            passport.GetFields().TryGetValue("iyr", out actualValue);
            if (int.Parse(actualValue) < 2010 || int.Parse(actualValue) > 2020)
                return false;
            passport.GetFields().TryGetValue("eyr", out actualValue);
            if (int.Parse(actualValue) < 2020 || int.Parse(actualValue) > 2030)
                return false;
            passport.GetFields().TryGetValue("hgt", out actualValue);
            if (IsValidHeight(actualValue) == false)
                return false;
            passport.GetFields().TryGetValue("hcl", out actualValue);
            if (IsValidHairColor(actualValue) == false)
                return false;
            passport.GetFields().TryGetValue("ecl", out actualValue);
            if (IsValidEyeColor(actualValue) == false)
                return false;
            passport.GetFields().TryGetValue("pid", out actualValue);
            if (IsValidPassportId(actualValue) == false)
                return false;
            return true;
        }

        private bool IsValidPassportId(string id)
        {
            return id.Length == 9 && int.TryParse(id, out _);
        }

        private bool IsValidEyeColor(string color)
        {
            if (color.Length != 3)
                return false;
            var validEyeColors = new List<string> {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
            return validEyeColors.Contains(color);
        }

        public bool IsValidHeight(string height)
        {
            var unit = height.Substring(height.Length -2, 2);
            var heightInUnits = height.Substring(0, height.Length - 2);
            if ((unit == "cm" && int.Parse(heightInUnits) >= 150) && (unit == "cm" && int.Parse(heightInUnits) <= 193))
                return true;
            if((unit == "in" && int.Parse(heightInUnits)>= 59) && (unit == "in" && int.Parse(heightInUnits) <= 76))
                return true;

            return false;
        }

        private bool IsValidHairColor(string color)
        {
            // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            if (color.StartsWith("#") == false || color.Length != 7)
                return false;

            var colorValue = color.Substring(1);
            for (int i = 0; i < 6; i++)
            {
                //probably some way to use hexadecimal here, think about that when cleaning up
                if (((colorValue[i] >= 48 && colorValue[i] <= 57) || (colorValue[i] >= 97 && colorValue[i] <= 102)) ==
                    false)
                    return false;
            }
            return true;
        }
    }

    public class Passport
    {
        private readonly Dictionary<string, string> _passportFields;

        public Passport()
        {
            _passportFields = new Dictionary<string, string>();
        }
        public Dictionary<string, string> GetFields()
        {
            return _passportFields;
        }

        public void AddFieldsFromLine(string line)
        {
            foreach(var field in line.Split(' '))
            {
                if (string.IsNullOrWhiteSpace(field)) continue;
                var pair = field.Split(':');
                _passportFields.Add(pair[0], pair[1]);
            }
        }

        public bool ContainsField(string key, string value)
        {
            return _passportFields.TryGetValue(key, out var actualValue) && value == actualValue;
        }
    }

    public class GetsPassportsFromFile
    {
        public List<Passport> Get(string testFile)
        {
            var lines = File.ReadAllLines(testFile).ToList();
            var result = new List<Passport>();
            int parsedLines = 0;
            while (parsedLines < lines.Count)
            {
                var passport = new Passport();
                while ( parsedLines < lines.Count && string.IsNullOrWhiteSpace(lines[parsedLines]) == false)
                {
                    passport.AddFieldsFromLine(lines[parsedLines]);
                    parsedLines++;
                }
                result.Add(passport);
                parsedLines++;
            }
            return result;
        }
    }
}