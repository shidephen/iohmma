﻿//
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
	public class InputHiddenMarkovSaw : MarkovProcessBase, IInputHiddenMarkovSaw {

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.InputHiddenMarkovSaw"/> class with the given number of hidden states.
		/// </summary>
		/// <param name="nhidden">The number of hidden states for the initialized hidden Markov saw.</param>
		public InputHiddenMarkovSaw (int nhidden) : base (nhidden) {
		}
	}
}

