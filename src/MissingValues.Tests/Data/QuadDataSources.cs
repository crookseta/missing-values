using System.Globalization;
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
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> BitIncrementTestData()
    {
        throw new NotImplementedException();
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

    public static IEnumerable<Func<(Quad, Quad, Quad)>> ILogBTestData()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
    
	public static IEnumerable<Func<(Quad, byte)>> ConvertToCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, byte)>> ConvertToSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, byte)>> ConvertToTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, ushort)>> ConvertToCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, uint)>> ConvertToCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, uint)>> ConvertToSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, uint)>> ConvertToTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, ulong)>> ConvertToCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, short)>> ConvertToCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, short)>> ConvertToSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, short)>> ConvertToTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, int)>> ConvertToCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, int)>> ConvertToSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, int)>> ConvertToTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, long)>> ConvertToCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, long)>> ConvertToSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, long)>> ConvertToTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Int128)>> ConvertToCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int128)>> ConvertToSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int128)>> ConvertToTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Int256)>> ConvertToCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int256)>> ConvertToSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int256)>> ConvertToTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Int512)>> ConvertToCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int512)>> ConvertToSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int512)>> ConvertToTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Half)>> ConvertToCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Half)>> ConvertToSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Half)>> ConvertToTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, float)>> ConvertToCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, float)>> ConvertToSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, float)>> ConvertToTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, double)>> ConvertToCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, double)>> ConvertToSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, double)>> ConvertToTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Octo)>> ConvertToCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Octo)>> ConvertToSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Octo)>> ConvertToTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte, Quad)>> ConvertFromCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Quad)>> ConvertFromSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Quad)>> ConvertFromTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(uint, Quad)>> ConvertFromCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Quad)>> ConvertFromSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Quad)>> ConvertFromTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt256, Quad)>> ConvertFromCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Quad)>> ConvertFromSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Quad)>> ConvertFromTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Quad)>> ConvertFromCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Quad)>> ConvertFromSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Quad)>> ConvertFromTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(short, Quad)>> ConvertFromCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Quad)>> ConvertFromSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Quad)>> ConvertFromTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(int, Quad)>> ConvertFromCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Quad)>> ConvertFromSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Quad)>> ConvertFromTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(long, Quad)>> ConvertFromCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Quad)>> ConvertFromSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Quad)>> ConvertFromTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Quad)>> ConvertFromCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Quad)>> ConvertFromSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Quad)>> ConvertFromTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Quad)>> ConvertFromCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Quad)>> ConvertFromSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Quad)>> ConvertFromTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Half, Quad)>> ConvertFromCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Quad)>> ConvertFromSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Quad)>> ConvertFromTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(float, Quad)>> ConvertFromCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Quad)>> ConvertFromSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Quad)>> ConvertFromTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(double, Quad)>> ConvertFromCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Quad)>> ConvertFromSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Quad)>> ConvertFromTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Quad)>> ConvertFromCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Quad)>> ConvertFromSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Quad)>> ConvertFromTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}
}