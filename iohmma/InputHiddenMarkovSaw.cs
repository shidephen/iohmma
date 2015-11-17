//
//  InputHiddenMarkovSaw.cs
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
	/// An implementation of a hidden Markov model with saw structure. The learning is done approximately.
	/// </summary>
	/// <remarks>
	/// <para>The learning is done approximately. No guarantees are given that the obtained model is effective.</para>
	/// </remarks>
	public class InputHiddenMarkovSaw : IInputHiddenMarkovSaw {

		/// <summary>
		/// The number of hidden states (to be replaced with a probability vector).
		/// </summary>
		private readonly int nstates;

		#region IHiddenStates implementation

		/// <summary>
		/// Get the number of hidden states for this given Input-Output Hidden Markov Saw.
		/// </summary>
		/// <value>The number of hidden states considered by the Input-Output hidden Markov Saw.</value>
		public int NumberOfHiddenStates {
			get {
				return this.nstates;
			}
		}

		#endregion

		public InputHiddenMarkovSaw (int nhidden) {
			this.nstates = nhidden;
		}
	}
}

