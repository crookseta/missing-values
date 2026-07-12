using System.Globalization;
using MissingValues.Tests.Extensions;
using static MissingValues.Tests.Data.QuadDataSources;

using Float = MissingValues.Quad;
using DataSources = MissingValues.Tests.Data.QuadDataSources;

namespace MissingValues.Tests.Numerics;

public class QuadGenericMathTests
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
	[MethodDataSource<DataSources>(nameof(AbsTestData))]
	public async Task AbsTest(Float value, Float expected)
	{
		Float result = Helper.Abs(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsCanonicalTestData))]
	public async Task IsCanonicalTest(Float value, bool expected)
	{
		bool result = Helper.IsCanonical(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsComplexNumberTestData))]
	public async Task IsComplexNumberTest(Float value, bool expected)
	{
		bool result = Helper.IsComplexNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsEvenIntegerTestData))]
	public async Task IsEvenIntegerTest(Float value, bool expected)
	{
		bool result = Helper.IsEvenInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsFiniteTestData))]
	public async Task IsFiniteTest(Float value, bool expected)
	{
		bool result = Helper.IsFinite(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsImaginaryNumberTestData))]
	public async Task IsImaginaryNumberTest(Float value, bool expected)
	{
		bool result = Helper.IsImaginaryNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsInfinityTestData))]
	public async Task IsInfinityTest(Float value, bool expected)
	{
		bool result = Helper.IsInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsIntegerTestData))]
	public async Task IsIntegerTest(Float value, bool expected)
	{
		bool result = Helper.IsInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsNaNTestData))]
	public async Task IsNaNTest(Float value, bool expected)
	{
		bool result = Helper.IsNaN(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsNegativeTestData))]
	public async Task IsNegativeTest(Float value, bool expected)
	{
		bool result = Helper.IsNegative(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsNegativeInfinityTestData))]
	public async Task IsNegativeInfinityTest(Float value, bool expected)
	{
		bool result = Helper.IsNegativeInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsNormalTestData))]
	public async Task IsNormalTest(Float value, bool expected)
	{
		bool result = Helper.IsNormal(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsOddIntegerTestData))]
	public async Task IsOddIntegerTest(Float value, bool expected)
	{
		bool result = Helper.IsOddInteger(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsPositiveTestData))]
	public async Task IsPositiveTest(Float value, bool expected)
	{
		bool result = Helper.IsPositive(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsPositiveInfinityTestData))]
	public async Task IsPositiveInfinityTest(Float value, bool expected)
	{
		bool result = Helper.IsPositiveInfinity(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsRealNumberTestData))]
	public async Task IsRealNumberTest(Float value, bool expected)
	{
		bool result = Helper.IsRealNumber(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsSubnormalTestData))]
	public async Task IsSubnormalTest(Float value, bool expected)
	{
		bool result = Helper.IsSubnormal(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(IsZeroTestData))]
	public async Task IsZeroTest(Float value, bool expected)
	{
		bool result = Helper.IsZero(value);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MaxMagnitudeTestData))]
	public async Task MaxMagnitudeTest(Float x, Float y, Float expected)
	{
		var result = Helper.MaxMagnitude(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MaxMagnitudeNumberTestData))]
	public async Task MaxMagnitudeNumberTest(Float x, Float y, Float expected)
	{
		var result = Helper.MaxMagnitudeNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MinMagnitudeTestData))]
	public async Task MinMagnitudeTest(Float x, Float y, Float expected)
	{
		var result = Helper.MinMagnitude(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MinMagnitudeNumberTestData))]
	public async Task MinMagnitudeNumberTest(Float x, Float y, Float expected)
	{
		var result = Helper.MinMagnitudeNumber(x, y);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(MultiplyAddEstimateTestData))]
	public async Task MultiplyAddEstimateTest(Float left, Float right, Float addend, Float expected)
	{
		var result = Helper.MultiplyAddEstimate(left, right, addend);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ParseTestData))]
	public async Task ParseTest(string s, NumberStyles style, IFormatProvider? provider, Float expected)
	{
		var result = Helper.Parse<Float>(s, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ParseSpanTestData))]
	public async Task ParseTest(char[] s, NumberStyles style, IFormatProvider? provider, Float expected)
	{
		var result = Helper.Parse<Float>(s, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ParseUtf8TestData))]
	public async Task ParseTest(byte[] utf8Text, NumberStyles style, IFormatProvider? provider, Float expected)
	{
		var result = Helper.Parse<Float>(utf8Text, style, provider);
		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(TryParseTestData))]
	public async Task TryParseTest(string s, NumberStyles style, IFormatProvider? provider, bool expected, Float expectedValue)
	{
		var success = Helper.TryParse<Float>(s, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(TryParseSpanTestData))]
	public async Task TryParseTest(char[] s, NumberStyles style, IFormatProvider? provider, bool expected, Float expectedValue)
	{
		var success = Helper.TryParse<Float>(s, style, provider, out var result);
		using (Assert.Multiple())
		{
			await Assert.That(success).IsEqualTo(expected);
			await Assert.That(result).IsEqualTo(expectedValue);
		}
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(TryParseUtf8TestData))]
	public async Task TryParseTest(byte[] utf8Text, NumberStyles style, IFormatProvider? provider, bool expected, Float expectedValue)
	{
		var success = Helper.TryParse<Float>(utf8Text, style, provider, out var result);
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
    public async Task AcosTest()
    {
	    await Assert.That(Float.Acos(Float.NaN)).IsNaN();
	    await Assert.That(Float.Acos(Float.Two)).IsNaN();
	    await Assert.That(Float.Acos(Float.NegativeTwo)).IsNaN();
	    await Assert.That(Float.Acos(Float.Half))
		    .IsApproximately(Float.Pi / Float.Three, Float.Delta);
	    await Assert.That(Float.Acos(Float.One))
		    .IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(Float.Acos(Float.NegativeOne))
		    .IsApproximately(Float.Pi, Float.Delta);
    }
    [Test]
    public async Task AcoshTest()
    {
	    await Assert.That(Float.Acosh(Float.Two))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFF_5124_2719_8043, 0x49BE_684B_D018_8D53), Float.Delta);
	    await Assert.That(Float.Acosh(Float.Half)).IsNaN();
	    await Assert.That(Float.Acosh(Float.Zero)).IsNaN();
	    await Assert.That(Float.Acosh(Float.NegativeOne)).IsNaN();
    }
    [Test]
    public async Task AsinTest()
    {
	    await Assert.That(Float.Asin(Float.Half))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFE_0C15_2382_D736, 0x5846_5BB3_2E0F_567B), Float.Delta)
		    .And.IsApproximately(Float.Pi / Float.Six, Float.Delta);
	    await Assert.That(Float.Asin(Float.Two)).IsNaN();
	    await Assert.That(Float.Asin(Float.One))
		    .IsApproximately(Float.Pi / Float.Two, Float.Delta);
	    await Assert.That(Float.Asin(Float.NegativeOne))
		    .IsApproximately(-Float.Pi / Float.Two, Float.Delta);
    }
    [Test]
    public async Task AsinhTest()
    {
	    await Assert.That(Float.Asinh(Float.Two))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFF_7192_1831_3D08, 0x72F8_E831_837F_0E95), Float.Delta);
	    await Assert.That(Float.Asinh(Float.Zero))
		    .IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(Float.Asinh(Values.CreateFloat<Float>(0xBFFF_8000_0000_0000, 0x0000_0000_0000_0000)))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFF_31DC_0090_B63D, 0x8682_7E4B_AAAD_1909), Float.Delta);
    }
    [Test]
    public async Task AtanTest()
    {
	    await Assert.That(Float.Atan(Float.Half))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFD_DAC6_7056_1BB4, 0xF1DE_7924_87B0_F0F3), Float.Delta);
	    await Assert.That(Float.Atan(Float.Zero))
		    .IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(Float.Atan(Float.PositiveInfinity))
		    .IsApproximately(Float.Pi / Float.Two, Float.Delta);
	    await Assert.That(Float.Atan(Float.Two))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFF_1B6E_192E_BBE4, 0x3F5A_7D44_566B_01A8), Float.Delta);
    }
    [Test]
    public async Task Atan2Test()
    {
	    await Assert.That(Float.Atan2(Float.Zero, Float.Two))
		    .IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(Float.Atan2(Float.Zero, Float.Zero))
		    .IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(Float.Atan2(Float.Zero, Float.NegativeTwo))
		    .IsApproximately(Float.Pi, Float.Delta);
	    await Assert.That(Float.Atan2(Float.One, Float.Two))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFD_DAC6_7056_1BB4, 0xF1DE_7924_87B0_F0F3), Float.Delta);
	    await Assert.That(Float.Atan2(Float.NegativeOne, Float.Two))
		    .IsApproximately(Values.CreateFloat<Float>(0xBFFD_DAC6_7056_1BB4, 0xF1DE_7924_87B0_F0F3), Float.Delta);
	    await Assert.That(Float.Atan2(Float.One, Float.NegativeTwo))
		    .IsApproximately(Values.CreateFloat<Float>(0x4000_56C6_E739_7F5A, 0xE130_A2BB_E272_574C), Float.Delta);
    }
    [Test]
    public async Task AtanhTest()
    {
	    await Assert.That(Float.Atanh(Float.Two))
		    .IsNaN();
	    await Assert.That(Float.Atanh(Float.NegativeFour))
		    .IsNaN();
	    await Assert.That(Float.Atanh(Float.Zero))
		    .IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(Float.Atanh(Float.Half))
		    .IsApproximately(Values.CreateFloat<Float>(0x3FFE_193E_A7AA_D030, 0xA976_BA8D_B53A_D6E3), Float.Delta);
    }
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
    public async Task CbrtTest()
    {
	    await Assert.That(Float.Cbrt(Values.CreateFloat<Float>(0x4005_0000_0000_0000, 0x0000_0000_0000_0000)))
		    .IsEqualTo(Float.Four);
	    await Assert.That(Float.Cbrt(Float.Zero))
		    .IsEqualTo(Float.Zero);
	    await Assert.That(Float.Cbrt(Float.NegativeZero))
		    .IsEqualTo(Float.NegativeZero);
	    await Assert.That(Float.Cbrt(Float.NegativeFour))
		    .IsApproximately(Values.CreateFloat<Float>(0xBFFF_965F_EA53_D6E3, 0xC82B_0599_9AB4_3DC5), Float.Delta);
	    await Assert.That(Float.Cbrt(Float.NaN)).IsNaN();
    }
    [Test]
	public async Task CosTest()
	{
		await Assert.That(Float.Cos(Float.Zero))
			.IsApproximately(Float.One, Float.Delta);
		await Assert.That(Float.Cos(Float.Pi / Float.Two))
			.IsApproximately(Float.Zero, Float.Delta);
		await Assert.That(Float.Cos(Float.Pi))
			.IsApproximately(Float.NegativeOne, Float.Delta);
		await Assert.That(Float.Cos(Float.Pi * Float.Two))
			.IsApproximately(Float.One, Float.Delta);
		await Assert.That(Float.Cos(Float.NaN))
			.IsNaN();
		await Assert.That(Float.Cos(Float.PositiveInfinity))
			.IsNaN();
		await Assert.That(Float.Cos(Float.NegativeInfinity))
			.IsNaN();
	}
	[Test]
	public async Task CoshTest()
	{
		await Assert.That(Float.Cosh(Float.Zero))
			.IsBitwiseEquivalentTo(Float.One);
		await Assert.That(Float.Cosh(Float.Two))
			.IsApproximately(Values.CreateFloat<Float>(0x4000_E18F_A0DF_2D9B, 0xC293_27F7_1777_4D0C), Float.Delta);
		await Assert.That(Float.Cosh(Float.Five))
			.IsApproximately(Values.CreateFloat<Float>(0x4005_28D6_FCBE_FF3A, 0x9C65_3333_916C_7D52), Float.Delta);
		await Assert.That(Float.Cosh(Float.NegativeFive))
			.IsApproximately(Values.CreateFloat<Float>(0x4005_28D6_FCBE_FF3A, 0x9C65_3333_916C_7D52), Float.Delta);
	}
	[Test]
	public async Task ExpTest()
	{
		await Assert.That(Float.Exp(Float.Two))
			.IsApproximately(Values.CreateFloat<Float>(0x4001_D8E6_4B8D_4DDA, 0xDCC3_3A3B_A206_B68B), Float.Delta);
		await Assert.That(Float.Exp(Float.NegativeHalf))
			.IsApproximately(Values.CreateFloat<Float>(0x3FFE_368B_2FC6_F960, 0x9FE7_ACEB_46AA_619C), Float.Delta);
		await Assert.That(Float.Exp(Values.CreateFloat<Float>(0x400C_7700_0000_0000, 0x0000_0000_0000_0000)))
			.IsEqualTo(Float.PositiveInfinity);
		await Assert.That(Float.Exp(Values.CreateFloat<Float>(0xC00C_7700_0000_0000, 0x0000_0000_0000_0000)))
			.IsEqualTo(Float.Zero);
		await Assert.That(Float.Exp(Float.Zero))
			.IsEqualTo(Float.One);
		await Assert.That(Float.Exp(Float.PositiveInfinity))
			.IsEqualTo(Float.PositiveInfinity);
		await Assert.That(Float.Exp(Float.NaN))
			.IsNaN();
		await Assert.That(Float.Exp(Float.NegativeInfinity))
			.IsEqualTo(Float.Zero);
	}
	[Test]
	public async Task ExpM1Test()
	{
		await Assert.That(Float.ExpM1(Float.Two))
			.IsApproximately(Float.Exp(Float.Two) - Float.One, Float.Delta);;
		await Assert.That(Float.ExpM1(Float.NegativeHalf))
			.IsApproximately(Float.Exp(Float.NegativeHalf) - Float.One, Float.Delta);
	}
	[Test]
	public async Task Exp10Test()
	{
		await Assert.That(Float.Exp10(Float.Two))
			.IsEqualTo(Float.Hundred);
		await Assert.That(Float.Exp10(Float.Zero))
			.IsEqualTo(Float.One);
		await Assert.That(Float.Exp10(Float.PositiveInfinity))
			.IsEqualTo(Float.PositiveInfinity);
		await Assert.That(Float.Exp10(Float.NaN))
			.IsNaN();
		await Assert.That(Float.Exp10(Float.NegativeInfinity))
			.IsEqualTo(Float.Zero);
	}
	[Test]
	public async Task Exp2Test()
	{
		await Assert.That(Float.Exp2(Float.Two))
			.IsEqualTo(Float.Four);
		await Assert.That(Float.Exp2(Float.Zero))
			.IsEqualTo(Float.One);
		await Assert.That(Float.Exp2(Float.PositiveInfinity))
			.IsEqualTo(Float.PositiveInfinity);
		await Assert.That(Float.Exp2(Float.NaN))
			.IsNaN();
		await Assert.That(Float.Exp2(Float.NegativeInfinity))
			.IsEqualTo(Float.Zero);
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
	public async Task HypotTest()
	{
		await Assert.That(Float.Hypot(Float.Hundred, Float.Ten))
			.IsApproximately(Values.CreateFloat<Float>(0x4005_91FE_B9F2_BF46, 0xC3A7_08A3_1212_49E7), Float.Delta);
		await Assert.That(Float.Hypot(Float.PositiveInfinity, Float.NegativeInfinity))
			.IsBitwiseEquivalentTo(Float.PositiveInfinity);
		await Assert.That(Float.Hypot(Float.NaN, Float.NaN))
			.IsNaN();
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
	public async Task LogTest()
	{
		await Assert.That(Float.Log(Float.Hundred))
			.IsApproximately(Values.CreateFloat<Float>(0x4001_26BB_1BBB_5551, 0x582D_D4AD_AC57_05A6), Float.Delta);
		await Assert.That(Float.Log(Float.One))
			.IsEqualTo(Float.Zero);
		await Assert.That(Float.Log(Float.Zero))
			.IsEqualTo(Float.NegativeInfinity);
		await Assert.That(Float.Log(Float.NegativeFive))
			.IsNaN();
	}
	[Test]
	public async Task LogP1Test()
	{
		await Assert.That(Float.LogP1(Float.One))
			.IsBitwiseEquivalentTo(Float.Log(Float.Two))
			.And.IsApproximately(Values.CreateFloat<Float>(0x3FFE_62E4_2FEF_A39E, 0xF357_93C7_6730_07E6), Float.Delta);
		await Assert.That(Float.LogP1(Float.Zero))
			.IsBitwiseEquivalentTo(Float.Zero);
		await Assert.That(Float.LogP1(Float.NegativeOne))
			.IsBitwiseEquivalentTo(Float.NegativeInfinity);
		await Assert.That(Float.LogP1(Float.NegativeFive))
			.IsNaN();
	}
    [Test]
    public async Task Log2Test()
    {
	    await Assert.That(Helper.Log2(Float.Four)).IsApproximately(Float.Two, Float.Delta);
	    await Assert.That(Helper.Log2(Float.One)).IsEqualTo(Float.Zero);
	    await Assert.That(Helper.Log2(Float.Zero)).IsEqualTo(Float.NegativeInfinity);
	    await Assert.That(Helper.Log2(Float.NegativeFive)).IsNaN();
    }
    [Test]
    public async Task Log10Test()
    {
	    await Assert.That(Float.Log10(Float.Thousand))
		    .IsApproximately(Float.Three, Float.Delta);
	    await Assert.That(Float.Log10(Float.One))
		    .IsEqualTo(Float.Zero);
	    await Assert.That(Float.Log10(Float.Zero))
		    .IsEqualTo(Float.NegativeInfinity);
	    await Assert.That(Float.Log10(Float.NegativeFive))
		    .IsNaN();
    }
    
    [Test]
	public async Task PowTest()
	{
	    await Assert.That(Float.Pow(Float.Three, Float.Ten))
	        .IsApproximately(Values.CreateFloat<Float>(0x400E_CD52_0000_0000, 0x0000_0000_0000_0000), Float.Delta);
	    await Assert.That(Float.Pow(Float.Two, Float.NegativeFour))
	        .IsApproximately(Values.CreateFloat<Float>(0x3FFB_0000_0000_0000, 0x0000_0000_0000_0000), Float.Delta);
	
	    Float anything = 8, oddInt = 7, nonInt = 7.5d, greaterThanOne = Float.GreaterThanOneSmallest, lessThanOne = Float.LessThanOneLargest;
	
	    await Assert.That(Float.Pow(anything, Float.Zero)).IsBitwiseEquivalentTo(Float.One);
	    await Assert.That(Float.Pow(anything, Float.One)).IsBitwiseEquivalentTo(anything);
	    await Assert.That(Float.Pow(anything, Float.NaN)).IsNaN();
	    await Assert.That(Float.Pow(Float.One, Float.NaN)).IsBitwiseEquivalentTo(Float.One);
	    await Assert.That(Float.Pow(Float.NaN, anything)).IsNaN();
	    await Assert.That(Float.Pow(greaterThanOne, Float.PositiveInfinity)).IsBitwiseEquivalentTo(Float.PositiveInfinity);
	    await Assert.That(Float.Pow(greaterThanOne, Float.NegativeInfinity)).IsBitwiseEquivalentTo(Float.Zero);
	    await Assert.That(Float.Pow(lessThanOne, Float.PositiveInfinity)).IsBitwiseEquivalentTo(Float.Zero);
	    await Assert.That(Float.Pow(lessThanOne, Float.NegativeInfinity)).IsBitwiseEquivalentTo(Float.PositiveInfinity);
	    await Assert.That(Float.Pow(Float.One, Float.PositiveInfinity)).IsBitwiseEquivalentTo(Float.One);
	    await Assert.That(Float.Pow(Float.One, Float.NegativeInfinity)).IsBitwiseEquivalentTo(Float.One);
	    await Assert.That(Float.Pow(Float.NegativeOne, Float.PositiveInfinity)).IsBitwiseEquivalentTo(Float.One);
	    await Assert.That(Float.Pow(Float.NegativeOne, Float.NegativeInfinity)).IsBitwiseEquivalentTo(Float.One);
	    await Assert.That(Float.Pow(Float.Zero, anything)).IsBitwiseEquivalentTo(Float.Zero);
	    await Assert.That(Float.Pow(Float.NegativeZero, anything)).IsBitwiseEquivalentTo(Float.Zero);
	    await Assert.That(Float.Pow(Float.Zero, -anything)).IsBitwiseEquivalentTo(Float.PositiveInfinity);
	    await Assert.That(Float.Pow(Float.NegativeZero, -anything)).IsBitwiseEquivalentTo(Float.PositiveInfinity);
	    
	    await Assert.That(Float.Pow(Float.NegativeZero, oddInt))
	        .IsBitwiseEquivalentTo(-(Float.Pow(Float.Zero, oddInt)));
	    await Assert.That(Float.Pow(Float.PositiveInfinity, anything)).IsBitwiseEquivalentTo(Float.PositiveInfinity);
	    await Assert.That(Float.Pow(Float.PositiveInfinity, -anything)).IsBitwiseEquivalentTo(Float.Zero);
	    await Assert.That(Float.Pow(Float.NegativeInfinity, anything))
	        .IsBitwiseEquivalentTo(Float.Pow(Float.NegativeZero, -anything));
	    await Assert.That(Float.Pow(-anything, oddInt))
	        .IsBitwiseEquivalentTo(Float.Pow(Float.NegativeOne, oddInt) * Float.Pow(+anything, oddInt));
	    await Assert.That(Float.Pow(-anything, nonInt)).IsNaN();
	}
	[Test]
	public async Task ReciprocalEstimateTest()
	{
		await Assert.That(Float.ReciprocalEstimate(Float.Two)).IsEqualTo(Float.Half);
		await Assert.That(Float.ReciprocalEstimate(Float.Three))
			.IsApproximately(Values.CreateFloat<Float>(0x3FFD_5555_5555_5555, 0x5555_165E_5289_24A5), Float.Delta);
		await Assert.That(Float.ReciprocalEstimate(Float.Four))
			.IsApproximately(Values.CreateFloat<Float>(0x3FFD_0000_0000_0000, 0x0000_0000_0000_0000), Float.Delta);
	}
	[Test]
	public async Task RootNTest()
	{
		await Assert.That(Float.RootN(Values.CreateFloat<Float>(0x4005_4400_0000_0000, 0x0000_0000_0000_0000), 4))
			.IsBitwiseEquivalentTo(Float.Three);
		await Assert.That(Float.RootN(Values.CreateFloat<Float>(0x4005_0000_0000_0000, 0x0000_0000_0000_0000), 3))
			.IsEqualTo(Float.Four)
			.And.IsEqualTo(Float.Cbrt(Values.CreateFloat<Float>(0x4005_0000_0000_0000, 0x0000_0000_0000_0000)));
		await Assert.That(Float.RootN(Float.Hundred, 2))
			.IsBitwiseEquivalentTo(Float.Ten)
			.And.IsEqualTo(Float.Sqrt(Float.Hundred));
		await Assert.That(Float.RootN(Float.Zero, 2))
			.IsBitwiseEquivalentTo(Float.Zero);
		await Assert.That(Float.RootN(Float.NegativeZero, 2))
			.IsBitwiseEquivalentTo(Float.Zero);
		await Assert.That(Float.RootN(Float.PositiveInfinity, 2))
			.IsBitwiseEquivalentTo(Float.PositiveInfinity);
		await Assert.That(Float.RootN(Float.NegativeFour, 2))
			.IsNaN();
		await Assert.That(Float.RootN(Float.NaN, 2))
			.IsNaN();
	}
	[Test]
	[MethodDataSource<DataSources>(nameof(ScaleBTestData))]
	public async Task ScaleBTest(Float x, int n, Float result)
	{
	    await Assert.That(Float.ScaleB(x, n))
		    .IsBitwiseEquivalentTo(result);
	}
	[Test]
	public async Task SinTest()
	{
	    await Assert.That(Float.Sin(Float.NaN))
		    .IsNaN();
	    await Assert.That(Float.Sin(Float.PositiveInfinity))
		    .IsNaN();
	    await Assert.That(Float.Sin(Float.NegativeInfinity))
		    .IsNaN();
	}
	[Test]
	public async Task SinCosTest()
	{
	    Float sin, cos;
	    (sin, cos) = Float.SinCos(Float.Zero);
	    await Assert.That(sin).IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(cos).IsApproximately(Float.One, Float.Delta);
	
	    (sin, cos) = Float.SinCos(Float.Pi);
	    await Assert.That(sin).IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(cos).IsApproximately(Float.NegativeOne, Float.Delta);
	
	    (sin, cos) = Float.SinCos(Float.Pi / Float.Two);
	    await Assert.That(sin).IsApproximately(Float.One, Float.Delta);
	    await Assert.That(cos).IsApproximately(Float.Zero, Float.Delta);
	
	    (sin, cos) = Float.SinCos(Float.Pi * Float.Two);
	    await Assert.That(sin).IsApproximately(Float.Zero, Float.Delta);
	    await Assert.That(cos).IsApproximately(Float.One, Float.Delta);
	}
	[Test]
	public async Task SinhTest()
	{
	    await Assert.That(Float.Sinh(Float.Two))
	        .IsApproximately(Values.CreateFloat<Float>(0x4000_D03C_F63B_6E19, 0xF6F3_4C80_2C96_2009), Float.Delta);
	    await Assert.That(Float.Sinh(Float.Zero))
		    .IsBitwiseEquivalentTo(Float.Zero);
	}
	[Test]
	public async Task SqrtTest()
	{
	    await Assert.That(Float.Sqrt(Float.Ten))
	        .IsApproximately(Values.CreateFloat<Float>(0x4000_94C5_83AD_A5B5, 0x2920_4A2B_C830_CD9C), Float.Delta);
	    await Assert.That(Float.Sqrt(Float.Hundred))
		    .IsApproximately(Float.Ten, Float.Delta);
	    await Assert.That(Float.Sqrt(Float.Zero))
		    .IsBitwiseEquivalentTo(Float.Zero);
	    await Assert.That(Float.Sqrt(-Float.NegativeZero))
		    .IsBitwiseEquivalentTo(-Float.NegativeZero);
	    await Assert.That(Float.Sqrt(Float.PositiveInfinity))
		    .IsBitwiseEquivalentTo(Float.PositiveInfinity);
	    await Assert.That(Float.Sqrt(Float.NegativeFour))
		    .IsNaN();
	    await Assert.That(Float.Sqrt(Float.NaN))
		    .IsNaN();
	}
	[Test]
	public async Task TanTest()
	{
	    await Assert.That(Float.Tan(Float.NaN))
		    .IsNaN();
	    await Assert.That(Float.Tan(Float.PositiveInfinity))
		    .IsNaN();
	    await Assert.That(Float.Tan(Float.NegativeInfinity))
		    .IsNaN();
	}
	[Test]
	public async Task TanhTest()
	{
	    await Assert.That(Float.Tanh(Float.Two))
	        .IsApproximately(Values.CreateFloat<Float>(0x3FFE_ED95_05E1_BC3D, 0x3D33_C432_FC3E_8256), Float.Delta);
	    await Assert.That(Float.Tanh(Float.NaN))
		    .IsNaN();
	    await Assert.That(Float.Tanh(Float.Zero))
		    .IsBitwiseEquivalentTo(Float.Zero);
	}
    #endregion
}