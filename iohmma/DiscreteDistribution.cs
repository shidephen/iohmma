﻿//
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
	public class DiscreteDistribution : IDiscreteDistribution, IFinite<int>, IHiddenStates {

		/// <summary>
		/// The array of probabilities, represented internally.
		/// </summary>
		private readonly double[] ps;

		#region IHiddenStates implementation

		/// <summary>
		/// Get the number of elements of the discrete distribution.
		/// </summary>
		/// <value>The number of hidden states of the discrete distribution.</value>
		public int NumberOfHiddenStates {
			get {
				return this.ps.Length;
			}
		}

		#endregion

		/// <summary>
		/// Get the probability of the given <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The index of the element to calculate the probability from.</param>
		public double this [int index] {
			get {
				return this.ps [index];
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.DiscreteDistribution"/> class with a given number of items.
		/// </summary>
		/// <param name="nstates">The number of items over which the discrete distribution is working.</param>
		public DiscreteDistribution (int nstates) {
			double[] pst = new double[nstates];
			double pi = 1.0d / nstates;
			for (int i = 0x00; i < nstates; i++) {
				pst [i] = pi;
			}
			this.ps = pst;
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
		/// <returns>An element according to the probability density function described by this probability.</returns>
		/// <remarks>The amortized time complexity of this method is constant time.</remarks>
		public int Sample (Random rand = null) {
			rand = (rand ?? StaticRandom.GetInstance ());
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
			double[] ps = this.ps;
			int n = ps.Length;
			double[] psn = new double[n];
			double sum = 0.0d;
			foreach (Tuple<int,double> pi in probabilities) {
				psn [pi.Item1] += pi.Item2;
				sum += pi.Item2;
			}
			sum = 1.0d / sum;
			double cof = 1.0d - fitting;
			for (int i = 0x00; i < n; i++) {
				ps [i] = cof * ps [i] + sum * fitting * psn [i];
			}
		}

		/// <summary>
		/// Fit the distribution using the data and their frequence, but it is not guaranteed that the probabilities sum
		/// up to one.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public void FitUnnormalized (IEnumerable<Tuple<int, double>> probabilities, double fitting = 1.0) {
			this.Fit (probabilities, fitting);
		}

	    public void Randomize(Random rand = null)
	    {
	        throw new NotImplementedException();
	    }

	    public void Reset()
	    {
	        throw new NotImplementedException();
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

