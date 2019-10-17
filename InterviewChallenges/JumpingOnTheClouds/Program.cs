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

namespace JumpingOnTheClouds {
    class Program {

        // Complete the jumpingOnClouds function below.
        static int jumpingOnClouds (int[] c) {
            var jumps = 0;
            // O(n), since at worst, we're going through clouds once, one by one
            for (var cloud = 0; cloud < c.Length; cloud++) {
                // Handle the scenario at the end
                var isOnLastCloud = cloud + 1 == c.Length;
                var isOneMoreJump = cloud + 2 == c.Length;
                if (isOnLastCloud) {
                    return jumps;
                } else if (isOneMoreJump) {
                    Console.WriteLine ($"jumping from {cloud} to {cloud + 1}");
                    return jumps + 1;
                }

                // Try to jump over one cloud if it's safe
                var isLongJumpSafe = c[cloud + 2] == 0;
                if (isLongJumpSafe)
                    cloud++;

                // The default action is to jump to the next cloud
                jumps++;
            }
                
            return jumps;
        }

            static void Main (string[] args) {
                foreach (var input in new [] {
                        new Tuple<int[], int> (new [] { 0, 0, 1, 0, 0, 1, 0 }, 4),
                            new Tuple<int[], int> (new [] { 0, 0, 0, 0, 1, 0 }, 3)
                    }) {
                    Console.WriteLine ("res: " + jumpingOnClouds (input.Item1));
                    Console.WriteLine ("ans: " + input.Item2);
                    Console.WriteLine ();
                }
            }
        }
    }