using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace StockMerchant
{
    class Program
    {
        // Complete the sockMerchant function below.
        static int sockMerchant(int n, int[] ar)
        {
            var colours = new Dictionary<int, int>();
            var socks = ar;

            foreach (var sock in socks)
            {
                if (colours.ContainsKey(sock))
                {
                    colours[sock] = colours[sock] + 1;
                }
                else
                {
                    colours[sock] = 1;
                }
            }
            return colours.Select(x => (int)Math.Floor((double)x.Value/2)).Sum();
        }

        static void Main(string[] args)
        {
            int n = "9".ToArray<int>().First();
            int[] ar = "10 20 20 10 10 30 50 10 20".ToArray<int>();
            int result = sockMerchant(n, ar);
            Console.WriteLine(result);
        }
    }
    
    public static class TestHelper
    {
        static TestHelper()
        {
            // Test();
        }

        public static TNumber[] ToArray<TNumber>(this string numbers) where TNumber: new()
        {
            return numbers?.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => 
                {
                    if (!double.TryParse(x, out double y)){
                        Console.WriteLine($"Couldn't parse '{x}' to number");
                    } 
                    return (TNumber)Convert.ChangeType(y, typeof(TNumber));
                })
                .ToArray();
        }

        static void Test()
        {
            var ans = new [] { -1.5, 0, 10 };
            var res = "-1.5, 0, 10".ToArray<double>();
            Console.WriteLine(string.Join(" ", res));
            Console.WriteLine(string.Join(" ", ans));
            Debug.Assert(res.ToList().SequenceEqual(ans.ToList()));
        }
    }
}
