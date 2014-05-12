//
//  Iohmm.cs
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
	/// A basic implementation of the <see cref="T:IIohmm`2"/> interface that represents an Input-Output Hidden Markov Model (IOHMM).
	/// </summary>
	/// <typeparam name='TInput'>The type of the input handled by the IOHMM.</typeparam>
	/// <typeparam name='TOutput'>The type of the output handled by the IOHMM.</typeparam>
	public abstract class Iohmm<TInput,TOutput> : IIohmm<TInput,TOutput> {

		private readonly double[] pi;
		#region IIohmm implementation
		/// <summary>
		/// Gets the number of hidden states.
		/// </summary>
		/// <value>The number of hidden states.</value>
		/// <remarks>
		/// <para>The number of hidden states is always larger than zero.</para>
		/// </remarks>
		public int NumberOfHiddenStates {
			get {
				return this.pi.Length;
			}
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		protected Iohmm (int numberOfHiddenStates) {
			if (numberOfHiddenStates <= 0x00) {
				throw new ArgumentException ("The number of hidden states must be greater than zero.");
			}
			double[] ps = new double[numberOfHiddenStates];
			double psi = 1.0d / numberOfHiddenStates;
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				ps [i] = psi;
			}
			this.pi = ps;
		}
		#endregion
		#region IIohmm implementation
		/// <summary>
		/// Gets the transition function describing the transition from the given <paramref name="state"/>.
		/// </summary>
		/// <returns>The <see cref="T:ITransitionDistribution`2"/> function for the given state.</returns>
		/// <param name="state">The given state for which the transition function must be returned.</param>
		public abstract ITransitionDistribution<TInput,int> GetTransition (int state);

		/// <summary>
		/// Gets the initial state distribution of the given state index.
		/// </summary>
		/// <returns>The initial distribution for the given index of the hidden states.</returns>
		/// <param name="index">The given state index.</param>
		/// <exception cref="IndexOutOfRangeException">If the given index is smaller than zero (<c>0</c>).</exception>
		/// <exception cref="IndexOutOfRangeException">If the given index is larger than or equal to the number of hidden states (<see cref="NumberOfHiddenStates"/>).</exception>
		public double GetPi (int index) {
			return this.pi [index];
		}

		/// <summary>
		/// Gets the probability of migrating from state <paramref name="statei"/> to <paramref name="statej"/> given
		/// the <paramref name="input"/>.
		/// </summary>
		/// <returns>The probability of the transition from the first index to the second given the index.</returns>
		/// <param name="input">The given input for the transition.</param>
		/// <param name="statei">The origin hidden state.</param>
		/// <param name="statej">The target hidden state.</param>
		public abstract double GetA (TInput input, int statei, int statej);

		/// <summary>
		/// Gets the probability of exhaust of <paramref name="output"/> given <paramref name="input"/> and
		/// current <paramref name="state"/>
		/// </summary>
		/// <returns>The probability of the output given the input and state.</returns>
		/// <param name="input">The input for this time stamp.</param>
		/// <param name="state">The current state of the hidden Markov model.</param>
		/// <param name="output">The assumed output for this time stamp.</param>
		public abstract double GetB (TInput input, int state, TOutput output);

		/// <summary>
		/// Calculates the probability of the output sequence given the input sequence for this hidden Markov model.
		/// </summary>
		/// <returns>The probability of the sequence of outputs given the sequence of inputs.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public abstract double Probability (IEnumerable<Tuple<TInput,TOutput>> inoutputs);

		/// <summary>
		/// Train this hidden Markov model with the given sequence of inputs and outputs.
		/// </summary>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public abstract void Train (IEnumerable<Tuple<TInput,TOutput>> inoutputs);

		/// <summary>
		/// Returns the most likely sequence of the hidden state of sequences for the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>The most likely sequence of hidden states.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public abstract IEnumerable<int> MostLikelyHiddenStateSequence (IEnumerable<Tuple<TInput,TOutput>> inoutputs);

		/// <summary>
		/// Generate a sequence of observations based on the given sequence of <paramref name="inputs"/> and the <see cref="T:IIohm`2"/>.
		/// </summary>
		/// <returns>A sequence of observations based on the given input.</returns>
		/// <param name="inputs">A <see cref="T:IEnumerable`1"/> of inputs.</param>
		public abstract IEnumerable<TOutput> GenerateObservationSequence (IEnumerable<TInput> inputs);

		/// <summary>
		/// Calculate the alpha values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the alpha values after each stage.</returns>
		/// <param name="inoutputs">A list of tuples containing the input and the appropriate output.</param>
		/// <remarks>
		/// <para>The output list is as long as the <paramref name="inoutputs"/> list.</para>
		/// <para>The values are computed lazily, infinite sequence are possible.</para>
		/// </remarks>
		public abstract IEnumerable<double[]> CalculateAlphas (IEnumerable<Tuple<TInput,TOutput>> inoutputs);

		/// <summary>
		/// Calculate the beta values based on the given reversed sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the beta values after each stage from end to begin.</returns>
		/// <param name="reversedinoutputs">A list of tuples containing the input and the appropriate output, the order
		/// is reversed: the first tuple contains the last observation.</param>
		/// <remarks>
		/// <para>The output list is as long as the <paramref name="reversedinoutputs"/> list.</para>
		/// <para>The values are computed lazily, infinite sequence are possible but the values should be reversed.</para>
		/// </remarks>
		public abstract IEnumerable<double[]> CalculateBetasReverse (IEnumerable<Tuple<TInput,TOutput>> reversedinoutputs);
		#endregion
	}
}

