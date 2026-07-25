using System.Globalization;
using System.Numerics;
using System.Text;
using MissingValues.Tests.Data.Sources;
using MissingValues.Tests.Extensions;

namespace MissingValues.Tests.Data;

public class QuadDataSources
    : IMathOperatorsDataSource<Quad>,
        IBitwiseOperatorsDataSource<Quad>,
        IEqualityOperatorsDataSource<Quad>,
        IComparisonOperatorsDataSource<Quad>,
        INumberBaseDataSource<Quad>,
        INumberDataSource<Quad>,
        IBinaryNumberDataSource<Quad>,
        IFloatingPointDataSource<Quad>,
        IFloatingPointIeee754DataSource<Quad>
{
    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_AdditionTestData()
    {
		yield return () => (Quad.One, Quad.One, Quad.Two);
		yield return () => (Quad.One, Quad.NegativeOne, Quad.Zero);
		yield return () => (Quad.One, Quad.NegativeTwo, Quad.NegativeOne);
		yield return () => (Quad.One, Quad.Four, Quad.Five);
		yield return () => (Quad.Three, Quad.Two, Quad.Five);
		yield return () => (Quad.SmallestSubnormal, Quad.GreatestSubnormal, Values.CreateFloat<Quad>(0x0001_0000_0000_0000, 0x0000_0000_0000_0000));
		yield return () => (Quad.PositiveInfinity, Quad.One, Quad.PositiveInfinity);
		yield return () => (Quad.NegativeInfinity, Quad.One, Quad.NegativeInfinity);
		yield return () => (Quad.PositiveInfinity, Quad.PositiveInfinity, Quad.PositiveInfinity);
		yield return () => (Quad.NegativeInfinity, Quad.NegativeInfinity, Quad.NegativeInfinity);
    }

    public static IEnumerable<Func<(Quad, Quad)>> op_DecrementTestData()
    {
		yield return () => (Quad.NegativeOne, Quad.NegativeTwo);
		yield return () => (Quad.Zero, Quad.NegativeOne);
		yield return () => (Quad.One, Quad.Zero);
		yield return () => (Quad.Two, Quad.One);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_DivisionTestData()
    {
		yield return () => (Quad.Ten, Quad.Ten, Quad.One);
		yield return () => (Quad.Hundred, Quad.Ten, Quad.Ten);
		yield return () => (Quad.NegativeThousand, Quad.Ten, Quad.NegativeHundred);
		yield return () => (Quad.Zero, Quad.Zero, Quad.NaN);
		yield return () => (Quad.One, Quad.Zero, Quad.PositiveInfinity);
		yield return () => (Quad.NegativeOne, Quad.Zero, Quad.NegativeInfinity);
		yield return () => (Quad.PositiveInfinity, Quad.PositiveInfinity, Quad.NaN);
		yield return () => (Quad.NegativeInfinity, Quad.NegativeInfinity, Quad.NaN);
    }

    public static IEnumerable<Func<(Quad, Quad)>> op_IncrementTestData()
    {
		yield return () => (Quad.NegativeTwo, Quad.NegativeOne);
		yield return () => (Quad.NegativeOne, Quad.Zero);
		yield return () => (Quad.Zero, Quad.One);
		yield return () => (Quad.One, Quad.Two);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_ModulusTestData()
    {
	    yield return () => (Quad.Two, Quad.Four, Quad.Two);
	    yield return () => (Quad.Half, Quad.Four, Quad.Half);
	    yield return () => (Quad.Four, Quad.Half, Quad.Zero);
	    yield return () => (Quad.NegativeFour, Quad.Half, Quad.NegativeZero);
	    yield return () => (Quad.NegativeFour, Quad.Thousand, Quad.NegativeFour);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_MultiplyTestData()
    {
		yield return () => (Quad.One, Quad.One, Quad.One);
		yield return () => (Quad.One, Quad.NegativeOne, Quad.NegativeOne);
		yield return () => (Quad.Ten, Quad.Ten, Quad.Hundred);
		yield return () => (Quad.NegativeHundred, Quad.Ten, Quad.NegativeThousand);
		yield return () => (Quad.NegativeTen, Quad.Hundred, Quad.NegativeThousand);
		yield return () => (Quad.Zero, Quad.NegativeThousand, Quad.NegativeZero);
		yield return () => (Quad.Zero, Quad.PositiveInfinity, Quad.NaN);
		yield return () => (Quad.NegativeZero, Quad.NegativeInfinity, Quad.NaN);
		yield return () => (Quad.PositiveInfinity, Quad.Zero, Quad.NaN);
		yield return () => (Quad.NegativeInfinity, Quad.NegativeZero, Quad.NaN);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_SubtractionTestData()
    {
		yield return () => (Quad.One, Quad.One, Quad.Zero);
		yield return () => (Quad.One, Quad.NegativeOne, Quad.Two);
		yield return () => (Quad.One, Quad.Two, Quad.NegativeOne);
		yield return () => (Quad.SmallestSubnormal, Quad.GreatestSubnormal, Values.CreateFloat<Quad>(0x8000_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
		yield return () => (Quad.PositiveInfinity, Quad.PositiveInfinity, Quad.NaN);
		yield return () => (Quad.NegativeInfinity, Quad.NegativeInfinity, Quad.NaN);
    }

    public static IEnumerable<Func<(Quad, Quad)>> op_UnaryNegationTestData()
    {
	    yield return () => (Quad.Zero, Quad.NegativeZero);
	    yield return () => (Quad.One, Quad.NegativeOne);
	    yield return () => (Quad.Two, Quad.NegativeTwo);
	    yield return () => (Quad.Ten, Quad.NegativeTen);
	    yield return () => (Quad.Hundred, Quad.NegativeHundred);
	    yield return () => (Quad.Thousand, Quad.NegativeThousand);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_BitwiseAndTestData()
    {
	    yield return () => (Quad.Zero, Quad.One, Quad.Zero);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), Values.CreateFloat<Quad>(0x0000_0000_0000_0000, 0x0000_0000_0000_0001), Values.CreateFloat<Quad>(0x0000_0000_0000_0000, 0x0000_0000_0000_0001));
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_BitwiseOrTestData()
    {
	    yield return () => (Quad.Zero, Quad.One, Quad.One);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), Values.CreateFloat<Quad>(0x0000_0000_0000_0000, 0x0000_0000_0000_0001), Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_BitwiseXorTestData()
    {
	    yield return () => (Quad.Zero, Quad.One, Quad.One);
	    yield return () => (Quad.One, Quad.One, Quad.Zero);
	    yield return () => (Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), Values.CreateFloat<Quad>(0x0000_0000_0000_0000, 0x0000_0000_0000_0001), Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
    }

    public static IEnumerable<Func<(Quad, Quad)>> op_OnesComplementTestData()
    {
	    yield return () => (Quad.Zero, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
	    yield return () => (Values.CreateFloat<Quad>(0x0000_0000_0000_0000, 0x0000_0000_0000_0001), Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
	    yield return () => (Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), Quad.Zero);
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_EqualityTestData()
    {
		yield return () => (Quad.One, Quad.One, true); 
		yield return () => (Quad.Two, Quad.Two, true);
		yield return () => (Quad.NaN, Quad.NaN, false);
		yield return () => (Quad.GreatestSubnormal, Quad.GreatestSubnormal, true);
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_InequalityTestData()
    {
		yield return () => (Quad.One, Quad.One, false);
		yield return () => (Quad.NaN, Quad.NaN, true);
	    yield return () => (Quad.NegativeTwo, Quad.Two, true);
		yield return () => (Quad.SmallestSubnormal, Quad.GreatestSubnormal, true);
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_GreaterThanOrEqualTestData()
    {
	    yield return () => (Quad.One, Quad.One, true); 
	    yield return () => (Quad.Two, Quad.Two, true);
	    yield return () => (Quad.NaN, Quad.NaN, false);
	    yield return () => (Quad.GreatestSubnormal, Quad.GreatestSubnormal, true);
	    yield return () => (Quad.Two, Quad.One, true);
	    yield return () => (Quad.Thousand, Quad.NegativeThousand, true);
	    yield return () => (Quad.NegativeQuarter, Quad.NegativeHalf, true);
	    yield return () => (Quad.Quarter, Quad.Half, false);
	    yield return () => (Quad.Ten, Quad.Hundred, false);
	    yield return () => (Quad.GreaterThanOneSmallest, Quad.One, true);
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_GreaterThanTestData()
    {
		yield return () => (Quad.Two, Quad.One, true);
		yield return () => (Quad.Thousand, Quad.NegativeThousand, true);
		yield return () => (Quad.NegativeQuarter, Quad.NegativeHalf, true);
		yield return () => (Quad.Quarter, Quad.Half, false);
		yield return () => (Quad.Ten, Quad.Hundred, false);
		yield return () => (Quad.GreaterThanOneSmallest, Quad.One, true);
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_LessThanOrEqualTestData()
    {
	    yield return () => (Quad.One, Quad.One, true); 
	    yield return () => (Quad.Two, Quad.Two, true);
	    yield return () => (Quad.NaN, Quad.NaN, false);
	    yield return () => (Quad.GreatestSubnormal, Quad.GreatestSubnormal, true);
	    yield return () => (Quad.Zero, Quad.One, true);
	    yield return () => (Quad.Zero, Quad.Quarter, true);
	    yield return () => (Quad.NegativeThousand, Quad.Thousand, true);
	    yield return () => (Quad.NegativeOne, Quad.NegativeThree, false);
	    yield return () => (Quad.Hundred, Quad.Two, false);
	    yield return () => (Quad.LessThanOneLargest, Quad.One, true);
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_LessThanTestData()
    {
		yield return () => (Quad.Zero, Quad.One, true);
		yield return () => (Quad.Zero, Quad.Quarter, true);
		yield return () => (Quad.NegativeThousand, Quad.Thousand, true);
		yield return () => (Quad.NegativeOne, Quad.NegativeThree, false);
		yield return () => (Quad.Hundred, Quad.Two, false);
		yield return () => (Quad.LessThanOneLargest, Quad.One, true);
    }

    public static IEnumerable<Func<(Quad, Quad)>> AbsTestData()
    {
	    yield return () => (Quad.One, Quad.One);
	    yield return () => (Quad.NegativeOne, Quad.One);
	    yield return () => (Quad.NegativeHalf, Quad.Half);
	    yield return () => (Quad.NegativeQuarter, Quad.Quarter);
	    yield return () => (Quad.NegativeZero, Quad.Zero);
	    yield return () => (Quad.NegativeInfinity, Quad.PositiveInfinity);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsCanonicalTestData()
    {
	    yield return () => (Quad.One, true);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsComplexNumberTestData()
    {
	    yield return () => (Quad.One, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsEvenIntegerTestData()
    {
	    yield return () => (Quad.Half, false);
	    yield return () => (Quad.One, false);
	    yield return () => (Quad.Two, true);
	    yield return () => (Quad.Three, false);
	    yield return () => (Quad.Four, true);
	    yield return () => (Quad.NegativeOne, false);
	    yield return () => (Quad.NegativeTwo, true);
	    yield return () => (Quad.NegativeThree, false);
	    yield return () => (Quad.NegativeFour, true);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsFiniteTestData()
    {
	    yield return () => (Quad.One, true);
	    yield return () => (Quad.NegativeOne, true);
	    yield return () => (Quad.NaN, false);
	    yield return () => (Quad.PositiveInfinity, false);
	    yield return () => (Quad.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsImaginaryNumberTestData()
    {
	    yield return () => (Quad.One, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsInfinityTestData()
    {
	    yield return () => (Quad.One, false);
	    yield return () => (Quad.NegativeOne, false);
	    yield return () => (Quad.NaN, false);
	    yield return () => (Quad.PositiveInfinity, true);
	    yield return () => (Quad.NegativeInfinity, true);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsIntegerTestData()
    {
	    yield return () => (Quad.Quarter, false);
	    yield return () => (Quad.Half, false);
	    yield return () => (Quad.Thousand, true);
	    yield return () => (Quad.One, true);
	    yield return () => (Quad.GreaterThanOneSmallest, false);
	    yield return () => (Quad.SmallestSubnormal, false);
	    yield return () => (Quad.NegativeOne, true);
	    yield return () => (Quad.NegativeThousand, true);
	    yield return () => (Quad.NegativeHalf, false);
	    yield return () => (Quad.NegativeQuarter, false);
	    yield return () => (Quad.NaN, false);
	    yield return () => (Quad.PositiveInfinity, false);
	    yield return () => (Quad.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsNaNTestData()
    {
	    yield return () => (Quad.One, false);
	    yield return () => (Quad.NegativeOne, false);
	    yield return () => (Quad.NaN, true);
	    yield return () => (Quad.PositiveInfinity, false);
	    yield return () => (Quad.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsNegativeTestData()
    {
	    yield return () => (Quad.One, false);
	    yield return () => (Quad.GreatestSubnormal, false);
	    yield return () => (Quad.PositiveInfinity, false);
	    yield return () => (Quad.NaN, true);
	    yield return () => (Quad.NegativeOne, true);
	    yield return () => (Quad.NegativeInfinity, true);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsNegativeInfinityTestData()
    {
	    yield return () => (Quad.One, false);
	    yield return () => (Quad.NegativeOne, false);
	    yield return () => (Quad.NaN, false);
	    yield return () => (Quad.PositiveInfinity, false);
	    yield return () => (Quad.NegativeInfinity, true);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsNormalTestData()
    {
	    yield return () => (Quad.GreatestSubnormal, false);
	    yield return () => (Quad.SmallestSubnormal, false);
	    yield return () => (Quad.MaxValue, true);
	    yield return () => (Quad.MinValue, true);
	    yield return () => (Quad.One, true);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsOddIntegerTestData()
    {
	    yield return () => (Quad.Half, false);
	    yield return () => (Quad.One, true);
	    yield return () => (Quad.Two, false);
	    yield return () => (Quad.Three, true);
	    yield return () => (Quad.Four, false);
	    yield return () => (Quad.NegativeOne, true);
	    yield return () => (Quad.NegativeTwo, false);
	    yield return () => (Quad.NegativeThree, true);
	    yield return () => (Quad.NegativeFour, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsPositiveTestData()
    {
	    yield return () => (Quad.One, true);
	    yield return () => (Quad.GreatestSubnormal, true);
	    yield return () => (Quad.PositiveInfinity, true);
	    yield return () => (Quad.NaN, false);
	    yield return () => (Quad.NegativeOne, false);
	    yield return () => (Quad.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsPositiveInfinityTestData()
    {
	    yield return () => (Quad.One, false);
	    yield return () => (Quad.NegativeOne, false);
	    yield return () => (Quad.NaN, false);
	    yield return () => (Quad.PositiveInfinity, true);
	    yield return () => (Quad.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsRealNumberTestData()
    {
	    yield return () => (Quad.GreatestSubnormal, true);
	    yield return () => (Quad.MaxValue, true);
	    yield return () => (Quad.NegativeThousand, true);
	    yield return () => (Quad.PositiveInfinity, true);
	    yield return () => (Quad.NegativeInfinity, true);
	    yield return () => (Quad.NaN, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsSubnormalTestData()
    {
	    yield return () => (Quad.GreatestSubnormal, true);
	    yield return () => (Quad.SmallestSubnormal, true);
	    yield return () => (Quad.MaxValue, false);
	    yield return () => (Quad.MinValue, false);
	    yield return () => (Quad.One, false);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsZeroTestData()
    {
	    yield return () => (Quad.One, false);
	    yield return () => (Quad.Epsilon, false);
	    yield return () => (Quad.Zero, true);
	    yield return () => (Quad.NegativeZero, true);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MaxMagnitudeTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.NegativeInfinity);
	    yield return () => (Quad.MinValue, Quad.One, Quad.MinValue);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.One);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, Quad.One);
	    yield return () => (-Quad.Epsilon, Quad.One, Quad.One);
	    yield return () => (Quad.NegativeZero, Quad.One, Quad.One);
	    yield return () => (Quad.NaN, Quad.One, Quad.NaN);
	    yield return () => (Quad.Zero, Quad.One, Quad.One);
	    yield return () => (Quad.Epsilon, Quad.One, Quad.One);
	    yield return () => (Quad.GreatestSubnormal, Quad.One, Quad.One);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.MaxValue);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.PositiveInfinity);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MaxMagnitudeNumberTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.NegativeInfinity);
	    yield return () => (Quad.MinValue, Quad.One, Quad.MinValue);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.One);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, Quad.One);
	    yield return () => (-Quad.Epsilon, Quad.One, Quad.One);
	    yield return () => (Quad.NegativeZero, Quad.One, Quad.One);
	    yield return () => (Quad.NaN, Quad.One, Quad.One);
	    yield return () => (Quad.Zero, Quad.One, Quad.One);
	    yield return () => (Quad.Epsilon, Quad.One, Quad.One);
	    yield return () => (Quad.GreatestSubnormal, Quad.One, Quad.One);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.MaxValue);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.PositiveInfinity);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MinMagnitudeTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.One);
	    yield return () => (Quad.MinValue, Quad.One, Quad.One);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.NegativeOne);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, -Quad.GreatestSubnormal);
	    yield return () => (-Quad.Epsilon, Quad.One, -Quad.Epsilon);
	    yield return () => (Quad.NegativeZero, Quad.One, Quad.NegativeZero);
	    yield return () => (Quad.NaN, Quad.One, Quad.NaN);
	    yield return () => (Quad.Zero, Quad.One, Quad.Zero);
	    yield return () => (Quad.Epsilon, Quad.One, Quad.Epsilon);
	    yield return () => (Quad.GreatestSubnormal, Quad.One, Quad.GreatestSubnormal);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.One);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.One);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MinMagnitudeNumberTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.One);
	    yield return () => (Quad.MinValue, Quad.One, Quad.One);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.NegativeOne);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, -Quad.GreatestSubnormal);
	    yield return () => (-Quad.Epsilon, Quad.One, -Quad.Epsilon);
	    yield return () => (Quad.NegativeZero, Quad.One, Quad.NegativeZero);
	    yield return () => (Quad.NaN, Quad.One, Quad.One);
	    yield return () => (Quad.Zero, Quad.One, Quad.Zero);
	    yield return () => (Quad.Epsilon, Quad.One, Quad.Epsilon);
	    yield return () => (Quad.GreatestSubnormal, Quad.One, Quad.GreatestSubnormal);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.One);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.One);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, Quad)>> MultiplyAddEstimateTestData()
    {
	    yield return () => (Quad.One, Quad.One, Quad.One, Quad.Two);
	    yield return () => (Quad.Ten, Quad.Ten, Quad.Zero, Quad.Hundred);
	    yield return () => (Quad.Five, Quad.Zero, Quad.Five, Quad.Five);
	    yield return () => (Values.CreateFloat<Quad>(0xBFFF_4000_0000_0000, 0), Quad.One, Quad.Two, Values.CreateFloat<Quad>(0x3FFE_8000_0000_0000, 0));
    }

    public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, Quad)>> ParseTestData()
    {
        yield return () => ("2.0", NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Two);
		yield return () => ("-2", NumberStyles.Float, CultureInfo.InvariantCulture, -Quad.Two);
		yield return () => ("1.189731495357231765085759326628007E+4932", NumberStyles.Float, CultureInfo.InvariantCulture, Quad.MaxValue);
		yield return () => ("0", NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Zero);
		yield return () => (NumberFormatInfo.InvariantInfo.PositiveInfinitySymbol, NumberStyles.Float, CultureInfo.InvariantCulture, Quad.PositiveInfinity);
		yield return () => (NumberFormatInfo.InvariantInfo.NegativeInfinitySymbol, NumberStyles.Float, CultureInfo.InvariantCulture, Quad.NegativeInfinity);
		yield return () => (NumberFormatInfo.InvariantInfo.NaNSymbol, NumberStyles.Float, CultureInfo.InvariantCulture, Quad.NaN);
		yield return () => ("256.4995", NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4007_007F_DF3B_645A, 0x1CAC_0831_26E9_78D5));
		yield return () => ("471581881", NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x401B_C1BC_4B90_0000, 0x0000_0000_0000_0000));
		yield return () => ("1.93561113", NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFF_EF84_3605_1FA4, 0x8B0F_3D34_BECE_8762));
		yield return () => ("9715574.2", NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4016_287E_EC66_6666, 0x6666_6666_6666_6666));
		yield return () => ("0.51438427732005011792", NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFE_075D_6041_5519, 0x72D0_0AD3_7DB4_57E9));
		yield return () => ("0.04201133209656899095", NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFA_5828_262D_512C, 0x8840_B3B3_D424_5947));
		yield return () => ("7.7E777", NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4A17_0F28_5D1D_4C84, 0xA11F_6899_101B_A9A4));
		yield return () => ("-7.7E-777", NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0xB5EC_BFCE_3AF6_4E08, 0x42C8_5750_BEBD_A572));
		
		yield return () => ("2.5E-1", NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Quarter);
		yield return () => ("0.250", NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Quarter);
		yield return () => ("$-0.25", NumberStyles.Currency, Helper.CustomInfo, Quad.NegativeQuarter);
		yield return () => ("1.000", NumberStyles.Float, CultureInfo.InvariantCulture, Quad.One);
		yield return () => ("1,000.00", NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Quad.Thousand);
		yield return () => ("-1,000.00", NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Quad.NegativeThousand);
    }

    public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Quad)>> ParseSpanTestData()
    {
	    yield return () => ("2.0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Two);
	    yield return () => ("-2".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, -Quad.Two);
	    yield return () => ("1.189731495357231765085759326628007E+4932".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.MaxValue);
	    yield return () => ("0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Zero);
	    yield return () => (NumberFormatInfo.InvariantInfo.PositiveInfinitySymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.PositiveInfinity);
	    yield return () => (NumberFormatInfo.InvariantInfo.NegativeInfinitySymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.NegativeInfinity);
	    yield return () => (NumberFormatInfo.InvariantInfo.NaNSymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.NaN);
	    yield return () => ("256.4995".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4007_007F_DF3B_645A, 0x1CAC_0831_26E9_78D5));
	    yield return () => ("471581881".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x401B_C1BC_4B90_0000, 0x0000_0000_0000_0000));
	    yield return () => ("1.93561113".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFF_EF84_3605_1FA4, 0x8B0F_3D34_BECE_8762));
	    yield return () => ("9715574.2".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4016_287E_EC66_6666, 0x6666_6666_6666_6666));
	    yield return () => ("0.51438427732005011792".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFE_075D_6041_5519, 0x72D0_0AD3_7DB4_57E9));
	    yield return () => ("0.04201133209656899095".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFA_5828_262D_512C, 0x8840_B3B3_D424_5947));
	    yield return () => ("7.7E777".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4A17_0F28_5D1D_4C84, 0xA11F_6899_101B_A9A4));
	    yield return () => ("-7.7E-777".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0xB5EC_BFCE_3AF6_4E08, 0x42C8_5750_BEBD_A572));
	    
	    yield return () => ("2.5E-1".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Quarter);
	    yield return () => ("0.250".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Quarter);
	    yield return () => ("$-0.25".ToCharArray(), NumberStyles.Currency, Helper.CustomInfo, Quad.NegativeQuarter);
	    yield return () => ("1.000".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.One);
	    yield return () => ("1,000.00".ToCharArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Quad.Thousand);
	    yield return () => ("-1,000.00".ToCharArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Quad.NegativeThousand);
    }

    public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Quad)>> ParseUtf8TestData()
    {
        yield return () => ("2.0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Two);
	    yield return () => ("-2"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, -Quad.Two);
	    yield return () => ("1.189731495357231765085759326628007E+4932"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.MaxValue);
	    yield return () => ("0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Zero);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.InvariantInfo.PositiveInfinitySymbol), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.PositiveInfinity);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.InvariantInfo.NegativeInfinitySymbol), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.NegativeInfinity);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.InvariantInfo.NaNSymbol), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.NaN);
	    yield return () => ("256.4995"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4007_007F_DF3B_645A, 0x1CAC_0831_26E9_78D5));
	    yield return () => ("471581881"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x401B_C1BC_4B90_0000, 0x0000_0000_0000_0000));
	    yield return () => ("1.93561113"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFF_EF84_3605_1FA4, 0x8B0F_3D34_BECE_8762));
	    yield return () => ("9715574.2"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4016_287E_EC66_6666, 0x6666_6666_6666_6666));
	    yield return () => ("0.51438427732005011792"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFE_075D_6041_5519, 0x72D0_0AD3_7DB4_57E9));
	    yield return () => ("0.04201133209656899095"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x3FFA_5828_262D_512C, 0x8840_B3B3_D424_5947));
	    yield return () => ("7.7E777"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0x4A17_0F28_5D1D_4C84, 0xA11F_6899_101B_A9A4));
	    yield return () => ("-7.7E-777"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Values.CreateFloat<Quad>(0xB5EC_BFCE_3AF6_4E08, 0x42C8_5750_BEBD_A572));
	    
	    yield return () => ("2.5E-1"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Quarter);
	    yield return () => ("0.250"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.Quarter);
	    yield return () => ("$-0.25"u8.ToArray(), NumberStyles.Currency, Helper.CustomInfo, Quad.NegativeQuarter);
	    yield return () => ("1.000"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Quad.One);
	    yield return () => ("1,000.00"u8.ToArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Quad.Thousand);
	    yield return () => ("-1,000.00"u8.ToArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Quad.NegativeThousand);
    }

    public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Quad)>> TryParseTestData()
    {
		yield return () => ("2.0", NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Two);
		yield return () => ("-2", NumberStyles.Float, CultureInfo.InvariantCulture, true, -Quad.Two);
		yield return () => ("1.189731495357231765085759326628007E+4932", NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.MaxValue);
		yield return () => ("0", NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Zero);
		yield return () => (NumberFormatInfo.InvariantInfo.PositiveInfinitySymbol, NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.PositiveInfinity);
		yield return () => (NumberFormatInfo.InvariantInfo.NegativeInfinitySymbol, NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.NegativeInfinity);
		yield return () => (NumberFormatInfo.InvariantInfo.NaNSymbol, NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.NaN);
		yield return () => ("256.4995", NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4007_007F_DF3B_645A, 0x1CAC_0831_26E9_78D5));
		yield return () => ("471581881", NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x401B_C1BC_4B90_0000, 0x0000_0000_0000_0000));
		yield return () => ("1.93561113", NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFF_EF84_3605_1FA4, 0x8B0F_3D34_BECE_8762));
		yield return () => ("9715574.2", NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4016_287E_EC66_6666, 0x6666_6666_6666_6666));
		yield return () => ("0.51438427732005011792", NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFE_075D_6041_5519, 0x72D0_0AD3_7DB4_57E9));
		yield return () => ("0.04201133209656899095", NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFA_5828_262D_512C, 0x8840_B3B3_D424_5947));
		yield return () => ("7.7E777", NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4A17_0F28_5D1D_4C84, 0xA11F_6899_101B_A9A4));
		yield return () => ("-7.7E-777", NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0xB5EC_BFCE_3AF6_4E08, 0x42C8_5750_BEBD_A572));
		yield return () => ("1A", NumberStyles.Float, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("2.5E-1", NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Quarter);
		yield return () => ("0.250", NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Quarter);
		yield return () => ("$-0.25", NumberStyles.Currency, Helper.CustomInfo, true, Quad.NegativeQuarter);
		yield return () => ("1.000", NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.One);
		yield return () => ("1,000.00", NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Quad.Thousand);
		yield return () => ("-1,000.00", NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Quad.NegativeThousand);
    }

    public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Quad)>> TryParseSpanTestData()
    {
        yield return () => ("2.0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Two);
		yield return () => ("-2".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, -Quad.Two);
		yield return () => ("1.189731495357231765085759326628007E+4932".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.MaxValue);
		yield return () => ("0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Zero);
		yield return () => (NumberFormatInfo.InvariantInfo.PositiveInfinitySymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.PositiveInfinity);
		yield return () => (NumberFormatInfo.InvariantInfo.NegativeInfinitySymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.NegativeInfinity);
		yield return () => (NumberFormatInfo.InvariantInfo.NaNSymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.NaN);
		yield return () => ("256.4995".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4007_007F_DF3B_645A, 0x1CAC_0831_26E9_78D5));
		yield return () => ("471581881".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x401B_C1BC_4B90_0000, 0x0000_0000_0000_0000));
		yield return () => ("1.93561113".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFF_EF84_3605_1FA4, 0x8B0F_3D34_BECE_8762));
		yield return () => ("9715574.2".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4016_287E_EC66_6666, 0x6666_6666_6666_6666));
		yield return () => ("0.51438427732005011792".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFE_075D_6041_5519, 0x72D0_0AD3_7DB4_57E9));
		yield return () => ("0.04201133209656899095".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFA_5828_262D_512C, 0x8840_B3B3_D424_5947));
		yield return () => ("7.7E777".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4A17_0F28_5D1D_4C84, 0xA11F_6899_101B_A9A4));
		yield return () => ("-7.7E-777".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0xB5EC_BFCE_3AF6_4E08, 0x42C8_5750_BEBD_A572));
		yield return () => ("1A".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("2.5E-1".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Quarter);
		yield return () => ("0.250".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Quarter);
		yield return () => ("$-0.25".ToCharArray(), NumberStyles.Currency, Helper.CustomInfo, true, Quad.NegativeQuarter);
		yield return () => ("1.000".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.One);
		yield return () => ("1,000.00".ToCharArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Quad.Thousand);
		yield return () => ("-1,000.00".ToCharArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Quad.NegativeThousand);
    }

    public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Quad)>> TryParseUtf8TestData()
    {
        yield return () => ("2.0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Two);
		yield return () => ("-2"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, -Quad.Two);
		yield return () => ("1.189731495357231765085759326628007E+4932"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.MaxValue);
		yield return () => ("0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Zero);
		yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.InvariantInfo.PositiveInfinitySymbol), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.PositiveInfinity);
		yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.InvariantInfo.NegativeInfinitySymbol), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.NegativeInfinity);
		yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.InvariantInfo.NaNSymbol), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.NaN);
		yield return () => ("256.4995"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4007_007F_DF3B_645A, 0x1CAC_0831_26E9_78D5));
		yield return () => ("471581881"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x401B_C1BC_4B90_0000, 0x0000_0000_0000_0000));
		yield return () => ("1.93561113"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFF_EF84_3605_1FA4, 0x8B0F_3D34_BECE_8762));
		yield return () => ("9715574.2"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4016_287E_EC66_6666, 0x6666_6666_6666_6666));
		yield return () => ("0.51438427732005011792"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFE_075D_6041_5519, 0x72D0_0AD3_7DB4_57E9));
		yield return () => ("0.04201133209656899095"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x3FFA_5828_262D_512C, 0x8840_B3B3_D424_5947));
		yield return () => ("7.7E777"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0x4A17_0F28_5D1D_4C84, 0xA11F_6899_101B_A9A4));
		yield return () => ("-7.7E-777"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Values.CreateFloat<Quad>(0xB5EC_BFCE_3AF6_4E08, 0x42C8_5750_BEBD_A572));
		yield return () => ("1A"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("2.5E-1"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Quarter);
		yield return () => ("0.250"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.Quarter);
		yield return () => ("$-0.25"u8.ToArray(), NumberStyles.Currency, Helper.CustomInfo, true, Quad.NegativeQuarter);
		yield return () => ("1.000"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Quad.One);
		yield return () => ("1,000.00"u8.ToArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Quad.Thousand);
		yield return () => ("-1,000.00"u8.ToArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Quad.NegativeThousand);
    }

    public static IEnumerable<Func<(Quad, string, IFormatProvider?, string)>> ToStringTestData()
    {
	    Quad value = Values.CreateFloat<Quad>(0x400C_81CD_6E63_1F8A, 0x0902_DE00_D1B7_1759);
	    
	    yield return () => (value, "F", CultureInfo.InvariantCulture, "12345.68");
	    yield return () => (value, "F", Helper.CustomInfo, "12345.67890");
	    yield return () => (value, "N3", CultureInfo.InvariantCulture, "12,345.679");
	    yield return () => (value, "N", Helper.CustomInfo, "1_23_45.67890");
	    yield return () => (value, "C", Helper.CustomInfo, "$12,345.68");
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, Quad)>> ClampTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (Quad.MinValue, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (-Quad.Epsilon, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (Quad.NaN, Quad.One, Quad.Thousand, Quad.NaN);
	    yield return () => (Quad.Zero, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (Quad.Epsilon, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (Quad.GreatestSubnormal, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (Quad.One, Quad.One, Quad.Thousand, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.Thousand, Quad.Thousand);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.Thousand, Quad.Thousand);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> CopySignTestData()
    {
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.One);
	    yield return () => (Quad.One, Quad.NegativeOne, Quad.NegativeOne);
	    yield return () => (Quad.Thousand, Quad.NegativeOne, Quad.NegativeThousand);
	    yield return () => (Quad.NegativeHundred, Quad.NegativeOne, Quad.NegativeHundred);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MaxTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.One);
	    yield return () => (Quad.MinValue, Quad.One, Quad.One);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.One);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, Quad.One);
	    yield return () => (-Quad.Epsilon, Quad.One, Quad.One);
	    yield return () => (Quad.NegativeZero, Quad.One, Quad.One);
	    yield return () => (Quad.NaN, Quad.One, Quad.NaN);
	    yield return () => (Quad.Zero, Quad.One, Quad.One);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.MaxValue);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.PositiveInfinity);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MaxNumberTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.One);
	    yield return () => (Quad.MinValue, Quad.One, Quad.One);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.One);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, Quad.One);
	    yield return () => (-Quad.Epsilon, Quad.One, Quad.One);
	    yield return () => (Quad.NegativeZero, Quad.One, Quad.One);
	    yield return () => (Quad.NaN, Quad.One, Quad.One);
	    yield return () => (Quad.Zero, Quad.One, Quad.One);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.MaxValue);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.PositiveInfinity);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MinTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.NegativeInfinity);
	    yield return () => (Quad.MinValue, Quad.One, Quad.MinValue);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.NegativeOne);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, -Quad.GreatestSubnormal);
	    yield return () => (-Quad.Epsilon, Quad.One, -Quad.Epsilon);
	    yield return () => (Quad.NegativeZero, Quad.One, Quad.NegativeZero);
	    yield return () => (Quad.NaN, Quad.One, Quad.NaN);
	    yield return () => (Quad.Zero, Quad.One, Quad.Zero);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.One);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.One);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MinNumberTestData()
    {
	    yield return () => (Quad.NegativeInfinity, Quad.One, Quad.NegativeInfinity);
	    yield return () => (Quad.MinValue, Quad.One, Quad.MinValue);
	    yield return () => (Quad.NegativeOne, Quad.One, Quad.NegativeOne);
	    yield return () => (-Quad.GreatestSubnormal, Quad.One, -Quad.GreatestSubnormal);
	    yield return () => (-Quad.Epsilon, Quad.One, -Quad.Epsilon);
	    yield return () => (Quad.NegativeZero, Quad.One, Quad.NegativeZero);
	    yield return () => (Quad.NaN, Quad.One, Quad.One);
	    yield return () => (Quad.Zero, Quad.One, Quad.Zero);
	    yield return () => (Quad.One, Quad.One, Quad.One);
	    yield return () => (Quad.MaxValue, Quad.One, Quad.One);
	    yield return () => (Quad.PositiveInfinity, Quad.One, Quad.One);
    }

    public static IEnumerable<Func<(Quad, int)>> SignTestData()
    {
	    yield return () => (Quad.One, 1);
	    yield return () => (Quad.NegativeOne, -1);
	    yield return () => (Quad.Ten, 1);
	    yield return () => (Quad.NegativeTen, -1);
	    yield return () => (Quad.Zero, 0);
	    yield return () => (Quad.NegativeZero, 0);
    }

    public static IEnumerable<Func<(Quad, bool)>> IsPow2TestData()
    {
	    yield return () => (Quad.Half, true);
	    yield return () => (Quad.One, true);
	    yield return () => (Quad.Two, true);
	    yield return () => (Quad.Three, false);
	    yield return () => (Quad.NegativeTwo, false);
    }

    public static IEnumerable<Func<(Quad, Quad)>> Log2TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> CeilingTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> FloorTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int, MidpointRounding, Quad)>> RoundTestData()
    {
		yield return () => (Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Quad.Four);
		yield return () => (Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.AwayFromZero, Quad.Three);
		yield return () => (Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Quad.Three);
		yield return () => (Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.AwayFromZero, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.AwayFromZero, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Quad.NegativeThree);
		yield return () => (Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.AwayFromZero, Quad.NegativeThree);
		yield return () => (Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Quad.NegativeFour);
		    
		yield return () => (Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Quad.Four);
		yield return () => (Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToEven, Quad.Three);
		yield return () => (Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToEven, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToEven, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToEven, Quad.NegativeThree);
		yield return () => (Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Quad.NegativeFour);
		    
		yield return () => (Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Quad.Three);
		yield return () => (Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToNegativeInfinity, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToNegativeInfinity, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToNegativeInfinity, Quad.NegativeThree);
		yield return () => (Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Quad.NegativeThree);
		yield return () => (Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToNegativeInfinity, Quad.NegativeThree);
		yield return () => (Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Quad.NegativeFour);
		    
		yield return () => (Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Quad.Four);
		yield return () => (Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToPositiveInfinity, Quad.Three);
		yield return () => (Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Quad.Three);
		yield return () => (Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToPositiveInfinity, Quad.Three);
		yield return () => (Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToPositiveInfinity, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToPositiveInfinity, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Quad.NegativeThree);
		    
		yield return () => (Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Quad.Three);
		yield return () => (Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToZero, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToZero, Quad.Two);
		yield return () => (Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToZero, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToZero, Quad.NegativeTwo);
		yield return () => (Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Quad.NegativeThree);
    }

    public static IEnumerable<Func<(Quad, Quad)>> TruncateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> GetExponentByteCountTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> GetExponentShortestBitLengthTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> GetSignificandBitLengthTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> GetSignificandByteCountTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, byte[], bool, int)>> TryWriteExponentBigEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, byte[], bool, int)>> TryWriteExponentLittleEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, byte[], bool, int)>> TryWriteSignificandBigEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, byte[], bool, int)>> TryWriteSignificandLittleEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> Atan2TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> Atan2PiTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> BitDecrementTestData()
    {
	    yield return () => (Quad.One, Values.CreateFloat<Quad>(0x3FFEFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
	    yield return () => (Quad.NegativeOne, Values.CreateFloat<Quad>(0xBFFF000000000000, 0x0000000000000001));
	    yield return () => (Quad.Zero, -Quad.Epsilon);
	    yield return () => (Quad.NegativeInfinity, Quad.NegativeInfinity);
	    yield return () => (Quad.PositiveInfinity, Quad.MaxValue);
    }

    public static IEnumerable<Func<(Quad, Quad)>> BitIncrementTestData()
    {
	    yield return () => (Quad.One, Values.CreateFloat<Quad>(0x3FFF000000000000, 0x0000000000000001));
	    yield return () => (Quad.NegativeOne, Values.CreateFloat<Quad>(0xBFFEFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
	    yield return () => (Quad.NegativeZero, Quad.Epsilon);
	    yield return () => (Quad.NegativeInfinity, Quad.MinValue);
	    yield return () => (Quad.PositiveInfinity, Quad.PositiveInfinity);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, Quad)>> FusedMultiplyAddTestData()
    {
		yield return () => (Quad.One, Quad.One, Quad.One, Quad.Two);
		yield return () => (Quad.Ten, Quad.Ten, Quad.Zero, Quad.Hundred);
		yield return () => (Quad.Five, Quad.Zero, Quad.Five, Quad.Five);
	    yield return () => (Values.CreateFloat<Quad>(0xBFFF_4000_0000_0000, 0), Quad.One, Quad.Two, Values.CreateFloat<Quad>(0x3FFE_8000_0000_0000, 0));
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> Ieee754RemainderTestData()
    {
	    yield return () => (Quad.Ten, Quad.Three, Quad.One);
	    yield return () => (Quad.Ten, Quad.Two, Quad.Zero);
	    yield return () => (Quad.NegativeTen, Quad.Three, Quad.NegativeOne);
	    yield return () => (Quad.NegativeTen, Quad.Two, Quad.NegativeZero);
	    yield return () => (Quad.NegativeTen, Quad.Zero, Quad.NaN);
    }

    public static IEnumerable<Func<(Quad, int)>> ILogBTestData()
    {
	    yield return () => (Values.CreateFloat<Quad>(0x4009_0000_0000_0000, 0x0000_0000_0000_0000), 10);
	    yield return () => (Values.CreateFloat<Quad>(0x403F_0000_0000_0000, 0x0000_0000_0000_0000), 64);
	    yield return () => (Values.CreateFloat<Quad>(0xC03F_0000_0000_0000, 0x0000_0000_0000_0000), 64);
	    yield return () => (Quad.Zero, int.MinValue);
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, Quad)>> LerpTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> ReciprocalEstimateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> ReciprocalSqrtEstimateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int, Quad)>> ScaleBTestData()
    {
	    yield return () => (Quad.Two, 3, Values.CreateFloat<Quad>(0x4003_0000_0000_0000, 0x0000_0000_0000_0000));
	    yield return () => (Quad.NegativeTwo, 3, Values.CreateFloat<Quad>(0xC003_0000_0000_0000, 0x0000_0000_0000_0000));
	    yield return () => (Quad.Zero, 6, Quad.Zero);
	    yield return () => (Quad.Two, 300000, Quad.PositiveInfinity);
	    yield return () => (Quad.Two, -300000, Quad.Zero);
    }
    
	public static IEnumerable<Func<(Quad, byte)>> ConvertToCheckedByteTestData()
	{
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, byte)>> ConvertToSaturatingByteTestData()
	{
		yield return () => (Quad.NegativeOne, 0);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.MaxValue, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, byte)>> ConvertToTruncatingByteTestData()
	{
		yield return () => (Quad.NegativeOne, 0);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.MaxValue, byte.MaxValue);
	}

	public static IEnumerable<Func<(Quad, ushort)>> ConvertToCheckedUInt16TestData()
	{
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		yield return () => (Quad.NegativeOne, 0);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.MaxValue, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		yield return () => (Quad.NegativeOne, 0);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.MaxValue, ushort.MaxValue);
	}

	public static IEnumerable<Func<(Quad, uint)>> ConvertToCheckedUInt32TestData()
	{
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, uint)>> ConvertToSaturatingUInt32TestData()
	{
		yield return () => (Quad.NegativeOne, 0);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
		yield return () => (Quad.MaxValue, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, uint)>> ConvertToTruncatingUInt32TestData()
	{
		yield return () => (Quad.NegativeOne, 0);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
		yield return () => (Quad.MaxValue, uint.MaxValue);
	}

	public static IEnumerable<Func<(Quad, ulong)>> ConvertToCheckedUInt64TestData()
	{
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
		yield return () => (Quad.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		yield return () => (Quad.NegativeOne, 0);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
		yield return () => (Quad.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Quad.MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		yield return () => (Quad.NegativeOne, 0);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
		yield return () => (Quad.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Quad.MaxValue, ulong.MaxValue);
	}

	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		yield return () => (Quad.Half, UInt128.Zero);
		yield return () => (Quad.One, UInt128.One);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
		yield return () => (Quad.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		yield return () => (Quad.Half, UInt128.Zero);
		yield return () => (Quad.One, UInt128.One);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
		yield return () => (Quad.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Quad.TwoOver128, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		yield return () => (Quad.Half, UInt128.Zero);
		yield return () => (Quad.One, UInt128.One);
		yield return () => (Quad.ByteMaxValue, byte.MaxValue);
		yield return () => (Quad.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Quad.UInt32MaxValue, uint.MaxValue);
		yield return () => (Quad.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Quad.TwoOver128, UInt128.MaxValue);
	}

	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		yield return () => (Quad.Half, UInt256.Zero);
		yield return () => (Quad.One, UInt256.One);
		yield return () => (Quad.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Quad.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Quad.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Quad.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Quad.TwoOver128, UInt256.UInt128MaxValue + UInt256.One);
	}
	
	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		yield return () => (Quad.Half, UInt256.Zero);
		yield return () => (Quad.One, UInt256.One);
		yield return () => (Quad.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Quad.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Quad.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Quad.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Quad.TwoOver128, UInt256.UInt128MaxValue + UInt256.One);
		yield return () => (Quad.TwoOver256, UInt256.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		yield return () => (Quad.Half, UInt256.Zero);
		yield return () => (Quad.One, UInt256.One);
		yield return () => (Quad.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Quad.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Quad.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Quad.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Quad.TwoOver128, UInt256.UInt128MaxValue + UInt256.One);
		yield return () => (Quad.TwoOver256, UInt256.MaxValue);
	}

	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		yield return () => (Quad.Half, UInt512.Zero);
		yield return () => (Quad.One, UInt512.One);
		yield return () => (Quad.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Quad.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Quad.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Quad.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Quad.TwoOver128, UInt512.UInt128MaxValue + UInt512.One);
		yield return () => (Quad.TwoOver256, UInt512.UInt256MaxValue + UInt512.One);
	}
	
	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		yield return () => (Quad.Half, UInt512.Zero);
		yield return () => (Quad.One, UInt512.One);
		yield return () => (Quad.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Quad.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Quad.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Quad.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Quad.TwoOver128, UInt512.UInt128MaxValue + UInt512.One);
		yield return () => (Quad.TwoOver256, UInt512.UInt256MaxValue + UInt512.One);
		yield return () => (Quad.TwoOver512, UInt512.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		yield return () => (Quad.Half, UInt512.Zero);
		yield return () => (Quad.One, UInt512.One);
		yield return () => (Quad.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Quad.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Quad.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Quad.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Quad.TwoOver128, UInt512.UInt128MaxValue + UInt512.One);
		yield return () => (Quad.TwoOver256, UInt512.UInt256MaxValue + UInt512.One);
		yield return () => (Quad.TwoOver512, UInt512.MaxValue);
	}

	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToCheckedSByteTestData()
	{
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		yield return () => (Quad.MinValue, sbyte.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.MaxValue, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		yield return () => (Quad.MinValue, sbyte.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.MaxValue, sbyte.MaxValue);
	}

	public static IEnumerable<Func<(Quad, short)>> ConvertToCheckedInt16TestData()
	{
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, short)>> ConvertToSaturatingInt16TestData()
	{
		yield return () => (Quad.MinValue, short.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.MaxValue, short.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, short)>> ConvertToTruncatingInt16TestData()
	{
		yield return () => (Quad.MinValue, short.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.MaxValue, short.MaxValue);
	}

	public static IEnumerable<Func<(Quad, int)>> ConvertToCheckedInt32TestData()
	{
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, int)>> ConvertToSaturatingInt32TestData()
	{
		yield return () => (Quad.MinValue, int.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, int)>> ConvertToTruncatingInt32TestData()
	{
		yield return () => (Quad.MinValue, int.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.MaxValue, int.MaxValue);
	}

	public static IEnumerable<Func<(Quad, long)>> ConvertToCheckedInt64TestData()
	{
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, long)>> ConvertToSaturatingInt64TestData()
	{
		yield return () => (Quad.MinValue, long.MinValue);
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
		yield return () => (Quad.MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, long)>> ConvertToTruncatingInt64TestData()
	{
		yield return () => (Quad.MinValue, long.MinValue);
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, -1);
		yield return () => (Quad.Half, 0);
		yield return () => (Quad.One, 1);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
		yield return () => (Quad.MaxValue, long.MaxValue);
	}

	public static IEnumerable<Func<(Quad, Int128)>> ConvertToCheckedInt128TestData()
	{
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, Int128.NegativeOne);
		yield return () => (Quad.Half, Int128.Zero);
		yield return () => (Quad.One, Int128.One);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, Int128)>> ConvertToSaturatingInt128TestData()
	{
		yield return () => (Quad.MinValue, Int128.MinValue);
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, Int128.NegativeOne);
		yield return () => (Quad.Half, Int128.Zero);
		yield return () => (Quad.One, Int128.One);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
		yield return () => (Quad.TwoOver127, Int128.MaxValue);
		yield return () => (Quad.MaxValue, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, Int128)>> ConvertToTruncatingInt128TestData()
	{
		yield return () => (Quad.MinValue, Int128.MinValue);
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, Int128.NegativeOne);
		yield return () => (Quad.Half, Int128.Zero);
		yield return () => (Quad.One, Int128.One);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
		yield return () => (Quad.TwoOver127, Int128.MaxValue);
		yield return () => (Quad.MaxValue, Int128.MaxValue);
	}

	public static IEnumerable<Func<(Quad, Int256)>> ConvertToCheckedInt256TestData()
	{
		yield return () => (Quad.Int64MinValue, Int256.Int64MinValue);
		yield return () => (Quad.Int32MinValue, Int256.Int32MinValue);
		yield return () => (Quad.Int16MinValue, Int256.Int16MinValue);
		yield return () => (Quad.SByteMinValue, Int256.SByteMinValue);
		yield return () => (Quad.NegativeOne, Int256.NegativeOne);
		yield return () => (Quad.Half, Int256.Zero);
		yield return () => (Quad.One, Int256.One);
		yield return () => (Quad.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (Quad.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (Quad.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (Quad.Int64MaxValue, Int256.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, Int256)>> ConvertToSaturatingInt256TestData()
	{
		yield return () => (Quad.MinValue, Int256.MinValue);
		yield return () => (Quad.Int64MinValue, Int256.Int64MinValue);
		yield return () => (Quad.Int32MinValue, Int256.Int32MinValue);
		yield return () => (Quad.Int16MinValue, Int256.Int16MinValue);
		yield return () => (Quad.SByteMinValue, Int256.SByteMinValue);
		yield return () => (Quad.NegativeOne, Int256.NegativeOne);
		yield return () => (Quad.Half, Int256.Zero);
		yield return () => (Quad.One, Int256.One);
		yield return () => (Quad.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (Quad.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (Quad.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (Quad.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (Quad.TwoOver127, Int256.Int128MaxValue + Int256.One);
		yield return () => (Quad.MaxValue, Int256.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, Int256)>> ConvertToTruncatingInt256TestData()
	{
		yield return () => (Quad.MinValue, Int256.MinValue);
		yield return () => (Quad.Int64MinValue, Int256.Int64MinValue);
		yield return () => (Quad.Int32MinValue, Int256.Int32MinValue);
		yield return () => (Quad.Int16MinValue, Int256.Int16MinValue);
		yield return () => (Quad.SByteMinValue, Int256.SByteMinValue);
		yield return () => (Quad.NegativeOne, Int256.NegativeOne);
		yield return () => (Quad.Half, Int256.Zero);
		yield return () => (Quad.One, Int256.One);
		yield return () => (Quad.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (Quad.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (Quad.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (Quad.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (Quad.TwoOver127, Int256.Int128MaxValue + Int256.One);
		yield return () => (Quad.MaxValue, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Quad, Int512)>> ConvertToCheckedInt512TestData()
	{
		yield return () => (Quad.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Quad.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Quad.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Quad.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Quad.NegativeOne, Int512.NegativeOne);
		yield return () => (Quad.Half, Int512.Zero);
		yield return () => (Quad.One, Int512.One);
		yield return () => (Quad.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Quad.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Quad.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Quad.Int64MaxValue, Int512.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, Int512)>> ConvertToSaturatingInt512TestData()
	{
		yield return () => (Quad.MinValue, Int512.MinValue);
		yield return () => (Quad.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Quad.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Quad.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Quad.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Quad.NegativeOne, Int512.NegativeOne);
		yield return () => (Quad.Half, Int512.Zero);
		yield return () => (Quad.One, Int512.One);
		yield return () => (Quad.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Quad.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Quad.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Quad.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (Quad.TwoOver127, Int512.Int128MaxValue + Int512.One);
		yield return () => (Quad.TwoOver255, Int512.Int256MaxValue + Int512.One);
		yield return () => (Quad.MaxValue, Int512.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, Int512)>> ConvertToTruncatingInt512TestData()
	{
		yield return () => (Quad.MinValue, Int512.MinValue);
		yield return () => (Quad.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Quad.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Quad.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Quad.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Quad.NegativeOne, Int512.NegativeOne);
		yield return () => (Quad.Half, Int512.Zero);
		yield return () => (Quad.One, Int512.One);
		yield return () => (Quad.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Quad.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Quad.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Quad.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (Quad.TwoOver127, Int512.Int128MaxValue + Int512.One);
		yield return () => (Quad.TwoOver255, Int512.Int256MaxValue + Int512.One);
		yield return () => (Quad.MaxValue, Int512.MaxValue);
	}
	
	public static IEnumerable<Func<(Quad, BigInteger)>> ConvertToCheckedBigIntegerTestData()
	{
		yield return () => (Quad.DoubleMinValue, (BigInteger)double.MinValue);
		yield return () => (Quad.SingleMinValue, (BigInteger)float.MinValue);
		yield return () => (Quad.HalfMinValue, (BigInteger)Half.MinValue);
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, BigInteger.MinusOne);
		yield return () => (Quad.Half, BigInteger.Zero);
		yield return () => (Quad.One, BigInteger.One);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
		yield return () => (Quad.HalfMaxValue, (BigInteger)Half.MaxValue);
		yield return () => (Quad.SingleMaxValue, (BigInteger)float.MaxValue);
		yield return () => (Quad.DoubleMaxValue, (BigInteger)double.MaxValue);
	}

	public static IEnumerable<Func<(Quad, BigInteger)>> ConvertToSaturatingBigIntegerTestData()
	{
		yield return () => (Quad.DoubleMinValue, (BigInteger)double.MinValue);
		yield return () => (Quad.SingleMinValue, (BigInteger)float.MinValue);
		yield return () => (Quad.HalfMinValue, (BigInteger)Half.MinValue);
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, BigInteger.MinusOne);
		yield return () => (Quad.Half, BigInteger.Zero);
		yield return () => (Quad.One, BigInteger.One);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
		yield return () => (Quad.HalfMaxValue, (BigInteger)Half.MaxValue);
		yield return () => (Quad.SingleMaxValue, (BigInteger)float.MaxValue);
		yield return () => (Quad.DoubleMaxValue, (BigInteger)double.MaxValue);
	}

	public static IEnumerable<Func<(Quad, BigInteger)>> ConvertToTruncatingBigIntegerTestData()
	{
		yield return () => (Quad.DoubleMinValue, (BigInteger)double.MinValue);
		yield return () => (Quad.SingleMinValue, (BigInteger)float.MinValue);
		yield return () => (Quad.HalfMinValue, (BigInteger)Half.MinValue);
		yield return () => (Quad.Int64MinValue, long.MinValue);
		yield return () => (Quad.Int32MinValue, int.MinValue);
		yield return () => (Quad.Int16MinValue, short.MinValue);
		yield return () => (Quad.SByteMinValue, sbyte.MinValue);
		yield return () => (Quad.NegativeOne, BigInteger.MinusOne);
		yield return () => (Quad.Half, BigInteger.Zero);
		yield return () => (Quad.One, BigInteger.One);
		yield return () => (Quad.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Quad.Int16MaxValue, short.MaxValue);
		yield return () => (Quad.Int32MaxValue, int.MaxValue);
		yield return () => (Quad.Int64MaxValue, long.MaxValue);
		yield return () => (Quad.HalfMaxValue, (BigInteger)Half.MaxValue);
		yield return () => (Quad.SingleMaxValue, (BigInteger)float.MaxValue);
		yield return () => (Quad.DoubleMaxValue, (BigInteger)double.MaxValue);
	}

	public static IEnumerable<Func<(Quad, Half)>> ConvertToCheckedHalfTestData()
	{
		yield return () => (Quad.NegativeInfinity, Half.NegativeInfinity);
		yield return () => (Quad.HalfMinValue, Half.MinValue);
		yield return () => (Quad.NegativeOne, Half.NegativeOne);
		yield return () => (Quad.Half, (Half)0.5f);
		yield return () => (Quad.One, Half.One);
		yield return () => (Quad.HalfMaxValue, Half.MaxValue);
		yield return () => (Quad.PositiveInfinity, Half.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Quad, Half)>> ConvertToSaturatingHalfTestData()
	{
		yield return () => (Quad.NegativeInfinity, Half.NegativeInfinity);
		yield return () => (Quad.HalfMinValue, Half.MinValue);
		yield return () => (Quad.NegativeOne, Half.NegativeOne);
		yield return () => (Quad.Half, (Half)0.5f);
		yield return () => (Quad.One, Half.One);
		yield return () => (Quad.HalfMaxValue, Half.MaxValue);
		yield return () => (Quad.PositiveInfinity, Half.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Quad, Half)>> ConvertToTruncatingHalfTestData()
	{
		yield return () => (Quad.NegativeInfinity, Half.NegativeInfinity);
		yield return () => (Quad.HalfMinValue, Half.MinValue);
		yield return () => (Quad.NegativeOne, Half.NegativeOne);
		yield return () => (Quad.Half, (Half)0.5f);
		yield return () => (Quad.One, Half.One);
		yield return () => (Quad.HalfMaxValue, Half.MaxValue);
		yield return () => (Quad.PositiveInfinity, Half.PositiveInfinity);
	}

	public static IEnumerable<Func<(Quad, float)>> ConvertToCheckedSingleTestData()
	{
		yield return () => (Quad.NegativeInfinity, float.NegativeInfinity);
		yield return () => (Quad.SingleMinValue, float.MinValue);
		yield return () => (Quad.NegativeOne, -1f);
		yield return () => (Quad.Half, 0.5f);
		yield return () => (Quad.One, 1f);
		yield return () => (Quad.SingleMaxValue, float.MaxValue);
		yield return () => (Quad.PositiveInfinity, float.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Quad, float)>> ConvertToSaturatingSingleTestData()
	{
		yield return () => (Quad.NegativeInfinity, float.NegativeInfinity);
		yield return () => (Quad.SingleMinValue, float.MinValue);
		yield return () => (Quad.NegativeOne, -1f);
		yield return () => (Quad.Half, 0.5f);
		yield return () => (Quad.One, 1f);
		yield return () => (Quad.SingleMaxValue, float.MaxValue);
		yield return () => (Quad.PositiveInfinity, float.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Quad, float)>> ConvertToTruncatingSingleTestData()
	{
		yield return () => (Quad.NegativeInfinity, float.NegativeInfinity);
		yield return () => (Quad.SingleMinValue, float.MinValue);
		yield return () => (Quad.NegativeOne, -1f);
		yield return () => (Quad.Half, 0.5f);
		yield return () => (Quad.One, 1f);
		yield return () => (Quad.SingleMaxValue, float.MaxValue);
		yield return () => (Quad.PositiveInfinity, float.PositiveInfinity);
	}

	public static IEnumerable<Func<(Quad, double)>> ConvertToCheckedDoubleTestData()
	{
		yield return () => (Quad.NegativeInfinity, double.NegativeInfinity);
		yield return () => (Quad.DoubleMinValue, double.MinValue);
		yield return () => (Quad.NegativeOne, -1d);
		yield return () => (Quad.Half, 0.5d);
		yield return () => (Quad.One, 1d);
		yield return () => (Quad.DoubleMaxValue, double.MaxValue);
		yield return () => (Quad.PositiveInfinity, double.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Quad, double)>> ConvertToSaturatingDoubleTestData()
	{
		yield return () => (Quad.NegativeInfinity, double.NegativeInfinity);
		yield return () => (Quad.DoubleMinValue, double.MinValue);
		yield return () => (Quad.NegativeOne, -1d);
		yield return () => (Quad.Half, 0.5d);
		yield return () => (Quad.One, 1d);
		yield return () => (Quad.DoubleMaxValue, double.MaxValue);
		yield return () => (Quad.PositiveInfinity, double.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Quad, double)>> ConvertToTruncatingDoubleTestData()
	{
		yield return () => (Quad.NegativeInfinity, double.NegativeInfinity);
		yield return () => (Quad.DoubleMinValue, double.MinValue);
		yield return () => (Quad.NegativeOne, -1d);
		yield return () => (Quad.Half, 0.5d);
		yield return () => (Quad.One, 1d);
		yield return () => (Quad.DoubleMaxValue, double.MaxValue);
		yield return () => (Quad.PositiveInfinity, double.PositiveInfinity);
	}

	public static IEnumerable<Func<(Quad, Octo)>> ConvertToCheckedOctoTestData()
	{
		yield return () => (Quad.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (Quad.NegativeOne, Octo.NegativeOne);
		yield return () => (Quad.Half, Octo.Half);
		yield return () => (Quad.One, Octo.One);
		yield return () => (Quad.PositiveInfinity, Octo.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Quad, Octo)>> ConvertToSaturatingOctoTestData()
	{
		yield return () => (Quad.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (Quad.NegativeOne, Octo.NegativeOne);
		yield return () => (Quad.Half, Octo.Half);
		yield return () => (Quad.One, Octo.One);
		yield return () => (Quad.PositiveInfinity, Octo.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Quad, Octo)>> ConvertToTruncatingOctoTestData()
	{
		yield return () => (Quad.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (Quad.NegativeOne, Octo.NegativeOne);
		yield return () => (Quad.Half, Octo.Half);
		yield return () => (Quad.One, Octo.One);
		yield return () => (Quad.PositiveInfinity, Octo.PositiveInfinity);
	}

	public static IEnumerable<Func<(byte, Quad)>> ConvertFromCheckedByteTestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, Quad)>> ConvertFromSaturatingByteTestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, Quad)>> ConvertFromTruncatingByteTestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
	}

	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromCheckedUInt16TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromSaturatingUInt16TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromTruncatingUInt16TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
	}

	public static IEnumerable<Func<(uint, Quad)>> ConvertFromCheckedUInt32TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, Quad)>> ConvertFromSaturatingUInt32TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, Quad)>> ConvertFromTruncatingUInt32TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
	}

	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromCheckedUInt64TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Quad.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromSaturatingUInt64TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Quad.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromTruncatingUInt64TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Quad.UInt64MaxValue);
	}

	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromCheckedUInt128TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Quad.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Quad.TwoOver128);
	}
	
	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromSaturatingUInt128TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Quad.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Quad.TwoOver128);
	}
	
	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromTruncatingUInt128TestData()
	{
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (byte.MaxValue, Quad.ByteMaxValue);
		yield return () => (ushort.MaxValue, Quad.UInt16MaxValue);
		yield return () => (uint.MaxValue, Quad.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Quad.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Quad.TwoOver128);
	}

	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromCheckedSByteTestData()
	{
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromSaturatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromTruncatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
	}

	public static IEnumerable<Func<(short, Quad)>> ConvertFromCheckedInt16TestData()
	{
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, Quad)>> ConvertFromSaturatingInt16TestData()
	{
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, Quad)>> ConvertFromTruncatingInt16TestData()
	{
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
	}

	public static IEnumerable<Func<(int, Quad)>> ConvertFromCheckedInt32TestData()
	{
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, Quad)>> ConvertFromSaturatingInt32TestData()
	{
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, Quad)>> ConvertFromTruncatingInt32TestData()
	{
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
	}

	public static IEnumerable<Func<(long, Quad)>> ConvertFromCheckedInt64TestData()
	{
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, Quad)>> ConvertFromSaturatingInt64TestData()
	{
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, Quad)>> ConvertFromTruncatingInt64TestData()
	{
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
	}

	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromCheckedInt128TestData()
	{
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
		yield return () => (Int128.MaxValue, Quad.TwoOver127);
	}
	
	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromSaturatingInt128TestData()
	{
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
		yield return () => (Int128.MaxValue, Quad.TwoOver127);
	}
	
	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromTruncatingInt128TestData()
	{
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (-1, Quad.NegativeOne);
		yield return () => (0, Quad.Zero);
		yield return () => (1, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
		yield return () => (Int128.MaxValue, Quad.TwoOver127);
	}
	
	public static IEnumerable<Func<(BigInteger, Quad)>> ConvertFromCheckedBigIntegerTestData()
	{
		yield return () => (Values.QuadMinValue, Quad.MinValue);
		yield return () => ((BigInteger)double.MinValue, Quad.DoubleMinValue);
		yield return () => ((BigInteger)float.MinValue, Quad.SingleMinValue);
		yield return () => ((BigInteger)Half.MinValue, Quad.HalfMinValue);
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (BigInteger.MinusOne, Quad.NegativeOne);
		yield return () => (BigInteger.Zero, Quad.Zero);
		yield return () => (BigInteger.One, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
		yield return () => ((BigInteger)Half.MaxValue, Quad.HalfMaxValue);
		yield return () => ((BigInteger)float.MaxValue, Quad.SingleMaxValue);
		yield return () => ((BigInteger)double.MaxValue, Quad.DoubleMaxValue);
		yield return () => (Values.QuadMaxValue, Quad.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, Quad)>> ConvertFromSaturatingBigIntegerTestData()
	{
		yield return () => (Values.QuadMinValue, Quad.MinValue);
		yield return () => ((BigInteger)double.MinValue, Quad.DoubleMinValue);
		yield return () => ((BigInteger)float.MinValue, Quad.SingleMinValue);
		yield return () => ((BigInteger)Half.MinValue, Quad.HalfMinValue);
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (BigInteger.MinusOne, Quad.NegativeOne);
		yield return () => (BigInteger.Zero, Quad.Zero);
		yield return () => (BigInteger.One, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
		yield return () => ((BigInteger)Half.MaxValue, Quad.HalfMaxValue);
		yield return () => ((BigInteger)float.MaxValue, Quad.SingleMaxValue);
		yield return () => ((BigInteger)double.MaxValue, Quad.DoubleMaxValue);
		yield return () => (Values.QuadMaxValue, Quad.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, Quad)>> ConvertFromTruncatingBigIntegerTestData()
	{
		yield return () => (Values.QuadMinValue, Quad.MinValue);
		yield return () => ((BigInteger)double.MinValue, Quad.DoubleMinValue);
		yield return () => ((BigInteger)float.MinValue, Quad.SingleMinValue);
		yield return () => ((BigInteger)Half.MinValue, Quad.HalfMinValue);
		yield return () => (long.MinValue, Quad.Int64MinValue);
		yield return () => (int.MinValue, Quad.Int32MinValue);
		yield return () => (short.MinValue, Quad.Int16MinValue);
		yield return () => (sbyte.MinValue, Quad.SByteMinValue);
		yield return () => (BigInteger.MinusOne, Quad.NegativeOne);
		yield return () => (BigInteger.Zero, Quad.Zero);
		yield return () => (BigInteger.One, Quad.One);
		yield return () => (sbyte.MaxValue, Quad.SByteMaxValue);
		yield return () => (short.MaxValue, Quad.Int16MaxValue);
		yield return () => (int.MaxValue, Quad.Int32MaxValue);
		yield return () => (long.MaxValue, Quad.Int64MaxValue);
		yield return () => ((BigInteger)Half.MaxValue, Quad.HalfMaxValue);
		yield return () => ((BigInteger)float.MaxValue, Quad.SingleMaxValue);
		yield return () => ((BigInteger)double.MaxValue, Quad.DoubleMaxValue);
		yield return () => (Values.QuadMaxValue, Quad.MaxValue);
	}

	public static IEnumerable<Func<(Half, Quad)>> ConvertFromCheckedHalfTestData()
	{
		yield return () => (Half.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (Half.MinValue, Quad.HalfMinValue);
		yield return () => (Half.NegativeOne, Quad.NegativeOne);
		yield return () => (-(Half)0.5f, Quad.NegativeHalf);
		yield return () => (Half.Zero, Quad.Zero);
		yield return () => ((Half)0.5f, Quad.Half);
		yield return () => (Half.One, Quad.One);
		yield return () => (Half.MaxValue, Quad.HalfMaxValue);
		yield return () => (Half.PositiveInfinity, Quad.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Half, Quad)>> ConvertFromSaturatingHalfTestData()
	{
		yield return () => (Half.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (Half.MinValue, Quad.HalfMinValue);
		yield return () => (Half.NegativeOne, Quad.NegativeOne);
		yield return () => (-(Half)0.5f, Quad.NegativeHalf);
		yield return () => (Half.Zero, Quad.Zero);
		yield return () => ((Half)0.5f, Quad.Half);
		yield return () => (Half.One, Quad.One);
		yield return () => (Half.MaxValue, Quad.HalfMaxValue);
		yield return () => (Half.PositiveInfinity, Quad.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Half, Quad)>> ConvertFromTruncatingHalfTestData()
	{
		yield return () => (Half.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (Half.MinValue, Quad.HalfMinValue);
		yield return () => (Half.NegativeOne, Quad.NegativeOne);
		yield return () => (-(Half)0.5f, Quad.NegativeHalf);
		yield return () => (Half.Zero, Quad.Zero);
		yield return () => ((Half)0.5f, Quad.Half);
		yield return () => (Half.One, Quad.One);
		yield return () => (Half.MaxValue, Quad.HalfMaxValue);
		yield return () => (Half.PositiveInfinity, Quad.PositiveInfinity);
	}

	public static IEnumerable<Func<(float, Quad)>> ConvertFromCheckedSingleTestData()
	{
		yield return () => (float.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (float.MinValue, Quad.SingleMinValue);
		yield return () => (-1f, Quad.NegativeOne);
		yield return () => (-0.5f, Quad.NegativeHalf);
		yield return () => (0f, Quad.Zero);
		yield return () => (0.5f, Quad.Half);
		yield return () => (1f, Quad.One);
		yield return () => (float.MaxValue, Quad.SingleMaxValue);
		yield return () => (float.PositiveInfinity, Quad.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(float, Quad)>> ConvertFromSaturatingSingleTestData()
	{
		yield return () => (float.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (float.MinValue, Quad.SingleMinValue);
		yield return () => (-1f, Quad.NegativeOne);
		yield return () => (-0.5f, Quad.NegativeHalf);
		yield return () => (0f, Quad.Zero);
		yield return () => (0.5f, Quad.Half);
		yield return () => (1f, Quad.One);
		yield return () => (float.MaxValue, Quad.SingleMaxValue);
		yield return () => (float.PositiveInfinity, Quad.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(float, Quad)>> ConvertFromTruncatingSingleTestData()
	{
		yield return () => (float.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (float.MinValue, Quad.SingleMinValue);
		yield return () => (-1f, Quad.NegativeOne);
		yield return () => (-0.5f, Quad.NegativeHalf);
		yield return () => (0f, Quad.Zero);
		yield return () => (0.5f, Quad.Half);
		yield return () => (1f, Quad.One);
		yield return () => (float.MaxValue, Quad.SingleMaxValue);
		yield return () => (float.PositiveInfinity, Quad.PositiveInfinity);
	}

	public static IEnumerable<Func<(double, Quad)>> ConvertFromCheckedDoubleTestData()
	{
		yield return () => (double.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (double.MinValue, Quad.DoubleMinValue);
		yield return () => (-1d, Quad.NegativeOne);
		yield return () => (-0.5d, Quad.NegativeHalf);
		yield return () => (0d, Quad.Zero);
		yield return () => (0.5d, Quad.Half);
		yield return () => (1d, Quad.One);
		yield return () => (double.MaxValue, Quad.DoubleMaxValue);
		yield return () => (double.PositiveInfinity, Quad.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(double, Quad)>> ConvertFromSaturatingDoubleTestData()
	{
		yield return () => (double.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (double.MinValue, Quad.DoubleMinValue);
		yield return () => (-1d, Quad.NegativeOne);
		yield return () => (-0.5d, Quad.NegativeHalf);
		yield return () => (0d, Quad.Zero);
		yield return () => (0.5d, Quad.Half);
		yield return () => (1d, Quad.One);
		yield return () => (double.MaxValue, Quad.DoubleMaxValue);
		yield return () => (double.PositiveInfinity, Quad.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(double, Quad)>> ConvertFromTruncatingDoubleTestData()
	{
		yield return () => (double.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (double.MinValue, Quad.DoubleMinValue);
		yield return () => (-1d, Quad.NegativeOne);
		yield return () => (-0.5d, Quad.NegativeHalf);
		yield return () => (0d, Quad.Zero);
		yield return () => (0.5d, Quad.Half);
		yield return () => (1d, Quad.One);
		yield return () => (double.MaxValue, Quad.DoubleMaxValue);
		yield return () => (double.PositiveInfinity, Quad.PositiveInfinity);
	}
}