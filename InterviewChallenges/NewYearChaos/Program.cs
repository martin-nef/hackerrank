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

        ///
        /// pass 1 - record distances
        /// pass 2 - simulate moving forward, record those that moved back
        /// pass 3 - find where the distance moved back doesn't equal end position
        ///          calculate new distance for them (should be positive)
        
        public static string minimumBribes (int[] q) {
            var output = default (string);
            var maximumBribe = 2;
            var bribesMade = new Dictionary<int, int> (q.Length);
            var simulatedQ = Enumerable.Range (1, q.Length).ToArray ();

            for (int i = q.Length - 1; i >= 0; i--) {
                var endPosition = q[i];
                var originalPosition = i + 1; // +1, because arrays in C# start from index 0 

                var distanceMoved = originalPosition - endPosition;
                if (distanceMoved <= 0) {
                    continue;
                } else if (distanceMoved > maximumBribe) {
                    output = "Too chaotic";
                    break;
                }

                var moving = $"from {originalPosition} to {endPosition}";
                // This is in O(1), since distance moved cannot exceed 2, otherwise the q is too chaotic
                for (int currentPosition = originalPosition; currentPosition > endPosition; currentPosition--) {
                    Assert.InRange (currentPosition, 0, q.Length);
                    countBribes (bribesMade, personBribing : originalPosition);
                    var swapping = $"from {currentPosition - 1} to {currentPosition - 2}";
                    swap (inList: simulatedQ, from: currentPosition - 1, to: currentPosition - 2);
                    Assert.Equal (simulatedQ[currentPosition - 2], originalPosition); // Ensure we're moving the same person
                }
            }

            output = $"{bribesMade.Values.Sum()}";
            Console.WriteLine (output);
            Assert.Equal (q, simulatedQ); // Ensure we have arrived at the given queue with our simulation
            return output;
        }

        static void swap (int[] inList, int from, int to) {
            var temp = inList[to];
            inList[to] = inList[from];
            inList[from] = temp;
        }
        static void countBribes (IDictionary<int, int> dict, int personBribing) {
            if (dict.ContainsKey (personBribing))
                dict[personBribing]++;
            else
                dict[personBribing] = 1;
        }

        public class ProgramTest {

            /// <summary>
            /// Minimum bribes for the q to go from "1 2 3 4 5" to a given state.
            /// Each person can only bribe once
            /// </summary>
            [Fact]
            public void TestMinimumBribes () {
                var test1 =
                    "3 1 2 - 2\n" +
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