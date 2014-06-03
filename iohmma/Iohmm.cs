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
using System.Linq;
using NUtils;

namespace iohmma {
	/// <summary>
	/// A basic implementation of the <see cref="T:IIohmm`2"/> interface that represents an Input-Output Hidden Markov Model (IOHMM).
	/// </summary>
	/// <typeparam name='TInput'>The type of the input handled by the IOHMM.</typeparam>
	/// <typeparam name='TOutput'>The type of the output handled by the IOHMM.</typeparam>
	public abstract class Iohmm<TInput,TOutput> : IIohmm<TInput,TOutput> {

		#region protected Fields
		/// <summary>
		/// The initial distribution on the hidden states.
		/// </summary>
		protected readonly double[] Pi;
		/// <summary>
		/// The transition distributions per hidden state.
		/// </summary>
		protected readonly ITransitionDistribution<TInput,int>[] Transitions;
		#endregion
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
				return this.Pi.Length;
			}
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class with a given list of initial hidden
		/// state probabilities and a generator function for a transition distribution.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributionGenerator">A generator function, that generates the transition functions, the function has no parameters.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		protected Iohmm (IEnumerable<double> pi, Func<ITransitionDistribution<TInput,int>> transitionDistributionGenerator) : this(pi,transitionDistributionGenerator.ShiftRightParameter<int,ITransitionDistribution<TInput,int>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributionGenerator">A generator function, that generates the transition functions, the function has no parameters.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		protected Iohmm (int numberOfHiddenStates, Func<ITransitionDistribution<TInput,int>> transitionDistributionGenerator) : this(numberOfHiddenStates,transitionDistributionGenerator.ShiftRightParameter<int,ITransitionDistribution<TInput,int>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class with a given list of initial hidden state probabilities and
		/// a generator function for transition distributions.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributionGenerator">A generator function, that generates the transition functions, it has one parameter: the original state.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		protected Iohmm (IEnumerable<double> pi, Func<int,ITransitionDistribution<TInput,int>> transitionDistributionGenerator) {
			double[] pia = pi.ToArray ();
			int numberOfHiddenStates = pia.Length;
			if (numberOfHiddenStates <= 0x00) {
				throw new ArgumentException ("The number of hidden states must be greater than zero.");
			}
			double sum = 0.0d, p;
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				p = pia [i];
				if (p < 0.0d) {
					throw new ArgumentException ("All initial state probabilities must be larger or equal to zero.");
				}
				sum += pia [i];
			}
			if (Math.Abs (1.0d - sum) > ProgramConstants.Epsilon) {
				throw new ArgumentException ("The given intial state probabilities must sum up to one.");
			}
			this.Pi = pia;
			ITransitionDistribution<TInput,int>[] tr = new ITransitionDistribution<TInput, int>[numberOfHiddenStates];
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				tr [i] = transitionDistributionGenerator (i);
			}
			this.Transitions = tr;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributionGenerator">A generator function, that generates the transition functions, it has one parameter: the original state.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		protected Iohmm (int numberOfHiddenStates, Func<int,ITransitionDistribution<TInput,int>> transitionDistributionGenerator) {
			if (numberOfHiddenStates <= 0x00) {
				throw new ArgumentException ("The number of hidden states must be greater than zero.");
			}
			this.Pi = new double[numberOfHiddenStates];
			this.ResetPi ();
			ITransitionDistribution<TInput,int>[] tr = new ITransitionDistribution<TInput, int>[numberOfHiddenStates];
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				tr [i] = transitionDistributionGenerator (i);
			}
			this.Transitions = tr;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class with a given list of initial probabilities
		/// and a given list of transition distributions.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributions">A list of initial transition distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// </remarks>
		protected Iohmm (IEnumerable<double> pi, params ITransitionDistribution<TInput,int>[] transitionDistributions) : this(pi,(IEnumerable<ITransitionDistribution<TInput,int>>) transitionDistributions) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class.
		/// </summary>
		/// <param name="numberOfHiddenStates">The given number of hidden states.</param>
		/// <param name="transitionDistributions">A list of initial transition distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// </remarks>
		protected Iohmm (int numberOfHiddenStates, params ITransitionDistribution<TInput,int>[] transitionDistributions) : this(numberOfHiddenStates,(IEnumerable<ITransitionDistribution<TInput,int>>) transitionDistributions) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributions">A list of initial transition distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// </remarks>
		protected Iohmm (int numberOfHiddenStates, IEnumerable<ITransitionDistribution<TInput,int>> transitionDistributions) {
			if (numberOfHiddenStates <= 0x00) {
				throw new ArgumentException ("The number of hidden states must be greater than zero.");
			}
			this.Pi = new double[numberOfHiddenStates];
			this.ResetPi ();
			ITransitionDistribution<TInput,int>[] tr = transitionDistributions.Take (numberOfHiddenStates).ToArray ();
			if (tr.Length < numberOfHiddenStates) {
				throw new ArgumentException ("The number of given initial transition distributions must be larger or equal to the number of hidden states.");
			}
			this.Transitions = tr;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Iohmm`2"/> class with a given list of initial probabilities
		/// and a given list of transition distributions.
		/// </summary>
		/// <param name="pi">A given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributions">A list of initial transition distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// </remarks>
		protected Iohmm (IEnumerable<double> pi, IEnumerable<ITransitionDistribution<TInput,int>> transitionDistributions) {
			double[] pia = pi.ToArray ();
			int numberOfHiddenStates = pia.Length;
			if (numberOfHiddenStates <= 0x00) {
				throw new ArgumentException ("The number of hidden states must be greater than zero.");
			}
			double sum = 0.0d, p;
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				p = pia [i];
				if (p < 0.0d) {
					throw new ArgumentException ("All initial state probabilities must be larger or equal to zero.");
				}
				sum += pia [i];
			}
			if (Math.Abs (1.0d - sum) > ProgramConstants.Epsilon) {
				throw new ArgumentException ("The given intial state probabilities must sum up to one.");
			}
			this.Pi = pia;
			ITransitionDistribution<TInput,int>[] tr = transitionDistributions.Take (numberOfHiddenStates).ToArray ();
			if (tr.Length < numberOfHiddenStates) {
				throw new ArgumentException ("The number of given initial transition distributions must be larger or equal to the number of hidden states.");
			}
			this.Transitions = tr;
		}
		#endregion
		#region IIohmm implementation
		/// <summary>
		/// Gets the transition function describing the transition from the given <paramref name="state"/>.
		/// </summary>
		/// <returns>The <see cref="T:ITransitionDistribution`2"/> function for the given state.</returns>
		/// <param name="state">The given state for which the transition function must be returned.</param>
		public ITransitionDistribution<TInput,int> GetTransition (int state) {
			return this.Transitions [state];
		}

		/// <summary>
		/// Gets the initial state distribution of the given state index.
		/// </summary>
		/// <returns>The initial distribution for the given index of the hidden states.</returns>
		/// <param name="index">The given state index.</param>
		/// <exception cref="IndexOutOfRangeException">If the given index is smaller than zero (<c>0</c>).</exception>
		/// <exception cref="IndexOutOfRangeException">If the given index is larger than or equal to the number of hidden states (<see cref="NumberOfHiddenStates"/>).</exception>
		public double GetPi (int index) {
			return this.Pi [index];
		}

		/// <summary>
		/// Gets the probability of migrating from state <paramref name="statei"/> to <paramref name="statej"/> given
		/// the <paramref name="input"/>.
		/// </summary>
		/// <returns>The probability of the transition from the first index to the second given the index.</returns>
		/// <param name="input">The given input for the transition.</param>
		/// <param name="statei">The origin hidden state.</param>
		/// <param name="statej">The target hidden state.</param>
		public double GetA (TInput input, int statei, int statej) {
			return this.Transitions [statei].GetPdf (input, statej);
		}

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
		/// <param name="fitting">A parameter that expresses how much the data should be taken into
		/// account compared with the old data stored in this Input-Output Hidden Markov Model.</param>
		public abstract void Train (IEnumerable<Tuple<TInput,TOutput>> inoutputs, double fitting = 1.0d);

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

		/// <summary>
		/// Resets the probability of being in a certain state at the first time stamp.
		/// </summary>
		public void ResetPi () {
			int n = this.NumberOfHiddenStates;
			double psi = 1.0d / n;
			double[] ps = this.Pi;
			for (int i = 0x00; i < n; i++) {
				ps [i] = psi;
			}
		}

		/// <summary>
		/// Calculates the probability of the output sequence given the input sequence for this hidden Markov model.
		/// </summary>
		/// <returns>The probability of the sequence of outputs given the sequence of inputs.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public double Probability (params Tuple<TInput, TOutput>[] inoutputs) {
			return this.Probability ((IEnumerable<Tuple<TInput, TOutput>>)inoutputs);
		}

		/// <summary>
		/// Returns the most likely sequence of the hidden state of sequences for the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>The most likely sequence of hidden states.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public IEnumerable<int> MostLikelyHiddenStateSequence (params Tuple<TInput, TOutput>[] inoutputs) {
			return this.MostLikelyHiddenStateSequence ((IEnumerable<Tuple<TInput, TOutput>>)inoutputs);
		}

		/// <summary>
		/// Generate a sequence of observations based on the given sequence of <paramref name="inputs"/> and the <see cref="T:IIohm`2"/>.
		/// </summary>
		/// <returns>A sequence of observations based on the given input.</returns>
		/// <param name="inputs">An sequence of inputs.</param>
		public IEnumerable<TOutput> GenerateObservationSequence (TInput[] inputs) {
			return this.GenerateObservationSequence ((IEnumerable<TInput>)inputs);
		}

		/// <summary>
		/// Calculate the alpha values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the alpha values after each stage.</returns>
		/// <param name="inoutputs">A list of tuples containing the input and the appropriate output.</param>
		/// <remarks>
		/// <para><paramref name="inoutputs"/> is allowed to have an infinite length.</para>
		/// </remarks>
		public IEnumerable<double[]> CalculateAlphas (params Tuple<TInput, TOutput>[] inoutputs) {
			return this.CalculateAlphas ((IEnumerable<Tuple<TInput, TOutput>>)inoutputs);
		}

		/// <summary>
		/// Calculate the beta values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the beta values after each stage from beginning to end.</returns>
		/// <param name="reversedinoutputs">A list of tuples containing the input and the appropriate output.</param>
		public IEnumerable<double[]> CalculateBetas (params Tuple<TInput, TOutput>[] reversedinoutputs) {
			return this.CalculateBetasReverse (reversedinoutputs.Reverse ()).Reverse ();
		}

		/// <summary>
		/// Calculate the beta values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the beta values after each stage from beginning to end.</returns>
		/// <param name="reversedinoutputs">A list of tuples containing the input and the appropriate output.</param>
		/// <remarks>
		/// <para><paramref name="inoutputs"/> is not allowed to have an infinite length.</para>
		/// </remarks>
		public IEnumerable<double[]> CalculateBetas (IEnumerable<Tuple<TInput, TOutput>> reversedinoutputs) {
			return this.CalculateBetasReverse (reversedinoutputs.Reverse ()).Reverse ();
		}

		/// <summary>
		/// Calculate the reversed list of beta values based on the given reversed sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the beta values after each stage from end to begin.</returns>
		/// <param name="reversedinoutputs">A list of tuples containing the input and the appropriate output, the order
		/// is reversed: the first tuple contains the last observation.</param>
		/// <remarks>
		/// <para><paramref name="inoutputs"/> is allowed to have an infinite length.</para>
		/// </remarks>
		public IEnumerable<double[]> CalculateBetasReverse (params Tuple<TInput, TOutput>[] reversedinoutputs) {
			return this.CalculateBetasReverse ((IEnumerable<Tuple<TInput, TOutput>>)reversedinoutputs);
		}

		/// <summary>
		/// Train this hidden Markov model with the given sequence of input-output sequences.
		/// </summary>
		/// <param name="inoutputseq">The list of observation sequences.</param>
		/// <param name="fitting">A parameter that expresses how much the data should be taken into
		/// account compared with the old data stored in this Input-Output Hidden Markov Model.</param>
		public abstract void Train (IEnumerable<IEnumerable<Tuple<TInput, TOutput>>> inoutputseq, double fitting = 1.0);

		/// <summary>
		/// Caclcuates a stream of eta-values, these are used to fit the transition probabilities (or the so-called A-values).
		/// </summary>
		/// <returns>A list of <see cref="T:Tuple`2"/> values where the first element is a <see cref="T:Tuple`2"/>
		/// of input and state. and as second item the probability of the migration.</returns>
		/// <param name="inoutputs">The original sequence of inputs and outputs that are used to train the Hidden Markov Model.</param>
		/// <param name="alpha">The calculated alpha values.</param>
		/// <param name="betar">The calculated beta values in reverse ordered.</param>
		/// <param name="sumab">A list of sums of the alpha and beta values of each time stamp.</param>
		/// <param name="i">The original index for the transition probabilities.</param>
		protected IEnumerable<Tuple<Tuple<TInput,int>,double>> GetEtas (IEnumerable<Tuple<TInput, TOutput>> inoutputs, double[][] alpha, double[][] betar, double[] sumab, int i) {
			int T = alpha.Length;
			int T1 = T - 0x01;
			int N = this.NumberOfHiddenStates;
			double den, denalphati;
			double[] alphat, betart;
			IEnumerator<Tuple<TInput, TOutput>> enumerator = inoutputs.GetEnumerator ();
			enumerator.MoveNext ();
			Tuple<TInput, TOutput> ct1 = enumerator.Current;
			TInput x0, x1 = ct1.Item1;
			TOutput y1;
			for (int t = 0x00; t < T1 && enumerator.MoveNext(); t++) {
				x0 = x1;
				ct1 = enumerator.Current;
				x1 = ct1.Item1;
				y1 = ct1.Item2;
				alphat = alpha [t];
				betart = betar [T1 - t];
				den = 1.0d / sumab [t];
				denalphati = alphat [i] * den;
				for (int j = 0x00; j < N; j++) {
					yield return new Tuple<Tuple<TInput,int>,double> (new Tuple<TInput,int> (x0, j), betart [j] * this.GetA (x0, i, j) * this.GetB (x1, j, y1) * denalphati);
				}
			}
		}

		/// <summary>
		/// Gets a list of input-state values together with the (unscaled) probabilities that would be used to train the transition probabilities Hidden Markov model for the given initial state.
		/// </summary>
		/// <returns>A list of input-state values together with the (unscaled) probabilities.</returns>
		/// <param name="inoutputs">A sequence of input-output values that would train the Hidden Markov model.</param>
		/// <param name="initialState">The given initial state.</param>
		public IEnumerable<Tuple<Tuple<TInput, int>, double>> CalculateNewTransition (IEnumerable<Tuple<TInput, TOutput>> inoutputs, int initialState) {
			//calculate Alpha- and Beta- values.
			double[][] alpha = this.CalculateAlphas (inoutputs).ToArray ();
			double[][] betar = this.CalculateBetasReverse (inoutputs.Reverse ()).ToArray ();
			int T = alpha.Length;
			int T1 = T - 0x01;
			int N = this.NumberOfHiddenStates;
			double[] alphat = null, betart = null, sumab = new double[T];
			double sum = 0.0d;
			//Calculate Gamma-like values.
			for (int t = T1; t >= 0x00; t--) {
				alphat = alpha [t];
				betart = betar [T1 - t];
				sum = 0.0d;
				for (int i = 0x00; i < N; i++) {
					sum += alphat [i] * betart [i];
				}
				sumab [t] = sum;
			}
			return this.GetEtas (inoutputs, alpha, betar, sumab, initialState);
		}
		#endregion
	}
}

