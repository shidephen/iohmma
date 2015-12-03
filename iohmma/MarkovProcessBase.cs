//
//  MarkovProcessBase.cs
//
//  Author:
//       Willem Van Onsem <vanonsem.willem@gmail.com>
//
//  Copyright (c) 2015 Willem Van Onsem
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
	/// A basic implementation of a Markov process. This contains the commonalities most Markov processes share.
	/// </summary>
	public class MarkovProcessBase : IMarkovProcess {

		/// <summary>
		/// The internal discrete distribution for the first state (sometimes referred to as pi).
		/// </summary>
		protected readonly DiscreteDistribution Pi;

		#region IHiddenStates implementation

		/// <summary>
		/// Get the number of hidden states for this given Input-Output Hidden Markov Saw.
		/// </summary>
		/// <value>The number of hidden states considered by the Input-Output hidden Markov Saw.</value>
		public int NumberOfHiddenStates {
			get {
				return this.Pi.NumberOfHiddenStates;
			}
		}

		#endregion

		#region IMarkovProcess implementation

		/// <summary>
		/// Gets the initial state distribution of the given state index.
		/// </summary>
		/// <returns>The initial distribution for the given index of the hidden states.</returns>
		/// <param name="index">The given state index.</param>
		/// <exception cref="IndexOutOfRangeException">If the given index is smaller than zero (<c>0</c>).</exception>
		/// <exception cref="IndexOutOfRangeException">If the given index is larger than or equal to the number of hidden states (<see cref="NumberOfHiddenStates"/>).</exception>
		public double GetPi (int index) {
			return this.Pi.GetPdf (index);
		}

		/// <summary>
		/// Resets the probability of being in a certain state at the first time stamp.
		/// </summary>
		public void ResetPi () {
			this.Pi.Reset ();
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.MarkovProcessBase"/> class with the given number of hidden states.
		/// </summary>
		/// <param name="nhidden">The number of hidden states for the initialized hidden Markov saw.</param>
		public MarkovProcessBase (int nhidden) {
			this.Pi = new DiscreteDistribution (nhidden);
		}

	}
}

