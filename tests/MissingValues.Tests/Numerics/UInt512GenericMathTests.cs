using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MissingValues.Tests.Extensions;
using static MissingValues.Tests.Data.UInt512DataSources;

using DataSources = MissingValues.Tests.Data.UInt512DataSources;

namespace MissingValues.Tests.Numerics;

public class UInt512GenericMathTests
{
	[Test]
	public async Task PowTest()
	{
		await Assert.That(UInt512.Pow(UInt512.One, int.MaxValue)).IsEqualTo(UInt512.One);
		
		await Assert.That(UInt512.Pow(UInt512.Two, 32)).IsEqualTo(UInt512.UInt32MaxValue + UInt512.One);
		
		await Assert.That(UInt512.Pow(new UInt512(0, 0, 0, 3), 100)).IsEqualTo(new UInt512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_5A46_53CA, 0x6737_6856_5B41_F775, 0xD694_7D55_CF38_13D1));
		await Assert.That(UInt512.Pow(new UInt512(0, 0, 0, 10), 77)).IsEqualTo(new UInt512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xDD15_FE86_AFFA_D912, 0x49EF_0EB7_13F3_9EBE, 0xAA98_7B6E_6FD2_A000, 0x0000_0000_0000_0000));
		await Assert.That(UInt512.Pow(new UInt512(0, 0, 0, 10), 154)).IsEqualTo(new UInt512(0xBEEE_FB58_4AFF_8603, 0xAAFB_550F_FACF_D8FA, 0x5CA4_7E4F_88D4_5371, 0x27CB_D2FE_6214_5F08, 0x4544_B653_3551_55B6, 0xAF99_D40A_E400_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	}
	
	#region Operators
	[Test]
	[MethodDataSource<DataSources>(nameof(op_AdditionTestData))]
	public async Task op_AdditionTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left + right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_CheckedAdditionTestData))]
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
	[MethodDataSource<DataSources>(nameof(op_IncrementTestData))]
	public async Task op_IncrementTest(UInt512 value, UInt512 expected)
	{
		var result = ++value;

		await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_CheckedIncrementTestData))]
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
	[MethodDataSource<DataSources>(nameof(op_SubtractionTestData))]
	public async Task op_SubtractionTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left - right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_CheckedSubtractionTestData))]
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
	[MethodDataSource<DataSources>(nameof(op_DecrementTestData))]
	public async Task op_DecrementTest(UInt512 value, UInt512 expected)
	{
		var result = --value;

		await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_CheckedDecrementTestData))]
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
	[MethodDataSource<DataSources>(nameof(op_MultiplyTestData))]
	public async Task op_MultiplyTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left * right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_CheckedMultiplyTestData))]
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
	[MethodDataSource<DataSources>(nameof(op_DivisionTestData))]
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
	[MethodDataSource<DataSources>(nameof(op_ModulusTestData))]
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
	[MethodDataSource<DataSources>(nameof(op_OnesComplementTestData))]
	public async Task op_OnesComplementTest(UInt512 value, UInt512 expected)
	{
		var result = ~value;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_BitwiseAndTestData))]
	public async Task op_BitwiseAndTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left & right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_BitwiseOrTestData))]
	public async Task op_BitwiseOrTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left | right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_BitwiseXorTestData))]
	public async Task op_BitwiseXorTest(UInt512 left, UInt512 right, UInt512 expected)
	{
		var result = left ^ right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_ShiftLeftTestData))]
	public async Task op_ShiftLeftTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = value << shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_ShiftRightTestData))]
	public async Task op_ShiftRightTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = value >> shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_UnsignedShiftRightTestData))]
	public async Task op_UnsignedShiftRightTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = value >>> shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_EqualityTestData))]
	public async Task op_EqualityTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left == right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_InequalityTestData))]
	public async Task op_InequalityTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left != right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_LessThanTestData))]
	public async Task op_LessThanTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left < right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_LessThanOrEqualTestData))]
	public async Task op_LessThanOrEqualTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left <= right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_GreaterThanTestData))]
	public async Task op_GreaterThanTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left > right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(op_GreaterThanOrEqualTestData))]
	public async Task op_GreaterThanOrEqualTest(UInt512 left, UInt512 right, bool expected)
	{
		var result = left >= right;

		await Assert.That(result).IsEqualTo(expected);
	}
	#endregion

	#region INumberBase
	[Test]
	[MethodDataSource<DataSources>(nameof(AbsTestData))]
	public async Task AbsTest(UInt512 value, UInt512 expected)
	{
		UInt512 result = Helper.Abs(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsCanonicalTestData))]
	public async Task IsCanonicalTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsCanonical(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsComplexNumberTestData))]
	public async Task IsComplexNumberTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsComplexNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsEvenIntegerTestData))]
	public async Task IsEvenIntegerTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsEvenInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsFiniteTestData))]
	public async Task IsFiniteTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsFinite(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsImaginaryNumberTestData))]
	public async Task IsImaginaryNumberTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsImaginaryNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsInfinityTestData))]
	public async Task IsInfinityTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsIntegerTestData))]
	public async Task IsIntegerTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsNaNTestData))]
	public async Task IsNaNTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsNaN(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsNegativeTestData))]
	public async Task IsNegativeTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsNegative(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsNegativeInfinityTestData))]
	public async Task IsNegativeInfinityTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsNegativeInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsNormalTestData))]
	public async Task IsNormalTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsNormal(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsOddIntegerTestData))]
	public async Task IsOddIntegerTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsOddInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsPositiveTestData))]
	public async Task IsPositiveTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsPositive(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsPositiveInfinityTestData))]
	public async Task IsPositiveInfinityTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsPositiveInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsRealNumberTestData))]
	public async Task IsRealNumberTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsRealNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsSubnormalTestData))]
	public async Task IsSubnormalTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsSubnormal(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsZeroTestData))]
	public async Task IsZeroTest(UInt512 value, bool expected)
	{
		bool result = Helper.IsZero(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MaxMagnitudeTestData))]
	public async Task MaxMagnitudeTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MaxMagnitude(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MaxMagnitudeNumberTestData))]
	public async Task MaxMagnitudeNumberTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MaxMagnitudeNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MinMagnitudeTestData))]
	public async Task MinMagnitudeTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MinMagnitude(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MinMagnitudeNumberTestData))]
	public async Task MinMagnitudeNumberTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MinMagnitudeNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MultiplyAddEstimateTestData))]
	public async Task MultiplyAddEstimateTest(UInt512 left, UInt512 right, UInt512 addend, UInt512 expected)
	{
		var result = Helper.MultiplyAddEstimate(left, right, addend);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ParseTestData))]
	public async Task ParseTest(string s, NumberStyles style, IFormatProvider? provider, UInt512 expected)
	{
		var result = Helper.Parse<UInt512>(s, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ParseSpanTestData))]
	public async Task ParseTest(char[] s, NumberStyles style, IFormatProvider? provider, UInt512 expected)
	{
		var result = Helper.Parse<UInt512>(s, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ParseUtf8TestData))]
	public async Task ParseTest(byte[] utf8Text, NumberStyles style, IFormatProvider? provider, UInt512 expected)
	{
		var result = Helper.Parse<UInt512>(utf8Text, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(TryParseTestData))]
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
	[MethodDataSource<DataSources>(nameof(TryParseSpanTestData))]
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
	[MethodDataSource<DataSources>(nameof(TryParseUtf8TestData))]
	public async Task TryParseTest(byte[] utf8Text, NumberStyles style, IFormatProvider? provider, bool expected, UInt512 expectedValue)
	{
		var success = Helper.TryParse<UInt512>(utf8Text, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(TryParsePartialTestData))]
	public async Task TryParsePartialTest(string s, NumberStyles style, IFormatProvider? provider, bool expected, UInt512 expectedValue, int expectedConsumed)
	{
		var success = Helper.TryParsePartial<UInt512>(s, style, provider, out var result, out int consumed);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
			await Assert.That(consumed).IsEqualTo(expectedConsumed);
		}
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(TryParsePartialSpanTestData))]
	public async Task TryParsePartialTest(char[] s, NumberStyles style, IFormatProvider? provider, bool expected, UInt512 expectedValue, int expectedConsumed)
	{
		var success = Helper.TryParsePartial<UInt512>(s, style, provider, out var result, out int consumed);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
			await Assert.That(consumed).IsEqualTo(expectedConsumed);
		}
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(TryParsePartialUtf8TestData))]
	public async Task TryParsePartialTest(byte[] s, NumberStyles style, IFormatProvider? provider, bool expected, UInt512 expectedValue, int expectedConsumed)
	{
		var success = Helper.TryParsePartial<UInt512>(s, style, provider, out var result, out int consumed);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
			await Assert.That(consumed).IsEqualTo(expectedConsumed);
		}
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ToStringTestData))]
	public async Task ToStringTest(UInt512 value, string fmt, IFormatProvider? provider, string expected)
	{
		await Assert.That(value.ToString(fmt, provider)).EqualTo(expected);
	}
	#endregion
	
	#region INumber
	[Test]
	[MethodDataSource<DataSources>(nameof(ClampTestData))]
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
	[MethodDataSource<DataSources>(nameof(CopySignTestData))]
	public async Task CopySignTest(UInt512 value, UInt512 sign, UInt512 expected)
	{
		var result = Helper.CopySign(value, sign);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MaxTestData))]
	public async Task MaxTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.Max(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MaxNumberTestData))]
	public async Task MaxNumberTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MaxNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MinTestData))]
	public async Task MinTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.Min(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MinNumberTestData))]
	public async Task MinNumberTest(UInt512 x, UInt512 y, UInt512 expected)
	{
		var result = Helper.MinNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(SignTestData))]
	public async Task SignTest(UInt512 value, int expected)
	{
		var result = Helper.Sign(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	#endregion
	
	#region IBinaryNumber
	[Test]
	[MethodDataSource<DataSources>(nameof(IsPow2TestData))]
	public async Task IsPow2Test(UInt512 value, bool expected)
	{
		var result = Helper.IsPow2(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(Log2TestData))]
	public async Task Log2Test(UInt512 value, UInt512 expected)
	{
		var result = Helper.Log2(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	#endregion
	
	#region IBinaryInteger
	[Test]
	[MethodDataSource<DataSources>(nameof(DivRemTestData))]
	public async Task DivRemTest(UInt512 left, UInt512 right, Pair<UInt512> expected)
	{
		var result = Helper.DivRem(left, right);
		await Assert.That((Pair<UInt512>)result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(LeadingZeroCountTestData))]
	public async Task LeadingZeroCountTest(UInt512 value, UInt512 expected)
	{
		var result = Helper.LeadingZeroCount(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(PopCountTestData))]
	public async Task PopCountTest(UInt512 value, UInt512 expected)
	{
		var result = Helper.PopCount(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ReadBigEndianTestData))]
	public async Task ReadBigEndianTest(byte[] source, bool isUnsigned, UInt512 expected)
	{
		var result = Helper.ReadBigEndian<UInt512>(source, isUnsigned);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ReadLittleEndianTestData))]
	public async Task ReadLittleEndianTest(byte[] source, bool isUnsigned, UInt512 expected)
	{
		var result = Helper.ReadLittleEndian<UInt512>(source, isUnsigned);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(RotateLeftTestData))]
	public async Task RotateLeftTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = Helper.RotateLeft(value, shiftAmount);

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(RotateRightTestData))]
	public async Task RotateRightTest(UInt512 value, int shiftAmount, UInt512 expected)
	{
		var result = Helper.RotateRight(value, shiftAmount);

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(TrailingZeroCountTestData))]
	public async Task TrailingZeroCountTest(UInt512 value, UInt512 expected)
	{
		var result = Helper.TrailingZeroCount(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(GetByteCountTestData))]
	public async Task GetByteCountTest(UInt512 value, int expected)
	{
		var result = Helper.GetByteCount(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(GetShortestBitLengthTestData))]
	public async Task GetShortestBitLengthTest(UInt512 value, int expected)
	{
		var result = Helper.GetShortestBitLength(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(WriteBigEndianTestData))]
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
	[MethodDataSource<DataSources>(nameof(WriteLittleEndianTestData))]
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

	#region Conversion
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedByteTestData))] public async Task ConvertToCheckedByteTest(UInt512 input, byte expected) => await Assert.That(byte.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingByteTestData))] public async Task ConvertToSaturatingByteTest(UInt512 input, byte expected) => await Assert.That(byte.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingByteTestData))] public async Task ConvertToTruncatingByteTest(UInt512 input, byte expected) => await Assert.That(byte.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt16TestData))] public async Task ConvertToCheckedUInt16Test(UInt512 input, ushort expected) => await Assert.That(ushort.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt16TestData))] public async Task ConvertToSaturatingUInt16Test(UInt512 input, ushort expected) => await Assert.That(ushort.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt16TestData))] public async Task ConvertToTruncatingUInt16Test(UInt512 input, ushort expected) => await Assert.That(ushort.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt32TestData))] public async Task ConvertToCheckedUInt32Test(UInt512 input, uint expected) => await Assert.That(uint.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt32TestData))] public async Task ConvertToSaturatingUInt32Test(UInt512 input, uint expected) => await Assert.That(uint.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt32TestData))] public async Task ConvertToTruncatingUInt32Test(UInt512 input, uint expected) => await Assert.That(uint.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt64TestData))] public async Task ConvertToCheckedUInt64Test(UInt512 input, ulong expected) => await Assert.That(ulong.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt64TestData))] public async Task ConvertToSaturatingUInt64Test(UInt512 input, ulong expected) => await Assert.That(ulong.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt64TestData))] public async Task ConvertToTruncatingUInt64Test(UInt512 input, ulong expected) => await Assert.That(ulong.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt128TestData))] public async Task ConvertToCheckedUInt128Test(UInt512 input, UInt128 expected) => await Assert.That(UInt128.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt128TestData))] public async Task ConvertToSaturatingUInt128Test(UInt512 input, UInt128 expected) => await Assert.That(UInt128.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt128TestData))] public async Task ConvertToTruncatingUInt128Test(UInt512 input, UInt128 expected) => await Assert.That(UInt128.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt256TestData))] public async Task ConvertToCheckedUInt256Test(UInt512 input, UInt256 expected) => await Assert.That(UInt256.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt256TestData))] public async Task ConvertToSaturatingUInt256Test(UInt512 input, UInt256 expected) => await Assert.That(UInt256.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt256TestData))] public async Task ConvertToTruncatingUInt256Test(UInt512 input, UInt256 expected) => await Assert.That(UInt256.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUIntPtrTestData))] public async Task ConvertToCheckedUIntPtrTest(UInt512 input, nuint expected) => await Assert.That(nuint.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUIntPtrTestData))] public async Task ConvertToSaturatingUIntPtrTest(UInt512 input, nuint expected) => await Assert.That(nuint.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUIntPtrTestData))] public async Task ConvertToTruncatingUIntPtrTest(UInt512 input, nuint expected) => await Assert.That(nuint.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedSByteTestData))] public async Task ConvertToCheckedSByteTest(UInt512 input, sbyte expected) => await Assert.That(sbyte.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingSByteTestData))] public async Task ConvertToSaturatingSByteTest(UInt512 input, sbyte expected) => await Assert.That(sbyte.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingSByteTestData))] public async Task ConvertToTruncatingByteTest(UInt512 input, sbyte expected) => await Assert.That(sbyte.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt16TestData))] public async Task ConvertToCheckedInt16Test(UInt512 input, short expected) => await Assert.That(short.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt16TestData))] public async Task ConvertToSaturatingInt16Test(UInt512 input, short expected) => await Assert.That(short.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt16TestData))] public async Task ConvertToTruncatingInt16Test(UInt512 input, short expected) => await Assert.That(short.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt32TestData))] public async Task ConvertToCheckedInt32Test(UInt512 input, int expected) => await Assert.That(int.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt32TestData))] public async Task ConvertToSaturatingInt32Test(UInt512 input, int expected) => await Assert.That(int.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt32TestData))] public async Task ConvertToTruncatingInt32Test(UInt512 input, int expected) => await Assert.That(int.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt64TestData))] public async Task ConvertToCheckedInt64Test(UInt512 input, long expected) => await Assert.That(long.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt64TestData))] public async Task ConvertToSaturatingInt64Test(UInt512 input, long expected) => await Assert.That(long.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt64TestData))] public async Task ConvertToTruncatingInt64Test(UInt512 input, long expected) => await Assert.That(long.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt128TestData))] public async Task ConvertToCheckedInt128Test(UInt512 input, Int128 expected) => await Assert.That(Int128.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt128TestData))] public async Task ConvertToSaturatingInt128Test(UInt512 input, Int128 expected) => await Assert.That(Int128.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt128TestData))] public async Task ConvertToTruncatingInt128Test(UInt512 input, Int128 expected) => await Assert.That(Int128.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt256TestData))] public async Task ConvertToCheckedInt256Test(UInt512 input, Int256 expected) => await Assert.That(Int256.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt256TestData))] public async Task ConvertToSaturatingInt256Test(UInt512 input, Int256 expected) => await Assert.That(Int256.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt256TestData))] public async Task ConvertToTruncatingInt256Test(UInt512 input, Int256 expected) => await Assert.That(Int256.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt512TestData))] public async Task ConvertToCheckedInt512Test(UInt512 input, Int512 expected) => await Assert.That(Int512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt512TestData))] public async Task ConvertToSaturatingInt512Test(UInt512 input, Int512 expected) => await Assert.That(Int512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt512TestData))] public async Task ConvertToTruncatingInt512Test(UInt512 input, Int512 expected) => await Assert.That(Int512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedIntPtrTestData))] public async Task ConvertToCheckedIntPtrTest(UInt512 input, nint expected) => await Assert.That(nint.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingIntPtrTestData))] public async Task ConvertToSaturatingIntPtrTest(UInt512 input, nint expected) => await Assert.That(nint.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingIntPtrTestData))] public async Task ConvertToTruncatingIntPtrTest(UInt512 input, nint expected) => await Assert.That(nint.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedBigIntegerTestData))] public async Task ConvertToCheckedBigIntegerTest(UInt512 input, BigInteger expected) => await Assert.That(BigInteger.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingBigIntegerTestData))] public async Task ConvertToSaturatingBigIntegerTest(UInt512 input, BigInteger expected) => await Assert.That(BigInteger.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingBigIntegerTestData))] public async Task ConvertToTruncatingBigIntegerTest(UInt512 input, BigInteger expected) => await Assert.That(BigInteger.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedHalfTestData))] public async Task ConvertToCheckedHalfTest(UInt512 input, Half expected) => await Assert.That(Half.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingHalfTestData))] public async Task ConvertToSaturatingHalfTest(UInt512 input, Half expected) => await Assert.That(Half.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingHalfTestData))] public async Task ConvertToTruncatingHalfTest(UInt512 input, Half expected) => await Assert.That(Half.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedSingleTestData))] public async Task ConvertToCheckedSingleTest(UInt512 input, float expected) => await Assert.That(float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingSingleTestData))] public async Task ConvertToSaturatingSingleTest(UInt512 input, float expected) => await Assert.That(float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingSingleTestData))] public async Task ConvertToTruncatingSingleTest(UInt512 input, float expected) => await Assert.That(float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedDoubleTestData))] public async Task ConvertToCheckedDoubleTest(UInt512 input, double expected) => await Assert.That(double.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingDoubleTestData))] public async Task ConvertToSaturatingDoubleTest(UInt512 input, double expected) => await Assert.That(double.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingDoubleTestData))] public async Task ConvertToTruncatingDoubleTest(UInt512 input, double expected) => await Assert.That(double.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedQuadTestData))] public async Task ConvertToCheckedQuadTest(UInt512 input, Quad expected) => await Assert.That(Quad.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingQuadTestData))] public async Task ConvertToSaturatingQuadTest(UInt512 input, Quad expected) => await Assert.That(Quad.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingQuadTestData))] public async Task ConvertToTruncatingQuadTest(UInt512 input, Quad expected) => await Assert.That(Quad.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedOctoTestData))] public async Task ConvertToCheckedOctoTest(UInt512 input, Octo expected) => await Assert.That(Octo.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingOctoTestData))] public async Task ConvertToSaturatingOctoTest(UInt512 input, Octo expected) => await Assert.That(Octo.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingOctoTestData))] public async Task ConvertToTruncatingOctoTest(UInt512 input, Octo expected) => await Assert.That(Octo.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedNFloatTestData))] public async Task ConvertToCheckedNFloatTest(UInt512 input, NFloat expected) => await Assert.That(NFloat.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingNFloatTestData))] public async Task ConvertToSaturatingNFloatTest(UInt512 input, NFloat expected) => await Assert.That(NFloat.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingNFloatTestData))] public async Task ConvertToTruncatingNFloatTest(UInt512 input, NFloat expected) => await Assert.That(NFloat.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedByteTestData))] public async Task ConvertFromCheckedByteTest(byte input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingByteTestData))] public async Task ConvertFromSaturatingByteTest(byte input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingByteTestData))] public async Task ConvertFromTruncatingByteTest(byte input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUInt16TestData))] public async Task ConvertFromCheckedUInt16Test(ushort input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUInt16TestData))] public async Task ConvertFromSaturatingUInt16Test(ushort input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUInt16TestData))] public async Task ConvertFromTruncatingUInt16Test(ushort input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUInt32TestData))] public async Task ConvertFromCheckedUInt32Test(uint input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUInt32TestData))] public async Task ConvertFromSaturatingUInt32Test(uint input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUInt32TestData))] public async Task ConvertFromTruncatingUInt32Test(uint input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUInt64TestData))] public async Task ConvertFromCheckedUInt64Test(ulong input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUInt64TestData))] public async Task ConvertFromSaturatingUInt64Test(ulong input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUInt64TestData))] public async Task ConvertFromTruncatingUInt64Test(ulong input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUInt128TestData))] public async Task ConvertFromCheckedUInt128Test(UInt128 input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUInt128TestData))] public async Task ConvertFromSaturatingUInt128Test(UInt128 input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUInt128TestData))] public async Task ConvertFromTruncatingUInt128Test(UInt128 input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUIntPtrTestData))] public async Task ConvertFromCheckedUIntPtrTest(nuint input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUIntPtrTestData))] public async Task ConvertFromSaturatingUIntPtrTest(nuint input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUIntPtrTestData))] public async Task ConvertFromTruncatingUIntPtrTest(nuint input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedSByteTestData))] public async Task ConvertFromCheckedSByteTest(sbyte input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingSByteTestData))] public async Task ConvertFromSaturatingSByteTest(sbyte input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingSByteTestData))] public async Task ConvertFromTruncatingSByteTest(sbyte input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedInt16TestData))] public async Task ConvertFromCheckedInt16Test(short input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingInt16TestData))] public async Task ConvertFromSaturatingInt16Test(short input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingInt16TestData))] public async Task ConvertFromTruncatingInt16Test(short input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedInt32TestData))] public async Task ConvertFromCheckedInt32Test(int input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingInt32TestData))] public async Task ConvertFromSaturatingInt32Test(int input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingInt32TestData))] public async Task ConvertFromTruncatingInt32Test(int input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedInt64TestData))] public async Task ConvertFromCheckedInt64Test(long input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingInt64TestData))] public async Task ConvertFromSaturatingInt64Test(long input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingInt64TestData))] public async Task ConvertFromTruncatingInt64Test(long input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedInt128TestData))] public async Task ConvertFromCheckedInt128Test(Int128 input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingInt128TestData))] public async Task ConvertFromSaturatingInt128Test(Int128 input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingInt128TestData))] public async Task ConvertFromTruncatingInt128Test(Int128 input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedIntPtrTestData))] public async Task ConvertFromCheckedIntPtrTest(nint input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingIntPtrTestData))] public async Task ConvertFromSaturatingIntPtrTest(nint input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingIntPtrTestData))] public async Task ConvertFromTruncatingIntPtrTest(nint input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedBigIntegerTestData))] public async Task ConvertFromCheckedBigIntegerTest(BigInteger input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingBigIntegerTestData))] public async Task ConvertFromSaturatingBigIntegerTest(BigInteger input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingBigIntegerTestData))] public async Task ConvertFromTruncatingBigIntegerTest(BigInteger input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedHalfTestData))] public async Task ConvertFromCheckedHalfTest(Half input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingHalfTestData))] public async Task ConvertFromSaturatingHalfTest(Half input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingHalfTestData))] public async Task ConvertFromTruncatingHalfTest(Half input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedSingleTestData))] public async Task ConvertFromCheckedSingleTest(float input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingSingleTestData))] public async Task ConvertFromSaturatingSingleTest(float input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingSingleTestData))] public async Task ConvertFromTruncatingSingleTest(float input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedDoubleTestData))] public async Task ConvertFromCheckedDoubleTest(double input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingDoubleTestData))] public async Task ConvertFromSaturatingDoubleTest(double input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingDoubleTestData))] public async Task ConvertFromTruncatingDoubleTest(double input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedNFloatTestData))] public async Task ConvertFromCheckedNFloatTest(NFloat input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingNFloatTestData))] public async Task ConvertFromSaturatingNFloatTest(NFloat input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingNFloatTestData))] public async Task ConvertFromTruncatingNFloatTest(NFloat input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);
	#endregion
}