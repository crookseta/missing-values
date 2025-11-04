using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using TUnit.Assertions.Attributes;
using TUnit.Assertions.Core;

namespace MissingValues.Tests.Extensions;

public static class NumberBaseAssertionExtensions
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GenerateAssertion(ExpectationMessage = "to be bitwise equivalent to {other}")]
	public static bool IsBitwiseEquivalentTo<T1, T2>(this T1 value, T2 other)
		where T1 : struct, IFloatingPointIeee754<T1>
		where T2 : struct, IBinaryInteger<T2>
	{
		return Unsafe.SizeOf<T1>() == Unsafe.SizeOf<T2>() && Unsafe.BitCast<T1, T2>(value) == other;
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GenerateAssertion(ExpectationMessage = "to be approximately equal to {precision}")]
	public static AssertionResult IsApproximately<T>(this T value, T expected, T precision)
		where T : IFloatingPointIeee754<T>
	{
		if (T.IsNaN(value))
		{
			return AssertionResult.Failed($"Cannot determine approximation of a {typeof(T)} to NaN");
		}
		if (T.IsNegative(precision))
		{
			return AssertionResult.Failed($"Cannot determine precision of a {typeof(T)} if its negative");
		}
		if (T.IsInfinity(value))
		{
			return AssertionResult.Failed($"Cannot determine approximation of a {typeof(T)} to Infinity");
		}
		
		T actualDifference = T.Abs(expected - value);

		if (actualDifference <= precision)
		{
			return AssertionResult.Failed($"Expected {value} to approximate {expected} +/- {precision}, but {value} differed by {actualDifference}.");
		}
		
		return AssertionResult.Passed;
	}
}