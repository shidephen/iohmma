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
using NUtils;
using NUtils.Maths;

namespace iohmma {
	/// <summary>
	/// An implementation of the <see cref="IIntegerRangeDistribution"/> interface.
	/// </summary>
	/// <remarks>
	/// <para>The implementation uses cummulative probability to make the <see cref="M:FiniteDistribution`1.Sample"/> method faster.
	/// Updating probabilities requires linear time.</para>
	/// </remarks>
	public class IntegerRangeDistribution : FiniteDistribution<int>, IIntegerRangeDistribution {

		#region implemented abstract members of FiniteDistribution

		/// <summary>
		/// A function that transforms input into their corresponding index. This is used by several methods
		/// to translate the input such that the implementation remains generic.
		/// </summary>
		/// <value>A function mapping inputs to indices.</value>
		public override Func<int, int> InputMapper {
			get {
				return x => x - this.Lower;
			}
		}

		/// <summary>
		/// A function that transforms indices into their corresponding input. This is used by several methods
		/// to translate the input such that the implementation remains generic.
		/// </summary>
		/// <value>A function mapping indices to inputs.</value>
		public override Func<int, int> IndexMapper {
			get {
				return x => x + this.Lower;
			}
		}

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
				return this.Lower + this.CumulativeProbabilities.Length;
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
		public IntegerRangeDistribution (int upper) : this (0x01, upper) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.IntegerRangeDistribution"/> class with a given
		/// lower and upper bound for the integer range.
		/// </summary>
		/// <param name="lower">The given lower bound for the integer range.</param>
		/// <param name="upper">The given upper bound for the integer range.</param>
		/// <exception cref="ArgumentException">If <paramref name="lower"/> is greater than <paramref name="upper"/>.</exception>
		public IntegerRangeDistribution (int lower, int upper) : base (upper - lower + 0x01) {
			this.Lower = lower;
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
		public IntegerRangeDistribution (params double[] initialProbabilities) : this ((IEnumerable<double>)initialProbabilities) {
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
		public IntegerRangeDistribution (IEnumerable<double> initialProbabilities) : this (0x01, initialProbabilities) {
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
		public IntegerRangeDistribution (int lower, params double[] initialProbabilities) : this (lower, (IEnumerable<double>)initialProbabilities) {
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
		public IntegerRangeDistribution (int lower, IEnumerable<double> initialProbabilities) : base (initialProbabilities) {
			this.Lower = lower;
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
			for (int i = 0x00; i < n; i++) {
				p = MathUtils.NextDouble ();
				probs [i] = p;
				sum += p;
			}
			sum = 1.0d / sum;
			for (int i = 0x00; i < n; i++) {
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

