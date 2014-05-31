//
//  IInputIndexMapping.cs
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
	/// An interface that specifies that the instance accepts some kind of input, that is converted to an index
	/// such that the index can be used internally.
	/// </summary>
	/// <typeparam name='TInput'> The type of input that must be converted to indices.</typeparam>
	public interface IInputIndexMapping<TInput> {
		
		/// <summary>
		/// A function that transforms input into their corresponding index. This is used by several methods
		/// to translate the input such that the implementation remains generic.
		/// </summary>
		/// <value>A function mapping inputs to indices.</value>
		Func<TInput,int> InputMapper {
			get;
		}

		/// <summary>
		/// A function that transforms indices into their corresponding input. This is used by several methods
		/// to translate the input such that the implementation remains generic.
		/// </summary>
		/// <value>A function mapping indices to inputs.</value>
		Func<int,TInput> IndexMapper {
			get;
		}
	}
}

