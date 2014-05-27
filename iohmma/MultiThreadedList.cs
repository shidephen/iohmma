//
//  MultiThreadedList.cs
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
	/// A <see cref="ICollection"/> that supports 
	/// </summary>
	/// <typeparam name='TData'>
	/// The type of the items stored in the collection.
	/// </typeparam>
	public class MultiThreadedList<TData> : ICollection<TData> {

		private int count = 0x00;
		private MultiThreadedListItem First;
		private MultiThreadedListItem Last;
		#region ICollection implementation
		/// <summary>
		/// Gets the number of items the <see cref="T:ICollection`1"/> instance contains.
		/// </summary>
		/// <value>The number of items.</value>
		public int Count {
			get {
				return this.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly {
			get {
				return false;
			}
		}
		#endregion
		#region ICollection implementation
		public void Add (TData item) {
			throw new NotImplementedException ();
		}

		public void Clear () {
			throw new NotImplementedException ();
		}

		public bool Contains (TData item) {
			throw new NotImplementedException ();
		}

		public void CopyTo (TData[] array, int arrayIndex) {
			throw new NotImplementedException ();
		}

		public bool Remove (TData item) {
			throw new NotImplementedException ();
		}
		#endregion
		#region IEnumerable implementation
		public IEnumerator<TData> GetEnumerator () {
			throw new NotImplementedException ();
		}
		#endregion
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			throw new NotImplementedException ();
		}
		#endregion
		private class MultiThreadedListItem {

			public TData data;
			public MultiThreadedListItem Next;
		}
	}
}

