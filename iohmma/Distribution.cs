//
//  Distribution.cs
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
using NUtils.Functional;

namespace iohmma {
	/// <summary>
	/// A basic implementation of the <see cref="T:IDistribution`1"/> interface.
	/// </summary>
	/// <typeparam name='TData'>
	/// The type of data over which the distribution spans.
	/// </typeparam>
	public abstract class Distribution<TData> : IDistribution<TData> {

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Distribution`1"/> class.
		/// </summary>
		protected Distribution () {
		}

		/// <summary>
		/// A utility method for the <see cref="M:iohmma.Distribution`1.Randomize"/> method where <paramref name="rand"/> is guaranteed to be effective.
		/// Randomize this distribution. This can be used if the given model does not make much sense, but it
		/// is somehow computationally expensive or impossible to improve the situation using fitting.
		/// </summary>
		/// <param name="rand">The random number generator to be used to randomize the distribution.
		/// This parameter is always effective.</param>
		protected abstract void RandomizeEffective (Random rand);

		#region IDistribution implementation

		/// <summary>
		/// Gets the probability density of the given element.
		/// </summary>
		/// <returns>The probability density of the given element.</returns>
		/// <param name="x">The given element to compute the probability density from.</param>
		/// <exception cref="ArgumentException">If the given element is not within bounds.</exception>
		/// <remarks>
		/// <para>The probability density function of any element must always be larger than or equal to zero.</para>
		/// <para>In case the <typeparamref name='TData'/> is discrete, the sum of the probability densities
		/// is equal to one. In case it is continuous, the integral of the probability density function is equal to one.</para>
		/// </remarks>
		public abstract double GetPdf (TData x);

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <param name="rand">The random generator that is used to sample. In case <c>null</c> is given, the <see cref="T:iohmma.StaticRandom"/> generator is used.</param>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		public abstract TData Sample (Random rand = null);

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>If no data is provided, the distribution is not midified.</para>
		/// <para>If the <paramref name="fitting"/> coefficient is one, only the new data is taken into account.
		/// If zero, only the old data.</para>
		/// <para>The given list of probabilities must sum up to one, if this is not the case, one should use the <see cref="FitUnnormalized"/> method.</para>
		/// </remarks>
		public abstract void Fit (IEnumerable<Tuple<TData, double>> probabilities, double fitting = 1.0);

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
		public virtual void FitUnnormalized (IEnumerable<Tuple<TData, double>> probabilities, double fitting = 1.0) {
			this.Fit (probabilities.Normalize (), fitting);
		}

		/// <summary>
		/// Randomize this distribution. This can be used if the given model does not make much sense, but it
		/// is somehow computationally expensive or impossible to improve the situation using fitting.
		/// </summary>
		/// <param name="rand">The random number generator to be used to randomize the distribution.
		/// If <c>null</c>, <see cref="T:iohmma.StaticRandom"/> is used.</param>
		public virtual void Randomize (Random rand = null) {
			this.Randomize (rand ?? StaticRandom.Random);
		}

	    public void Reset()
	    {
	        throw new NotImplementedException();
	    }

	    #endregion
	}
}

