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

namespace SockMerchant {
    class Program {
        // Complete the sockMerchant function below.
        static int sockMerchant (int n, int[] ar) {
            // Using a dict for performance, losing memory in n
            var colours = new Dictionary<int, int> (capacity: ar.Length);
            var socks = ar;

            // O(n)
            foreach (var sock in socks) {
                // ContainsKey O(1)
                if (colours.ContainsKey (sock)) {
                    // Add O(1)
                    colours[sock] = colours[sock] + 1;
                } else {
                    // Add O(1)
                    colours[sock] = 1;
                }
            }

            // O(m) where m is the number of colours
            return colours.Select (x => (int) Math.Floor ((double) x.Value / 2)).Sum ();
        }
        // Total Time O(n + m) where m is the number of colours
        // We know  that m <= n, since there can be only so many colours as there are entries in ar
        // O(n + m) <= O(2n) = O(n)
        // So total complexity is O(n)

        static void Main (string[] args) {
            int n = "9".ToArray<int> ().First ();
            int[] ar = "10 20 20 10 10 30 50 10 20".ToArray<int> ();
            int result = sockMerchant (n, ar);
            Console.WriteLine (result);
        }
    }

    public static class TestHelper {
        static TestHelper () {
            // Test();
        }

        public static TNumber[] ToArray<TNumber> (this string numbers) where TNumber : new () {
            return numbers?.Split (new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select (x => {
                    if (!double.TryParse (x, out double y)) {
                        Console.WriteLine ($"Couldn't parse '{x}' to number");
                    }
                    return (TNumber) Convert.ChangeType (y, typeof (TNumber));
                })
                .ToArray ();
        }

        static void Test () {
            var ans = new [] {-1.5, 0, 10 };
            var res = "-1.5, 0, 10".ToArray<double> ();
            Console.WriteLine (string.Join (" ", res));
            Console.WriteLine (string.Join (" ", ans));
            Debug.Assert (res.ToList ().SequenceEqual (ans.ToList ()));
        }
    }
}