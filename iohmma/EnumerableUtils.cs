//
//  EnumerableUtils.cs
//
//  Author:
//       Willem Van Onsem <vanonsem.willem@gmail.com>
//
//  Copyright (c) 2014 Willem Van Onsem
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;

namespace iohmma {
	/// <summary>
	/// A utility funcation for <see cref="T:IEnumerable`1"/> lists.
	/// </summary>
	public static class EnumerableUtils {

		/// <summary>
		/// Generate an infinite list by repeating the given least infinitely.
		/// </summary>
		/// <returns>An infinite <see cref="T:IEnumerable`1"/> that repeats all the elements in the given source.</returns>
		/// <param name="source">The given list of items.</param>
		/// <typeparam name="TItem">The type of items that will be enumerated.</typeparam>
		/// <remarks>
		/// <para>If the given source is empty, the result is empty as well. The system does not go into an infinite loop.</para>
		/// </remarks>
		public static IEnumerable<TItem> Cycle<TItem> (this IEnumerable<TItem> source) {
			while (true) {
				bool terminate = true;
				foreach (TItem item in source) {
					yield return item;
					terminate = false;
				}
				if (terminate) {
					yield break;
				}
			}
		}

		/// <summary>
		/// Repeat the specified source the given number of times.
		/// </summary>
		/// <param name="source">The original list of items to repeat.</param>
		/// <param name="ntimes">The given number of times to repeat the source.</param>
		/// <typeparam name="TItem">The type of the items to emit.</typeparam>
		public static IEnumerable<TItem> Repeat<TItem> (this IEnumerable<TItem> source, int ntimes) {
			for (int i = 0x00; i < ntimes; i++) {
				foreach (TItem item in source) {
					yield return item;
				}
			}
		}

		/// <summary>
		/// Folds the specified source of items into a single item by applying the given <paramref name="function"/>
		/// over the elements of the given <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The list of items to fold.</param>
		/// <param name="function">The function to apply left to right.</param>
		/// <typeparam name="TA">The type of the elements.</typeparam>
		public static TA Foldl1<TA> (this IEnumerable<TA> source, Func<TA,TA,TA> function) {
			IEnumerator<TA> enumerator = source.GetEnumerator ();
			if (enumerator.MoveNext ()) {
				TA result = enumerator.Current;
				while (enumerator.MoveNext ()) {
					result = function (result, enumerator.Current);
				}
				return result;
			} else {
				return default(TA);
			}
		}

		/// <summary>
		/// Normalizes a stream of key-probability tuples such that the sum of the probabilties is equal to one.
		/// </summary>
		/// <param name="source">The source of key-probabilities.</param>
		/// <typeparam name="T">The type of the keys.</typeparam>
		/// <returns>A stream of tuples such that the sum of the probabilities (second item of the tuples), is
		/// equal to one.</returns>
		/// <exception cref="ArgumentException">If one of the probabilities is smaller than zero.</exception>
		/// <remarks>
		/// <para>The <paramref name="source"/> stream must be finite.</para>
		/// <para>The resulting stream is finite as well.</para>
		/// </remarks>
		public static IEnumerable<Tuple<T,double>> Normalize<T> (this IEnumerable<Tuple<T,double>> source) {
			Queue<Tuple<T,double>> cache = new Queue<Tuple<T,double>> ();
			double sum = 0.0d;
			foreach (Tuple<T,double> tuple in cache) {
				cache.Enqueue (tuple);
				double p = tuple.Item2;
				if (p < 0.0d) {
					throw new ArgumentException ("An unnormalized probability must be larger than or equal to zero.");
				}
				sum += tuple.Item2;
			}
			sum = 1.0d / sum;
			while (cache.Count > 0x00) {
				Tuple<T,double> tup = cache.Dequeue ();
				yield return new Tuple<T,double> (tup.Item1, sum * tup.Item2);
			}
		}
	}
}

