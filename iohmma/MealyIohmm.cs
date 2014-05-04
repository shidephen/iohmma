//
//  MealyIohmm.cs
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
	/// An implementation of the <see cref="T:IMealyIohmm`2"/> interface. A hidden Markov model using the
	/// Mealy flavor.
	/// </summary>
	/// <typeparam name='TInput'>The type of the input handled by the IOHMM.</typeparam>
	/// <typeparam name='TOutput'>The type of the output handled by the IOHMM.</typeparam>
	public class MealyIohmm<TInput,TOutput> : Iohmm<TInput,TOutput>, IMealyIohmm<TInput,TOutput> {

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		public MealyIohmm (int numberOfHiddenStates) : base(numberOfHiddenStates) {
		}
		#endregion
		#region IMealyIohmm implementation
		/// <summary>
		/// Gets the transition function describing the transition from the given <paramref name="state"/>.
		/// </summary>
		/// <returns>The <see cref="T:ITransitionDistribution`1"/> function for the given state.</returns>
		/// <param name="state">The given state for which the transition function must be returned.</param>
		public ITransitionDistribution<TInput> GetTransition (int state) {
			throw new NotImplementedException ();
		}
		#endregion
		#region implemented abstract members of Iohmm
		/// <summary>
		/// Gets the probability of migrating from state <paramref name="statei"/> to <paramref name="statej"/> given
		/// the <paramref name="input"/>.
		/// </summary>
		/// <returns>The probability of the transition from the first index to the second given the index.</returns>
		/// <param name="input">The given input for the transition.</param>
		/// <param name="statei">The origin hidden state.</param>
		/// <param name="statej">The target hidden state.</param>
		public override double GetA (TInput input, int statei, int statej) {
			return this.GetTransition (statei).GetPdf (input, statej);
		}

		/// <summary>
		/// Gets the probability of exhaust of <paramref name="output"/> given <paramref name="input"/> and
		/// current <paramref name="state"/>
		/// </summary>
		/// <returns>The probability of the output given the input and state.</returns>
		/// <param name="input">The input for this time stamp.</param>
		/// <param name="state">The current state of the hidden Markov model.</param>
		/// <param name="output">The assumed output for this time stamp.</param>
		public override double GetB (TInput input, int state, TOutput output) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Calculates the probability of the output sequence given the input sequence for this hidden Markov model.
		/// </summary>
		/// <returns>The probability of the sequence of outputs given the sequence of inputs.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public override double Probability (IEnumerable<Tuple<TInput, TOutput>> inoutputs) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Train this hidden Markov model with the given sequence of inputs and outputs.
		/// </summary>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public override void Train (IEnumerable<Tuple<TInput, TOutput>> inoutputs) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Returns the most likely sequence of the hidden state of sequences for the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>The most likely sequence of hidden states.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public override IEnumerable<int> MostLikelyHiddenStateSequence (IEnumerable<Tuple<TInput, TOutput>> inoutputs) {
			throw new NotImplementedException ();
		}
		#endregion
	}
}