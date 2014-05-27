//
//  TransitionDistribution.cs
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

namespace iohmma {
	/// <summary>
	/// An implementation of a <see cref="T:ITransitionDistribution`1"/>
	/// </summary>
	/// <typeparam name='TInput'>The type of input on which the transition distribution depends.</typeparam>
	public abstract class TransitionDistribution<TInput,TOutput> : Distribution<Tuple<TInput,TOutput>>, ITransitionDistribution<TInput,TOutput> {

		#region ITransitionDistribution implementation
		/// <summary>
		/// Gets the probability density function for the given <paramref name="input"/> and the given output <paramref name="state"/>.
		/// </summary>
		/// <returns>The probability density function for the given input and the given output state.</returns>
		/// <param name="input">The given input to calculate the probability for.</param>
		/// <param name="state">The given output to calculate the probability for.</param>
		/// <remarks>
		/// <para>For each valid input, the sum of the probabilities of the output must sum up to one if the
		/// output is discrete. If the output is continu, the integral of the probabilities of the outputs must sum up to one.</para>
		/// </remarks>
		public abstract double GetPdf (TInput input, TOutput state);
		#endregion
		#region IDistribution implementation
		/// <summary>
		/// Gets the probability density of the given element.
		/// </summary>
		/// <returns>The probability density of the given element.</returns>
		/// <param name="x">The given element to compute the probability density from.</param>
		public override double GetPdf (Tuple<TInput, TOutput> x) {
			return this.GetPdf (x.Item1, x.Item2);
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution for the given input.
		/// </summary>
		/// <param name="input">The given input</param>
		/// <returns>A randomly chosen element in the set according to the probability density function and the input.</returns>
		/// <exception cref="ArgumentException">If the given input is not within the specified bounds.</exception>
		public abstract TOutput Sample (TInput input);
		#endregion
		#region ITransitionDistribution implementation
		/// <summary>
		/// Fit the distribution using the input-output data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of input-output data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>This method is a more convenient way to fit an <see cref="T:ITransitionDistribution`2"/> instance, but does nothing else
		/// than the <see cref="M:IDistribution`2.Fit"/> method.</para>
		/// <para>If the <paramref name="fitting"/> coefficient is one, only the new data is taken into account.
		/// If zero, only the old data.</para>
		/// <para>The given list of probabilities must sum up to one, if this is not the case, one should use the <see cref="M:ITransitionDistribution`2.FitUnnormalized"/> method.</para>
		/// <para>When implementing this method, please be aware that the same input may occur multiple times.</para>
		/// </remarks>
		public void Fit (IEnumerable<Tuple<TInput, TOutput, double>> probabilities, double fitting = 1.0) {
			this.Fit (probabilities.Select (x => new Tuple<Tuple<TInput,TOutput>,double> (new Tuple <TInput,TOutput> (x.Item1, x.Item2), x.Item3)), fitting);
		}

		/// <summary>
		/// Fit the distribution using the input-output data and their frequence, but it is not guaranteed that the probabilities sum
		/// up to one.
		/// </summary>
		/// <param name="probabilities">A list of input-output data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>This method is a more convenient way to fit an <see cref="T:ITransitionDistribution`2"/> instance, but does nothing else
		/// than the <see cref="M:IDistribution`2.FitUnnormalized"/> method.</para>
		/// <para>If the <paramref name="fitting"/> coefficient is one, only the new data is taken into account.
		/// If zero, only the old data.</para>
		/// <para>When implementing this method, please be aware that the same input may occur multiple times.</para>
		/// </remarks>
		public void FitUnnormalized (IEnumerable<Tuple<TInput, TOutput, double>> probabilities, double fitting = 1.0) {
			this.FitUnnormalized (probabilities.Select (x => new Tuple<Tuple<TInput,TOutput>,double> (new Tuple <TInput,TOutput> (x.Item1, x.Item2), x.Item3)), fitting);
		}
		#endregion
	}
}

