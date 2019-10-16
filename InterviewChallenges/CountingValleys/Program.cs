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
using Helpers;

namespace CountingValleys {
    class Program {

        // Complete the countingValleys function below.
        static int countingValleys (int n, string s) {
            var valleys = 0;
            var seaLevel = 0;
            var currentHeight = 0;
            var inValley = false;
            var UP = 'U';
            var DOWN = 'D';

            // O(n)
            for (var i = 0; i < n; i++) {
                // O(1)
                char step = s[i];
                if (step == UP) {
                    currentHeight++;
                } else if (step == DOWN) {
                    currentHeight--;
                }
                if (currentHeight < seaLevel) {
                    if (!inValley){
                        valleys++;
                    }
                    inValley = true;
                } else if (currentHeight >= 0) {
                    inValley = false;
                }
            }
            return valleys;
        }

        static void Main (string[] args) {

            int? valleys = null;
            var timeSpent = Timer.TimeMs(() => valleys = countingValleys (8, "UDDDUDUU"));
            Console.WriteLine($"valleys {valleys}");
            Console.WriteLine($"timeSpent {timeSpent} ms");
        }
    }
}