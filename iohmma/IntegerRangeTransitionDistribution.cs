//
//  IntegerRangeTransitionDistribution.cs
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
	/// An implementation of a <see cref="T:ITransitionDistribution`1"/> that uses a range of integers as input.
	/// </summary>
	public class IntegerRangeTransitionDistribution : TransitionDistribution<int>, IRange<int> {

		private readonly double[,] probabilities;
		#region IRange implementation
		/// <summary>
		/// Gets the lower value of the <see cref="T:IRange`1"/>.
		/// </summary>
		/// <value>The lower bound on the range.</value>
		/// <remarks>
		/// <para>The lower bound must be less than or equal to the <see cref="Upper"/> bound.</para>
		/// </remarks>
		public int Lower {
			get;
			private set;
		}

		/// <summary>
		/// Gets the upper value of the <see cref="T:IRange`1"/>.
		/// </summary>
		/// <value>The upper bound on the range.</value>
		/// <remarks>
		/// <para>The upper bound must be greater than or equal to the <see cref="Lower"/> bound.</para>
		/// </remarks>
		public int Upper {
			get;
			private set;
		}
		#endregion
		#region implemented abstract members of TransitionDistribution
		/// <summary>
		/// Gets the number of hidden states involved.
		/// </summary>
		/// <value>The number of hidden states involved.</value>
		public override int NumberOfHiddenStates {
			get {
				return this.probabilities.GetLength (0x01);
			}
		}
		#endregion
		#region Constructors
		public IntegerRangeTransitionDistribution (int upper, int numberOfHiddenStates) : this(0x01,upper,numberOfHiddenStates) {
		}

		public IntegerRangeTransitionDistribution (int lower, int upper, int numberOfHiddenStates) {
		}
		#endregion
		#region implemented abstract members of TransitionDistribution
		/// <summary>
		/// Gets the probability density function for the given <paramref name="input"/> and the given output <paramref name="state"/>.
		/// </summary>
		/// <returns>The probability density function for the given input and the given output state.</returns>
		/// <param name="input">The given input to calculate the probability for.</param>
		/// <param name="state">The given output state to calculate the probability for.</param>
		public override double GetPdf (int input, int state) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Generate a random element based on the density of the distribution.
		/// </summary>
		/// <returns>A randomly chosen element in the set according to the probability density function.</returns>
		public override Tuple<int, int> Sample () {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Fit the distribution using the data and their frequency.
		/// </summary>
		/// <param name="probabilities">A list of data together with the observed probabilities.</param>
		/// <param name="fitting">The fitting coefficient.</param>
		public override void Fit (IEnumerable<Tuple<Tuple<int, int>, double>> probabilities, double fitting = 1.0) {
			throw new NotImplementedException ();
		}
		#endregion
	}
}