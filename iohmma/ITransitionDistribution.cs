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

namespace iohmma {
	/// <summary>
	/// An interface that specifies a distribution for state transitions depending on an input.
	/// </summary>
	/// <typeparam name='TData'>The type of data on which the transition distribution depends.</typeparam>
	public interface ITransitionDistribution<TData> : IHiddenStates, IDistribution<Tuple<TData,int>> {

		/// <summary>
		/// Gets the probability density function for the given <paramref name="input"/> and the given output <paramref name="state"/>.
		/// </summary>
		/// <returns>The probability density function for the given input and the given output state.</returns>
		/// <param name="input">The given input to calculate the probability for.</param>
		/// <param name="state">The given output state to calculate the probability for.</param>
		double GetPdf (TData input, int state);

		/// <summary>
		/// Generate a random element based on the density of the distribution for the given input.
		/// </summary>
		/// <param name="input">The given input</param>
		/// <returns>A randomly chosen element in the set according to the probability density function and the input.</returns>
		TData Sample (TData input);
	}
}