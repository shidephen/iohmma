//
//  ScaledFittingTransitionDistribution.cs
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
	/// A class representing a <see cref="T:TransitionDistribution`2"/> instance, but where fitting the transition distribution
	/// unnormalized is actual the same as futting the transition distribution as if it was normalized.
	/// </summary>
	public abstract class ScaledFittingTransitionDistribution<TInput,TOutput> : TransitionDistribution<TInput,TOutput> {

		#region FitUnnormalized implementation
		/// <summary>
		/// Fit the distribution using the data and their frequence, but it is not guaranteed that the probabilities sum
		/// up to one.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>The fitting of an unnormalized list is in this setting the same as fitting a normalized list.</para>
		/// </remarks>
		public override void FitUnnormalized (System.Collections.Generic.IEnumerable<Tuple<Tuple<TInput, TOutput>, double>> probabilities, double fitting) {
			this.Fit (probabilities, fitting);
		}
		#endregion
	}
}

