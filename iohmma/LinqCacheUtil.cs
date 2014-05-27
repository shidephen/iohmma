//
//  LinqCacheUtil.cs
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
	/// A static class to cache the results of a query intermediately.
	/// </summary>
	public static class LinqCacheUtil {

		#region Extension methods
		/// <summary>
		/// Caches the items of the specified source such that enumerating over it multiple times will increase performance.
		/// </summary>
		/// <param name="source">A source of items to be cached and enumerated eventually.</param>
		/// <typeparam name="T">The type of elements produced by the source.</typeparam>
		/// <remarks>
		/// <para>This is merely done if the given <paramref name="source"/> is an expensive LINQ-query that should
		/// be executed several times and would always yield the same result.</para>
		/// <para>Infinite <see cref="T:IEnumerable`1"/> instances are supported since the data
		/// is cached lazely.</para>
		/// <para>The cached version supports multiple enumerations at once.</para>
		/// </remarks>
		public static IEnumerable<T> Cache<T> (this IEnumerable<T> source) {
			return new LinqCache<T> (source);
		}
		#endregion
	}
}

