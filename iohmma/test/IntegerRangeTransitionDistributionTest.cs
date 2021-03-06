//
//  IntegerRangeTransitionDistributionTest.cs
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
	[TestFixture()]
	public class IntegerRangeTransitionDistributionTest {

		[Test()]
		public void TestConstructor1 () {
			IntegerRangeTransitionDistribution<int> irtd;
			irtd = new IntegerRangeTransitionDistribution<int> (new IntegerRangeDistribution (0.0d, 1.0d), new IntegerRangeDistribution (1.0d, 0.0d));
			Assert.AreEqual (0x01, irtd.Lower);
			Assert.AreEqual (0x02, irtd.Upper);
			Assert.AreEqual (0x01, irtd.Sample (0x02));
			Assert.AreEqual (0x02, irtd.Sample (0x01));
		}
	}
}

