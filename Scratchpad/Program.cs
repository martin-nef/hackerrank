using System;
using System.Diagnostics;

namespace Scratchpad {
    public class Program {
        public void Foo () {
            Debugger.Break ();
            var c = 0;
            Console.WriteLine (c++);
            c = 0;
            Console.WriteLine (++c);
        }

        public static void Main (string[] args) {
            new Program ().Foo ();
        }
    }
}