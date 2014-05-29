//
//  IntegerRangeTransitionDistribution.cs
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

namespace iohmma {
	/// <summary>
	/// An implementation of a <see cref="T:ITransitionDistribution`1"/> that uses a range of integers as input.
	/// </summary>
	public class IntegerRangeTransitionDistribution<TOutput> : FiniteTransitionDistribution<int,TOutput>, IRange<int> {
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
				return this.Lower + this.Subdistributions.Length - 0x01;
			}
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// list of distributions for every discrete input.
		/// </summary>
		/// <param name="distributions">A list of distributions ordered per input.</param>
		/// <exception cref="ArgumentException">If no distribution is given.</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		public IntegerRangeTransitionDistribution (params IDistribution<TOutput>[] distributions) : this(0x01,distributions) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// list of distributions for every discrete input.
		/// </summary>
		/// <param name="distributions">A list of distributions ordered per input.</param>
		/// <exception cref="ArgumentException">If no distribution is given.</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		public IntegerRangeTransitionDistribution (IEnumerable<IDistribution<TOutput>> distributions) : this(0x01,distributions) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// lower bound and a list of distributions for every discrete input.
		/// </summary>
		/// <param name="lower">The lower bound on the input.</param>
		/// <param name="distributions">A list of distributions ordered per input.</param>
		/// <exception cref="ArgumentException">If no distribution is given.</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		public IntegerRangeTransitionDistribution (int lower, params IDistribution<TOutput>[] distributions) : this(lower,(IEnumerable<IDistribution<TOutput>>) distributions) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// lower bound and a list of distributions for every discrete input.
		/// </summary>
		/// <param name="lower">The lower bound on the input.</param>
		/// <param name="distributions">A list of distributions ordered per input.</param>
		/// <exception cref="ArgumentException">If no distribution is given.</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		public IntegerRangeTransitionDistribution (int lower, IEnumerable<IDistribution<TOutput>> distributions) : base(distributions) {
			this.Lower = lower;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// upper bound and a generator function that constructs new distributions.
		/// </summary>
		/// <param name="upper">The given upper bound on the input</param>
		/// <param name="subdistributionGenerator">A generator that constructs the distributions. The function takes no inputs.</param>
		/// <exception cref="ArgumentException">If the given upper bound is less than one (<c>1</c>).</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// <para>The lower bound is set to one (<c>1</c>).</para>
		/// </remarks>
		public IntegerRangeTransitionDistribution (int upper, Func<IDistribution<TOutput>> subdistributionGenerator) : this(0x01,upper,subdistributionGenerator) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// lower- and upper bound and a generator function that constructs new distributions.
		/// </summary>
		/// <param name="lower">The given lower bound on the input.</param>
		/// <param name="upper">The given upper bound on the input</param>
		/// <param name="subdistributionGenerator">A generator that constructs the distributions. The function takes no inputs.</param>
		/// <exception cref="ArgumentException">If the given upper bound is less than the given lower bound.</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		public IntegerRangeTransitionDistribution (int lower, int upper, Func<IDistribution<TOutput>> subdistributionGenerator) : this(lower,upper,subdistributionGenerator.ShiftRightParameter<int,IDistribution<TOutput>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// upper bound and a generator function that constructs new distributions.
		/// </summary>
		/// <param name="upper">The given upper bound on the input</param>
		/// <param name="subdistributionGenerator">A generator that constructs the distributions. The function takes as input
		/// the input value for which a distribution must be generated.</param>
		/// <exception cref="ArgumentException">If the given upper bound is less than one (<c>1</c>).</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// <para>The lower bound is set to one (<c>1</c>).</para>
		/// </remarks>
		public IntegerRangeTransitionDistribution (int upper, Func<int,IDistribution<TOutput>> subdistributionGenerator) : this(0x01,upper,subdistributionGenerator) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// lower- and upper bound and a generator function that constructs new distributions.
		/// </summary>
		/// <param name="lower">The given lower bound on the input.</param>
		/// <param name="upper">The given upper bound on the input</param>
		/// <param name="subdistributionGenerator">A generator that constructs the distributions. The function takes as input
		/// the input value for which a distribution must be generated.</param>
		/// <exception cref="ArgumentException">If the given upper bound is less than the given lower bound.</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		public IntegerRangeTransitionDistribution (int lower, int upper, Func<int,IDistribution<TOutput>> subdistributionGenerator) : base(upper-lower+0x01, x => x+lower,subdistributionGenerator) {
			this.Lower = lower;
		}
		#endregion
		#region implemented abstract members of TransitionDistribution
		/// <summary>
		/// Gets the probability density function for the given <paramref name="input"/> and the given output <paramref name="state"/>.
		/// </summary>
		/// <returns>The probability density function for the given input and the given output state.</returns>
		/// <param name="input">The given input to calculate the probability for.</param>
		/// <param name="output">The given output to calculate the probability for.</param>
		/// <exception cref="ArgumentException">If the given input is not withing range.</exception>
		/// <exception cref="ArgumentException">If the given output is not in range of the distribution.</exception>
		/// <remarks>
		/// <para>If the output is discrete, for any given input the sum of the probabilities must be equal to one.</para>
		/// <para>If the output is continu, for any given input the integral of the probabilities must be equal to one.</para>
		/// </remarks>
		public override double GetPdf (int input, TOutput output) {
			return this.InnerGetPdf (input - this.Lower, output);
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		public override Tuple<int, TOutput> Sample () {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution for the given input.
		/// </summary>
		/// <param name="input">The given input</param>
		/// <returns>A randomly chosen element in the set according to the probability density function and the input.</returns>
		/// <exception cref="ArgumentException">If the given input is not within bounds.</exception>
		public override TOutput Sample (int input) {
			return this.InnerSample (input - this.Lower);
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public override void Fit (IEnumerable<Tuple<Tuple<int, TOutput>, double>> probabilities, double fitting = 1.0) {
			this.InnerFit (from x in probabilities select new Tuple<Tuple<int, TOutput>,double> (new Tuple<int, TOutput> (x.Item1.Item1 - this.Lower, x.Item1.Item2), x.Item2), fitting);
		}
		#endregion
	}
}