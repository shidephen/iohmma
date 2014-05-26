//
//  IntegerRangeDistributionTest.cs
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
using iohmma;

namespace IohmmTest {
	[TestFixture]
	public class IntegerRangeDistributionTest {
		[Test]
		public void TestConstructor () {
			IntegerRangeDistribution ird;
			ird = new IntegerRangeDistribution (1, 5);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.2d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x05), TestConstants.Tolerance);
			ird = new IntegerRangeDistribution (5);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.2d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x05), TestConstants.Tolerance);
			ird = new IntegerRangeDistribution (1, 8);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (8, ird.Upper);
			Assert.AreEqual (0.125d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x05), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x06), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x07), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x08), TestConstants.Tolerance);
			ird = new IntegerRangeDistribution (0, 7);
			Assert.AreEqual (0, ird.Lower);
			Assert.AreEqual (7, ird.Upper);
			Assert.AreEqual (0.125d, ird.GetPdf (0x00), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x05), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x06), TestConstants.Tolerance);
			Assert.AreEqual (0.125d, ird.GetPdf (0x07), TestConstants.Tolerance);
		}

		[Test]
		public void TestFitting () {
			IntegerRangeDistribution ird;
			ird = new IntegerRangeDistribution (1, 5);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.2d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x05), TestConstants.Tolerance);
			ird.Fit (new Tuple<int, double>[] { new Tuple<int,double> (0x03,1.0d) });
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.0d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (1.0d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x05), TestConstants.Tolerance);
			ird.Fit (new Tuple<int, double>[] { new Tuple<int,double> (0x02,1.0d) }, 0.25d);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.0d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.25d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.75d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x05), TestConstants.Tolerance);
		}

		[Test]
		public void TestFittingUnnormalized1 () {
			IntegerRangeDistribution ird;
			ird = new IntegerRangeDistribution (1, 5);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.2d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x05), TestConstants.Tolerance);
			ird.FitUnnormalized (new Tuple<int, double>[] { new Tuple<int,double> (0x03,1.0d) });
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.0d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (1.0d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x05), TestConstants.Tolerance);
			ird.FitUnnormalized (new Tuple<int, double>[] { new Tuple<int,double> (0x02,1.0d) }, 0.25d);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.0d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.25d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.75d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x05), TestConstants.Tolerance);
		}

		[Test]
		public void TestFittingUnnormalized2 () {
			IntegerRangeDistribution ird;
			ird = new IntegerRangeDistribution (1, 5);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.2d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.2d, ird.GetPdf (0x05), TestConstants.Tolerance);
			ird.FitUnnormalized (new Tuple<int, double>[] { new Tuple<int,double> (0x03,2.0d) });
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.0d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (1.0d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x05), TestConstants.Tolerance);
			ird.FitUnnormalized (new Tuple<int, double>[] { new Tuple<int,double> (0x02,0.125d) }, 0.25d);
			Assert.AreEqual (1, ird.Lower);
			Assert.AreEqual (5, ird.Upper);
			Assert.AreEqual (0.0d, ird.GetPdf (0x01), TestConstants.Tolerance);
			Assert.AreEqual (0.25d, ird.GetPdf (0x02), TestConstants.Tolerance);
			Assert.AreEqual (0.75d, ird.GetPdf (0x03), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x04), TestConstants.Tolerance);
			Assert.AreEqual (0.0d, ird.GetPdf (0x05), TestConstants.Tolerance);
		}
	}
}