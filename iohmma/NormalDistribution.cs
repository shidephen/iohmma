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
using NUtils.Maths;

namespace iohmma {
	/// <summary>
	/// An implementation of the <see cref="T:INormalDistribution`1"/> interface on scalar floating point numbers.
	/// </summary>
	public class NormalDistribution : ScaledFittingDistribution<double>, INormalDistribution<double> {

		private double sigma = 1.0d;

		#region INormalDistribution implementation

		/// <summary>
		/// Gets or sets the mean of the <see cref="T:iohmma.INormalDistribution`1"/>.
		/// </summary>
		/// <value>The mean of the <see cref="T:iohmma.INormalDistribution`1"/>.</value>
		public double Mean {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the standard deviation of the <see cref="T:iohmma.INormalDistribution`1"/>.
		/// </summary>
		/// <value>The standard deviation of the <see cref="T:iohmma.INormalDistribution`1"/>.</value>
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
		public override double GetPdf (double x) {
			double si = 1 / this.sigma;
			double z = (x - this.Mean);
			z *= z;
			z *= -0.5d * si * si;
			return Math.Exp (z) * si * MathUtils.InvSqrt2Pi;
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <param name="rand">The random generator that is used to sample. In case <c>null</c> is given, the <see cref="T:iohmma.StaticRandom"/> generator is used.</param>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		public override double Sample (Random rand = null) {
			rand = (rand ?? StaticRandom.GetInstance ());
			double u1 = rand.NextDouble ();
			double u2 = rand.NextDouble ();
			return this.Mean + this.sigma * Math.Sqrt (-2.0 * Math.Log (u1)) * Math.Sin (2.0 * Math.PI * u2);
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		/// <remarks>
		/// <para>If no data is provided, the distribution is not midified.</para>
		/// </remarks>
		public override void Fit (IEnumerable<Tuple<double, double>> probabilities, double fitting = 1.0) {
			double mean = 0.0d;
			double stdv = 0.0d;
			double fittinh = 1.0d - fitting;
			double x, p;
			bool data = false;
			foreach (Tuple<double,double> item in probabilities) {
				x = item.Item1;
				p = item.Item2;
				mean += p * x;
				data = true;
			}
			if (data) {
				double sq = mean * mean;
				foreach (Tuple<double,double> item in probabilities) {
					x = item.Item1;
					p = item.Item2;
					stdv += p * (x * x - sq);
				}
				this.Mean = fitting * mean + fittinh * this.Mean;
				this.Sigma = fitting * stdv + fittinh * this.Sigma;
			}
		}

		#endregion

		#region ToString method

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="NormalDistribution"/>.
		/// </summary>
		/// <returns>A <see cref="String"/> that represents the current <see cref="NormalDistribution"/>.</returns>
		/// <remarks>
		/// <para>The format of the returned text is <para>N(0,1)</para> where <c>0</c> is substituted by
		/// the actual mean and <c>1</c> by the actual standard deviation.</para>
		/// </remarks>
		public override string ToString () {
			return string.Format ("~N({0};{1})", this.Mean, this.Sigma);
		}

		#endregion
	}
}

