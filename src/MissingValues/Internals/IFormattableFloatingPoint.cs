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
		abstract static bool ExplicitLeadingBit { get; }
		abstract static int NormalMantissaBits { get; }
		abstract static int DenormalMantissaBits { get; }
		abstract static int MinimumDecimalExponent { get; }
		abstract static int MaximumDecimalExponent { get; }
		abstract static int MinBiasedExponent { get; }
		abstract static int MaxBiasedExponent { get; }
		abstract static int MaxSignificandPrecision { get; }
		abstract static int ExponentBits { get; }
		abstract static int ExponentBias { get; }
		abstract static int OverflowDecimalExponent { get; }
		abstract static UInt128 DenormalMantissaMask { get; }
		abstract static UInt128 NormalMantissaMask { get; }
		abstract static UInt128 TrailingSignificandMask { get; }
		abstract static UInt128 PositiveZeroBits { get; }
		abstract static UInt128 PositiveInfinityBits { get; }
		abstract static UInt128 NegativeInfinityBits { get; }

		static abstract TSelf BitsToFloat(UInt128 bits);
		static abstract UInt128 FloatToBits(TSelf value);

	}
}
