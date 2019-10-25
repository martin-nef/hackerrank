using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers {
    public static class LargeTestHelper {
        public static void TestWithTimeout (Action test, int timeoutSeconds = 3) {
            var timer = Stopwatch.StartNew ();
            var testTask = Task.Run (test);
            testTask.Wait (TimeSpan.FromSeconds (timeoutSeconds));
            var elapsed = timer.Elapsed;
            if (testTask.IsCompletedSuccessfully) {
                return;
            } else if (testTask.Exception != null) {
                string error = "";
                if (elapsed.Seconds > 0) {
                    error = $"Test failed after {(int)Math.Floor ((double)elapsed.Seconds)} seconds {elapsed.Milliseconds}";
                } else {
                    error = $"Test failed after {Math.Round (elapsed.TotalMilliseconds)}";
                }
                throw new Exception (error, testTask.Exception);
            } else {
                throw new Exception ($"Failed to complete the test in {timeoutSeconds} seconds");
            }
        }

        public static void TestWithTimeout<TInput> (this IEnumerable<TInput> largeInput, Action<IEnumerable<TInput>> test, int timeoutSeconds = 3) {
            largeInput.TestWithTimeout (test: test, timeoutSeconds: timeoutSeconds);
        }
    }
}