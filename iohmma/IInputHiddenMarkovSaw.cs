//
//  IInputHiddenMarkovSaw.cs
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
	/// A hidden Markov model with an input alphabet, hidden states and where the output tokens depend on two hidden states.
	/// </summary>
	/// <remarks>
	/// <para>This hidden Markov model introduces cycles which implies it is hard to learn models. Since the cycles are however
	/// cliques of three items, we expect we can approximate learning and furthermore .</para>
	/// </remarks>
	public interface IInputHiddenMarkovSaw : IHiddenStates {
	}
}

