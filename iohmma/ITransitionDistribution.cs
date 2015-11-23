//
//  ITransitionDistribution.cs
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
	/// An interface that specifies a distribution for state transitions depending on an input.
	/// </summary>
	/// <typeparam name='TInput'>The type of input on which the transition distribution depends.</typeparam>
	/// <typeparam name='TOutput'>The type of output on which the transition distribution depends.</typeparam>
	public interface ITransitionDistribution<TInput,TOutput> : IDistribution<Tuple<TInput,TOutput>> {

		/// <summary>
		/// Gets the probability density function for the given <paramref name="input"/> and the given output <paramref name="state"/>.
		/// </summary>
		/// <returns>The probability density function for the given input and the given output state.</returns>
		/// <param name="input">The given input to calculate the probability for.</param>
		/// <param name="output">The given output to calculate the probability for.</param>
		double GetPdf (TInput input, TOutput output);

		/// <summary>
		/// Generate a random element based on the density of the distribution for the given input.
		/// </summary>
		/// <param name="input">The given input</param>
		/// <param name="rand">The given random generator to sample from. If <c>null</c>, <see cref="T:iohmma.StaticRandom"/> will be used.</param>
		/// <returns>A randomly chosen element in the set according to the probability density function and the input.</returns>
		TOutput Sample (TInput input, Random rand = null);

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
		void Fit (IEnumerable<Tuple<TInput,TOutput,double>> probabilities, double fitting = 1.0d);

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
		void FitUnnormalized (IEnumerable<Tuple<TInput,TOutput,double>> probabilities, double fitting = 1.0d);
	}
}