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
using System.Text;

namespace iohmma {
	/// <summary>
	/// An implementation of a <see cref="T:ITransitionDistribution`1"/> that uses a range of integers as input.
	/// </summary>
	public class IntegerRangeTransitionDistribution<TOutput> : FiniteTransitionDistribution<int,TOutput>, IRange<int> {

		#region implemented abstract members of FiniteTransitionDistribution
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
		public IntegerRangeTransitionDistribution (int lower, int upper, Func<int,IDistribution<TOutput>> subdistributionGenerator) : base(upper-lower+0x01,subdistributionGenerator) {
			this.Lower = lower;
		}
		#endregion
		#region Structure constructors
		/// <summary>
		/// Creates a <see cref="T:IntegerRangeTransitionDistribution`1"/> that works as a transition function for a specific hidden state to another hidden
		/// state given an input. The probabilites are given.
		/// </summary>
		/// <returns>The state transition distributions.</returns>
		/// <param name="lower">The lower bound on the input.</param>">
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="probabilities">Probabilities.</param>
		/// <exception cref="ArgumentException">If the given <paramref name="numberOfHiddenStates"/> is less than or equal to zero (<c>0</c>).</exception>
		/// <exception cref="ArgumentException">If the given number of generates sub distributions is less than or equal to zero (<c>0</c>).</exception>
		/// <exception cref="ArgumentException">If the subsequences with a length of <paramref name="numberOfHiddenStates"/> don't sum up to one.</exception>
		/// <exception cref="ArgumentException">If one of the given probabilities is less than zero.</exception>
		public static IntegerRangeTransitionDistribution<int> HiddenStateTransitionDistributions (int lower, int numberOfHiddenStates, params double[] probabilities) {
			return HiddenStateTransitionDistributions (lower, numberOfHiddenStates, (IEnumerable<double>)probabilities);
		}

		/// <summary>
		/// Creates a <see cref="T:IntegerRangeTransitionDistribution`1"/> that works as a transition function for a specific hidden state to another hidden
		/// state given an input. The probabilites are given.
		/// </summary>
		/// <returns>The state transition distributions.</returns>
		/// <param name="lower">The lower bound on the input.</param>">
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="probabilities">Probabilities.</param>
		/// <exception cref="ArgumentException">If the given <paramref name="numberOfHiddenStates"/> is less than or equal to zero (<c>0</c>).</exception>
		/// <exception cref="ArgumentException">If the given number of generates sub distributions is less than or equal to zero (<c>0</c>).</exception>
		/// <exception cref="ArgumentException">If the subsequences with a length of <paramref name="numberOfHiddenStates"/> don't sum up to one.</exception>
		/// <exception cref="ArgumentException">If one of the given probabilities is less than zero.</exception>
		public static IntegerRangeTransitionDistribution<int> HiddenStateTransitionDistributions (int lower, int numberOfHiddenStates, IEnumerable<double> probabilities) {
			if (numberOfHiddenStates <= 0x00) {
				throw new ArgumentException ("The number of hidden states must be larger than zero.");
			}
			double[] probi = new double[numberOfHiddenStates];
			List<IntegerRangeDistribution> subprobs = new List<IntegerRangeDistribution> ();
			IEnumerator<double> enumerator = probabilities.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				probi [0x00] = enumerator.Current;
				for (int i = 0x01; i < numberOfHiddenStates && enumerator.MoveNext (); i++) {
					probi [i] = enumerator.Current;
				}
				subprobs.Add (new IntegerRangeDistribution (0x00, probi));
			}
			return new IntegerRangeTransitionDistribution<int> (lower, subprobs);
		}
		#endregion
	}
}