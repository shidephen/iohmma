//
//  StaticRandom.cs
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

namespace iohmma {
	/// <summary>
	/// A utility class to generate random numbers.
	/// </summary>
	public static class StaticRandom {

		/// <summary>
		/// Internal random generator.
		/// </summary>
		public static readonly Random Random = new Random ();

		/// <summary>
		/// Generate a next random value between zero (inclusive) and one (exclusive).
		/// </summary>
		/// <returns>The next random value.</returns>
		public static double NextDouble () {
			return Random.NextDouble ();
		}

		/// <summary>
		/// Get the instance of the random number generator.
		/// </summary>
		/// <returns>A <see cref="T:System.Random"/> instance that generates the random numbers statically.</returns>
		public static Random GetInstance () {
			return Random;
		}

	}
}

