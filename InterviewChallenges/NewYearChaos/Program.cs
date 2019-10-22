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
    public class Program {
        static readonly TextWriter console = new StringWriter ();
        static void Log (string message) {
            console.WriteLine (message);
        }

        // Complete the minimumBribes function below.
        static void minimumBribes (int[] q) {
            var isValidQ = true;
            if (!isValidQ) {
                Log ("Too chaotic");
                return;
            }

            var minBribes = 0;

            // TODO: calculate minBribes

            Log ($"{minBribes}");
        }

        /// <summary>
        /// Minimum bribes for the q to go from "1 2 3 4 5" to a given state.
        /// Each person can only bribe once
        /// </summary>
        [Fact]
        public void TestMinimumBribes () {
            var test1 = 
                        "2 5 1 3 4 - Too chaotic" +
                        "\n" + 
                        "2 1 5 3 4 - 3"
                        ;
            var testNumber = 1;
            foreach (var test in StringToTestData (test1)) {
                minimumBribes (test.Input);
                string output = console.ToString ().Split ("\n", StringSplitOptions.RemoveEmptyEntries).Last ().Trim ();
                if (test.Expected != output) {
                    throw new Exception ($"Test {testNumber++} failed:\nInput {string.Join (" ", test.Input)}\nExpected {test.Expected}\nOutput {output}\n");
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