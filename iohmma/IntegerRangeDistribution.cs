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
using NUtils;

namespace iohmma {
	/// <summary>
	/// An implementation of the <see cref="IIntegerRangeDistribution"/> interface.
	/// </summary>
	/// <remarks>
	/// <para>The implementation uses cummulative probability to make the <see cref="Sample"/> method faster.
	/// Updating probabilities requires linear time.</para>
	/// </remarks>
	public class IntegerRangeDistribution : ScaledFittingDistribution<int>, IIntegerRangeDistribution, IEnumerable<double>, IFormatFormatProviderToString {

		#region Fields
		private readonly double[] cprobs;
		#endregion
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
		/// Initializes a new instance of the <see cref="IntegerRangeDistribution"/> class with a given list of
		/// initial probabilities.
		/// </summary>
		/// <param name="initialProbabilities">The list of initial probabilities, ordered in ascending index order.</param>
		/// <exception cref="ArgumentException">If <paramref name="initialProbabilities"/> contains no elements.</exception>
		/// <exception cref="ArgumentException">If the <paramref name="initialProbabilities"/> don't sum up to one.</exception>
		/// <remarks>
		/// <para>The list of initial probabilities must be finite.</para>
		/// <para>The probabilities must sum up to one.</para>
		/// <para>The lower bound is set to one (<c>1</c>).</para>
		/// </remarks>
		public IntegerRangeDistribution (params double[] initialProbabilities) : this((IEnumerable<double>) initialProbabilities) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerRangeDistribution"/> class with a given list of
		/// initial probabilities.
		/// </summary>
		/// <param name="initialProbabilities">The list of initial probabilities, ordered in ascending index order.</param>
		/// <exception cref="ArgumentException">If <paramref name="initialProbabilities"/> contains no elements.</exception>
		/// <exception cref="ArgumentException">If the <paramref name="initialProbabilities"/> don't sum up to one.</exception>
		/// <remarks>
		/// <para>The list of initial probabilities must be finite.</para>
		/// <para>The probabilities must sum up to one.</para>
		/// <para>The lower bound is set to one (<c>1</c>).</para>
		/// </remarks>
		public IntegerRangeDistribution (IEnumerable<double> initialProbabilities) : this(0x01,initialProbabilities) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerRangeDistribution"/> class with a given lower
		/// boudn and a list of initial probabilities.
		/// </summary>
		/// <param name="lower">The given lower bound for the integer range.</param>
		/// <param name="initialProbabilities">The list of initial probabilities, ordered in ascending index order.</param>
		/// <exception cref="ArgumentException">If <paramref name="initialProbabilities"/> contains no elements.</exception>
		/// <exception cref="ArgumentException">If the <paramref name="initialProbabilities"/> don't sum up to one.</exception>
		/// <remarks>
		/// <para>The list of initial probabilities must be finite.</para>
		/// <para>The probabilities must sum up to one.</para>
		/// </remarks>
		public IntegerRangeDistribution (int lower, params double[] initialProbabilities) : this(lower,(IEnumerable<double>) initialProbabilities) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerRangeDistribution"/> class with a given
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
			if (Math.Abs (p - 1.0d) > ProgramConstants.Epsilon) {
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
		public override void Fit (IEnumerable<Tuple<int,double>> probabilities, double fitting = 1.0d) {
			double[] cps = this.cprobs;
			int n = cps.Length;
			double[] pas = new double[n];
			int low = this.Lower;
			double p, sum = 0.0d;
			foreach (Tuple<int,double> tup in probabilities) {
				int x = tup.Item1 - low;
				p = tup.Item2;
				sum += p;
				if (x < 0x00 || x > n) {
					throw new ArgumentException ("While fitting, one can only enumerate valid values.");
				} else if (x < n) {
					pas [x] += p;
				}
			}
			double fittinh = 1.0d - fitting;
			double fitsum = fitting / sum;
			double cur = 0.0d;
			for (int i = 0x00; i < n; i++) {
				cur += fitsum * pas [i];
				cps [i] = fittinh * cps [i] + cur;
			}
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
			return string.Format ("{0}::{1}::{2}", this.Lower, string.Join ("; ", this), this.Upper);
		}
		#endregion
		#region IFormatFormatProviderToString implementation
		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation using the specified format and
		/// culture-specific format information.
		/// </summary>
		/// <returns>
		/// The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.
		/// </returns>
		/// <param name="format">A numeric format string.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information.</param>
		public string ToString (string format, IFormatProvider provider) {
			return string.Format ("{0}::{1}::{2}", this.Lower, string.Join ("; ", from x in this select x.ToString (format, provider)), this.Upper);
		}
		#endregion
		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.
		/// </summary>
		/// <returns>
		/// The string representation of the value of this instance as specified by <paramref name="provider"/>.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information.</param>
		#region IFormatProviderToString implementation
		public string ToString (IFormatProvider provider) {
			return string.Format ("{0}::{1}::{2}", this.Lower, string.Join ("; ", from x in this select x.ToString (provider)), this.Upper);
		}
		#endregion
		#region IFormatToString implementation
		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation, using the specified format.
		/// </summary>
		/// <returns>
		/// The string representation of the value of this instance as specified by <paramref name="format" />.
		/// </returns>
		/// <param name="format">A numeric format string.</param>
		/// <exception cref="T:System.FormatException"><paramref name="format" /> is invalid.</exception>
		public string ToString (string format) {
			return string.Format ("{0}::{1}::{2}", this.Lower, string.Join ("; ", from x in this select x.ToString (format)), this.Upper);
		}
		#endregion
		#region Random generators
		/// <summary>
		/// Generates a random <see cref="IntegerRangeDistribution"/> with a given lower and upper bound. The probabilities
		/// are random numbers between zero and one that sum up to one.
		/// </summary>
		/// <returns>A random <see cref="IntegerRangeDistribution"/> with a given lower and upper bound.</returns>
		/// <param name="lower">The lower bound of the generated <see cref="IntegerRangeDistribution"/>.</param>
		/// <param name="upper">The upper bound of the generated <see cref="IntegerRangeDistribution"/>.</param>
		/// <exception cref="ArgumentException">If the given upper bound is smaller than the given lower bound.</exception>
		public static IntegerRangeDistribution GenerateRandom (int lower, int upper) {
			int n = upper - lower + 0x01;
			if (n <= 0x00) {
				throw new ArgumentException ("The upper bound must at least be as large as the lower bound.");
			}
			double[] probs = new double[n];
			double p, sum = 0.0d;
			for (int i = 0x00; i <n; i++) {
				p = MathUtils.NextDouble ();
				probs [i] = p;
				sum += p;
			}
			sum = 1.0d / sum;
			for (int i = 0x00; i <n; i++) {
				probs [i] *= sum;
			}
			return new IntegerRangeDistribution (lower, probs);
		}

		/// <summary>
		/// Generates a random <see cref="IntegerRangeDistribution"/> with a given upper bound. The probabilities
		/// are random numbers between zero and one that sum up to one.
		/// </summary>
		/// <returns>A random <see cref="IntegerRangeDistribution"/> with a given upper bound.</returns>
		/// <param name="upper">The upper bound of the generated <see cref="IntegerRangeDistribution"/>.</param>
		/// <exception cref="ArgumentException">If the given upper bound is smaller than one (<c>1</c>).</exception>
		/// <remarks>
		/// <para>The lower bound is set to one (<c>1</c>).</para>
		/// </remarks>
		public static IntegerRangeDistribution GenerateRandom (int upper) {
			return GenerateRandom (0x01, upper);
		}
		#endregion
	}
}

