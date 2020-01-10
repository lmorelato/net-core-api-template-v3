using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Core.Helpers
{
    public static class UserHelper
    {
        private static readonly Random Rand = new Random();

        public static string GenerateCode(string prefix = null, int size = 8)
        {
            var start = Convert.ToInt32("1".PadRight(size, '0'));
            var end = Convert.ToInt32(string.Empty.PadLeft(size, '9'));
            var code = (prefix ?? string.Empty) + Rand.Next(start, end);
            return code.Substring(0, size);
        }

        // https://github.com/Darkseal/PasswordGenerator/blob/master/PasswordGenerator.cs
        public static string GeneratePassword(
            int requiredLength = 6,
            int requiredUniqueChars = 4,
            bool requireDigit = true,
            bool requireUppercase = false,
            bool requireLowercase = true,
            bool requireNonAlphanumeric = false)
        {
            var randomChars = new List<string>();
            var chars = new List<char>();
            var upperCase = "ABCDEGHJKLMNOPQRSTUVWXYZ";
            var lowerCase = "abcdefghijkmnopqrstuvwxyz";
            var digit = "0123456789";
            var nonAlphanumeric = "!@#$%&+=?";

            if (requireUppercase)
            {
                randomChars.Add(upperCase);
                chars.Insert(Rand.Next(0, chars.Count), upperCase[Rand.Next(0, upperCase.Length)]);
            }

            if (requireLowercase)
            {
                randomChars.Add(lowerCase);
                chars.Insert(Rand.Next(0, chars.Count), lowerCase[Rand.Next(0, lowerCase.Length)]);
            }

            if (requireDigit)
            {
                randomChars.Add(digit);
                chars.Insert(Rand.Next(0, chars.Count), digit[Rand.Next(0, digit.Length)]);
            }

            if (requireNonAlphanumeric)
            {
                randomChars.Add(nonAlphanumeric);
                chars.Insert(Rand.Next(0, chars.Count), nonAlphanumeric[Rand.Next(0, nonAlphanumeric.Length)]);
            }

            for (var i = chars.Count; i < requiredLength || chars.Distinct().Count() < requiredUniqueChars; i++)
            {
                var rcs = randomChars[Rand.Next(0, randomChars.Count)];
                chars.Insert(Rand.Next(0, chars.Count), rcs[Rand.Next(0, rcs.Length)]);
            }

            var text = chars.OrderBy(x => Rand.Next()).ToArray();
            return new string(text);
        }
    }
}
