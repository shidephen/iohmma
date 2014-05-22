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
using System.Linq;
using System.Text;

namespace iohmma {
	/// <summary>
	/// An implementation of the <see cref="IIntegerRangeDistribution"/> interface.
	/// </summary>
	/// <remarks>
	/// <para>The implementation uses cummulative probability to make the <see cref="Sample"/> method faster.
	/// Updating probabilities requires linear time.</para>
	/// </remarks>
	public class IntegerRangeDistribution : Distribution<int>, IIntegerRangeDistribution, IEnumerable<double> {

		/// <summary>
		/// The tolaterated difference between one and the sum of the given probabilities in constructors, methods, etc.
		/// </summary>
		/// <remarks>
		/// <para>If the sum of the items does not equal one (with a tolerance of epsilon), a <see cref="ArgumentException"/> will be thrown.</para>
		/// </remarks>
		public const double EPSILON = 1e-6d;
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
			get {
				return this.Lower + this.cprobs.Length;
			}
		}
		#endregion
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
			int n = upper - lower;
			if (n < 0x00) {
				throw new ArgumentException ("The upper bound must be larger than or equal to the lower bound.");
			}
			double[] cp = new double[n];
			double pi = 1.0d / (n + 0x01);
			double p = pi;
			for (int i = 0x00; i < n; i++) {
				cp [i] = p;
				p += pi;
			}
			this.cprobs = cp;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.IntegerRangeDistribution"/> class with a given
		/// lower bound and a list of initial probabilities.
		/// </summary>
		/// <param name="lower">The given lower bound for the integer range.</param>
		/// <param name="initialProbabilities">The list of initial probabilities, ordered in ascending index order.</param>
		/// <exception cref="ArgumentException">If <paramref name="initialProbabilities"/> contains no elements.</exception>
		/// <exception cref="ArgumentException">If the <paramref name="initialProbabilities"/> don't sum up to one.</exception>
		/// <remarks>
		/// <para>The list of initial probabilities must be finite.</para>
		/// <para>The probabilities must sum up to one.</para>
		/// </remarks>
		public IntegerRangeDistribution (int lower, IEnumerable<double> initialProbabilities) {
			this.Lower = lower;
			List<double> cps = new List<double> ();
			IEnumerator<double> enumerator = initialProbabilities.GetEnumerator ();
			if (!enumerator.MoveNext ()) {
				throw new ArgumentException ("The list of probabilities must contain at least one element.");
			}
			double p = enumerator.Current;
			while (enumerator.MoveNext ()) {
				cps.Add (p);
				p += enumerator.Current;
			}
			if (Math.Abs (p - 1.0d) > EPSILON) {
				throw new ArgumentException ("The probabilities must sum up to one.");
			}
			this.cprobs = cps.ToArray ();
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
		/// <exception cref="ArgumentException">If the given <paramref name="x"/> is not in the integer range.</exception>
		/// <remarks>
		/// <para>The probability density function of any element must always be larger than or equal to zero.</para>
		/// <para>The sum of the probability densities of the range is equal to one.</para>
		/// </remarks>
		public override double GetPdf (int x) {
			int index = x - this.Lower;
			double[] cp = this.cprobs;
			int cpn = cp.Length;
			if (index == 0x00) {
				return cp [0x00];
			} else if (index > 0x00 && index < cpn) {
				return cp [index] - cp [index - 0x01];
			} else if (index == cpn) {
				return 1.0d - cp [index - 0x01];
			} else {
				throw new ArgumentException ("The given value is not within the range.");
			}
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		public override int Sample () {
			double[] cp = this.cprobs;
			double x = StaticRandom.NextDouble ();
			int value = Array.BinarySearch (cp, x);
			if (value < 0x00) {
				value = ~value;
			}
			return value + this.Lower;
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>If the <paramref name="fitting"/> coefficient is one, only the new data is taken into account.
		/// If zero, only the old data.</para>
		/// </remarks>
		public override void Fit (IEnumerable<Tuple<int,double>> probabilities, double fitting = 1.0d) {//TODO
			throw new NotImplementedException ();
		}
		#endregion
		#region IEnumerable implementation
		/// <summary>
		/// Gets the enumerator that iterates over the probabilities of the several options.
		/// </summary>
		/// <returns>An enumerator where the <c>i</c>-th item is the probability of item <c>i+lower</c>.</returns>
		/// <remarks>
		/// <para>The iterator is finite.</para>
		/// <para>The items in the iterator sum up to one.</para>
		/// </remarks>
		public IEnumerator<double> GetEnumerator () {
			double p = 0.0d;
			foreach (double pi in cprobs) {
				yield return pi - p;
				p = pi;
			}
			yield return 1.0d - p;
		}
		#endregion
		#region IEnumerable implementation
		/// <summary>
		/// Gets the enumerator for the <see cref="System.Collections.IEnumerable"/> interface.
		/// </summary>
		/// <returns>An enumerator where the <c>i</c>-th item is the probability of item <c>i+lower</c>.</returns>
		/// <remarks>
		/// <para>The iterator iterates over <see cref="double"/> instances.</para>
		/// <para>The iterator is finite.</para>
		/// <para>The items in the iterator sum up to one.</para>
		/// </remarks>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return this.GetEnumerator ();
		}
		#endregion
		#region ToString method
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="IntegerRangeDistribution"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="IntegerRangeDistribution"/>.</returns>
		/// <remarks>
		/// <para>The result has a format <c>1/0.5,2/0.2,3/0.3</c>.</para>
		/// </remarks>
		public override string ToString () {
			StringBuilder sb = new StringBuilder ();
			int index = this.Lower;
			IEnumerator<double> pis = this.GetEnumerator ();
			pis.MoveNext ();
			sb.Append (index++);
			sb.Append ('/');
			sb.Append (pis.Current);
			while (pis.MoveNext ()) {
				sb.Append (index++);
				sb.Append ('/');
				sb.Append (pis.Current);
			}
			return sb.ToString ();
		}
		#endregion
	}
}

