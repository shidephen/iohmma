//
//  NormalDistribution.cs
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
	/// An implementation of the <see cref="T:INormalDistribution`1"/> interface on scalar floating point numbers.
	/// </summary>
	public class NormalDistribution : INormalDistribution<double> {

		private double sigma = 1.0d;
		#region INormalDistribution implementation
		/// <summary>
		/// Gets or sets the mean of the <see cref="T:INormalDistribution`1"/>.
		/// </summary>
		/// <value>The mean of the <see cref="T:INormalDistribution`1"/>.</value>
		public double Mean {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the standard deviation of the <see cref="T:INormalDistribution`1"/>.
		/// </summary>
		/// <value>The standard deviation of the <see cref="T:INormalDistribution`1"/>.</value>
		/// <exception cref="ArgumentException">If the given value for sigma is smaller than zero.</exception>
		/// <remarks>
		/// <para>The value is always larger than zero.</para>
		/// </remarks>
		public double Sigma {
			get {
				return this.sigma;
			}
			set {
				if (value <= 0.0d) {
					throw new ArgumentException ("The given standard deviation must be larger than or equal to zero.");
				}
				this.sigma = value;
			}
		}
		#endregion
		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.NormalDistribution"/> class with a given mean and standard
		/// devation.
		/// </summary>
		/// <param name="mean">The given initial mean of the distribution.</param>
		/// <param name="sigma">The given initial standard devation of the distribution.</param>
		/// <exception cref="ArgumentException">If the given value for <paramref name="sigma"/> is smaller than zero.</exception>
		public NormalDistribution (double mean = 0.0d, double sigma = 1.0d) {
			this.Mean = mean;
			this.Sigma = sigma;
		}
		#region IDistribution implementation
		/// <summary>
		/// Gets the probability density of the given element.
		/// </summary>
		/// <returns>The probability density of the given element.</returns>
		/// <param name="x">The given element to compute the probability density from.</param>
		/// <exception cref="ArgumentException">If the given element is not within bounds.</exception>
		public double GetPdf (double x) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		public double Sample () {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public void Fit (IEnumerable<Tuple<double, double>> probabilities, double fitting = 1.0) {//TODO
			double mean = 0.0d;
			double stdv = 0.0d;
			double fittinh = 1.0d - fitting;
			double x, p;
			foreach (Tuple<double,double> item in probabilities) {
				x = item.Item1;
				p = item.Item2;
				mean += p * x;
			}
			double sq = mean * mean;
			foreach (Tuple<double,double> item in probabilities) {
				x = item.Item1;
				p = item.Item2;
				stdv += p * (x * x - sq);
			}
			this.Mean = fitting * mean + fittinh * this.Mean;
			this.Sigma = fitting * stdv + fittinh * this.Sigma;
		}
		#endregion
	}
}

