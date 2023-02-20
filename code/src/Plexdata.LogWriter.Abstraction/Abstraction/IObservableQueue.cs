/*
 * MIT License
 * 
 * Copyright (c) 2023 plexdata.de
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

using System;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents an observable queue.
    /// </summary>
    /// <remarks>
    /// Classes derived from this interface may handle an access to the queued 
    /// items in different ways.
    /// </remarks>
    /// <typeparam name="TItem">
    /// The type to be assigned with an instance of the queue.
    /// </typeparam>
    public interface IObservableQueue<TItem>
    {
        /// <summary>
        /// Informs about item enqueuing.
        /// </summary>
        /// <remarks>
        /// This event occurs as soon as a new item has been successfully enqueued.
        /// </remarks>
        /// <seealso cref="IObservableQueue{TItem}.Enqueue"/>
        event EventHandler Enqueued;

        /// <summary>
        /// Informs about item dequeuing.
        /// </summary>
        /// <remarks>
        /// This event occurs as soon as an existing item has been successfully dequeued.
        /// </remarks>
        /// <seealso cref="IObservableQueue{TItem}.Dequeue"/>
        event EventHandler Dequeued;

        /// <summary>
        /// Gets the number of items currently in the queue.
        /// </summary>
        /// <remarks>
        /// This property determines the number of items in the queue and returns it.
        /// </remarks>
        /// <value>
        /// The number of currently queued items.
        /// </value>
        Int32 Count { get; }

        /// <summary>
        /// Determines if the queue is currently empty.
        /// </summary>
        /// <remarks>
        /// This property determines if there are items in the queue.
        /// </remarks>
        /// <value>
        /// True if the queue contains at least one item and false otherwise.
        /// </value>
        Boolean IsEmpty { get; }

        /// <summary>
        /// Adds provided item at the end of the queue.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method puts provided item into the queue and if successful it 
        /// raises the event <see cref="IObservableQueue{TItem}.Enqueued"/>.
        /// </para>
        /// <para>
        /// Be aware, nothing is enqueued if provided item is <c>null</c>.
        /// </para>
        /// </remarks>
        /// <param name="item">
        /// </param>
        /// <seealso cref="IObservableQueue{TItem}.Enqueued"/>
        void Enqueue(TItem item);

        /// <summary>
        /// Removes an item from the top of the queue and returns it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method removes an item from the queue and if successful it 
        /// raises the event <see cref="IObservableQueue{TItem}.Dequeued"/>. 
        /// Thereafter the removed item is returned.
        /// </para>
        /// <para>
        /// The <c>default</c> value of type <typeparamref name="TItem"/> is 
        /// returned if the queue is empty.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The item that has been removed from the queue.
        /// </returns>
        /// <seealso cref="IObservableQueue{TItem}.Peek"/>
        /// <seealso cref="IObservableQueue{TItem}.IsEmpty"/>
        /// <seealso cref="IObservableQueue{TItem}.DequeueAll()"/>
        TItem Dequeue();

        /// <summary>
        /// Removes all items from the queue and returns them.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method removes all items from the queue and if successful it 
        /// raises the event <see cref="IObservableQueue{TItem}.Dequeued"/>. 
        /// Thereafter the list of removed items is returned.
        /// </para>
        /// <para>
        /// An empty list is returned if the queue is empty.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The list of items that have been removed from the queue.
        /// </returns>
        /// <seealso cref="IObservableQueue{TItem}.Peek"/>
        /// <seealso cref="IObservableQueue{TItem}.IsEmpty"/>
        /// <seealso cref="IObservableQueue{TItem}.Dequeue()"/>
        TItem[] DequeueAll();

        /// <summary>
        /// Returns the item at the beginning of the queue but without removing it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method peeks the top item of the queue and returns it. 
        /// </para>
        /// <para>
        /// The <c>default</c> value of type <typeparamref name="TItem"/> is returned 
        /// if the queue is empty.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The top item of the queue.
        /// </returns>
        /// <seealso cref="IObservableQueue{TItem}.Dequeue"/>
        /// <seealso cref="IObservableQueue{TItem}.IsEmpty"/>
        TItem Peek();

        /// <summary>
        /// Removes all items from the queue.
        /// </summary>
        /// <remarks>
        /// This method removes all existing items from the queue.
        /// </remarks>
        void Clear();

        /// <summary>
        /// Sets the capacity to the actual number of queued items.
        /// </summary>
        /// <remarks>
        /// This method sets the capacity to the actual number of items in the 
        /// queue if that number is less than 90 percent of current capacity.
        /// </remarks>
        void Trim();
    }
}
