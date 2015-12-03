//
//  FiniteDistribution.cs
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
using NUtils;
using NUtils.Textual;

namespace iohmma {
	/// <summary>
	/// A basic <see cref="T:Distribution`1"/> class where the number of outputs is finite. The class stores
	/// a list of probabilities. Subclasses of this class must map the outputs to the the corresponding
	/// indices of this class.
	/// </summary>
	public abstract class FiniteDistribution<TData> : ScaledFittingDistribution<TData>, IInputIndexMapping<TData>, IEnumerable<double>, IFormatFormatProviderToString {

		#region Protected properties

		/// <summary>
		/// An array of doubles listing the cumulative probabilities.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The cumulative probability is used to increase the performance of sampling from such distributions.
		/// </para>
		/// </remarks>
		protected readonly double[] CumulativeProbabilities;

		#endregion

		#region IInputIndexMapping implementation

		/// <summary>
		/// A function that transforms input into their corresponding index. This is used by several methods
		/// to translate the input such that the implementation remains generic.
		/// </summary>
		/// <value>A function mapping inputs to indices.</value>
		public abstract Func<TData,int> InputMapper {
			get;
		}

		/// <summary>
		/// A function that transforms indices into their corresponding input. This is used by several methods
		/// to translate the input such that the implementation remains generic.
		/// </summary>
		/// <value>A function mapping indices to inputs.</value>
		public abstract Func<int,TData> IndexMapper {
			get;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FiniteDistribution`1"/> class with the given number of elements
		/// of the finite distribution.
		/// </summary>
		/// <param name="nElements">The given number of elements.</param>
		/// <exception cref="ArgumentException">If the number of elements is less than or equal to zero (<c>0</c>).</exception>
		protected FiniteDistribution (int nElements) {
			int n = nElements - 0x01;
			if (n < 0x00) {
				throw new ArgumentException ("The number of elements of the finite distribution must be larger than zero.");
			}
			this.CumulativeProbabilities = new double[n];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FiniteDistribution`1"/> class with a given list
		/// of initial probabilities.
		/// </summary>
		/// <param name="initialProbabilities">A given list of initial probabilities.</param>
		/// <exception cref="ArgumentException">If <paramref name="initialProbabilities"/> contains no elements.</exception>
		/// <exception cref="ArgumentException">If the <paramref name="initialProbabilities"/> don't sum up to one.</exception>
		/// <remarks>
		/// <para>The list of initial probabilities must be finite.</para>
		/// <para>The probabilities must sum up to one.</para>
		/// </remarks>
		protected FiniteDistribution (IEnumerable<double> initialProbabilities) {
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
			this.CumulativeProbabilities = cps.ToArray ();
		}

		#endregion

		#region IDistribution implementation

		/// <summary>
		/// Gets the probability density of the given element.
		/// </summary>
		/// <returns>The probability density of the given element.</returns>
		/// <param name="x">The given element to compute the probability density from.</param>
		/// <exception cref="ArgumentException">If the given <paramref name="x"/> is not a valid input.</exception>
		/// <remarks>
		/// <para>The probability density function of any element must always be larger than or equal to zero.</para>
		/// <para>The sum of the probability densities of the range is equal to one.</para>
		/// </remarks>
		public override double GetPdf (TData x) {
			int index = this.InputMapper (x);
			double[] cp = this.CumulativeProbabilities;
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
		/// <param name="rand">The random generator that is used to sample. In case <c>null</c> is given, the <see cref="T:iohmma.StaticRandom"/> is used.</param>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		public override TData Sample (Random rand = null) {
			double[] cp = this.CumulativeProbabilities;
			double x = (rand ?? StaticRandom.GetInstance ()).NextDouble ();
			int value = Array.BinarySearch (cp, x);
			if (value < 0x00) {
				value = ~value;
			}
			return this.IndexMapper (value);
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>If no data is provided, the distribution is not midified.</para>
		/// <para>If the <paramref name="fitting"/> coefficient is one, only the new data is taken into account.
		/// If zero, only the old data.</para>
		/// </remarks>
		public override void Fit (IEnumerable<Tuple<TData,double>> probabilities, double fitting = 1.0d) {
			double[] cps = this.CumulativeProbabilities;
			int n = cps.Length;
			double[] pas = new double[n];
			Func<TData,int> im = this.InputMapper;
			double p, sum = 0.0d;
			bool data = false;
			foreach (Tuple<TData,double> tup in probabilities) {
				int x = im (tup.Item1);
				p = tup.Item2;
				sum += p;
				if (x < 0x00 || x > n) {
					throw new ArgumentException ("While fitting, one can only enumerate valid values.");
				} else if (x < n) {
					pas [x] += p;
				}
				data = true;
			}
			if (data) {
				double fittinh = 1.0d - fitting;
				double fitsum = fitting / sum;
				double cur = 0.0d;
				for (int i = 0x00; i < n; i++) {
					cur += fitsum * pas [i];
					cps [i] = fittinh * cps [i] + cur;
				}
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
			foreach (double pi in CumulativeProbabilities) {
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
		/// Returns a <see cref="System.String"/> that represents the current <see cref="T:FiniteDistribution`1"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="T:FiniteDistribution`1"/>.</returns>
		/// <remarks>
		/// <para>The result has a format <c>1:0.5;0.2;0.3:3</c>.</para>
		/// </remarks>
		public override string ToString () {
			return string.Format ("{0}:{1}:{2}", this.IndexMapper (0x00), string.Join (";", this), this.IndexMapper (this.CumulativeProbabilities.Length));
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
			return string.Format ("{0}:{1}:{2}", this.IndexMapper (0x00), string.Join (";", from x in this
			                                                                                select x.ToString (format, provider)), this.IndexMapper (this.CumulativeProbabilities.Length));
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
			return string.Format ("{0}:{1}:{2}", this.IndexMapper (0x00), string.Join (";", from x in this
			                                                                                select x.ToString (provider)), this.IndexMapper (this.CumulativeProbabilities.Length));
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
			return string.Format ("{0}:{1}:{2}", this.IndexMapper (0x00), string.Join (";", from x in this
			                                                                                select x.ToString (format)), this.IndexMapper (this.CumulativeProbabilities.Length));
		}

		#endregion

		#region IDistribution implementation

		/// <summary>
		/// Resets the distribution to its original state.
		/// </summary>
		public void Reset () {
			double[] cp = this.CumulativeProbabilities;
			int n = cp.Length;
			double pi = 1.0d / (n + 0x01);
			double p = pi;
			for (int i = 0x00; i < n; i++) {
				cp [i] = p;
				p += pi;
			}
		}

		#endregion
	}
}

