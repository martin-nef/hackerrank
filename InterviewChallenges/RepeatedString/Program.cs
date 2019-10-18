using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace RepeatedString {
    class Program {
        // Complete the repeatedString function below.
        static long repeatedString (string s, long n) {
            char countedChar = 'a';

            // Unexpected input
            if (string.IsNullOrEmpty (s) || s.Length > 100) {
                throw new ArgumentException (nameof (s), "Need |s| >= 1");
            }
            if (n <= 0) {
                throw new ArgumentException (nameof (s), "Need n >= 1");
            }

            // O(|s|)
            int asInString = s.Count (x => x == countedChar);
            if (asInString == 0) {
                return 0;
            }

            // Find how many times all of s fits in the n-long string
            long fullRepeats = (long) Math.Floor ((double) n / s.Length); // (cast n to double, otherwise result will be cast to long and rounded)
            // Find how long the rest of the n-long string is
            long lenOfTrailingPart = n % (long) s.Length;

            // Count a's in the whole part of the string
            return fullRepeats * asInString +
                // Count a's in incomplete part of s at the end of the n-long string
                s.Take ((int) lenOfTrailingPart) // (fast, since lenOfTrailingPart < |s| <= 100)
                .Count (x => x == countedChar);
        }

        static void Main (string[] args) {
            var res = repeatedString ("aba", 10);
            var res2 = repeatedString ("abcac", 10);
            Console.WriteLine ($"ans: {7}  res:{res}");
            Console.WriteLine ($"ans: {4}  res:{res2}");
            Console.WriteLine ($"ans: ?  res:{repeatedString ("a", (long)Math.Pow(10, 12))}");
        }
    }
}