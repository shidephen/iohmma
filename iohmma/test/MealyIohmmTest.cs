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
		[Test()]
		public void TestConstructor () {
			MealyIohmm<int,int> mi = new MealyIohmm<int,int> (new double[] { 0.2, 0.8 }, new ITransitionDistribution<int,int>[] {
				IntegerRangeTransitionDistribution<int>.HiddenStateTransitionDistributions (0x01, 0x02, 0.5d, 0.5d),
				IntegerRangeTransitionDistribution<int>.HiddenStateTransitionDistributions (0x01, 0x02, 0.3d, 0.7d)
			}, new ITransitionDistribution<int,int>[] { 
				IntegerRangeTransitionDistribution<int>.HiddenStateTransitionDistributions (0x01, 0x02, 0.3d, 0.7d),
				IntegerRangeTransitionDistribution<int>.HiddenStateTransitionDistributions (0x01, 0x02, 0.8d, 0.2d)
			});
			Console.WriteLine (mi);
		}
	}
}

