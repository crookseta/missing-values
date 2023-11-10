using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Internals
{
	internal interface IFormattableFloatingPoint<TSelf> : IFloatingPoint<TSelf>
		where TSelf : IFormattableFloatingPoint<TSelf>
	{
		abstract static ReadOnlySpan<TSelf> PowersOfTen { get; }
	}
	internal interface IFormattableBinaryFloatingPoint<TSelf> : IFormattableFloatingPoint<TSelf>, IBinaryFloatingPointIeee754<TSelf>
		where TSelf : IFormattableBinaryFloatingPoint<TSelf>
	{
		abstract static int MantissaExplicitBits { get; }
		abstract static int MinimumExponent { get; }
		abstract static int InfinitePower { get; }
		abstract static int SignBitIndex { get; }
		abstract static int LargestPowerOfTen { get; }
		abstract static int SmallestPowerOfTen { get; }
	}
}
