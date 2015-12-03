//
//  IIohmm.cs
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
	/// An interface specifying an Input-output Hidden Markov Model (IOHMM).
	/// </summary>
	/// <typeparam name='TInput'>The type of the input handled by the IOHMM.</typeparam>
	/// <typeparam name='TOutput'>The type of the output handled by the IOHMM.</typeparam>
	public interface IIohmm<TInput,TOutput> : IMarkovProcess {

		/// <summary>
		/// Gets the transition function describing the transition from the given <paramref name="state"/>.
		/// </summary>
		/// <returns>The <see cref="T:ITransitionDistribution`2"/> function for the given state.</returns>
		/// <param name="state">The given state for which the transition function must be returned.</param>
		ITransitionDistribution<TInput,int> GetTransition (int state);

		/// <summary>
		/// Gets the probability of migrating from state <paramref name="statei"/> to <paramref name="statej"/> given
		/// the <paramref name="input"/>.
		/// </summary>
		/// <returns>The probability of the transition from the first index to the second given the index.</returns>
		/// <param name="input">The given input for the transition.</param>
		/// <param name="statei">The origin hidden state.</param>
		/// <param name="statej">The target hidden state.</param>
		double GetA (TInput input, int statei, int statej);

		/// <summary>
		/// Gets the probability of exhaust of <paramref name="output"/> given <paramref name="input"/> and
		/// current <paramref name="state"/>
		/// </summary>
		/// <returns>The probability of the output given the input and state.</returns>
		/// <param name="input">The input for this time stamp.</param>
		/// <param name="state">The current state of the hidden Markov model.</param>
		/// <param name="output">The assumed output for this time stamp.</param>
		double GetB (TInput input, int state, TOutput output);

		/// <summary>
		/// Calculates the probability of the output sequence given the input sequence for this hidden Markov model.
		/// </summary>
		/// <returns>The probability of the sequence of outputs given the sequence of inputs.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		double Probability (IEnumerable<Tuple<TInput,TOutput>> inoutputs);

		/// <summary>
		/// Calculates the probability of the output sequence given the input sequence for this hidden Markov model.
		/// </summary>
		/// <returns>The probability of the sequence of outputs given the sequence of inputs.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		double Probability (params Tuple<TInput,TOutput>[] inoutputs);

		/// <summary>
		/// Train this hidden Markov model with the given sequence of inputs and outputs.
		/// </summary>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		/// <param name="fitting">A parameter that expresses how much the data should be taken into
		/// account compared with the old data stored in this Input-Output Hidden Markov Model.</param>
		void Train (IEnumerable<Tuple<TInput,TOutput>> inoutputs, double fitting = 1.0d);

		/// <summary>
		/// Train this hidden Markov model with the given sequence of input-output sequences.
		/// </summary>
		/// <param name="inoutputseq">The list of observation sequences.</param>
		/// <param name="fitting">A parameter that expresses how much the data should be taken into
		/// account compared with the old data stored in this Input-Output Hidden Markov Model.</param>
		void Train (IEnumerable<IEnumerable<Tuple<TInput,TOutput>>> inoutputseq, double fitting = 1.0d);

		/// <summary>
		/// Returns the most likely sequence of the hidden state of sequences for the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>The most likely sequence of hidden states.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		IEnumerable<int> MostLikelyHiddenStateSequence (IEnumerable<Tuple<TInput,TOutput>> inoutputs);

		/// <summary>
		/// Returns the most likely sequence of the hidden state of sequences for the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>The most likely sequence of hidden states.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		IEnumerable<int> MostLikelyHiddenStateSequence (params Tuple<TInput,TOutput>[] inoutputs);

		/// <summary>
		/// Generate a sequence of observations based on the given sequence of <paramref name="inputs"/> and the <see cref="T:IIohm`2"/>.
		/// </summary>
		/// <returns>A sequence of observations based on the given input.</returns>
		/// <param name="inputs">An <see cref="T:IEnumerable`1"/> of inputs.</param>
		IEnumerable<TOutput> GenerateObservationSequence (IEnumerable<TInput> inputs);

		/// <summary>
		/// Generate a sequence of observations based on the given sequence of <paramref name="inputs"/> and the <see cref="T:IIohm`2"/>.
		/// </summary>
		/// <returns>A sequence of observations based on the given input.</returns>
		/// <param name="inputs">An sequence of inputs.</param>
		IEnumerable<TOutput> GenerateObservationSequence (TInput[] inputs);

		/// <summary>
		/// Calculate the alpha values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the alpha values after each stage.</returns>
		/// <param name="inoutputs">A list of tuples containing the input and the appropriate output.</param>
		/// <remarks>
		/// <para>The output list is as long as the <paramref name="inoutputs"/> list.</para>
		/// <para>The values are computed lazily, infinite sequence are possible.</para>
		/// </remarks>
		IEnumerable<double[]> CalculateAlphas (IEnumerable<Tuple<TInput,TOutput>> inoutputs);

		/// <summary>
		/// Calculate the alpha values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the alpha values after each stage.</returns>
		/// <param name="inoutputs">A list of tuples containing the input and the appropriate output.</param>
		/// <remarks>
		/// <para>The output list is as long as the <paramref name="inoutputs"/> list.</para>
		/// </remarks>
		IEnumerable<double[]> CalculateAlphas (params Tuple<TInput,TOutput>[] inoutputs);

		/// <summary>
		/// Calculate the beta values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the beta values after each stage from beginning to end.</returns>
		/// <param name="reversedinoutputs">A list of tuples containing the input and the appropriate output.</param>
		IEnumerable<double[]> CalculateBetas (params Tuple<TInput, TOutput>[] reversedinoutputs);

		/// <summary>
		/// Calculate the beta values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the beta values after each stage from beginning to end.</returns>
		/// <param name="reversedinoutputs">A list of tuples containing the input and the appropriate output.</param>
		/// <remarks>
		/// <para><paramref name="inoutputs"/> is not allowed to have an infinite length.</para>
		/// </remarks>
		IEnumerable<double[]> CalculateBetas (IEnumerable<Tuple<TInput, TOutput>> reversedinoutputs);

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
		IEnumerable<double[]> CalculateBetasReverse (IEnumerable<Tuple<TInput,TOutput>> reversedinoutputs);

		/// <summary>
		/// Calculate the beta values based on the given reversed sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the beta values after each stage from end to begin.</returns>
		/// <param name="reversedinoutputs">A list of tuples containing the input and the appropriate output, the order
		/// is reversed: the first tuple contains the last observation.</param>
		/// <remarks>
		/// <para>The output list is as long as the <paramref name="reversedinoutputs"/> list.</para>
		/// </remarks>
		IEnumerable<double[]> CalculateBetasReverse (params Tuple<TInput,TOutput>[] reversedinoutputs);

		/// <summary>
		/// Gets a list of input-state values together with the (unscaled) probabilities that would be used to train the transition probabilities Hidden Markov model for the given initial state.
		/// </summary>
		/// <returns>A list of input-state values together with the (unscaled) probabilities.</returns>
		/// <param name="inoutputs">A sequence of input-output values that would train the Hidden Markov model.</param>
		/// <param name="initialState">The given initial state.</param>
		IEnumerable<Tuple<Tuple<TInput,int>,double>> CalculateNewTransition (IEnumerable<Tuple<TInput, TOutput>> inoutputs, int initialState);
	}
}