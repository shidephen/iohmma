//
//  IntegerRangeDistribution.cs
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
	/// An implementation of the <see cref="IIntegerRangeDistribution"/> interface.
	/// </summary>
	public class IntegerRangeDistribution : IIntegerRangeDistribution {

		private readonly double[] cprobs;
		#region IRange implementation
		/// <summary>
		/// Gets the lower value of the <see cref="T:IRange`1"/>.
		/// </summary>
		/// <value>The lower bound on the range.</value>
		/// <remarks>
		/// <para>The lower bound must be less than or equal to the <see cref="Upper"/> bound.</para>
		/// </remarks>
		public int Lower {
			get;
			private set;
		}

		/// <summary>
		/// Gets the upper value of the <see cref="T:IRange`1"/>.
		/// </summary>
		/// <value>The upper bound on the range.</value>
		/// <remarks>
		/// <para>The upper bound must be greater than or equal to the <see cref="Lower"/> bound.</para>
		/// </remarks>
		public int Upper {
			get;
			private set;
		}
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.IntegerRangeDistribution"/> class with a given upper bound for the integer range.
		/// </summary>
		/// <param name="upper">The given upper bound for the integer range.</param>
		/// <exception cref="ArgumentException">If the <paramref name="upper"/> bound is less than one.</exception>
		/// <remarks>
		/// <para>The lower bound of the integer range is set to one (<c>1</c>).</para>
		/// </remarks>
		public IntegerRangeDistribution (int upper) : this(0x01,upper) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.IntegerRangeDistribution"/> class with a given
		/// lower and upper bound for the integer range.
		/// </summary>
		/// <param name="lower">The given lower bound for the integer range.</param>
		/// <param name="upper">The given upper bound for the integer range.</param>
		/// <exception cref="ArgumentException">If <paramref name="lower"/> is greater than <paramref name="upper"/>.</exception>
		public IntegerRangeDistribution (int lower, int upper) {
			this.Lower = lower;
			this.Upper = upper;
			int n = upper - lower + 0x01;
			if (n <= 0x00) {
				throw new ArgumentException ("The upper bound must be larger than or equal to the lower bound.");
			}
			double[] cp = new double[n];
			double pi = 1.0d / n;
			double p = pi;
			for (int i = 0x00; i < n; i++) {
				cp [i] = p;
				p += pi;
			}
			this.cprobs = cp;
		}
		#endregion
		#region IFinite implementation
		/// <summary>
		/// Enumerates all the elements of this instance.
		/// </summary>
		/// <remarks>
		/// <para>The enumerable is guaranteed to be finite.</para>
		/// </remarks>
		public IEnumerable<int> Elements () {
			int high = this.Upper;
			for (int x = this.Lower; x <= high; x++) {
				yield return x;
			}
		}
		#endregion
		#region IDistribution implementation
		/// <summary>
		/// Gets the probability density of the given element.
		/// </summary>
		/// <returns>The probability density of the given element.</returns>
		/// <param name="x">The given element to compute the probability density from.</param>
		public double GetPdf (int x) {
			int low = this.Lower;
			if (x == low) {

			}
			return this.cprobs [x - this.Lower];
		}

		public int Sample () {
			throw new NotImplementedException ();
		}

		public void Fit () {
			throw new NotImplementedException ();
		}
		#endregion
		#endregion
	}
}

