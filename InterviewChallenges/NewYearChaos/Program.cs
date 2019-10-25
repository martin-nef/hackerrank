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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NewYearChaos {
    public class Person {
        const int MAX_BRIBES = 2;
        public int OriginalPosition { get; }
        public int Position { get; set; }
        int bribes = 0;
        public int Bribes {
            get => bribes;
            private set {
                bribes = value;
                if (bribes > MAX_BRIBES)
                    throw new Exception ("Too chaotic");
            }
        }
        public bool Sorted => Position == OriginalPosition;
        public int DistanceToOrigin => OriginalPosition - Position;
        public void Bribed (Person p) => Bribes++;
        public bool WasInFrontOf (Person p) {
            return OriginalPosition < p.OriginalPosition;
        }
        public bool WasBehind (Person p) {
            return OriginalPosition > p.OriginalPosition;
        }
        public static implicit operator int (Person p) => p.OriginalPosition;

        #region Under-the-hood code
        public Person (int original, int current) {
            OriginalPosition = original;
            Position = current;
        }

        public override string ToString () => ((int) this).ToString ();
        public override bool Equals (object obj) => int.Equals ((int) this, obj);
        public override int GetHashCode () => (int) this;
        #endregion

    }

    public class Queue : IList<Person> {
        public int Bribes => this.Sum (x => x.Bribes);
        public void UnBribe (Person personAhead, Person personBehind) {
            personAhead.Bribed (personBehind);
            var from = personBehind.Position;
            var to = personAhead.Position;
            var temp = this [to];
            this [to] = this [from];
            this [from] = temp;
        }

        public int UnsortedLength => Count - SortedEndLength;
        public int SortedEndLength { get; set; } = 0;
        public void CalulateUnsortedLength () {
            var sortedEndLength = 0;
            for (int i = this.Count - 1; i > 0; i--) {
                var person = this [i];
                if (person.Sorted) {
                    sortedEndLength++;
                } else {
                    break;
                }
            }
            SortedEndLength = sortedEndLength;
        }

        static IEnumerable<Person> Create (IEnumerable<int> q) {
            var count = 0;
            return q.Select (x => new Person (original: ++count, current : x));
        }

        #region Under-the-hood code
        public static implicit operator Queue (int[] queue) => new Queue (queue);
        public static implicit operator Queue (List<int> queue) => new Queue (queue);
        private static IEnumerable<Person> ZeroList => new List<Person> { default };
        public Queue (IEnumerable<Person> q) => _Queue = ZeroList.Concat (q).ToList ();
        public Queue (IEnumerable<int> q) : this (Create (q)) { }
        public Queue (int length = 4) : this (Enumerable.Range (1, length)) { }

        #region IList implementation
        public int IndexOf (Person item) {
            return ((IList<Person>) _Queue).IndexOf (item);
        }

        public void Insert (int index, Person item) {
            ((IList<Person>) _Queue).Insert (index, item);
        }

        public void RemoveAt (int index) {
            ((IList<Person>) _Queue).RemoveAt (index);
        }

        public void Add (Person item) {
            ((IList<Person>) _Queue).Add (item);
        }

        public void Clear () {
            ((IList<Person>) _Queue).Clear ();
        }

        public bool Contains (Person item) {
            return ((IList<Person>) _Queue).Contains (item);
        }

        public void CopyTo (Person[] array, int arrayIndex) {
            ((IList<Person>) _Queue).CopyTo (array, arrayIndex);
        }

        public bool Remove (Person item) {
            return ((IList<Person>) _Queue).Remove (item);
        }

        public IEnumerator<Person> GetEnumerator () {
            return ((IList<Person>) _Queue).GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator () {
            return ((IList<Person>) _Queue).GetEnumerator ();
        }

        public IList<Person> _Queue { get; set; }

        public int Count => ((IList<Person>) _Queue).Count;

        public bool IsReadOnly => ((IList<Person>) _Queue).IsReadOnly;

        public Person this [int index] { get => ((IList<Person>) _Queue) [index]; set => ((IList<Person>) _Queue) [index] = value; }
        #endregion

        public override bool Equals (object obj) {
            if (obj == null || obj.GetType () != typeof (Queue)) {
                return base.Equals (obj);
            } else {
                return this.SequenceEqual ((Queue) obj);
            }
        }

        public override int GetHashCode () {
            return this.Sum (x => x.Position);
        }
        #endregion

    }

    public static class Program {

        ///
        /// pass 1 - record distances
        /// pass 2 - simulate moving forward, record those that moved back
        /// pass 3 - find where the distance moved back doesn't equal end position
        ///          calculate new distance for them (should be positive)
        public static string minimumBribes (int[] q) {
            var queue = new Queue (q);
            queue.CalulateUnsortedLength ();
            var personToSort = queue.UnsortedLength;
            while (queue.SortedEndLength != queue.Count) {
                for (int i = 1; i < queue.UnsortedLength; i++) {
                    var person = queue[i];
                    var nextPerson = queue[person.Position + 1];
                    if (person == personToSort && person.WasBehind (nextPerson)) {
                        queue.UnBribe (person, nextPerson);
                    }
                }
                queue.CalulateUnsortedLength ();
            }
            return $"{queue.Bribes}";
        }
    }

    public class ProgramTest {
        #region Test code
        [Fact]
        public void TestMinimumBribesL () {
            int[] largeTest = Enumerable.Range (1, 1000).ToArray ();
            bribe (largeTest, 13);
            bribe (largeTest, 321);
            bribe (largeTest, 444);
            bribe (largeTest, 666);
            bribe (largeTest, 667);
            bribe (largeTest, 665);
            bribe (largeTest, 664);
            var timeout = TimeSpan.FromMilliseconds (largeTest.Length * largeTest.Length * 0.001);
            largeTest.TestWithTimeout (x => Assert.Equal ("7", Program.minimumBribes ((int[]) x)), timeout);
        }

        [Fact]
        public void TestMinimumBribesXL () {
            int[] largeTest = Enumerable.Range (1, 100000).ToArray ();
            bribe (largeTest, 13);
            bribe (largeTest, 3321);
            bribe (largeTest, 4444);
            bribe (largeTest, 6666);
            bribe (largeTest, 6667);
            bribe (largeTest, 6665);
            bribe (largeTest, 6664);
            var timeout = TimeSpan.FromMilliseconds (largeTest.Length * Math.Log2(largeTest.Length) * 0.001);
            largeTest.TestWithTimeout (x => Assert.Equal ("7", Program.minimumBribes ((int[]) x)), timeout);
        }

        /// <summary>
        /// Minimum bribes for the q to go from "1 2 3 4 5" to a given state.
        /// Each person can only bribe once
        /// </summary>
        [Fact]
        public void TestMinimumBribes () {
            var test1 =
                "3 1 2 - 2\n" +
                "2 5 1 3 4 - Too chaotic\n" +
                "1 2 3 4 5 6 - 0\n" +
                "2 1 5 3 4 - 3\n" +
                "5 1 2 3 7 8 6 4 - Too chaotic\n" +
                "1 2 5 3 7 8 6 4 - 7\n";
            var testNumber = 0;
            foreach (var test in StringToTestData (test1)) {
                testNumber++;
                var output = Program.minimumBribes (test.Input);
                if (test.Expected != output) {
                    throw new Exception ($"Test {testNumber} failed:\nInput {string.Join (" ", test.Input)}\nExpected {test.Expected}\nOutput {output}\n");
                }
            }
        }
        #endregion

        #region Test helpers

        static void bribe (IList<int> inList, int briberPosition) {
            int to = briberPosition - 1;
            int from = briberPosition;
            var temp = inList[to];
            inList[to] = inList[from];
            inList[from] = temp;
        }

        public class TestData {
            public int[] Input { get; set; }
            public string Expected { get; set; }
        }

        public static IEnumerable<TestData> StringToTestData (string numberString) {
            return numberString.Split ('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select (x => x.Split ('-', StringSplitOptions.RemoveEmptyEntries)
                    .Select (y => y.Trim ())
                    .ToArray ())
                .Select (x => new TestData { Input = ToArray<int> (x[0]), Expected = x[1] });
        }

        public static TNumber[] ToArray<TNumber> (string numbers) where TNumber : new () {
            return numbers?.Split (new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select (x => {
                    if (!double.TryParse (x, out double y)) {
                        throw new Exception ($"Couldn't parse '{x}' to number");
                    }
                    return (TNumber) Convert.ChangeType (y, typeof (TNumber));
                })
                .ToArray ();
        }
        #endregion
    }

    #region Large input test helpers
    public static class LargeTestHelper {
        public static void TestWithTimeout (Action test, TimeSpan timeout) {
            var start = DateTime.UtcNow;
            var testTask = Task.Run (test);
            testTask.Wait (timeout);
            var elapsed = DateTime.UtcNow - start;
            if (testTask.IsCompletedSuccessfully) {
                return;
            } else if (testTask.Exception == null) {
                string error = "";
                if (elapsed.TotalSeconds > 0) {
                    error = $"Test failed after {(int)Math.Floor ((double)elapsed.Seconds)} seconds {elapsed.Milliseconds}";
                } else {
                    error = $"Test failed after {Math.Round (elapsed.TotalMilliseconds)}";
                }
                throw new Exception (error, testTask.Exception);
            } else {
                throw new Exception ($"Failed to complete the test in {timeout.TotalSeconds} seconds");
            }
        }

        public static void TestWithTimeout<TInput> (this IEnumerable<TInput> largeInput, Action<IEnumerable<TInput>> test, TimeSpan timeout) {
            TestWithTimeout (test: () => test (largeInput), timeout : timeout);
        }
    }
    #endregion
}