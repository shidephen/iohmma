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
using System.Linq;
using NUtils;

namespace iohmma {
	/// <summary>
	/// An implementation of the <see cref="T:IMealyIohmm`2"/> interface. A hidden Markov model using the
	/// Mealy flavor.
	/// </summary>
	/// <typeparam name='TInput'>The type of the input handled by the IOHMM.</typeparam>
	/// <typeparam name='TOutput'>The type of the output handled by the IOHMM.</typeparam>
	public class MealyIohmm<TInput,TOutput> : Iohmm<TInput,TOutput>, IMealyIohmm<TInput,TOutput> {

		#region Fields
		private readonly ITransitionDistribution<TInput,TOutput>[] emissions;
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has no parameters.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator has no parameters.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		public MealyIohmm (int numberOfHiddenStates, Func<ITransitionDistribution<TInput,int>> transitionDistributionGenerator, Func<ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : this(numberOfHiddenStates,transitionDistributionGenerator,emissionDistributionGenerator.ShiftRightParameter<int,ITransitionDistribution<TInput,TOutput>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has an input parameter: the initial state of the transition.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator has no parameters.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		public MealyIohmm (int numberOfHiddenStates, Func<int,ITransitionDistribution<TInput,int>> transitionDistributionGenerator, Func<ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : this(numberOfHiddenStates,transitionDistributionGenerator,emissionDistributionGenerator.ShiftRightParameter<int,ITransitionDistribution<TInput,TOutput>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributions">A list of initial distributions for the hidden states transitions.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator has no parameters.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (int numberOfHiddenStates, IEnumerable<ITransitionDistribution<TInput,int>> transitionDistributions, Func<ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : this(numberOfHiddenStates,transitionDistributions,emissionDistributionGenerator.ShiftRightParameter<int,ITransitionDistribution<TInput,TOutput>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has no parameters.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator takes one parameter: the state that will emit.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		public MealyIohmm (int numberOfHiddenStates, Func<ITransitionDistribution<TInput,int>> transitionDistributionGenerator, Func<int,ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : base(numberOfHiddenStates,transitionDistributionGenerator) {
			ITransitionDistribution<TInput,TOutput>[] em = new ITransitionDistribution<TInput,TOutput>[numberOfHiddenStates];
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				em [i] = emissionDistributionGenerator (i);
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has an input parameter: the initial state of the transition.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator takes one parameter: the state that will emit.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		public MealyIohmm (int numberOfHiddenStates, Func<int,ITransitionDistribution<TInput,int>> transitionDistributionGenerator, Func<int,ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : base(numberOfHiddenStates,transitionDistributionGenerator) {
			ITransitionDistribution<TInput,TOutput>[] em = new ITransitionDistribution<TInput,TOutput>[numberOfHiddenStates];
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				em [i] = emissionDistributionGenerator (i);
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributions">A list of initial distributions for the hidden states transitions.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator takes one parameter: the state that will emit.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (int numberOfHiddenStates, IEnumerable<ITransitionDistribution<TInput,int>> transitionDistributions, Func<int,ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : base(numberOfHiddenStates,transitionDistributions) {
			ITransitionDistribution<TInput,TOutput>[] em = new ITransitionDistribution<TInput,TOutput>[numberOfHiddenStates];
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				em [i] = emissionDistributionGenerator (i);
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has no parameters.</param>
		/// <param name="emissionDistributions">A list of initial distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="emissionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="emissionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (int numberOfHiddenStates, Func<ITransitionDistribution<TInput,int>> transitionDistributionGenerator, IEnumerable<ITransitionDistribution<TInput,TOutput>> emissionDistributions) : base(numberOfHiddenStates,transitionDistributionGenerator) {
			ITransitionDistribution<TInput,TOutput>[] em = emissionDistributions.Take (numberOfHiddenStates).ToArray ();
			if (em.Length < numberOfHiddenStates) {
				throw new ArgumentException ("The number of given initial emission distributions must be larger or equal to the number of hidden states.");
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has an input parameter: the initial state of the transition.</param>
		/// <param name="emissionDistributions">A list of initial distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="emissionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="emissionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (int numberOfHiddenStates, Func<int,ITransitionDistribution<TInput,int>> transitionDistributionGenerator, IEnumerable<ITransitionDistribution<TInput,TOutput>> emissionDistributions) : base(numberOfHiddenStates,transitionDistributionGenerator) {
			ITransitionDistribution<TInput,TOutput>[] em = emissionDistributions.Take (numberOfHiddenStates).ToArray ();
			if (em.Length < numberOfHiddenStates) {
				throw new ArgumentException ("The number of given initial emission distributions must be larger or equal to the number of hidden states.");
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="numberOfHiddenStates">Number of hidden states.</param>
		/// <param name="transitionDistributions">A list of initial distributions for the hidden states transitions.</param>
		/// <param name="emissionDistributions">A list of initial distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the number of hidden states is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="emissionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// <para>Additional items in the <paramref name="emissionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (int numberOfHiddenStates, IEnumerable<ITransitionDistribution<TInput,int>> transitionDistributions, IEnumerable<ITransitionDistribution<TInput,TOutput>> emissionDistributions) : base(numberOfHiddenStates,transitionDistributions) {
			ITransitionDistribution<TInput,TOutput>[] em = emissionDistributions.Take (numberOfHiddenStates).ToArray ();
			if (em.Length < numberOfHiddenStates) {
				throw new ArgumentException ("The number of given initial emission distributions must be larger or equal to the number of hidden states.");
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has no parameters.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator has no parameters.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		public MealyIohmm (IEnumerable<double> pi, Func<ITransitionDistribution<TInput,int>> transitionDistributionGenerator, Func<ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : this(pi,transitionDistributionGenerator,emissionDistributionGenerator.ShiftRightParameter<int,ITransitionDistribution<TInput,TOutput>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has an input parameter: the initial state of the transition.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator has no parameters.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		public MealyIohmm (IEnumerable<double> pi, Func<int,ITransitionDistribution<TInput,int>> transitionDistributionGenerator, Func<ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : this(pi,transitionDistributionGenerator,emissionDistributionGenerator.ShiftRightParameter<int,ITransitionDistribution<TInput,TOutput>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributions">A list of initial distributions for the hidden states transitions.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator has no parameters.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (IEnumerable<double> pi, IEnumerable<ITransitionDistribution<TInput,int>> transitionDistributions, Func<ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : this(pi,transitionDistributions,emissionDistributionGenerator.ShiftRightParameter<int,ITransitionDistribution<TInput,TOutput>> ()) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has no parameters.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator takes one parameter: the state that will emit.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		public MealyIohmm (IEnumerable<double> pi, Func<ITransitionDistribution<TInput,int>> transitionDistributionGenerator, Func<int,ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : base(pi,transitionDistributionGenerator) {
			int numberOfHiddenStates = this.NumberOfHiddenStates;
			ITransitionDistribution<TInput,TOutput>[] em = new ITransitionDistribution<TInput,TOutput>[numberOfHiddenStates];
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				em [i] = emissionDistributionGenerator (i);
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has an input parameter: the initial state of the transition.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator takes one parameter: the state that will emit.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		public MealyIohmm (IEnumerable<double> pi, Func<int,ITransitionDistribution<TInput,int>> transitionDistributionGenerator, Func<int,ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : base(pi,transitionDistributionGenerator) {
			int numberOfHiddenStates = this.NumberOfHiddenStates;
			ITransitionDistribution<TInput,TOutput>[] em = new ITransitionDistribution<TInput,TOutput>[numberOfHiddenStates];
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				em [i] = emissionDistributionGenerator (i);
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributions">A list of initial distributions for the hidden states transitions.</param>
		/// <param name="emissionDistributionGenerator">A generator that constructs emmission probabilities. The generator takes one parameter: the state that will emit.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (IEnumerable<double> pi, IEnumerable<ITransitionDistribution<TInput,int>> transitionDistributions, Func<int,ITransitionDistribution<TInput,TOutput>> emissionDistributionGenerator) : base(pi,transitionDistributions) {
			int numberOfHiddenStates = this.NumberOfHiddenStates;
			ITransitionDistribution<TInput,TOutput>[] em = new ITransitionDistribution<TInput,TOutput>[numberOfHiddenStates];
			for (int i = 0x00; i < numberOfHiddenStates; i++) {
				em [i] = emissionDistributionGenerator (i);
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has no parameters.</param>
		/// <param name="emissionDistributions">A list of initial distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="emissionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="emissionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (IEnumerable<double> pi, Func<ITransitionDistribution<TInput,int>> transitionDistributionGenerator, IEnumerable<ITransitionDistribution<TInput,TOutput>> emissionDistributions) : base(pi,transitionDistributionGenerator) {
			int numberOfHiddenStates = this.NumberOfHiddenStates;
			ITransitionDistribution<TInput,TOutput>[] em = emissionDistributions.Take (numberOfHiddenStates).ToArray ();
			if (em.Length < numberOfHiddenStates) {
				throw new ArgumentException ("The number of given initial emission distributions must be larger or equal to the number of hidden states.");
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributionGenerator">A generator that constructs input-dependent transition probabilities. The generator has an input parameter: the initial state of the transition.</param>
		/// <param name="emissionDistributions">A list of initial distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="emissionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="emissionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (IEnumerable<double> pi, Func<int,ITransitionDistribution<TInput,int>> transitionDistributionGenerator, IEnumerable<ITransitionDistribution<TInput,TOutput>> emissionDistributions) : base(pi,transitionDistributionGenerator) {
			int numberOfHiddenStates = this.NumberOfHiddenStates;
			ITransitionDistribution<TInput,TOutput>[] em = emissionDistributions.Take (numberOfHiddenStates).ToArray ();
			if (em.Length < numberOfHiddenStates) {
				throw new ArgumentException ("The number of given initial emission distributions must be larger or equal to the number of hidden states.");
			}
			this.emissions = em;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MealyIohmm`2"/> class, an Input-output Hidden Markov model with Mealy flavor.
		/// </summary>
		/// <param name="pi">The given list of initial hidden state probabilities.</param>
		/// <param name="transitionDistributions">A list of initial distributions for the hidden states transitions.</param>
		/// <param name="emissionDistributions">A list of initial distributions for the hidden states.</param>
		/// <exception cref="ArgumentException">If the length of <paramref name="pi"/> is smaller than or equal to zero.</exception>
		/// <exception cref="ArgumentException">If one of the given initial probabilities is less than zero.</exception>
		/// <exception cref="ArgumentException">If the list of initial probabilities do not sum up to one.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="transitionDistributions"/> is less than the number of hidden states.</exception>
		/// <exception cref="ArgumentException">The number of elements in the <paramref name="emissionDistributions"/> is less than the number of hidden states.</exception>
		/// <remarks>
		/// <para>Additional items in the <paramref name="transitionDistributions"/> are simply ignored.</para>
		/// <para>Additional items in the <paramref name="emissionDistributions"/> are simply ignored.</para>
		/// </remarks>
		public MealyIohmm (IEnumerable<double> pi, IEnumerable<ITransitionDistribution<TInput,int>> transitionDistributions, IEnumerable<ITransitionDistribution<TInput,TOutput>> emissionDistributions) : base(pi,transitionDistributions) {
			int numberOfHiddenStates = this.NumberOfHiddenStates;
			ITransitionDistribution<TInput,TOutput>[] em = emissionDistributions.Take (numberOfHiddenStates).ToArray ();
			if (em.Length < numberOfHiddenStates) {
				throw new ArgumentException ("The number of given initial emission distributions must be larger or equal to the number of hidden states.");
			}
			this.emissions = em;
		}
		#endregion
		#region IMealyIohmm implementation
		/// <summary>
		/// Gets the transition function discribing the emission from the given <paramref name="state"/>.
		/// </summary>
		/// <returns>The <see cref="T:ITransitionDistribution`2"/> function for the given state that describes
		/// the distribution with respect to the input.</returns>
		/// <param name="state">The given state for which the emission function must be returned.</param>
		public ITransitionDistribution<TInput,TOutput> GetEmission (int state) {
			return this.emissions [state];
		}
		#endregion
		#region implemented abstract members of Iohmm
		/// <summary>
		/// Gets the probability of exhaust of <paramref name="output"/> given <paramref name="input"/> and
		/// current <paramref name="state"/>
		/// </summary>
		/// <returns>The probability of the output given the input and state.</returns>
		/// <param name="input">The input for this time stamp.</param>
		/// <param name="state">The current state of the hidden Markov model.</param>
		/// <param name="output">The assumed output for this time stamp.</param>
		public override double GetB (TInput input, int state, TOutput output) {
			return this.GetEmission (state).GetPdf (input, output);
		}

		/// <summary>
		/// Calculates the probability of the output sequence given the input sequence for this hidden Markov model.
		/// </summary>
		/// <returns>The probability of the sequence of outputs given the sequence of inputs.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public override double Probability (IEnumerable<Tuple<TInput, TOutput>> inoutputs) {
			return this.CalculateAlphas (inoutputs).Last ().Foldl1 ((x, y) => x + y);
		}

		/// <summary>
		/// Train this hidden Markov model with the given sequence of inputs and outputs.
		/// </summary>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		/// <param name="fitting">A parameter that expresses how much the data should be taken into
		/// account compared with the old data stored in this Input-Output Hidden Markov Model.</param>
		/// <remarks>
		/// <para>The sequence of inputs and outputs must be finite.</para>
		/// </remarks>
		public override void Train (IEnumerable<Tuple<TInput, TOutput>> inoutputs, double fitting = 1.0d) {
			//calculate Alpha- and Beta- values.
			double[][] alpha = this.CalculateAlphas (inoutputs).ToArray ();
			double[][] betar = this.CalculateBetasReverse (inoutputs.Reverse ()).ToArray ();
			int T = alpha.Length;
			int T1 = T - 0x01;
			int N = this.NumberOfHiddenStates;
			double[] pi = this.Pi;
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
			sum = fitting / sum;
			double fittingb = 1.0d - fitting;
			for (int i = 0x00; i < N; i++) {
				pi [i] = fittingb * pi [i] + alphat [i] * betart [i] * sum;
			}
			//fitting the transition probabilities based on the Alpha- and Beta-values.
			for (int i = 0x00; i < N; i++) {
				this.GetTransition (i).FitUnnormalized (GetEtas (inoutputs, alpha, betar, sumab, i).Cache (), fitting);
			}
			//fitting emmission probabilities based on the Alpha- and Beta-values.
			for (int i = 0x00; i < N; i++) {
				this.GetEmission (i).FitUnnormalized (GetGammas (inoutputs, alpha, betar, sumab, i).Cache (), fitting);
			}
		}

		/// <summary>
		/// Calculates a stream of gamma-values, these are used for fitting the emmission probabilities (the so-called B-values).
		/// </summary>
		/// <returns>A list of <see cref="T:Tuple`2"/> instances where the first item is a <see cref="T:Tuple`2"/> that contains
		/// the input and output pair and the probability as a second item.</returns>
		/// <param name="inoutputs">The original list of inputs- and outputs used to train the Hidden Markov Model.</param>
		/// <param name="alpha">The calculated alpha values.</param>
		/// <param name="betar">The calculated beta values in recversed order.</param>
		/// <param name="sumab">A list of sums of the alpha and beta values of each time stamp.</param>
		/// <param name="i">The original index for the emission probabilities.</param>
		private IEnumerable<Tuple<Tuple<TInput,TOutput>,double>> GetGammas (IEnumerable<Tuple<TInput, TOutput>> inoutputs, double[][] alpha, double[][] betar, double[] sumab, int i) {
			int T = alpha.Length;
			int T1 = T - 0x01;
			int N = this.NumberOfHiddenStates;
			double[] alphat, betart;
			IEnumerator<Tuple<TInput, TOutput>> enumerator = inoutputs.GetEnumerator ();
			Tuple<TInput, TOutput> ct1;
			TInput x1;
			TOutput y1;
			for (int t = 0x00; t < T1 && enumerator.MoveNext(); t++) {
				ct1 = enumerator.Current;
				x1 = ct1.Item1;
				y1 = ct1.Item2;
				alphat = alpha [t];
				betart = betar [T1 - t];
				yield return new Tuple<Tuple<TInput,TOutput>,double> (new Tuple<TInput,TOutput> (x1, y1), alphat [i] * betart [i]);
			}
			yield break;
		}

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
		private IEnumerable<Tuple<Tuple<TInput,int>,double>> GetEtas (IEnumerable<Tuple<TInput, TOutput>> inoutputs, double[][] alpha, double[][] betar, double[] sumab, int i) {
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
		/// Returns the most likely sequence of the hidden state of sequences for the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>The most likely sequence of hidden states.</returns>
		/// <param name="inoutputs">The sequence of inputs and outputs.</param>
		public override IEnumerable<int> MostLikelyHiddenStateSequence (IEnumerable<Tuple<TInput, TOutput>> inoutputs) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Generate a sequence of observations based on the given sequence of <paramref name="inputs"/> and the <see cref="T:IIohm`2"/>.
		/// </summary>
		/// <returns>A sequence of observations based on the given input.</returns>
		/// <param name="inputs">A <see cref="T:IEnumerable`1"/> of inputs.</param>
		public override IEnumerable<TOutput> GenerateObservationSequence (IEnumerable<TInput> inputs) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Calculate the alpha values based on the given sequence of inputs and outputs.
		/// </summary>
		/// <returns>A list of probability arrays describing the alpha values after each stage.</returns>
		/// <param name="inoutputs">A list of tuples containing the input and the appropriate output.</param>
		/// <remarks>
		/// <para>The output list is as long as the <paramref name="inoutputs"/> list.</para>
		/// <para>The values are computed lazily, infinite sequence are possible.</para>
		/// </remarks>
		public override IEnumerable<double[]> CalculateAlphas (IEnumerable<Tuple<TInput, TOutput>> inoutputs) {
			IEnumerator<Tuple<TInput,TOutput>> enumerator = inoutputs.GetEnumerator ();
			if (enumerator.MoveNext ()) {
				TInput xt0, xt1;
				TOutput yt1;
				Tuple<TInput,TOutput> cur = enumerator.Current;
				xt1 = cur.Item1;
				yt1 = cur.Item2;
				int nhidden = this.NumberOfHiddenStates;
				double[] result1 = new double[nhidden], result0;
				for (int si = 0x00; si < nhidden; si++) {
					result1 [si] = this.GetPi (si) * this.GetB (xt1, si, yt1);
				}
				yield return result1;
				while (enumerator.MoveNext ()) {
					cur = enumerator.Current;
					xt0 = xt1;
					xt1 = cur.Item1;
					yt1 = cur.Item2;
					result0 = result1;
					result1 = new double[nhidden];
					for (int sj = 0x00; sj < nhidden; sj++) {
						double p = 0.0d;
						for (int si = 0x00; si < nhidden; si++) {
							p += result0 [si] * this.GetA (xt0, si, sj);
						}
						result1 [sj] = p * this.GetB (xt1, sj, yt1);
					}
					yield return result1;
				}
			}
		}

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
		public override IEnumerable<double[]> CalculateBetasReverse (IEnumerable<Tuple<TInput, TOutput>> reversedinoutputs) {
			IEnumerator<Tuple<TInput,TOutput>> reversedenumerator = reversedinoutputs.GetEnumerator ();
			if (reversedenumerator.MoveNext ()) {
				TInput xt0, xt1;
				TOutput yt1;
				Tuple<TInput,TOutput> cur = reversedenumerator.Current;
				xt1 = cur.Item1;
				yt1 = cur.Item2;
				int nhidden = this.NumberOfHiddenStates;
				double[] result1 = new double[nhidden], result0;
				for (int si = 0x00; si < nhidden; si++) {
					result1 [si] = this.GetPi (si) * this.GetB (xt1, si, yt1);
				}
				yield return result1;
				while (reversedenumerator.MoveNext ()) {
					cur = reversedenumerator.Current;
					xt0 = xt1;
					xt1 = cur.Item1;
					yt1 = cur.Item2;
					result0 = result1;
					result1 = new double[nhidden];
					for (int sj = 0x00; sj < nhidden; sj++) {
						double p = 0.0d;
						for (int si = 0x00; si < nhidden; si++) {
							p += result0 [si] * this.GetA (xt0, si, sj);
						}
						result1 [sj] = p * this.GetB (xt1, sj, yt1);
					}
					yield return result1;
				}
			}
		}
		#endregion
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="T:MealyIohmm`2"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="T:MealyIohmm`2"/>.</returns>
		/// <remarks>
		/// <para>The Hidden Markov Model is formatted as a two-dimensional table with in the x-direction the hidden states
		/// and in the y-direction the initial probability, the transition probabilities and the emission probabilities.</para>
		/// </remarks>
		public override string ToString () {
			return TablePrinter.WriteTable ((IEnumerable<object>)(IEnumerable<double>)this.Pi, (IEnumerable<ITransitionDistribution<TInput,int>>)this.Transitions, (IEnumerable<ITransitionDistribution<TInput,int>>)this.emissions);
		}
	}
}