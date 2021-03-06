//
//  IFinite.cs
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
	/// An interface that specifies that the instance contains a finite number of elements.
	/// </summary>
	/// <typeparam name='TElement'>The type of the elements in the set.</typeparam>
	public interface IFinite<out TElement> {

		/// <summary>
		/// Enumerates all the elements of this instance.
		/// </summary>
		/// <remarks>
		/// <para>The <see cref="T:IEnumerable`1"/> should be guaranteed to be finite.</para>
		/// </remarks>
		IEnumerable<TElement> Elements ();
	}
}

