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

namespace _2dArrayDs {
    class Program {

        // Complete the hourglassSum function below.
        static int hourglassSum (int[][] arr) {
            var sideOfHourglass = 3;
            var iLength = arr.Length;
            var jLength = arr[0].Length;
            if (iLength < sideOfHourglass || jLength < sideOfHourglass) {
                return 0;
            }
            var maxHourglassSum = 7 * 9; // maxHourglassSum = (hourglassNumberCount * maxHourglassNumberValue)
            var currentMaxSum = 0;
            var iMax = 0;
            var jMax = 0;
            // move the center of the hourglass along the matrix, only so it fully fits
            for (int i = 1; i < iLength - 1; i++) {
                for (int j = 1; j < jLength - 1; j++) {

                    // sum, calculated starting from the hourglasses center arr[i][j]
                    var hourglasSum = arr[i - 1][j - 1] + arr[i - 1][j] + arr[i - 1][j + 1] +
                        arr[i][j - 1] + arr[i][j] + arr[i][j + 1] +
                        arr[i + 1][j - 1] + arr[i + 1][j] + arr[i + 1][j + 1];

                    if (hourglasSum == maxHourglassSum) {
                        return maxHourglassSum;
                    } else if (hourglasSum > currentMaxSum) {
                        currentMaxSum = hourglasSum;
                        jMax = j;
                        iMax = i;
                    }
                }
            }
            Console.WriteLine ($"max hourglass {currentMaxSum}");
            Console.WriteLine ($"{arr[iMax - 1][jMax - 1]} {arr[iMax - 1][jMax]} {arr[iMax - 1][jMax + 1]}\n"+
                            $"  {arr[iMax][jMax]} \n"+
                            $"{arr[iMax + 1][jMax - 1]} {arr[iMax + 1][jMax]} {arr[iMax + 1][jMax + 1]}"
                        );
            return currentMaxSum;
        }

        static void Main (string[] args) {
            // var test1 = new int[][] {
            //     new int[] { 1, 1, 1, 0, 0, 0 },
            //     new int[] { 0, 1, 0, 0, 0, 0 },
            //     new int[] { 1, 1, 1, 0, 0, 0 },
            //     new int[] { 0, 0, 2, 4, 4, 0 },
            //     new int[] { 0, 0, 0, 2, 0, 0 },
            //     new int[] { 0, 0, 1, 2, 4, 0 },
            // };
            var test1 = new int[][] {
                new int[] { 1, 1, 1, 0, 0, 0, },
                new int[] { 0, 1, 0, 0, 0, 0, },
                new int[] { 1, 1, 1, 0, 0, 0, },
                new int[] { 0, 9, 2, -4, -4, 0, },
                new int[] { 0, 0, 0, -2, 0, 0, },
                new int[] { 0, 0, -1, -2, -4, 0, },
            };
            Console.WriteLine (string.Join ("\n", test1.Select (x => string.Join (" ", x))));
            Console.WriteLine ($"ans: 19   res: {hourglassSum(test1)}");
        }
    }
}