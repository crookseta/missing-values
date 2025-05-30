using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions.Throws;
using static MissingValues.Tests.Data.UInt512DataSources;

using DataSources = MissingValues.Tests.Data.UInt512DataSources;

namespace MissingValues.Tests.Numerics;

public class UInt512GenericMathTests
{
	#region Operators
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_AdditionTestData))]
	public async Task op_AdditionTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left + right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_CheckedAdditionTestData))]
	public async Task op_CheckedAdditionTest(UInt512 left, UInt512 right, UInt512 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(left + right)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(left + right);
			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_IncrementTestData))]
	public async Task op_IncrementTest(UInt512 value, UInt512 expected)
	{
		var result = ++value;

		await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_CheckedIncrementTestData))]
	public async Task op_CheckedIncrementTest(UInt512 value, UInt512 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(++value)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(++value);
			await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_SubtractionTestData))]
	public async Task op_SubtractionTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left - right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_CheckedSubtractionTestData))]
	public async Task op_CheckedSubtractionTest(UInt512 left, UInt512 right, UInt512 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(left - right)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(left - right);
			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_DecrementTestData))]
	public async Task op_DecrementTest(UInt512 value, UInt512 expected)
	{
		var result = --value;

		await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_CheckedDecrementTestData))]
	public async Task op_CheckedDecrementTest(UInt512 value, UInt512 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(--value)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(--value);
			await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_MultiplyTestData))]
	public async Task op_MultiplyTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left * right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_CheckedMultiplyTestData))]
	public async Task op_CheckedMultiplyTest(UInt512 left, UInt512 right, UInt512 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(left * right)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(left * right);
			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_DivisionTestData))]
	public async Task op_DivisionTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		if (right == UInt512.Zero)
		{
			await Assert.That(() => left / right).Throws<DivideByZeroException>();
		}
		else
		{
			var result = left / right;

			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_ModulusTestData))]
	public async Task op_ModulusTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		if (right == UInt512.Zero)
		{
			await Assert.That(() => left % right).Throws<DivideByZeroException>();
		}
		else
		{
			var result = left % right;

			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_OnesComplementTestData))]
	public async Task op_OnesComplementTest(UInt512 value, UInt512 expected)
	{
		var result = ~value;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_BitwiseAndTestData))]
	public async Task op_BitwiseAndTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left & right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_BitwiseOrTestData))]
	public async Task op_BitwiseOrTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left | right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_BitwiseXorTestData))]
	public async Task op_BitwiseXorTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left ^ right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_ShiftLeftTestData))]
	public async Task op_ShiftLeftTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = value << shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_ShiftRightTestData))]
	public async Task op_ShiftRightTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = value >> shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_UnsignedShiftRightTestData))]
	public async Task op_UnsignedShiftRightTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = value >>> shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_EqualityTestData))]
	public async Task op_EqualityTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left == right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_InequalityTestData))]
	public async Task op_InequalityTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left != right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_LessThanTestData))]
	public async Task op_LessThanTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left < right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_LessThanOrEqualTestData))]
	public async Task op_LessThanOrEqualTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left <= right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_GreaterThanTestData))]
	public async Task op_GreaterThanTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left > right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(op_GreaterThanOrEqualTestData))]
	public async Task op_GreaterThanOrEqualTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left >= right;

		await Assert.That(result).IsEqualTo(expected);
	}
	#endregion

	#region INumberBase
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(AbsTestData))]
	public async Task AbsTest(UInt512 value, UInt512 expected)
	{
		UInt512 result = Helper.Abs(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsCanonicalTestData))]
	public async Task IsCanonicalTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsCanonical(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsComplexNumberTestData))]
	public async Task IsComplexNumberTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsComplexNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsEvenIntegerTestData))]
	public async Task IsEvenIntegerTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsEvenInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsFiniteTestData))]
	public async Task IsFiniteTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsFinite(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsImaginaryNumberTestData))]
	public async Task IsImaginaryNumberTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsImaginaryNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsInfinityTestData))]
	public async Task IsInfinityTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsIntegerTestData))]
	public async Task IsIntegerTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsNaNTestData))]
	public async Task IsNaNTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsNaN(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsNegativeTestData))]
	public async Task IsNegativeTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsNegative(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsNegativeInfinityTestData))]
	public async Task IsNegativeInfinityTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsNegativeInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsNormalTestData))]
	public async Task IsNormalTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsNormal(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsOddIntegerTestData))]
	public async Task IsOddIntegerTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsOddInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsPositiveTestData))]
	public async Task IsPositiveTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsPositive(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsPositiveInfinityTestData))]
	public async Task IsPositiveInfinityTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsPositiveInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsRealNumberTestData))]
	public async Task IsRealNumberTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsRealNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsSubnormalTestData))]
	public async Task IsSubnormalTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsSubnormal(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsZeroTestData))]
	public async Task IsZeroTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsZero(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MaxMagnitudeTestData))]
	public async Task MaxMagnitudeTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MaxMagnitude(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MaxMagnitudeNumberTestData))]
	public async Task MaxMagnitudeNumberTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MaxMagnitudeNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MinMagnitudeTestData))]
	public async Task MinMagnitudeTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MinMagnitude(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MinMagnitudeNumberTestData))]
	public async Task MinMagnitudeNumberTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MinMagnitudeNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MultiplyAddEstimateTestData))]
	public async Task MultiplyAddEstimateTest(UInt512 left, UInt512 right, UInt512 addend, UInt512 expected)
	{
		var result = Helper.MultiplyAddEstimate(left, right, addend);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(ParseTestData))]
	public async Task ParseTest(string s, NumberStyles style, IFormatProvider? provider, UInt512 expected)
	{
		var result = Helper.Parse<UInt512>(s, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(ParseSpanTestData))]
	public async Task ParseTest(char[] s, NumberStyles style, IFormatProvider? provider, UInt512 expected)
	{
		var result = Helper.Parse<UInt512>(s, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(ParseUtf8TestData))]
	public async Task ParseTest(byte[] utf8Text, NumberStyles style, IFormatProvider? provider, UInt512 expected)
	{
		var result = Helper.Parse<UInt512>(utf8Text, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(TryParseTestData))]
	public async Task TryParseTest(string s, NumberStyles style, IFormatProvider? provider, bool expected, UInt512 expectedValue)
	{
		var success = Helper.TryParse<UInt512>(s, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(TryParseSpanTestData))]
	public async Task TryParseTest(char[] s, NumberStyles style, IFormatProvider? provider, bool expected, UInt512 expectedValue)
	{
		var success = Helper.TryParse<UInt512>(s, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(TryParseUtf8TestData))]
	public async Task TryParseTest(byte[] utf8Text, NumberStyles style, IFormatProvider? provider, bool expected, UInt512 expectedValue)
	{
		var success = Helper.TryParse<UInt512>(utf8Text, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	#endregion
	
	#region INumber
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(ClampTestData))]
	public async Task ClampTest(UInt512 value, UInt512 min, UInt512 max, UInt512 expected)
	{
		if (min > max)
		{
			await Assert.That(() => Helper.Clamp(value, min, max)).Throws<ArgumentException>();
		}
		else
		{
			var result = Helper.Clamp(value, min, max);
			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(CopySignTestData))]
	public async Task CopySignTest(UInt512 value, UInt512 sign, UInt512 expected)
	{
		var result = Helper.CopySign(value, sign);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MaxTestData))]
	public async Task MaxTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.Max(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MaxNumberTestData))]
	public async Task MaxNumberTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MaxNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MinTestData))]
	public async Task MinTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.Min(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(MinNumberTestData))]
	public async Task MinNumberTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MinNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(SignTestData))]
	public async Task SignTest(UInt512 value, int expected)
	{
		var result = Helper.Sign(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	#endregion
	
	#region IBinaryNumber
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(IsPow2TestData))]
	public async Task IsPow2Test(UInt512 value, bool expected)
	{
		var result = Helper.IsPow2(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(Log2TestData))]
	public async Task Log2Test(UInt512 value, UInt512 expected)
	{
		var result = Helper.Log2(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	#endregion
	
	#region IBinaryInteger
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(DivRemTestData))]
	public async Task DivRemTest(UInt512 left, UInt512 right, (UInt512, UInt512) expected)
	{
		var result = Helper.DivRem(left, right);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(LeadingZeroCountTestData))]
	public async Task LeadingZeroCountTest(UInt512 value, UInt512 expected)
	{
		var result = Helper.LeadingZeroCount(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(PopCountTestData))]
	public async Task PopCountTest(UInt512 value, UInt512 expected)
	{
		var result = Helper.PopCount(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(ReadBigEndianTestData))]
	public async Task ReadBigEndianTest(byte[] source, bool isUnsigned, UInt512 expected)
	{
		var result = Helper.ReadBigEndian<UInt512>(source, isUnsigned);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(ReadLittleEndianTestData))]
	public async Task ReadLittleEndianTest(byte[] source, bool isUnsigned, UInt512 expected)
	{
		var result = Helper.ReadLittleEndian<UInt512>(source, isUnsigned);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(RotateLeftTestData))]
	public async Task RotateLeftTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = Helper.RotateLeft(value, shiftAmount);

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(RotateRightTestData))]
	public async Task RotateRightTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = Helper.RotateRight(value, shiftAmount);

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(TrailingZeroCountTestData))]
	public async Task TrailingZeroCountTest(UInt512 value, UInt512 expected)
	{
		var result = Helper.TrailingZeroCount(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(GetByteCountTestData))]
	public async Task GetByteCountTest(UInt512 value, int expected)
	{
		var result = Helper.GetByteCount(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(GetShortestBitLengthTestData))]
	public async Task GetShortestBitLengthTest(UInt512 value, int expected)
	{
		var result = Helper.GetShortestBitLength(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(WriteBigEndianTestData))]
	public async Task WriteBigEndianTest(UInt512 value, byte[] expectedDestination, int expected)
	{
		byte[] buffer = new byte[UInt512.Size];
		var result = Helper.WriteBigEndian(value, buffer);

		using (Assert.Multiple())
		{
			await Assert.That(result).IsEqualTo(expected);
			await Assert.That(buffer.Length).IsEqualTo(expectedDestination.Length);
			for (int i = 0; i < buffer.Length; i++)
			{
				await Assert.That(buffer[i]).IsEqualTo(expectedDestination[i]);
			}
		}
	}
	[Test]
	[MethodDataSource(typeof(DataSources), nameof(WriteLittleEndianTestData))]
	public async Task WriteLittleEndianTest(UInt512 value, byte[] expectedDestination, int expected)
	{
		byte[] buffer = new byte[UInt512.Size];
		var result = Helper.WriteLittleEndian(value, buffer);

		using (Assert.Multiple())
		{
			await Assert.That(result).IsEqualTo(expected);
			await Assert.That(buffer.Length).IsEqualTo(expectedDestination.Length);
			for (int i = 0; i < buffer.Length; i++)
			{
				await Assert.That(buffer[i]).IsEqualTo(expectedDestination[i]);
			}
		}
	}
	#endregion
}