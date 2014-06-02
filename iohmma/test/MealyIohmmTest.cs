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
using NUtils;
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
		public const double M1Pr00 = 0.449d;
		public const double M1Pr01 = 0.251d;
		public const double M1Pr10 = 0.181d;
		public const double M1Pr11 = 0.119d;

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
			Assert.AreEqual (M1Pr00, mi.Probability (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x00)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr01, mi.Probability (new Tuple<int,int> (0x01, 0x00), new Tuple<int,int> (0x01, 0x01)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr10, mi.Probability (new Tuple<int,int> (0x01, 0x01), new Tuple<int,int> (0x01, 0x00)), TestConstants.Tolerance);
			Assert.AreEqual (M1Pr11, mi.Probability (new Tuple<int,int> (0x01, 0x01), new Tuple<int,int> (0x01, 0x01)), TestConstants.Tolerance);
		}

		[Test()]
		public void TestTrain1 () {
			MealyIohmm<int,int> mi = CreateMealy1 ();
			Tuple<int,int>[] observations = new Tuple<int, int>[] {
				new Tuple<int,int> (0x01, 0x00),
				new Tuple<int,int> (0x01, 0x01),
				new Tuple<int,int> (0x01, 0x00),
				new Tuple<int,int> (0x01, 0x01),
				new Tuple<int,int> (0x01, 0x00),
				new Tuple<int,int> (0x01, 0x01),
				new Tuple<int,int> (0x01, 0x00),
				new Tuple<int,int> (0x01, 0x01)
			};
			mi.Train (observations);
			Console.WriteLine (mi);
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

