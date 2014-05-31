//
//  ScaledFittingDistribution.cs
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
	/// A basic implementation of the <see cref="T:IDistribution`1"/> interface, but where the default fitting
	/// operator is oblivious to the actual sum of the elements. This might improve performance for fitting
	/// using the <see cref="FitUnnormalized"/> method.
	/// </summary>
	public abstract class ScaledFittingDistribution<TData> : Distribution<TData> {

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ScaledFittingDistribution`1"/> class.
		/// </summary>
		protected ScaledFittingDistribution () {
		}
		#region Distribution implementation
		/// <summary>
		/// Fit the distribution using the data and their frequence, but it is not guaranteed that the probabilities sum
		/// up to one.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>If no data is provided, the distribution is not midified.</para>
		/// <para>If the <paramref name="fitting"/> coefficient is one, only the new data is taken into account.
		/// If zero, only the old data.</para>
		/// </remarks>
		public override void FitUnnormalized (IEnumerable<Tuple<TData, double>> probabilities, double fitting = 1.0) {
			this.Fit (probabilities, fitting);
		}
		#endregion
	}
}