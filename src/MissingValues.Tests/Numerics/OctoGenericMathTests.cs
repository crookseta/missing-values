using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissingValues.Tests.Data;
using MissingValues.Tests.Extensions;
using static MissingValues.Tests.Data.OctoDataSources;

using Float = MissingValues.Octo;
using DataSources = MissingValues.Tests.Data.OctoDataSources;

namespace MissingValues.Tests.Numerics;

public class OctoGenericMathTests
{
    #region Operators
    [Test]
    [MethodDataSource<DataSources>(nameof(op_AdditionTestData))]
    public async Task op_AdditionTest(Float left, Float right, Float expected)
    {
        var result = left + right;

        await Assert.That(result).IsEqualTo(expected).And.IsBitwiseEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_IncrementTestData))]
    public async Task op_IncrementTest(Float value, Float expected)
    {
        var result = ++value;

        using (Assert.Multiple())
        {
            await Assert.That(result).IsEqualTo(expected).And.IsBitwiseEquivalentTo(expected);
            await Assert.That(result).IsEqualTo(value).And.IsBitwiseEquivalentTo(value);
        }
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_SubtractionTestData))]
    public async Task op_SubtractionTest(Float left, Float right, Float expected)
    {
        var result = left - right;

        if (Float.IsNaN(expected))
        {
	        await Assert.That(result).IsNaN();
        }
        else
        {
			await Assert.That(result).IsEqualTo(expected).And.IsBitwiseEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_DecrementTestData))]
    public async Task op_DecrementTest(Float value, Float expected)
    {
        var result = --value;

        using (Assert.Multiple())
        {
            await Assert.That(result).IsEqualTo(expected).And.IsBitwiseEquivalentTo(expected);
            await Assert.That(result).IsEqualTo(value).And.IsBitwiseEquivalentTo(value);
        }
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_MultiplyTestData))]
    public async Task op_MultiplyTest(Float left, Float right, Float expected)
    {
        var result = left * right;

        await Assert.That(result).IsEqualTo(expected).And.IsBitwiseEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_DivisionTestData))]
    public async Task op_DivisionTest(Float left, Float right, Float expected)
    {
        var result = left / right;

        await Assert.That(result).IsEqualTo(expected).And.IsBitwiseEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_ModulusTestData))]
    public async Task op_ModulusTest(Float left, Float right, Float expected)
    {
        var result = left % right;

        await Assert.That(result).IsEqualTo(expected).And.IsBitwiseEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_OnesComplementTestData))]
    public async Task op_OnesComplementTest(Float value, Float expected)
    {
        var result = Helper.OnesComplement(value);

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_BitwiseAndTestData))]
    public async Task op_BitwiseAndTest(Float left, Float right, Float expected)
    {
        var result = Helper.And(left, right);

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_BitwiseOrTestData))]
    public async Task op_BitwiseOrTest(Float left, Float right, Float expected)
    {
        var result = Helper.Or(left, right);

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_BitwiseXorTestData))]
    public async Task op_BitwiseXorTest(Float left, Float right, Float expected)
    {
        var result = Helper.Xor(left, right);

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_EqualityTestData))]
    public async Task op_EqualityTest(Float left, Float right, bool expected)
    {
        var result = left == right;

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_InequalityTestData))]
    public async Task op_InequalityTest(Float left, Float right, bool expected)
    {
        var result = left != right;

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_LessThanTestData))]
    public async Task op_LessThanTest(Float left, Float right, bool expected)
    {
        var result = left < right;

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_LessThanOrEqualTestData))]
    public async Task op_LessThanOrEqualTest(Float left, Float right, bool expected)
    {
        var result = left <= right;

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_GreaterThanTestData))]
    public async Task op_GreaterThanTest(Float left, Float right, bool expected)
    {
        var result = left > right;

        await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(op_GreaterThanOrEqualTestData))]
    public async Task op_GreaterThanOrEqualTest(Float left, Float right, bool expected)
    {
        var result = left >= right;

        await Assert.That(result).IsEqualTo(expected);
    }
    #endregion

    #region INumberBase
    [Test]
	[MethodDataSource<QuadDataSources>(nameof(AbsTestData))]
	public async Task AbsTest(Quad value, Quad expected)
	{
		Quad result = Helper.Abs(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsCanonicalTestData))]
	public async Task IsCanonicalTest(Quad value, bool expected)
	{
		bool result = Helper.IsCanonical(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsComplexNumberTestData))]
	public async Task IsComplexNumberTest(Quad value, bool expected)
	{
		bool result = Helper.IsComplexNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsEvenIntegerTestData))]
	public async Task IsEvenIntegerTest(Quad value, bool expected)
	{
		bool result = Helper.IsEvenInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsFiniteTestData))]
	public async Task IsFiniteTest(Quad value, bool expected)
	{
		bool result = Helper.IsFinite(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsImaginaryNumberTestData))]
	public async Task IsImaginaryNumberTest(Quad value, bool expected)
	{
		bool result = Helper.IsImaginaryNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsInfinityTestData))]
	public async Task IsInfinityTest(Quad value, bool expected)
	{
		bool result = Helper.IsInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsIntegerTestData))]
	public async Task IsIntegerTest(Quad value, bool expected)
	{
		bool result = Helper.IsInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsNaNTestData))]
	public async Task IsNaNTest(Quad value, bool expected)
	{
		bool result = Helper.IsNaN(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsNegativeTestData))]
	public async Task IsNegativeTest(Quad value, bool expected)
	{
		bool result = Helper.IsNegative(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsNegativeInfinityTestData))]
	public async Task IsNegativeInfinityTest(Quad value, bool expected)
	{
		bool result = Helper.IsNegativeInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsNormalTestData))]
	public async Task IsNormalTest(Quad value, bool expected)
	{
		bool result = Helper.IsNormal(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsOddIntegerTestData))]
	public async Task IsOddIntegerTest(Quad value, bool expected)
	{
		bool result = Helper.IsOddInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsPositiveTestData))]
	public async Task IsPositiveTest(Quad value, bool expected)
	{
		bool result = Helper.IsPositive(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsPositiveInfinityTestData))]
	public async Task IsPositiveInfinityTest(Quad value, bool expected)
	{
		bool result = Helper.IsPositiveInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsRealNumberTestData))]
	public async Task IsRealNumberTest(Quad value, bool expected)
	{
		bool result = Helper.IsRealNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsSubnormalTestData))]
	public async Task IsSubnormalTest(Quad value, bool expected)
	{
		bool result = Helper.IsSubnormal(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(IsZeroTestData))]
	public async Task IsZeroTest(Quad value, bool expected)
	{
		bool result = Helper.IsZero(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(MaxMagnitudeTestData))]
	public async Task MaxMagnitudeTest(Quad x, Quad y, Quad expected)
	{
		var result = Helper.MaxMagnitude(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(MaxMagnitudeNumberTestData))]
	public async Task MaxMagnitudeNumberTest(Quad x, Quad y, Quad expected)
	{
		var result = Helper.MaxMagnitudeNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(MinMagnitudeTestData))]
	public async Task MinMagnitudeTest(Quad x, Quad y, Quad expected)
	{
		var result = Helper.MinMagnitude(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(MinMagnitudeNumberTestData))]
	public async Task MinMagnitudeNumberTest(Quad x, Quad y, Quad expected)
	{
		var result = Helper.MinMagnitudeNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(MultiplyAddEstimateTestData))]
	public async Task MultiplyAddEstimateTest(Quad left, Quad right, Quad addend, Quad expected)
	{
		var result = Helper.MultiplyAddEstimate(left, right, addend);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(ParseTestData))]
	public async Task ParseTest(string s, NumberStyles style, IFormatProvider? provider, Quad expected)
	{
		var result = Helper.Parse<Quad>(s, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(ParseSpanTestData))]
	public async Task ParseTest(char[] s, NumberStyles style, IFormatProvider? provider, Quad expected)
	{
		var result = Helper.Parse<Quad>(s, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(ParseUtf8TestData))]
	public async Task ParseTest(byte[] utf8Text, NumberStyles style, IFormatProvider? provider, Quad expected)
	{
		var result = Helper.Parse<Quad>(utf8Text, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(TryParseTestData))]
	public async Task TryParseTest(string s, NumberStyles style, IFormatProvider? provider, bool expected, Quad expectedValue)
	{
		var success = Helper.TryParse<Quad>(s, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(TryParseSpanTestData))]
	public async Task TryParseTest(char[] s, NumberStyles style, IFormatProvider? provider, bool expected, Quad expectedValue)
	{
		var success = Helper.TryParse<Quad>(s, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	[Test]
	[MethodDataSource<QuadDataSources>(nameof(TryParseUtf8TestData))]
	public async Task TryParseTest(byte[] utf8Text, NumberStyles style, IFormatProvider? provider, bool expected, Quad expectedValue)
	{
		var success = Helper.TryParse<Quad>(utf8Text, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ToStringTestData))]
	public async Task ToStringTest(Float value, string fmt, IFormatProvider? provider, string expected)
	{
		await Assert.That(value.ToString(fmt, provider)).EqualTo(expected);
	}
    #endregion
    
    #region INumber
    [Test]
    [MethodDataSource<DataSources>(nameof(ClampTestData))]
    public async Task ClampTest(Float value, Float min, Float max, Float expected)
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
    public async Task CopySignTest(Float value, Float sign, Float expected)
    {
	    var result = Helper.CopySign(value, sign);
	    await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(MaxTestData))]
    public async Task MaxTest(Float x, Float y, Float expected)
    {
	    var result = Helper.Max(x, y);
	    await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(MaxNumberTestData))]
    public async Task MaxNumberTest(Float x, Float y, Float expected)
    {
	    var result = Helper.MaxNumber(x, y);
	    await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(MinTestData))]
    public async Task MinTest(Float x, Float y, Float expected)
    {
	    var result = Helper.Min(x, y);
	    await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(MinNumberTestData))]
    public async Task MinNumberTest(Float x, Float y, Float expected)
    {
	    var result = Helper.MinNumber(x, y);
	    await Assert.That(result).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(SignTestData))]
    public async Task SignTest(Float value, int expected)
    {
	    var result = Helper.Sign(value);
	    await Assert.That(result).IsEqualTo(expected);
    }
    #endregion
    
    #region IBinaryNumber
    [Test]
    [MethodDataSource<DataSources>(nameof(IsPow2TestData))]
    public async Task IsPow2Test(Float value, bool expected)
    {
	    var result = Helper.IsPow2(value);
	    await Assert.That(result).IsEqualTo(expected);
    }
    #endregion
    
    #region IFloatingPoint
    [Test]
    [MethodDataSource<DataSources>(nameof(RoundTestData))]
    public async Task RoundTest(Float value, int digits, MidpointRounding midpointRounding, Float expected)
    {
	    var result = Float.Round(value, digits, midpointRounding);
	    await Assert.That(result).IsEqualTo(expected);
    }
    #endregion

    #region IFloatingPointIeee754
    [Test]
    [MethodDataSource<DataSources>(nameof(BitDecrementTestData))]
    public async Task BitDecrementTest(Float value, Float expected)
    {
	    await Assert.That(Float.BitDecrement(value)).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(BitIncrementTestData))]
    public async Task BitIncrementTest(Float value, Float expected)
    {
	    await Assert.That(Float.BitIncrement(value)).IsEqualTo(expected);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(FusedMultiplyAddTestData))]
    public async Task FusedMultiplyAddTest(Float left, Float right, Float addend, Float result)
    {
	    await Assert.That(Float.FusedMultiplyAdd(left, right, addend))
		    .IsEqualTo(result).And
		    .IsBitwiseEquivalentTo(result);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(Ieee754RemainderTestData))]
    public async Task IeeeRemainderTest(Float left, Float right, Float result)
    {
	    if (Float.IsNaN(result))
	    {
		    await Assert.That(Float.Ieee754Remainder(left, right))
			    .IsNaN();
	    }
	    else
	    {
		    await Assert.That(Float.Ieee754Remainder(left, right))
			    .IsBitwiseEquivalentTo(result);
	    }
    }

    [Test]
    [MethodDataSource<DataSources>(nameof(ILogBTestData))]
    public async Task ILogBTest(Float x, int result)
    {
	    await Assert.That(Float.ILogB(x)).IsEqualTo(result);
    }
    [Test]
    public async Task ReciprocalEstimateTest()
    {
	    await Assert.That(Float.ReciprocalEstimate(Float.Two)).IsEqualTo(Float.Half);
	    await Assert.That(Float.ReciprocalEstimate(Float.Four))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFF_D000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), Float.Delta);
    }
    [Test]
    [MethodDataSource<DataSources>(nameof(ScaleBTestData))]
    public async Task ScaleBTest(Float x, int n, Float result)
    {
	    await Assert.That(Float.ScaleB(x, n))
		    .IsBitwiseEquivalentTo(result);
    }

    [Test]
    public async Task SqrtTest()
    {
	    await Assert.That(Float.Sqrt(Float.Zero)).EqualTo(Float.Zero);
	    await Assert.That(Float.Sqrt(Float.NegativeZero)).EqualTo(Float.NegativeZero);
	    await Assert.That(Float.Sqrt(Float.PositiveInfinity)).EqualTo(Float.PositiveInfinity);
	    await Assert.That(Float.Sqrt(Float.NegativeFour)).IsNaN();
	    await Assert.That(Float.Sqrt(Float.NaN)).IsNaN();
	    
	    await Assert.That(Float.Sqrt(Float.Hundred)).IsApproximately(Float.Ten, Float.Delta);
    }
    #endregion

    #region Conversion
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedByteTestData))] public async Task ConvertToCheckedByteTest(Float input, byte expected) => await Assert.That(byte.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingByteTestData))] public async Task ConvertToSaturatingByteTest(Float input, byte expected) => await Assert.That(byte.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingByteTestData))] public async Task ConvertToTruncatingByteTest(Float input, byte expected) => await Assert.That(byte.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt16TestData))] public async Task ConvertToCheckedUInt16Test(Float input, ushort expected) => await Assert.That(ushort.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt16TestData))] public async Task ConvertToSaturatingUInt16Test(Float input, ushort expected) => await Assert.That(ushort.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt16TestData))] public async Task ConvertToTruncatingUInt16Test(Float input, ushort expected) => await Assert.That(ushort.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt32TestData))] public async Task ConvertToCheckedUInt32Test(Float input, uint expected) => await Assert.That(uint.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt32TestData))] public async Task ConvertToSaturatingUInt32Test(Float input, uint expected) => await Assert.That(uint.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt32TestData))] public async Task ConvertToTruncatingUInt32Test(Float input, uint expected) => await Assert.That(uint.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt64TestData))] public async Task ConvertToCheckedUInt64Test(Float input, ulong expected) => await Assert.That(ulong.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt64TestData))] public async Task ConvertToSaturatingUInt64Test(Float input, ulong expected) => await Assert.That(ulong.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt64TestData))] public async Task ConvertToTruncatingUInt64Test(Float input, ulong expected) => await Assert.That(ulong.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt128TestData))] public async Task ConvertToCheckedUInt128Test(Float input, UInt128 expected) => await Assert.That(UInt128.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt128TestData))] public async Task ConvertToSaturatingUInt128Test(Float input, UInt128 expected) => await Assert.That(UInt128.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt128TestData))] public async Task ConvertToTruncatingUInt128Test(Float input, UInt128 expected) => await Assert.That(UInt128.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt256TestData))] public async Task ConvertToCheckedUInt256Test(Float input, UInt256 expected) => await Assert.That(UInt256.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt256TestData))] public async Task ConvertToSaturatingUInt256Test(Float input, UInt256 expected) => await Assert.That(UInt256.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt256TestData))] public async Task ConvertToTruncatingUInt256Test(Float input, UInt256 expected) => await Assert.That(UInt256.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedUInt512TestData))] public async Task ConvertToCheckedUInt512Test(Float input, UInt512 expected) => await Assert.That(UInt512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingUInt512TestData))] public async Task ConvertToSaturatingUInt512Test(Float input, UInt512 expected) => await Assert.That(UInt512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingUInt512TestData))] public async Task ConvertToTruncatingUInt512Test(Float input, UInt512 expected) => await Assert.That(UInt512.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedSByteTestData))] public async Task ConvertToCheckedSByteTest(Float input, sbyte expected) => await Assert.That(sbyte.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingSByteTestData))] public async Task ConvertToSaturatingSByteTest(Float input, sbyte expected) => await Assert.That(sbyte.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingSByteTestData))] public async Task ConvertToTruncatingByteTest(Float input, sbyte expected) => await Assert.That(sbyte.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt16TestData))] public async Task ConvertToCheckedInt16Test(Float input, short expected) => await Assert.That(ushort.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt16TestData))] public async Task ConvertToSaturatingInt16Test(Float input, short expected) => await Assert.That(ushort.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt16TestData))] public async Task ConvertToTruncatingInt16Test(Float input, short expected) => await Assert.That(ushort.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt32TestData))] public async Task ConvertToCheckedInt32Test(Float input, int expected) => await Assert.That(uint.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt32TestData))] public async Task ConvertToSaturatingInt32Test(Float input, int expected) => await Assert.That(uint.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt32TestData))] public async Task ConvertToTruncatingInt32Test(Float input, int expected) => await Assert.That(uint.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt64TestData))] public async Task ConvertToCheckedInt64Test(Float input, long expected) => await Assert.That(ulong.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt64TestData))] public async Task ConvertToSaturatingInt64Test(Float input, long expected) => await Assert.That(ulong.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt64TestData))] public async Task ConvertToTruncatingInt64Test(Float input, long expected) => await Assert.That(ulong.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt128TestData))] public async Task ConvertToCheckedInt128Test(Float input, Int128 expected) => await Assert.That(Int128.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt128TestData))] public async Task ConvertToSaturatingInt128Test(Float input, Int128 expected) => await Assert.That(Int128.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt128TestData))] public async Task ConvertToTruncatingInt128Test(Float input, Int128 expected) => await Assert.That(Int128.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt256TestData))] public async Task ConvertToCheckedInt256Test(Float input, Int256 expected) => await Assert.That(Int256.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt256TestData))] public async Task ConvertToSaturatingInt256Test(Float input, Int256 expected) => await Assert.That(Int256.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt256TestData))] public async Task ConvertToTruncatingInt256Test(Float input, Int256 expected) => await Assert.That(Int256.CreateTruncating(input)).IsEqualTo(expected);

	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedInt512TestData))] public async Task ConvertToCheckedInt512Test(Float input, Int512 expected) => await Assert.That(Int512.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingInt512TestData))] public async Task ConvertToSaturatingInt512Test(Float input, Int512 expected) => await Assert.That(Int512.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingInt512TestData))] public async Task ConvertToTruncatingInt512Test(Float input, Int512 expected) => await Assert.That(Int512.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedHalfTestData))] public async Task ConvertToCheckedHalfTest(Float input, Half expected) => await Assert.That(Half.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingHalfTestData))] public async Task ConvertToSaturatingHalfTest(Float input, Half expected) => await Assert.That(Half.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingHalfTestData))] public async Task ConvertToTruncatingHalfTest(Float input, Half expected) => await Assert.That(Half.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedSingleTestData))] public async Task ConvertToCheckedSingleTest(Float input, float expected) => await Assert.That(float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingSingleTestData))] public async Task ConvertToSaturatingSingleTest(Float input, float expected) => await Assert.That(float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingSingleTestData))] public async Task ConvertToTruncatingSingleTest(Float input, float expected) => await Assert.That(float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedDoubleTestData))] public async Task ConvertToCheckedDoubleTest(Float input, double expected) => await Assert.That(double.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingDoubleTestData))] public async Task ConvertToSaturatingDoubleTest(Float input, double expected) => await Assert.That(double.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingDoubleTestData))] public async Task ConvertToTruncatingDoubleTest(Float input, double expected) => await Assert.That(double.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertToCheckedQuadTestData))] public async Task ConvertToCheckedQuadTest(Float input, Quad expected) => await Assert.That(Quad.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToSaturatingQuadTestData))] public async Task ConvertToSaturatingQuadTest(Float input, Quad expected) => await Assert.That(Quad.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertToTruncatingQuadTestData))] public async Task ConvertToTruncatingQuadTest(Float input, Quad expected) => await Assert.That(Quad.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedByteTestData))] public async Task ConvertFromCheckedByteTest(byte input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingByteTestData))] public async Task ConvertFromSaturatingByteTest(byte input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingByteTestData))] public async Task ConvertFromTruncatingByteTest(byte input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUInt16TestData))] public async Task ConvertFromCheckedUInt16Test(ushort input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUInt16TestData))] public async Task ConvertFromSaturatingUInt16Test(ushort input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUInt16TestData))] public async Task ConvertFromTruncatingUInt16Test(ushort input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUInt32TestData))] public async Task ConvertFromCheckedUInt32Test(uint input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUInt32TestData))] public async Task ConvertFromSaturatingUInt32Test(uint input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUInt32TestData))] public async Task ConvertFromTruncatingUInt32Test(uint input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUInt64TestData))] public async Task ConvertFromCheckedUInt64Test(ulong input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUInt64TestData))] public async Task ConvertFromSaturatingUInt64Test(ulong input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUInt64TestData))] public async Task ConvertFromTruncatingUInt64Test(ulong input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedUInt128TestData))] public async Task ConvertFromCheckedUInt128Test(UInt128 input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingUInt128TestData))] public async Task ConvertFromSaturatingUInt128Test(UInt128 input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingUInt128TestData))] public async Task ConvertFromTruncatingUInt128Test(UInt128 input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedSByteTestData))] public async Task ConvertFromCheckedSByteTest(sbyte input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingSByteTestData))] public async Task ConvertFromSaturatingSByteTest(sbyte input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingSByteTestData))] public async Task ConvertFromTruncatingSByteTest(sbyte input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedInt16TestData))] public async Task ConvertFromCheckedInt16Test(short input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingInt16TestData))] public async Task ConvertFromSaturatingInt16Test(short input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingInt16TestData))] public async Task ConvertFromTruncatingInt16Test(short input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedInt32TestData))] public async Task ConvertFromCheckedInt32Test(int input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingInt32TestData))] public async Task ConvertFromSaturatingInt32Test(int input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingInt32TestData))] public async Task ConvertFromTruncatingInt32Test(int input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedInt64TestData))] public async Task ConvertFromCheckedInt64Test(long input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingInt64TestData))] public async Task ConvertFromSaturatingInt64Test(long input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingInt64TestData))] public async Task ConvertFromTruncatingInt64Test(long input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedInt128TestData))] public async Task ConvertFromCheckedInt128Test(Int128 input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingInt128TestData))] public async Task ConvertFromSaturatingInt128Test(Int128 input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingInt128TestData))] public async Task ConvertFromTruncatingInt128Test(Int128 input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedHalfTestData))] public async Task ConvertFromCheckedHalfTest(Half input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingHalfTestData))] public async Task ConvertFromSaturatingHalfTest(Half input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingHalfTestData))] public async Task ConvertFromTruncatingHalfTest(Half input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedSingleTestData))] public async Task ConvertFromCheckedSingleTest(float input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingSingleTestData))] public async Task ConvertFromSaturatingSingleTest(float input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingSingleTestData))] public async Task ConvertFromTruncatingSingleTest(float input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
	
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromCheckedDoubleTestData))] public async Task ConvertFromCheckedDoubleTest(double input, Float expected) => await Assert.That(Float.CreateChecked(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromSaturatingDoubleTestData))] public async Task ConvertFromSaturatingDoubleTest(double input, Float expected) => await Assert.That(Float.CreateSaturating(input)).IsEqualTo(expected);
	[Test, MethodDataSource<DataSources>(nameof(ConvertFromTruncatingDoubleTestData))] public async Task ConvertFromTruncatingDoubleTest(double input, Float expected) => await Assert.That(Float.CreateTruncating(input)).IsEqualTo(expected);
    #endregion
}