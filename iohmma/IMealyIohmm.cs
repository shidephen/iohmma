//
//  IMealyIohmm.cs
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
	/// An interface of a <see cref="T:IIohmm`2"/> using the Mealy flavor.
	/// </summary>
	/// <typeparam name='TInput'>The type of the input handled by the IOHMM.</typeparam>
	/// <typeparam name='TOutput'>The type of the output handled by the IOHMM.</typeparam>
	public interface IMealyIohmm<TInput,TOutput> : IIohmm<TInput,TOutput> {

		/// <summary>
		/// Gets the transition function discribing the emission from the given <paramref name="state"/>.
		/// </summary>
		/// <returns>The <see cref="T:ITransitionDistribution`2"/> function for the given state that describes
		/// the distribution with respect to the input.</returns>
		/// <param name="state">The given state for which the emission function must be returned.</param>
		ITransitionDistribution<TInput,TOutput> GetEmission (int state);

		/// <summary>
		/// Gets a list of input-output values together with the (unscaled) probabilities that would be used to train the emission probabilities Hidden Markov model for the given initial state.
		/// </summary>
		/// <returns>A list of input-output values together with the (unscaled) probabilities.</returns>
		/// <param name="inoutputs">A sequence of input-output values that would train the Hidden Markov model.</param>
		/// <param name="initialState">The given initial state.</param>
		IEnumerable<Tuple<Tuple<TInput,TOutput>,double>> CalculateNewEmission (IEnumerable<Tuple<TInput, TOutput>> inoutputs, int initialState);
	}
}