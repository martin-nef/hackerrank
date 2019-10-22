using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace _2dArrayDs {
    class Program {

        // Complete the hourglassSum function below.
        static int hourglassSum (int[][] arr) {
            var sideOfHourglass = 3;
            var iLength = arr.Length;
            var jLength = arr[0].Length;
            if (iLength < sideOfHourglass || jLength < sideOfHourglass) {
                throw new ArgumentException ("Needs to at least be 3 by 3", nameof (arr));
            }
            var maxHourglassSum = 7 * 9; // maxHourglassSum = (hourglassNumberCount * maxHourglassNumberValue)
            int? currentMaxSum = null;
            var first = true;

            // move the center of the hourglass along the matrix, only so it fully fits
            for (int i = 1; i < iLength - 1; i++) {
                for (int j = 1; j < jLength - 1; j++) {
                    // sum of hourglass values
                    var hourglassSum = arr[i - 1][j - 1] + arr[i - 1][j] + arr[i - 1][j + 1] +
                                                           arr[i][j] +
                                       arr[i + 1][j - 1] + arr[i + 1][j] + arr[i + 1][j + 1];

                    if (hourglassSum == maxHourglassSum) {
                        // if we encounted the maximum possible value, return
                        return maxHourglassSum;
                    } else if (first || hourglassSum > currentMaxSum) {
                        // must set the current maximum on the first iteration,
                        if (first) {
                            first = false;
                        }
                        // or if the new one's bigger than the last
                        currentMaxSum = hourglassSum;
                    }
                }
            }

            return currentMaxSum.Value;
        }

        static void Main (string[] args) {
            var test1 = new int[][] {
                new int[] { 1, 1, 1, 0, 0, 0, },
                new int[] { 0, 1, 0, 0, 0, 0, },
                new int[] { 1, 1, 1, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, },
            };
            var test2 = new int[][] {
                new int[] {-9, -9, -9, 1, 1, 1, },
                new int[] { 0, -9, 0, 4, 3, 2, },
                new int[] {-9, -9, -9, 1, 2, 3, },
                new int[] { 0, 0, 8, 6, 6, 0, },
                new int[] { 0, 0, 0, -2, 0, 0, },
                new int[] { 0, 0, 1, 2, 4, 0, },
            };
            var test3 = new int[][] {
                new int[] { 1, 1, 1, 0, 0, 0, },
                new int[] { 0, 1, 0, 0, 0, 0, },
                new int[] { 1, 1, 1, 0, 0, 0, },
                new int[] { 0, 0, 2, 4, 4, 0, },
                new int[] { 0, 0, 0, 2, 0, 0, },
                new int[] { 0, 0, 1, 2, 4, 0, },
            };
            Console.WriteLine ("test 1\n" + PrintMatrix (test1));
            Console.WriteLine ($"ans: 7   res: {hourglassSum(test1)}\n");
            Console.WriteLine ("test 2\n" + PrintMatrix (test2));
            Console.WriteLine ($"ans: 28   res: {hourglassSum(test2)}\n");
            Console.WriteLine ("test 3\n" + PrintMatrix (test2));
            Console.WriteLine ($"ans: 19   res: {hourglassSum(test3)}\n");
        }

        private static string PrintMatrix (int[][] test1) {
            return string.Join ("\n", test1.Select ((int[] x) => string.Join ("", x.Select ((int y) => y < 0 ? $" {y}" : $"  {y}"))));
        }
    }
}