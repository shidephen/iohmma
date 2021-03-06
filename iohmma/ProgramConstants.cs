//
//  ProgramConstants.cs
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
	/// A utility class that stores a number of constants used by different algorithms in the program.
	/// </summary>
	public static class ProgramConstants {

		#region Constants
		/// <summary>
		/// The tolaterated difference between one and the sum of the given probabilities in constructors, methods, etc.
		/// </summary>
		/// <remarks>
		/// <para>If the sum of the items does not equal one (with a tolerance of epsilon), a <see cref="ArgumentException"/> will be thrown.</para>
		/// </remarks>
		public const double Epsilon = 1e-6d;
		#endregion
	}
}

