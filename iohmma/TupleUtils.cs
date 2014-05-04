//
//  TupleUtils.cs
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
	/// A utility class that defines operations on tuples. For instance filtering the elements.
	/// </summary>
	public static class TupleUtils {

		/// <summary>
		/// Generate a <see cref="T:IEnumerable`1"/> containing the first elements of the list of tuples.
		/// </summary>
		/// <param name="source">The list of tuples.</param>
		/// <returns>A list containing the first element of each tuple in the given list.</returns>
		/// <typeparam name="T1">The type of the first element of the tuple list.</typeparam>
		/// <typeparam name="T2">The type of the second element of the tuple list.</typeparam>
		public static IEnumerable<T1> First<T1,T2> (this IEnumerable<Tuple<T1,T2>> source) {
			foreach (Tuple<T1,T2> tuple in source) {
				yield return tuple.Item1;
			}
		}

		/// <summary>
		/// Generate a <see cref="T:IEnumerable`1"/> containing the first elements of the list of tuples.
		/// </summary>
		/// <param name="source">The list of tuples.</param>
		/// <returns>A list containing the first element of each tuple in the given list.</returns>
		/// <typeparam name="T1">The type of the first element of the tuple list.</typeparam>
		/// <typeparam name="T2">The type of the second element of the tuple list.</typeparam>
		/// <typeparam name="T3">The type of the third element of the tuple list.</typeparam>
		public static IEnumerable<T1> First<T1,T2,T3> (this IEnumerable<Tuple<T1,T2,T3>> source) {
			foreach (Tuple<T1,T2,T3> tuple in source) {
				yield return tuple.Item1;
			}
		}
	}
}

