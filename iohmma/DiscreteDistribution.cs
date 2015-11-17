//
//  DiscreteDistribution.cs
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
using System.Collections.Generic;
using NUtils.Functional;

namespace iohmma {

	/// <summary>
	/// An implementation of the <see cref="IDiscreteDistribution"/> interface: a distribution with discrete
	/// probabilities. The items are called with indices.
	/// </summary>
	public class DiscreteDistribution : IDiscreteDistribution, IFinite<int> {

		/// <summary>
		/// The array of probabilities, represented internally.
		/// </summary>
		private readonly double[] ps;

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.DiscreteDistribution"/> class with a given number of items.
		/// </summary>
		/// <param name="n">N.</param>
		public DiscreteDistribution (int n) {
			double[] ps = new double[n];
			double pi = 1.0d / n;
			for (int i = 0x00; i < n; i++) {
				ps [i] = pi;
			}
			this.ps = ps;
		}

		#region IDistribution implementation

		/// <summary>
		/// Gets the probability density of the given element.
		/// </summary>
		/// <returns>The probability density of the given element.</returns>
		/// <param name="x">The given element to compute the probability density from.</param>
		/// <exception cref="ArgumentException">If the given element is not within bounds.</exception>
		public double GetPdf (int x) {
			return this.ps [x];
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <param name="rand">The random generator that is used to sample. In case <c>null</c> is given, the <see cref="T:iohmma.StaticRandom"/> generator is used.</param>
		/// <returns>An element according to the probability density function described by this probability.<returns>
		/// <remarks>The amortized time complexity of this method is constant time.</remarks>
		public int Sample (Random rand = null) {
			rand = ((rand != null) ? rand : StaticRandom.GetInstance ());
			double[] ps = this.ps;
			int n = ps.Length, i;
			double p;
			do {
				p = rand.NextDouble ();//TODO: fit with maximum probability for better efficiency
				i = rand.Next (n);
			} while(ps [i] < p);
			return i;
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public void Fit (IEnumerable<Tuple<int, double>> probabilities, double fitting = 1.0) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Fit the distribution using the data and their frequence, but it is not guaranteed that the probabilities sum
		/// up to one.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public void FitUnnormalized (IEnumerable<Tuple<int, double>> probabilities, double fitting = 1.0) {
			throw new NotImplementedException ();
		}

		#endregion


		#region IFinite implementation

		/// <summary>
		/// Enumerates all the elements of this instance.
		/// </summary>
		public IEnumerable<int> Elements () {
			int n = this.ps.Length;
			for (int i = 0x00; i < n; i++) {
				yield return i;
			}
		}

		#endregion
	}
}

