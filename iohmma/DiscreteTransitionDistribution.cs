//
//  DiscreteTransitionDistribution.cs
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
	/// An implementation of the <see cref="IDiscreteTransitionDistribution"/> interface: an interface that
	/// defines a transition distribution: a matrix that carries probabilities that can be trained.
	/// </summary>
	public class DiscreteTransitionDistribution : TransitionDistribution<int,int>, IDiscreteTransitionDistribution {

		private readonly DiscreteDistribution[] distributions;

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.DiscreteTransitionDistribution"/> class with a given number of states.
		/// </summary>
		/// <param name="nstates">The given number of states: the number of input and the number of output tokens.</param>
		public DiscreteTransitionDistribution (int nstates) : this (nstates, nstates) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="iohmma.DiscreteTransitionDistribution"/> class with a given number of input tokens and a given number of output tokens.
		/// </summary>
		/// <param name="ninput">The given number of input tokens.</param>
		/// <param name="noutput">The given number of output tokens.</param>
		public DiscreteTransitionDistribution (int ninput, int noutput) {
			DiscreteDistribution[] dists = new DiscreteDistribution[ninput];
			for (int i = 0x00; i < ninput; i++) {
				dists [i] = new DiscreteDistribution (noutput);
			}
			this.distributions = dists;
		}

		#region ITransitionDistribution implementation

		/// <summary>
		/// Get the probability density for the given output, given the given input.
		/// </summary>
		/// <returns>The probability density for the given output, given the given input.</returns>
		/// <param name="input">The given input to reason about.</param>
		/// <param name="output">The given output to reason about.</param>
		public override double GetPdf (int input, int output) {
			return this.distributions [input].GetPdf (output);
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution for the given input.
		/// </summary>
		/// <param name="input">The given input</param>
		/// <param name="rand">The random generator to use, if <c>null</c>, <see cref="T:iohmma.StaticRandom"/> is used.</param>
		public override int Sample (int input, Random rand = null) {
			return this.distributions [input].Sample (rand);
		}

		#endregion

		#region implemented abstract members of Distribution

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <returns></returns>
		/// <param name="rand">The given random generator to generate the sample. If <c>null</c>, <see cref="T:iohmma.StaticRandom"/> is used.</param>
		public override Tuple<int, int> Sample (Random rand = null) {
			Random rnd = rand ?? StaticRandom.Random;
			DiscreteDistribution[] dst = this.distributions;
			int ti = rnd.Next (dst.Length);
			int to = dst [ti].Sample (rnd);
			return new Tuple<int, int> (ti, to);
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public override void Fit (IEnumerable<Tuple<Tuple<int, int>, double>> probabilities, double fitting = 1.0) {
			DiscreteDistribution[] dst = this.distributions;
			int n = dst.Length;
			for (int i = 0x00; i < n; i++) {
				dst [i].FitUnnormalized (probabilities.Where (x => x.Item1.Item1 == i).Select (x => new Tuple<int,double> (x.Item1.Item2, x.Item2)), fitting);
			}
		}

		/// <summary>
		/// Fit the distribution using the data and their frequence, but it is not guaranteed that the probabilities sum
		/// up to one.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public override void FitUnnormalized (IEnumerable<Tuple<Tuple<int, int>, double>> probabilities, double fitting = 1.0) {
			DiscreteDistribution[] dst = this.distributions;
			int n = dst.Length;
			for (int i = 0x00; i < n; i++) {
				dst [i].FitUnnormalized (probabilities.Where (x => x.Item1.Item1 == i).Select (x => new Tuple<int,double> (x.Item1.Item2, x.Item2)), fitting);
			}
		}

		#endregion
	}
}

