/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using NUnit.Framework;
using Plexdata.LogWriter.Queuing;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;

namespace Plexdata.LogWriter.Abstraction.Tests.Queuing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(ObservableQueue<String>))]
    public class ObservableQueueTests
    {
        #region Prologue

        private ObservableQueue<String> instance = null;

        [SetUp]
        public void Setup()
        {
            this.instance = new ObservableQueue<String>();
        }

        #endregion

        #region Construction

        [Test]
        public void ObservableQueueExtended_CapacityLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => new ObservableQueue<String>(-1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ObservableQueueExtended_ValidateDefaultCapacity_DefaultCapacityIsSixteen()
        {
            Assert.That(this.GetPrivateArraySize(), Is.EqualTo(16));
        }

        #endregion

        #region Property tests

        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 1)]
        [TestCase(2, 0, 2)]
        [TestCase(1, 1, 0)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 0)]
        [TestCase(1, 2, 0)]
        [TestCase(2, 3, 0)]
        [TestCase(2, 4, 0)]
        public void Count_EnqueueAndDequeue_ResultAsExpected(Int32 enqueue, Int32 dequeue, Int32 expected)
        {
            for (Int32 index = 0; index < enqueue; index++)
            {
                this.instance.Enqueue($"Iten {index}");
            }

            for (Int32 index = 0; index < dequeue; index++)
            {
                this.instance.Dequeue();
            }

            Assert.That(this.instance.Count, Is.EqualTo(expected));
        }

        [TestCase(0, true)]
        [TestCase(1, false)]
        public void IsEmpty_EnqueueOnly_ResultAsExpected(Int32 enqueue, Boolean expected)
        {
            for (Int32 index = 0; index < enqueue; index++)
            {
                this.instance.Enqueue($"Iten {index}");
            }

            Assert.That(this.instance.IsEmpty, Is.EqualTo(expected));
        }

        #endregion

        #region Method tests

        [TestCase(0, null)]
        [TestCase(1, "item1")]
        [TestCase(3, "item1", null, "item2", null, "item3")]
        public void Enqueue_ItemsValidAndInvalid_ResultAsExpected(Int32 expected, params String[] items)
        {
            foreach (String item in items)
            {
                this.instance.Enqueue(item);
            }

            Assert.That(this.instance.Count, Is.EqualTo(expected));
        }

        [TestCase("item1", 1, "item1", "item2", "item3", "item4", "item5")]
        [TestCase("item2", 2, "item1", "item2", "item3", "item4", "item5")]
        [TestCase("item3", 3, "item1", "item2", "item3", "item4", "item5")]
        [TestCase("item4", 4, "item1", "item2", "item3", "item4", "item5")]
        [TestCase("item5", 5, "item1", "item2", "item3", "item4", "item5")]
        [TestCase(null, 6, "item1", "item2", "item3", "item4", "item5")]
        public void Dequeue_EnqueueAndDequeue_LastDequeuedAsExpected(String expected, Int32 dequeue, params String[] items)
        {
            foreach (String item in items)
            {
                this.instance.Enqueue(item);
            }

            String actual = null;

            for (Int32 index = 0; index < dequeue; index++)
            {
                actual = this.instance.Dequeue();
            }

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("item1", 0, "item1")]
        [TestCase("item1", 1, null)]
        [TestCase("item1,item2", 0, "item1")]
        [TestCase("item1,item2", 1, "item2")]
        [TestCase("item1,item2", 2, null)]
        public void Peek_EnqueueAndDequeue_ResultAsExpected(String items, Int32 dequeue, String expected)
        {
            foreach (String item in items.Split(','))
            {
                this.instance.Enqueue(item);
            }

            for (Int32 index = 0; index < dequeue; index++)
            {
                this.instance.Dequeue();
            }

            Assert.That(this.instance.Peek(), Is.EqualTo(expected));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Clear_EnqueueAndFree_ResultIsEmpty(Int32 enqueue)
        {
            for (Int32 index = 0; index < enqueue; index++)
            {
                this.instance.Enqueue($"Iten {index}");
            }

            this.instance.Clear();

            Assert.That(this.instance.IsEmpty, Is.True);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(5, 5)]
        [TestCase(6, 6)]
        [TestCase(7, 7)]
        [TestCase(8, 8)]
        [TestCase(9, 9)]
        [TestCase(10, 10)]
        [TestCase(11, 11)]
        [TestCase(12, 12)]
        [TestCase(13, 13)]
        [TestCase(14, 16)]
        [TestCase(15, 16)]
        [TestCase(16, 16)]
        [TestCase(17, 17)]
        [TestCase(18, 18)]
        [TestCase(19, 19)]
        [TestCase(20, 20)]
        public void Trim_EnqueueAndTrim_ArraySizeAsExpected(Int32 enqueue, Int32 expected)
        {
            for (Int32 index = 0; index < enqueue; index++)
            {
                this.instance.Enqueue($"Iten {index}");
            }

            this.instance.Trim();

            // This is actually an integration test.
            Assert.That(this.GetPrivateArraySize(), Is.EqualTo(expected));
        }

        [TestCase("item1", "item2", "item3", "item4", "item5")]
        public void Enqueue_RaiseEnqueuedEvent_EventFiredForEachItem(params String[] items)
        {
            Int32 fired = 0;

            this.instance.Enqueued += (sender, args) => { fired++; };

            foreach (String item in items)
            {
                this.instance.Enqueue(item);
            }

            Thread.Sleep(100); // Increase waiting time if test fails.

            Assert.That(fired, Is.EqualTo(items.Length));
        }

        [TestCase("item1", "item2", "item3", "item4", "item5")]
        public void Dequeue_RaiseDequeuedEvent_EventFiredForEachItem(params String[] items)
        {
            Int32 fired = 0;

            this.instance.Dequeued += (sender, args) => { fired++; };

            foreach (String item in items)
            {
                this.instance.Enqueue(item);
            }

            while (!this.instance.IsEmpty)
            {
                this.instance.Dequeue();
            }

            Thread.Sleep(100); // Increase waiting time if test fails.

            Assert.That(fired, Is.EqualTo(items.Length));
        }

        [TestCase("item1")]
        [TestCase("item1", "item2", "item3")]
        [TestCase("item1", "item2", "item3", "item4", "item5")]
        public void DequeueAll_RaiseDequeuedEvent_EventFiredOnce(params String[] items)
        {
            Int32 fired = 0;
            Int32 count = 0;

            this.instance.Dequeued += (sender, args) => { fired++; };

            foreach (String item in items)
            {
                this.instance.Enqueue(item);
            }

            while (!this.instance.IsEmpty)
            {
                count += this.instance.DequeueAll().Length;
            }

            Thread.Sleep(100); // Increase waiting time if test fails.

            Assert.That(fired, Is.EqualTo(1));
            Assert.That(count, Is.EqualTo(items.Length));
        }

        #endregion

        #region Private helper methods

        private Int32 GetPrivateArraySize()
        {
            FieldInfo info = this.instance.GetType().GetField("queue", BindingFlags.NonPublic | BindingFlags.Instance);

            if (info != null)
            {
                var queue = info.GetValue(this.instance);

                info = queue.GetType().GetField("_array", BindingFlags.NonPublic | BindingFlags.Instance);

                if (info != null && info.GetValue(queue) is String[] items)
                {
                    return items.Length;
                }
            }

            return -1;
        }

        #endregion
    }
}
