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

using Plexdata.LogWriter.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plexdata.LogWriter.Queuing
{
    /// <summary>
    /// The default implementation of interface <see cref="IObservableQueue{TItem}"/>.
    /// </summary>
    /// <remarks>
    /// This class represents the default but thread-safe implementation of interface 
    /// <see cref="IObservableQueue{TItem}"/>.
    /// </remarks>
    /// <typeparam name="TItem">
    /// The type to be assigned with an instance of this queue.
    /// </typeparam>
    public class ObservableQueue<TItem> : IObservableQueue<TItem>
    {
        #region Public events

        /// <inheritdoc />
        public event EventHandler Enqueued;

        /// <inheritdoc />
        public event EventHandler Dequeued;

        #endregion

        #region  Private fields

        /// <summary>
        /// The number of items in the queue.
        /// </summary>
        /// <remarks>
        /// This field represents the number of items that are currently 
        /// available in the queue.
        /// </remarks>
        /// <seealso cref="ObservableQueue{TItem}.Count"/>
        private volatile Int32 count = 0;

        /// <summary>
        /// This field holds the instance of the internal synchronization 
        /// object.
        /// </summary>
        /// <remarks>
        /// This field represents the object to be used to lock an access 
        /// from different threads.
        /// </remarks>
        private readonly Object interlock = null;

        /// <summary>
        /// The typed queue of items.
        /// </summary>
        /// <remarks>
        /// This field represents the item queue for a particular 
        /// type.
        /// </remarks>
        private readonly Queue<TItem> queue = null;

        #endregion

        #region Construction

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls the extended constructor by providing 
        /// a default capacity of <c>16</c> items.
        /// </remarks>
        /// <seealso cref="ObservableQueue{TItem}.ObservableQueue(Int32)"/>
        public ObservableQueue()
            : this(16)
        {
        }

        /// <summary>
        /// The extended class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes all fields and properties with their 
        /// default values.
        /// </remarks>
        /// <param name="capacity">
        /// The initial queue capacity.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is throw if provided capacity is less than zero.
        /// </exception>
        public ObservableQueue(Int32 capacity)
            : base()
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            this.count = 0;
            this.interlock = new Object();
            this.queue = new Queue<TItem>(capacity);
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor cleans its resources.
        /// </remarks>
        ~ObservableQueue()
        {
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public Int32 Count
        {
            get
            {
                return this.count;
            }
        }

        /// <inheritdoc />
        public Boolean IsEmpty
        {
            get
            {
                return this.count == 0;
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void Enqueue(TItem item)
        {
            if (item != null)
            {
                lock (this.interlock)
                {
                    this.queue.Enqueue(item);
                    this.count = this.queue.Count;
                }

                this.RaiseEnqueued();
            }
        }

        /// <inheritdoc />
        public TItem Dequeue()
        {
            TItem result = default(TItem);

            if (!this.IsEmpty)
            {
                lock (this.interlock)
                {
                    result = this.queue.Dequeue();
                    this.count = this.queue.Count;
                }

                this.RaiseDequeued();
            }

            return result;
        }

        /// <inheritdoc />
        public TItem[] DequeueAll()
        {
            List<TItem> result = new List<TItem>();

            if (!this.IsEmpty)
            {
                lock (this.interlock)
                {
                    while (this.queue.Count > 0)
                    {
                        result.Add(this.queue.Dequeue());
                    }

                    this.count = this.queue.Count;
                }

                this.RaiseDequeued();
            }

            return result.ToArray();
        }

        /// <inheritdoc />
        public TItem Peek()
        {
            TItem result = default(TItem);

            if (!this.IsEmpty)
            {
                lock (this.interlock)
                {
                    result = this.queue.Peek();
                }
            }

            return result;
        }

        /// <inheritdoc />
        public void Clear()
        {
            lock (this.interlock)
            {
                this.queue.Clear();
                this.count = this.queue.Count;
            }
        }

        /// <inheritdoc />
        public void Trim()
        {
            lock (this.interlock)
            {
                this.queue.TrimExcess();
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Raises the event <see cref="ObservableQueue{TItem}.Enqueued"/>.
        /// </summary>
        /// <remarks>
        /// This method fires (and forgets) event <see cref="ObservableQueue{TItem}.Enqueued"/>. 
        /// But this event occurs in a different thread because of <see cref="Task.Run(Action)"/> 
        /// is used.
        /// </remarks>
        protected virtual void RaiseEnqueued()
        {
            Task.Run(() => { this.Enqueued?.Invoke(this, EventArgs.Empty); }).ConfigureAwait(false);
        }

        /// <summary>
        /// Raises the event <see cref="ObservableQueue{TItem}.Dequeued"/>.
        /// </summary>
        /// <remarks>
        /// This method fires (and forgets) event <see cref="ObservableQueue{TItem}.Dequeued"/>. 
        /// But this event occurs in a different thread because of <see cref="Task.Run(Action)"/> 
        /// is used.
        /// </remarks>
        protected virtual void RaiseDequeued()
        {
            Task.Run(() => { this.Dequeued?.Invoke(this, EventArgs.Empty); }).ConfigureAwait(false);
        }

        #endregion
    }
}

