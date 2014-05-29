//
//  FiniteRangeTransitionDistribution.cs
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
	/// A basic <see cref="T:TransitionDistribution`2"/> class where the number of inputs is finite. The class stores
	/// a list of sub distributions. Subclasses of this class must map the inputs to the their corresponding
	/// indices of this class.
	/// </summary>
	public abstract class FiniteTransitionDistribution<TInput,TOutput> : TransitionDistribution<TInput,TOutput> {

		#region Protected fields
		/// <summary>
		/// The list of distributions that depend on the input.
		/// </summary>
		protected readonly IDistribution<TOutput>[] Subdistributions;
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// lower bound and a list of distributions for every discrete input.
		/// </summary>
		/// <param name="distributions">A list of distributions ordered per input.</param>
		/// <exception cref="ArgumentException">If the list of <paramref name="distributions"/> is empty.</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		protected FiniteTransitionDistribution (params IDistribution<TOutput>[] distributions) : this((IEnumerable<IDistribution<TOutput>>) distributions) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// lower bound and a list of distributions for every discrete input.
		/// </summary>
		/// <param name="distributions">A list of distributions ordered per input.</param>
		/// <exception cref="ArgumentException">If the list of <paramref name="distributions"/> is empty.</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		protected FiniteTransitionDistribution (IEnumerable<IDistribution<TOutput>> distributions) {
			this.Subdistributions = distributions.ToArray ();
			if (this.Subdistributions.Length <= 0x00) {
				throw new ArgumentException ("The number of given distributions must be larger than zero.");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// upper bound and a generator function that constructs new distributions.
		/// </summary>
		/// <param name="size">The given number of sub distributions.</param>
		/// <param name="subdistributionGenerator">A generator that constructs the distributions. The function takes no inputs.</param>
		/// <exception cref="ArgumentException">If the given <paramref name="size"/> is less than one (<c>1</c>).</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		protected FiniteTransitionDistribution (int size, Func<IDistribution<TOutput>> subdistributionGenerator) : this(size,x => default(TInput),subdistributionGenerator.ShiftRightParameter<TInput,IDistribution<TOutput>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IntegerRangeTransitionDistribution`1"/> class with a given
		/// upper bound and a generator function that constructs new distributions.
		/// </summary>
		/// <param name="size">The given number of sub distributions.</param>
		/// <param name="indexMapper">A function that takes as input the index and maps it to the expected input of the distribution generator.</param>
		/// <param name="subdistributionGenerator">A generator that constructs the distributions. The function takes as input
		/// the input value for which a distribution must be generated.</param>
		/// <exception cref="ArgumentException">If the given <paramref name="size"/> is less than one (<c>1</c>).</exception>
		/// <remarks>
		/// <para>The distributions are not cloned: modifications to the given distributions will have an impact
		/// in this transitional distribution.</para>
		/// </remarks>
		protected FiniteTransitionDistribution (int size, Func<int,TInput> indexMapper, Func<TInput,IDistribution<TOutput>> subdistributionGenerator) {
			if (size <= 0x00) {
				throw new ArgumentException ("The number of sub probabilities must be larger or equal to one.");
			}
			this.Subdistributions = new IDistribution<TOutput>[size];
			for (int i = 0x00; i < size; i++) {
				this.Subdistributions [i] = subdistributionGenerator (indexMapper (i));
			}
		}
		#endregion
		#region Inner functions used for fast subclassing
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
		protected double InnerGetPdf (int input, TOutput output) {
			IDistribution<TOutput>[] ps = this.Subdistributions;
			if (input >= 0x00 && input < ps.Length) {
				return ps [input].GetPdf (output);
			} else {
				throw new ArgumentException ("The given input is not within range.");
			}
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		protected Tuple<int, TOutput> InnerSample () {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution for the given input.
		/// </summary>
		/// <param name="input">The given input</param>
		/// <returns>A randomly chosen element in the set according to the probability density function and the input.</returns>
		/// <exception cref="ArgumentException">If the given input is not within bounds.</exception>
		protected TOutput InnerSample (int input) {
			IDistribution<TOutput>[] ps = this.Subdistributions;
			if (input >= 0x00 && input < ps.Length) {
				return ps [input].Sample ();
			} else {
				throw new ArgumentException ("The given input is not within range.");
			}
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		protected void InnerFit (IEnumerable<Tuple<Tuple<int, TOutput>, double>> probabilities, double fitting = 1.0) {
			IDistribution<TOutput>[] pc = this.Subdistributions;
			int n = pc.Length;
			for (int i = 0x00; i < n; i++) {
				pc [i].Fit (from p in probabilities where p.Item1.Item1 == i select new Tuple<TOutput,double> (p.Item1.Item2, p.Item2), fitting);
			}
		}
		#endregion
	}
}

