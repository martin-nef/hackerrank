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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NewYearChaos {
    public static class Program {
        public static string minimumBribes (int[] q) {
            var output = default (string);
            var maximumBribe = 2;
            var totalDistanceMoved = 0;

            // Calculate, how far each person has moved.
            for (int i = 0; i < q.Length; i++) {
                var originalPosition = q[i];
                var currentPosition = i + 1;

                // Calculate how many bribes it would take to move froward from original to current
                var distanceMoved = originalPosition - currentPosition;
                // A person can accept as many bribes as they like, so distance can be negative, yet not too chaotic
                #error Calculating bribes from distance does not account for case when bribe is made after accepting any
                if (distanceMoved > maximumBribe) {
                    output = "Too chaotic";
                    break;
                }
                if (distanceMoved > 0) {
                    totalDistanceMoved += distanceMoved;
                }
            }
            if (output == null) {
                output = $"{totalDistanceMoved}";
            }
            return output;
        }

        public class ProgramTest {

            /// <summary>
            /// Minimum bribes for the q to go from "1 2 3 4 5" to a given state.
            /// Each person can only bribe once
            /// </summary>
            [Fact]
            public void TestMinimumBribes () {
                var test1 =
                    "2 5 1 3 4 - Too chaotic\n" +
                    "1 2 3 4 5 6 - 0\n" +
                    "2 1 5 3 4 - 3\n" +
                    "5 1 2 3 7 8 6 4 - Too chaotic\n" +
                    "1 2 5 3 7 8 6 4 - 7\n";
                var testNumber = 0;
                foreach (var test in StringToTestData (test1)) {
                    testNumber++;
                    var output = Program.minimumBribes (test.Input);
                    if (test.Expected != output) {
                        throw new Exception ($"Test {testNumber} failed:\nInput {string.Join (" ", test.Input)}\nExpected {test.Expected}\nOutput {output}\n");
                    }
                }
            }

            static IEnumerable<TestData> StringToTestData (string numberString) {
                return numberString.Split ('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select (x => x.Split ('-', StringSplitOptions.RemoveEmptyEntries)
                        .Select (y => y.Trim ())
                        .ToArray ())
                    .Select (x => new TestData { Input = ToArray<int> (x[0]), Expected = x[1] });
            }

            class TestData {
                public int[] Input { get; set; }
                public string Expected { get; set; }
            }

            static TNumber[] ToArray<TNumber> (string numbers) where TNumber : new () {
                return numbers?.Split (new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select (x => {
                        if (!double.TryParse (x, out double y)) {
                            throw new Exception ($"Couldn't parse '{x}' to number");
                        }
                        return (TNumber) Convert.ChangeType (y, typeof (TNumber));
                    })
                    .ToArray ();
            }
        }
    }
}