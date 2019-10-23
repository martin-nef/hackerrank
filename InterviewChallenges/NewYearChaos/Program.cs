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
            var bribesMade = new Dictionary<int, int> (q.Length);
            var simulatedQ = Enumerable.Range (1, q.Length).ToArray ();

            for (int i = 0; i < q.Length; i++) {
                var originalPosition = q[i];
                var endPosition = i + 1;
                var currentPosition = simulatedQ[i];
                // ? How do we use current properly?
                var distanceMoved = originalPosition - endPosition;
                if (distanceMoved <= 0) {
                    continue;
                } else if (distanceMoved > maximumBribe) {
                    output = "Too chaotic";
                    break;
                }

                void move (int[] q, int from, int to) {
                    var temp = q[to];
                    q[to] = q[from];
                    q[from] = temp;
                }
                void increment (IDictionary<int, int> dict, int key) {
                    if (dict.ContainsKey (key))
                        dict[key] = 1;
                    else
                        dict[key]++;
                }
                var steps = Enumerable.Range (endPosition, distanceMoved).Reverse ().ToArray ();
                if (steps.Length < 2) {
                    continue;
                }
                foreach (var s in steps) {
                    // Simulate moves needed to move numbers the required distance, counting swaps
                    /// * Simulate moves needed to move numbers the required distance, counting swaps
                    increment(bribesMade, simulatedQ[s]);
                    move (simulatedQ, s, s + 1);
                }
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