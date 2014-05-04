//
//  IDistribution.cs
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
	/// An interface describing a distribution on the <typeparamref name='TData'/> type.
	/// </summary>
	/// <typeparam name='TData'>The type of the distribution.</typeparam>
	/// <remarks>
	/// <para>A distribution is a mapping of a given set to real numbers where the real numbers where an element
	/// corresponds to its probability density.</para>
	/// <para>A density can be fitted given a sequence of values and their frequency. Samples can be derived
	/// from the distribution.</para>
	/// </remarks>
	public interface IDistribution<TData> {

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
		double GetPdf (TData x);

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		TData Sample ();

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>If the <paramref name="fitting"/> coefficient is one, only the new data is taken into account.
		/// If zero, only the old data.</para>
		/// </remarks>
		void Fit (IEnumerable<Tuple<TData,double>> probabilities, double fitting = 1.0d);
	}
}