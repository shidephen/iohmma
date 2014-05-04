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

namespace iohmma {
	/// <summary>
	/// An implementation of a <see cref="T:ITransitionDistribution`1"/>
	/// </summary>
	/// <typeparam name='TData'>The type of data on which the transition distribution depends.</typeparam>
	public abstract class TransitionDistribution<TData> : ITransitionDistribution<TData> {

		#region IHiddenStates implementation
		/// <summary>
		/// Gets the number of involved hidden states.
		/// </summary>
		/// <value>The number of involved hidden states.</value>
		public abstract int NumberOfHiddenStates {
			get;
		}
		#endregion
		#region ITransitionDistribution implementation
		/// <summary>
		/// Gets the probability density function for the given <paramref name="input"/> and the given output <paramref name="state"/>.
		/// </summary>
		/// <returns>The probability density function for the given input and the given output state.</returns>
		/// <param name="input">The given input to calculate the probability for.</param>
		/// <param name="state">The given output state to calculate the probability for.</param>
		public abstract double GetPdf (TData input, int state);
		#endregion
		#region IDistribution implementation
		/// <summary>
		/// Gets the probability density of the given element.
		/// </summary>
		/// <returns>The probability density of the given element.</returns>
		/// <param name="x">The given element to compute the probability density from.</param>
		public double GetPdf (Tuple<TData, int> x) {
			return this.GetPdf (x.Item1, x.Item2);
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		public abstract Tuple<TData, int> Sample ();

		/// <summary>
		/// Generate a random element based on the density of the distribution for the given input.
		/// </summary>
		/// <param name="input">The given input</param>
		/// <returns>A randomly chosen element in the set according to the probability density function and the input.</returns>
		/// <exception cref="ArgumentException">If the given input is not within the specified bounds.</exception>
		public abstract TData Sample (TData input);

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public abstract void Fit (IEnumerable<Tuple<Tuple<TData, int>, double>> probabilities, double fitting = 1.0);
		#endregion
	}
}

