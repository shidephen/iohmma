//
//  IIohmm.cs
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
	/// An interface specifying an Input-output Hidden Markov Model (IOHMM).
	/// </summary>
	/// <typeparam name='TInput'>The type of the input handled by the IOHMM.</typeparam>
	/// <typeparam name='TOutput'>The type of the output handled by the IOHMM.</typeparam>
	public interface IIohmm<TInput,TOutput> {

		/// <summary>
		/// Gets the number of hidden states.
		/// </summary>
		/// <value>The number of hidden states.</value>
		int NumberOfHiddenStates {
			get;
		}

		/// <summary>
		/// Resets the number of hidden states by the given amount
		/// </summary>
		/// <param name="numberOfHiddenStates">The new number of hidden states.</param>
		/// <exception cref="ArgumentException">If the given number is smaller than one.</exception>
		void Resize (int numberOfHiddenStates);

		/// <summary>
		/// Gets the initial state distribution of the given state index.
		/// </summary>
		/// <returns>The initial distribution for the given index of the hidden states.</returns>
		/// <param name="index">The given state index.</param>
		double GetPi (int index);
	}
}