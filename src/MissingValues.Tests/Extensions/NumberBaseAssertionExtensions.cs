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
	public static bool IsBitwiseEquivalentTo<T1>(this T1 value, T1 other)
		where T1 : struct, IFloatingPointIeee754<T1>
	{
		switch (Unsafe.SizeOf<T1>())
		{
			case 16:
				return Unsafe.BitCast<T1, UInt128>(value) == Unsafe.BitCast<T1, UInt128>(other);
			case UInt256.Size:
				return Unsafe.BitCast<T1, UInt256>(value) == Unsafe.BitCast<T1, UInt256>(other);
			default:
				return false;
		}
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
			return AssertionResult.Passed;
		}
		
		return AssertionResult.Failed($"Expected {value} to approximate {expected} +/- {precision}, but {value} differed by {actualDifference}.");
	}
	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GenerateAssertion(ExpectationMessage = "to be Not a Number")]
	public static bool IsNaN(this Quad value)
	{
		return Quad.IsNaN(value);
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GenerateAssertion(ExpectationMessage = "to be Not a Number")]
	public static bool IsNaN(this Octo value)
	{
		return Octo.IsNaN(value);
	}
	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GenerateAssertion(ExpectationMessage = "to be Negative Infinity")]
	public static bool IsNegativeInfinity(this Quad value)
	{
		return Quad.IsNegativeInfinity(value);
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GenerateAssertion(ExpectationMessage = "to be Negative Infinity")]
	public static bool IsNegativeInfinity(this Octo value)
	{
		return Octo.IsNegativeInfinity(value);
	}
	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GenerateAssertion(ExpectationMessage = "to be Negative Infinity")]
	public static bool IsPositiveInfinity(this Quad value)
	{
		return Quad.IsPositiveInfinity(value);
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GenerateAssertion(ExpectationMessage = "to be Negative Infinity")]
	public static bool IsPositiveInfinity(this Octo value)
	{
		return Octo.IsPositiveInfinity(value);
	}
}