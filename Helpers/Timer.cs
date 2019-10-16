using System;

namespace Helpers {
    public static class Timer {
        public static TimeSpan Time (Action action) {
            var watch = System.Diagnostics.Stopwatch.StartNew ();
            action?.Invoke ();
            watch.Stop ();
            return watch.Elapsed;
        }
        public static long TimeMs (Action action) {
            var watch = System.Diagnostics.Stopwatch.StartNew ();
            action?.Invoke ();
            watch.Stop ();
            return watch.ElapsedMilliseconds;
        }
    }
}