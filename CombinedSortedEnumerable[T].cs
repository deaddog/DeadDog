using System;
using System.Collections.Generic;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Supports enumeration over a collections of <see cref="IEnumerable{T}" />.
    /// Note that these must all adhere to the same sorting.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public class CombinedSortedEnumerable<T> : IEnumerable<T>
    {
        private List<IEnumerable<T>> loaders;
        private Comparison<T> comparison;

        private CombinedSortedEnumerable(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CombinedSortedEnumerable{T}"/> class which allows for iteration of multiple enumerables at once.
        /// </summary>
        /// <param name="comparison">A method for comparing two instances of <typeparamref name="T"/>, which in turn is used for sorting the enumerables.</param>
        /// <param name="loaders">A collection of enumerables. These must all adhere to the sorting defined by <paramref name="comparison"/>.</param>
        public CombinedSortedEnumerable(Comparison<T> comparison, params IEnumerable<T>[] loaders)
            : this(comparison)
        {
            this.loaders = new List<IEnumerable<T>>(loaders);
        }

        /// <summary>
        /// Adds an additional enumerable to a <see cref="CombinedSortedEnumerable{T}"/>.
        /// </summary>
        /// <param name="combinedLoader">The <see cref="CombinedSortedEnumerable{T}"/> to which the enumerable is added.</param>
        /// <param name="loader">The <see cref="IEnumerable{T}"/> that is added.</param>
        /// <returns>A new <see cref="CombinedSortedEnumerable{T}"/> containing the full collection of enumerables.</returns>
        public static CombinedSortedEnumerable<T> Add(CombinedSortedEnumerable<T> combinedLoader, IEnumerable<T> loader)
        {
            if (loader is CombinedSortedEnumerable<T>)
                return combinedLoader + (loader as CombinedSortedEnumerable<T>);

            CombinedSortedEnumerable<T> cse = new CombinedSortedEnumerable<T>(combinedLoader.comparison, loader);
            for (int i = 0; i < combinedLoader.loaders.Count; i++)
                cse.loaders.Insert(i, combinedLoader.loaders[i]);
            return cse;
        }

        /// <summary>
        /// Joins two <see cref="CombinedSortedEnumerable{T}"/> into one.
        /// </summary>
        /// <param name="a">The first <see cref="CombinedSortedEnumerable{T}"/> to join.</param>
        /// <param name="b">The second <see cref="CombinedSortedEnumerable{T}"/> to join.</param>
        /// <returns>A new <see cref="CombinedSortedEnumerable{T}"/> containing the full collection of enumerables.</returns>
        public static CombinedSortedEnumerable<T> operator +(CombinedSortedEnumerable<T> a, CombinedSortedEnumerable<T> b)
        {
            CombinedSortedEnumerable<T> cse = new CombinedSortedEnumerable<T>(a.comparison);
            cse.loaders = new List<IEnumerable<T>>();
            cse.loaders.AddRange(a.loaders);
            cse.loaders.AddRange(b.loaders);
            return cse;
        }

        /// <summary>
        /// Joins an <see cref="IEnumerable{T}"/> into an existing <see cref="CombinedSortedEnumerable{T}"/>.
        /// </summary>
        /// <param name="a">The <see cref="CombinedSortedEnumerable{T}"/>.</param>
        /// <param name="b">The <see cref="IEnumerable{T}"/>.</param>
        /// <returns>A new <see cref="CombinedSortedEnumerable{T}"/> containing the full collection of enumerables.</returns>
        public static CombinedSortedEnumerable<T> operator +(CombinedSortedEnumerable<T> a, IEnumerable<T> b)
        {
            CombinedSortedEnumerable<T> cse = new CombinedSortedEnumerable<T>(a.comparison);
            cse.loaders = new List<IEnumerable<T>>();
            cse.loaders.AddRange(a.loaders);
            cse.loaders.Add(b);
            return cse;
        }
        /// <summary>
        /// Joins an <see cref="IEnumerable{T}"/> into an existing <see cref="CombinedSortedEnumerable{T}"/>.
        /// </summary>
        /// <param name="a">The <see cref="IEnumerable{T}"/>.</param>
        /// <param name="b">The <see cref="CombinedSortedEnumerable{T}"/>.</param>
        /// <returns>A new <see cref="CombinedSortedEnumerable{T}"/> containing the full collection of enumerables.</returns>
        public static CombinedSortedEnumerable<T> operator +(IEnumerable<T> a, CombinedSortedEnumerable<T> b)
        {
            CombinedSortedEnumerable<T> cse = new CombinedSortedEnumerable<T>(b.comparison);
            cse.loaders = new List<IEnumerable<T>>();
            cse.loaders.Add(a);
            cse.loaders.AddRange(b.loaders);
            return cse;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CombinedSortedEnumerable{T}"/>.
        /// </summary>
        /// <returns>A <see cref="Enumerator"/> for the <see cref="CombinedSortedEnumerable{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Enumerates all the elements in a <see cref="CombinedSortedEnumerable{T}"/>
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            private Comparison<T> comparison;
            private List<IEnumerator<T>> loaders;
            private T current;
            private bool firstDone;

            internal Enumerator(CombinedSortedEnumerable<T> loader)
            {
                this.comparison = loader.comparison;
                this.current = default(T);
                this.firstDone = false;

                this.loaders = new List<IEnumerator<T>>();
                for (int i = 0; i < loader.loaders.Count; i++)
                    this.loaders.Add(loader.loaders[i].GetEnumerator());
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                get { return current; }
            }

            void System.IDisposable.Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get { return current; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the <see cref="CombinedSortedEnumerable{T}"/>.
            /// This uses the <see cref="Comparison{T}"/> method defined by the <see cref="CombinedSortedEnumerable{T}"/>, 
            /// ensuring that values are returned in the correct sequence.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                if (!firstDone)
                {
                    for (int i = 0; i < loaders.Count; i++)
                    {
                        if (!loaders[i].MoveNext())
                        {
                            loaders.RemoveAt(i);
                            i--;
                        }
                    }
                    loaders.Sort(compareCurrent);
                    firstDone = true;
                }
                else
                {
                    if (!loaders[0].MoveNext())
                        loaders.RemoveAt(0);
                    else
                        loaders.Sort(compareCurrent);
                }

                if (loaders.Count > 0)
                    current = loaders[0].Current;
                else
                    current = default(T);

                return loaders.Count > 0;
            }

            void System.Collections.IEnumerator.Reset()
            {
                throw new InvalidOperationException("Unable to reset " + this.GetType().Name);
            }

            private int compareCurrent(IEnumerator<T> x, IEnumerator<T> y)
            {
                return comparison(x.Current, y.Current);
            }
        }
    }
}
