//
//  Test.cs
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
using NUnit.Framework;
using System;
using System.Linq;
using iohmma;

namespace IohmmTest {
	[TestFixture()]
	public class MealyTest {

		public const double M1P0 = 0.2d;
		public const double M1P1 = 0.8d;
		public const double M1A00 = 0.5d;
		public const double M1A01 = 0.5d;
		public const double M1A10 = 0.3d;
		public const double M1A11 = 0.7d;
		public const double M1B00 = 0.3d;
		public const double M1B01 = 0.7d;
		public const double M1B10 = 0.8d;
		public const double M1B11 = 0.2d;
		public const double M1Pr0 = 0.7d;
		public const double M1Pr1 = 0.3d;
		public const double M1Pr00 = 0.449d;
		public const double M1Pr01 = 0.251d;
		public const double M1Pr10 = 0.181d;
		public const double M1Pr11 = 0.119d;
		public const double M1O0A00 = 0.06d;
		public const double M1O0A01 = 0.64d;
		public const double M1O1A00 = 0.14d;
		public const double M1O1A01 = 0.16d;
		public const double M1O00A00 = 0.06d;
		public const double M1O00A01 = 0.64d;
		public const double M1O00A10 = 0.0666d;
		public const double M1O00A11 = 0.3824d;
		public const double M1O01A00 = 0.06d;
		public const double M1O01A01 = 0.64d;
		public const double M1O01A10 = 0.1554d;
		public const double M1O01A11 = 0.0956d;
		public const double M1O0B00 = 1.0d;
		public const double M1O0B01 = 1.0d;
		public const double M1O1B00 = 1.0d;
		public const double M1O1B01 = 1.0d;
		public const double M1O00B00 = 0.55d;
		public const double M1O00B01 = 0.65d;
		public const double M1O00B10 = 1.0d;
		public const double M1O00B11 = 1.0d;
		public const double M1O01B00 = 0.45d;
		public const double M1O01B01 = 0.35d;
		public const double M1O01B10 = 1.0d;
		public const double M1O01B11 = 1.0d;
		public const double M1TA00 = 0.06d;
		public const double M1TA01 = 0.64d;
		public const double M1TA10 = 0.1554d;
		public const double M1TA11 = 0.0956d;
		public const double M1TA20 = 0.031914d;
		public const double M1TA21 = 0.115696d;
		public const double M1TA30 = 0.03546606d;
		public const double M1TA31 = 0.01938884d;
		public const double M1TB00 = 0.45d;
		public const double M1TB01 = 0.35d;
		public const double M1TB10 = 0.45d;
		public const double M1TB11 = 0.35d;
		public const double M1TB20 = 0.45d;
		public const double M1TB21 = 0.35d;
		public const double M1TB30 = 1.0d;
		public const double M1TB31 = 1.0d;

		[Test()]
		public void TestConstructor () {
			MealyIohmm<int,int> mi = CreateMealy1 ();
			Assert.AreEqual (M1P0, mi.GetPi (0x00), TestConstants.Tolerance);
			Assert.AreEqual (M1P1, mi.GetPi (0x01), TestConstants.Tolerance);
			Assert.AreEqual (M1A00, mi.GetA (0x01, 0x00, 0x00), TestConstants.Tolerance);
			Assert.AreEqual (M1A01, mi.GetA (0x01, 0x00, 0x01), TestConstants.Tolerance);
			Assert.AreEqual (M1A10, mi.GetA (0x01, 0x01, 0x00), TestConstants.Tolerance);
			Assert.AreEqual (M1A11, mi.GetA (0x01, 0x01, 0x01), TestConstants.Tolerance);
			Assert.AreEqual (M1B00, mi.GetB (0x01, 0x00, 0x00), TestConstants.Tolerance);
			Assert.AreEqual (M1B01, mi.GetB (0x01, 0x00, 0x01), TestConstants.Tolerance);
			Assert.AreEqual (M1B10, mi.GetB (0x01, 0x01, 0x00), TestConstants.Tolerance);
			Assert.AreEqual (M1B11, mi.GetB (0x01, 0x01, 0x01), TestConstants.Tolerance);
		}

		[Test()]
		public void TestProbability1 () {
			MealyIohmm<int,int> mi = CreateMealy1 ();
			Assert.AreEqual (M1Pr0, mi.Probability (new Tuple<int,int> (0x01, 0x00)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr1, mi.Probability (new Tuple<int,int> (0x01, 0x01)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr00, mi.Probability (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x00)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr00, mi.Probability (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x00)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr01, mi.Probability (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x01)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr10, mi.Probability (new Tuple<int,int> (0x01, 0x01), new Tuple<int,int> (0x01, 0x00)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr11, mi.Probability (new Tuple<int,int> (0x01, 0x01), new Tuple<int,int> (0x01, 0x01)), TestConstants.Tolerance);
		}

		[Test()]
		public void TestCalculateAlphas1 () {
			MealyIohmm<int,int> mi = CreateMealy1 ();
			double[][] alpha;
			alpha = mi.CalculateAlphas (new Tuple<int,int> (0x01, 0x00)).ToArray ();
			Assert.AreEqual (M1O0A00, alpha [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O0A01, alpha [0x00] [0x01], TestConstants.Tolerance);
			alpha = mi.CalculateAlphas (new Tuple<int,int> (0x01, 0x01)).ToArray ();
			Assert.AreEqual (M1O1A00, alpha [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O1A01, alpha [0x00] [0x01], TestConstants.Tolerance);
			alpha = mi.CalculateAlphas (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x00)).ToArray ();
			Assert.AreEqual (M1O00A00, alpha [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O00A01, alpha [0x00] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1O00A10, alpha [0x01] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O00A11, alpha [0x01] [0x01], TestConstants.Tolerance);
			alpha = mi.CalculateAlphas (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x01)).ToArray ();
			Assert.AreEqual (M1O01A00, alpha [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O01A01, alpha [0x00] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1O01A10, alpha [0x01] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O01A11, alpha [0x01] [0x01], TestConstants.Tolerance);
		}

		[Test()]
		public void TestCalculateBetas1 () {
			MealyIohmm<int,int> mi = CreateMealy1 ();
			double[][] beta;
			beta = mi.CalculateBetas (new Tuple<int,int> (0x01, 0x00)).ToArray ();
			Assert.AreEqual (M1O0B00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O0B01, beta [0x00] [0x01], TestConstants.Tolerance);
			beta = mi.CalculateBetas (new Tuple<int,int> (0x01, 0x01)).ToArray ();
			Assert.AreEqual (M1O1B00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O1B01, beta [0x00] [0x01], TestConstants.Tolerance);
			beta = mi.CalculateBetas (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x00)).ToArray ();
			Assert.AreEqual (M1O00B00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O00B01, beta [0x00] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1O00B10, beta [0x01] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O00B11, beta [0x01] [0x01], TestConstants.Tolerance);
			beta = mi.CalculateBetas (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x01)).ToArray ();
			Assert.AreEqual (M1O01B00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O01B01, beta [0x00] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1O01B10, beta [0x01] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O01B11, beta [0x01] [0x01], TestConstants.Tolerance);
		}

		[Test()]
		public void TestCalculateBetasReverse1 () {
			MealyIohmm<int,int> mi = CreateMealy1 ();
			double[][] beta;
			beta = mi.CalculateBetasReverse (new Tuple<int,int> (0x01, 0x00)).Reverse ().ToArray ();
			Assert.AreEqual (M1O0B00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O0B01, beta [0x00] [0x01], TestConstants.Tolerance);
			beta = mi.CalculateBetasReverse (new Tuple<int,int> (0x01, 0x01)).Reverse ().ToArray ();
			Assert.AreEqual (M1O1B00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O1B01, beta [0x00] [0x01], TestConstants.Tolerance);
			beta = mi.CalculateBetasReverse (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x00)).Reverse ().ToArray ();
			Assert.AreEqual (M1O00B00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O00B01, beta [0x00] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1O00B10, beta [0x01] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O00B11, beta [0x01] [0x01], TestConstants.Tolerance);
			beta = mi.CalculateBetasReverse (new Tuple<int,int> (0x01, 0x01), new Tuple<int,int> (0x01, 0x00)).Reverse ().ToArray ();
			Assert.AreEqual (M1O01B00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O01B01, beta [0x00] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1O01B10, beta [0x01] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1O01B11, beta [0x01] [0x01], TestConstants.Tolerance);
		}

		[Test()]
		public void TestTrain1 () {
			MealyIohmm<int,int> mi = CreateMealy1 ();
			Tuple<int,int>[] observations = new Tuple<int, int>[] {
				new Tuple<int,int> (0x01, 0x00),
				new Tuple<int,int> (0x01, 0x01),
				new Tuple<int,int> (0x01, 0x00),
				new Tuple<int,int> (0x01, 0x01),
			};
			double[][] alpha = mi.CalculateAlphas (observations).ToArray ();
			Assert.AreEqual (M1TA00, alpha [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1TA01, alpha [0x00] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1TA10, alpha [0x01] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1TA11, alpha [0x01] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1TA20, alpha [0x02] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1TA21, alpha [0x02] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1TA30, alpha [0x03] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1TA31, alpha [0x03] [0x01], TestConstants.Tolerance);
			double[][] beta = mi.CalculateBetas (observations).ToArray ();
			Assert.AreEqual (M1TB00, beta [0x00] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1TB01, beta [0x00] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1TB10, beta [0x01] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1TB11, beta [0x01] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1TB20, beta [0x02] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1TB21, beta [0x02] [0x01], TestConstants.Tolerance);
			Assert.AreEqual (M1TB30, beta [0x03] [0x00], TestConstants.Tolerance);
			Assert.AreEqual (M1TB31, beta [0x03] [0x01], TestConstants.Tolerance);
			for (int i = 0x00; i < 0x10; i++) {
				mi.Train (observations);
				Console.WriteLine (mi);
			}
		}

		private static MealyIohmm<int,int> CreateMealy1 () {
			return new MealyIohmm<int,int> (new double[] { M1P0, M1P1 }, new ITransitionDistribution<int,int>[] {
				IntegerRangeTransitionDistribution<int>.HiddenStateTransitionDistributions (0x01, 0x02, M1A00, M1A01),
				IntegerRangeTransitionDistribution<int>.HiddenStateTransitionDistributions (0x01, 0x02, M1A10, M1A11)
			}, new ITransitionDistribution<int,int>[] { 
				IntegerRangeTransitionDistribution<int>.HiddenStateTransitionDistributions (0x01, 0x02, M1B00, M1B01),
				IntegerRangeTransitionDistribution<int>.HiddenStateTransitionDistributions (0x01, 0x02, M1B10, M1B11)
			});
		}
	}
}

