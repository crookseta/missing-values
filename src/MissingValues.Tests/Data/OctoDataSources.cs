using System.Globalization;
using System.Numerics;
using System.Text;
using MissingValues.Tests.Data.Sources;
using MissingValues.Tests.Extensions;

namespace MissingValues.Tests.Data;

public class OctoDataSources
    : IMathOperatorsDataSource<Octo>,
        IBitwiseOperatorsDataSource<Octo>,
        IEqualityOperatorsDataSource<Octo>,
        IComparisonOperatorsDataSource<Octo>,
        INumberBaseDataSource<Octo>,
        INumberDataSource<Octo>,
        IBinaryNumberDataSource<Octo>,
        IFloatingPointDataSource<Octo>,
        IFloatingPointIeee754DataSource<Octo>
{
    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_AdditionTestData()
    {
	    yield return () => (Octo.One, Octo.One, Octo.Two);
	    yield return () => (Octo.One, Octo.NegativeOne, Octo.Zero);
	    yield return () => (Octo.One, Octo.NegativeTwo, Octo.NegativeOne);
	    yield return () => (Octo.One, Octo.Four, Octo.Five);
	    yield return () => (Octo.Three, Octo.Two, Octo.Five);
	    yield return () => (Octo.SmallestSubnormal, Octo.GreatestSubnormal, Values.CreateFloat<Octo>(0x0000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.PositiveInfinity);
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.NegativeInfinity);
	    yield return () => (Octo.PositiveInfinity, Octo.PositiveInfinity, Octo.PositiveInfinity);
	    yield return () => (Octo.NegativeInfinity, Octo.NegativeInfinity, Octo.NegativeInfinity);
    }

    public static IEnumerable<Func<(Octo, Octo)>> op_DecrementTestData()
    {
	    yield return () => (Octo.NegativeOne, Octo.NegativeTwo);
	    yield return () => (Octo.Zero, Octo.NegativeOne);
	    yield return () => (Octo.One, Octo.Zero);
	    yield return () => (Octo.Two, Octo.One);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_DivisionTestData()
    {
	    yield return () => (Octo.Ten, Octo.Ten, Octo.One);
	    yield return () => (Octo.Hundred, Octo.Ten, Octo.Ten);
	    yield return () => (Octo.NegativeThousand, Octo.Ten, Octo.NegativeHundred);
	    yield return () => (Octo.Zero, Octo.Zero, Octo.NaN);
	    yield return () => (Octo.One, Octo.Zero, Octo.PositiveInfinity);
	    yield return () => (Octo.NegativeOne, Octo.Zero, Octo.NegativeInfinity);
	    yield return () => (Octo.PositiveInfinity, Octo.PositiveInfinity, Octo.NaN);
	    yield return () => (Octo.NegativeInfinity, Octo.NegativeInfinity, Octo.NaN);
    }

    public static IEnumerable<Func<(Octo, Octo)>> op_IncrementTestData()
    {
	    yield return () => (Octo.NegativeTwo, Octo.NegativeOne);
	    yield return () => (Octo.NegativeOne, Octo.Zero);
	    yield return () => (Octo.Zero, Octo.One);
	    yield return () => (Octo.One, Octo.Two);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_ModulusTestData()
    {
	    yield return () => (Octo.Two, Octo.Four, Octo.Two);
	    yield return () => (Octo.Half, Octo.Four, Octo.Half);
	    yield return () => (Octo.Four, Octo.Half, Octo.Zero);
	    yield return () => (Octo.NegativeFour, Octo.Half, Octo.NegativeZero);
	    yield return () => (Octo.NegativeFour, Octo.Thousand, Octo.NegativeFour);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_MultiplyTestData()
    {
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.One, Octo.NegativeOne, Octo.NegativeOne);
	    yield return () => (Octo.Ten, Octo.Ten, Octo.Hundred);
	    yield return () => (Octo.NegativeHundred, Octo.Ten, Octo.NegativeThousand);
	    yield return () => (Octo.NegativeTen, Octo.Hundred, Octo.NegativeThousand);
	    yield return () => (Octo.Zero, Octo.NegativeThousand, Octo.NegativeZero);
	    yield return () => (Octo.Zero, Octo.PositiveInfinity, Octo.NaN);
	    yield return () => (Octo.NegativeZero, Octo.NegativeInfinity, Octo.NaN);
	    yield return () => (Octo.PositiveInfinity, Octo.Zero, Octo.NaN);
	    yield return () => (Octo.NegativeInfinity, Octo.NegativeZero, Octo.NaN);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_SubtractionTestData()
    {
	    yield return () => (Octo.One, Octo.One, Octo.Zero);
	    yield return () => (Octo.One, Octo.NegativeOne, Octo.Two);
	    yield return () => (Octo.One, Octo.Two, Octo.NegativeOne);
	    yield return () => (Octo.SmallestSubnormal, Octo.GreatestSubnormal, Values.CreateFloat<Octo>(0x8000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
	    yield return () => (Octo.PositiveInfinity, Octo.PositiveInfinity, Octo.NaN);
	    yield return () => (Octo.NegativeInfinity, Octo.NegativeInfinity, Octo.NaN);
    }

    public static IEnumerable<Func<(Octo, Octo)>> op_UnaryNegationTestData()
    {
	    yield return () => (Octo.Zero, Octo.NegativeZero);
	    yield return () => (Octo.One, Octo.NegativeOne);
	    yield return () => (Octo.Two, Octo.NegativeTwo);
	    yield return () => (Octo.Ten, Octo.NegativeTen);
	    yield return () => (Octo.Hundred, Octo.NegativeHundred);
	    yield return () => (Octo.Thousand, Octo.NegativeThousand);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_BitwiseAndTestData()
    {
	    yield return () => (Octo.Zero, Octo.One, Octo.Zero);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), Values.CreateFloat<Octo>(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), Values.CreateFloat<Octo>(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001));
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_BitwiseOrTestData()
    {
	    yield return () => (Octo.Zero, Octo.One, Octo.One);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), Values.CreateFloat<Octo>(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_BitwiseXorTestData()
    {
	    yield return () => (Octo.Zero, Octo.One, Octo.One);
	    yield return () => (Octo.One, Octo.One, Octo.Zero);
	    yield return () => (Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), Values.CreateFloat<Octo>(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
    }

    public static IEnumerable<Func<(Octo, Octo)>> op_OnesComplementTestData()
    {
	    yield return () => (Octo.Zero, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
	    yield return () => (Values.CreateFloat<Octo>(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
	    yield return () => (Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), Octo.Zero);
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_EqualityTestData()
    {
	    yield return () => (Octo.One, Octo.One, true); 
	    yield return () => (Octo.Two, Octo.Two, true);
	    yield return () => (Octo.NaN, Octo.NaN, false);
	    yield return () => (Octo.GreatestSubnormal, Octo.GreatestSubnormal, true);
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_InequalityTestData()
    {
	    yield return () => (Octo.One, Octo.One, false);
	    yield return () => (Octo.NaN, Octo.NaN, true);
	    yield return () => (Octo.NegativeTwo, Octo.Two, true);
	    yield return () => (Octo.SmallestSubnormal, Octo.GreatestSubnormal, true);
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_GreaterThanOrEqualTestData()
    {
	    yield return () => (Octo.One, Octo.One, true); 
	    yield return () => (Octo.Two, Octo.Two, true);
	    yield return () => (Octo.NaN, Octo.NaN, false);
	    yield return () => (Octo.GreatestSubnormal, Octo.GreatestSubnormal, true);
	    yield return () => (Octo.Two, Octo.One, true);
	    yield return () => (Octo.Thousand, Octo.NegativeThousand, true);
	    yield return () => (Octo.NegativeQuarter, Octo.NegativeHalf, true);
	    yield return () => (Octo.Quarter, Octo.Half, false);
	    yield return () => (Octo.Ten, Octo.Hundred, false);
	    yield return () => (Octo.GreaterThanOneSmallest, Octo.One, true);
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_GreaterThanTestData()
    {
	    yield return () => (Octo.Two, Octo.One, true);
	    yield return () => (Octo.Thousand, Octo.NegativeThousand, true);
	    yield return () => (Octo.NegativeQuarter, Octo.NegativeHalf, true);
	    yield return () => (Octo.Quarter, Octo.Half, false);
	    yield return () => (Octo.Ten, Octo.Hundred, false);
	    yield return () => (Octo.GreaterThanOneSmallest, Octo.One, true);
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_LessThanOrEqualTestData()
    {
	    yield return () => (Octo.One, Octo.One, true); 
	    yield return () => (Octo.Two, Octo.Two, true);
	    yield return () => (Octo.NaN, Octo.NaN, false);
	    yield return () => (Octo.GreatestSubnormal, Octo.GreatestSubnormal, true);
	    yield return () => (Octo.Zero, Octo.One, true);
	    yield return () => (Octo.Zero, Octo.Quarter, true);
	    yield return () => (Octo.NegativeThousand, Octo.Thousand, true);
	    yield return () => (Octo.NegativeOne, Octo.NegativeThree, false);
	    yield return () => (Octo.Hundred, Octo.Two, false);
	    yield return () => (Octo.LessThanOneLargest, Octo.One, true);
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_LessThanTestData()
    {
	    yield return () => (Octo.Zero, Octo.One, true);
	    yield return () => (Octo.Zero, Octo.Quarter, true);
	    yield return () => (Octo.NegativeThousand, Octo.Thousand, true);
	    yield return () => (Octo.NegativeOne, Octo.NegativeThree, false);
	    yield return () => (Octo.Hundred, Octo.Two, false);
	    yield return () => (Octo.LessThanOneLargest, Octo.One, true);
    }

    public static IEnumerable<Func<(Octo, Octo)>> AbsTestData()
    {
	    yield return () => (Octo.One, Octo.One);
	    yield return () => (Octo.NegativeOne, Octo.One);
	    yield return () => (Octo.NegativeHalf, Octo.Half);
	    yield return () => (Octo.NegativeQuarter, Octo.Quarter);
	    yield return () => (Octo.NegativeZero, Octo.Zero);
	    yield return () => (Octo.NegativeInfinity, Octo.PositiveInfinity);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsCanonicalTestData()
    {
	    yield return () => (Octo.One, true);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsComplexNumberTestData()
    {
	    yield return () => (Octo.One, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsEvenIntegerTestData()
    {
	    yield return () => (Octo.Half, false);
	    yield return () => (Octo.One, false);
	    yield return () => (Octo.Two, true);
	    yield return () => (Octo.Three, false);
	    yield return () => (Octo.Four, true);
	    yield return () => (Octo.NegativeOne, false);
	    yield return () => (Octo.NegativeTwo, true);
	    yield return () => (Octo.NegativeThree, false);
	    yield return () => (Octo.NegativeFour, true);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsFiniteTestData()
    {
	    yield return () => (Octo.One, true);
	    yield return () => (Octo.NegativeOne, true);
	    yield return () => (Octo.NaN, false);
	    yield return () => (Octo.PositiveInfinity, false);
	    yield return () => (Octo.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsImaginaryNumberTestData()
    {
	    yield return () => (Octo.One, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsInfinityTestData()
    {
	    yield return () => (Octo.One, false);
	    yield return () => (Octo.NegativeOne, false);
	    yield return () => (Octo.NaN, false);
	    yield return () => (Octo.PositiveInfinity, true);
	    yield return () => (Octo.NegativeInfinity, true);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsIntegerTestData()
    {
	    yield return () => (Octo.Quarter, false);
	    yield return () => (Octo.Half, false);
	    yield return () => (Octo.Thousand, true);
	    yield return () => (Octo.One, true);
	    yield return () => (Octo.GreaterThanOneSmallest, false);
	    yield return () => (Octo.SmallestSubnormal, false);
	    yield return () => (Octo.NegativeOne, true);
	    yield return () => (Octo.NegativeThousand, true);
	    yield return () => (Octo.NegativeHalf, false);
	    yield return () => (Octo.NegativeQuarter, false);
	    yield return () => (Octo.NaN, false);
	    yield return () => (Octo.PositiveInfinity, false);
	    yield return () => (Octo.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsNaNTestData()
    {
	    yield return () => (Octo.One, false);
	    yield return () => (Octo.NegativeOne, false);
	    yield return () => (Octo.NaN, true);
	    yield return () => (Octo.PositiveInfinity, false);
	    yield return () => (Octo.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsNegativeTestData()
    {
	    yield return () => (Octo.One, false);
	    yield return () => (Octo.GreatestSubnormal, false);
	    yield return () => (Octo.PositiveInfinity, false);
	    yield return () => (Octo.NaN, true);
	    yield return () => (Octo.NegativeOne, true);
	    yield return () => (Octo.NegativeInfinity, true);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsNegativeInfinityTestData()
    {
	    yield return () => (Octo.One, false);
	    yield return () => (Octo.NegativeOne, false);
	    yield return () => (Octo.NaN, false);
	    yield return () => (Octo.PositiveInfinity, false);
	    yield return () => (Octo.NegativeInfinity, true);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsNormalTestData()
    {
	    yield return () => (Octo.GreatestSubnormal, false);
	    yield return () => (Octo.SmallestSubnormal, false);
	    yield return () => (Octo.MaxValue, true);
	    yield return () => (Octo.MinValue, true);
	    yield return () => (Octo.One, true);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsOddIntegerTestData()
    {
	    yield return () => (Octo.Half, false);
	    yield return () => (Octo.One, true);
	    yield return () => (Octo.Two, false);
	    yield return () => (Octo.Three, true);
	    yield return () => (Octo.Four, false);
	    yield return () => (Octo.NegativeOne, true);
	    yield return () => (Octo.NegativeTwo, false);
	    yield return () => (Octo.NegativeThree, true);
	    yield return () => (Octo.NegativeFour, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsPositiveTestData()
    {
	    yield return () => (Octo.One, true);
	    yield return () => (Octo.GreatestSubnormal, true);
	    yield return () => (Octo.PositiveInfinity, true);
	    yield return () => (Octo.NaN, false);
	    yield return () => (Octo.NegativeOne, false);
	    yield return () => (Octo.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsPositiveInfinityTestData()
    {
	    yield return () => (Octo.One, false);
	    yield return () => (Octo.NegativeOne, false);
	    yield return () => (Octo.NaN, false);
	    yield return () => (Octo.PositiveInfinity, true);
	    yield return () => (Octo.NegativeInfinity, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsRealNumberTestData()
    {
	    yield return () => (Octo.GreatestSubnormal, true);
	    yield return () => (Octo.MaxValue, true);
	    yield return () => (Octo.NegativeThousand, true);
	    yield return () => (Octo.PositiveInfinity, true);
	    yield return () => (Octo.NegativeInfinity, true);
	    yield return () => (Octo.NaN, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsSubnormalTestData()
    {
	    yield return () => (Octo.GreatestSubnormal, true);
	    yield return () => (Octo.SmallestSubnormal, true);
	    yield return () => (Octo.MaxValue, false);
	    yield return () => (Octo.MinValue, false);
	    yield return () => (Octo.One, false);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsZeroTestData()
    {
	    yield return () => (Octo.One, false);
	    yield return () => (Octo.Epsilon, false);
	    yield return () => (Octo.Zero, true);
	    yield return () => (Octo.NegativeZero, true);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MaxMagnitudeTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.NegativeInfinity);
	    yield return () => (Octo.MinValue, Octo.One, Octo.MinValue);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.One);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, Octo.One);
	    yield return () => (-Octo.Epsilon, Octo.One, Octo.One);
	    yield return () => (Octo.NegativeZero, Octo.One, Octo.One);
	    yield return () => (Octo.NaN, Octo.One, Octo.NaN);
	    yield return () => (Octo.Zero, Octo.One, Octo.One);
	    yield return () => (Octo.Epsilon, Octo.One, Octo.One);
	    yield return () => (Octo.GreatestSubnormal, Octo.One, Octo.One);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.MaxValue);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.PositiveInfinity);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MaxMagnitudeNumberTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.NegativeInfinity);
	    yield return () => (Octo.MinValue, Octo.One, Octo.MinValue);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.One);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, Octo.One);
	    yield return () => (-Octo.Epsilon, Octo.One, Octo.One);
	    yield return () => (Octo.NegativeZero, Octo.One, Octo.One);
	    yield return () => (Octo.NaN, Octo.One, Octo.One);
	    yield return () => (Octo.Zero, Octo.One, Octo.One);
	    yield return () => (Octo.Epsilon, Octo.One, Octo.One);
	    yield return () => (Octo.GreatestSubnormal, Octo.One, Octo.One);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.MaxValue);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.PositiveInfinity);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MinMagnitudeTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.One);
	    yield return () => (Octo.MinValue, Octo.One, Octo.One);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.NegativeOne);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, -Octo.GreatestSubnormal);
	    yield return () => (-Octo.Epsilon, Octo.One, -Octo.Epsilon);
	    yield return () => (Octo.NegativeZero, Octo.One, Octo.NegativeZero);
	    yield return () => (Octo.NaN, Octo.One, Octo.NaN);
	    yield return () => (Octo.Zero, Octo.One, Octo.Zero);
	    yield return () => (Octo.Epsilon, Octo.One, Octo.Epsilon);
	    yield return () => (Octo.GreatestSubnormal, Octo.One, Octo.GreatestSubnormal);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.One);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.One);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MinMagnitudeNumberTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.One);
	    yield return () => (Octo.MinValue, Octo.One, Octo.One);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.NegativeOne);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, -Octo.GreatestSubnormal);
	    yield return () => (-Octo.Epsilon, Octo.One, -Octo.Epsilon);
	    yield return () => (Octo.NegativeZero, Octo.One, Octo.NegativeZero);
	    yield return () => (Octo.NaN, Octo.One, Octo.One);
	    yield return () => (Octo.Zero, Octo.One, Octo.Zero);
	    yield return () => (Octo.Epsilon, Octo.One, Octo.Epsilon);
	    yield return () => (Octo.GreatestSubnormal, Octo.One, Octo.GreatestSubnormal);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.One);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.One);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, Octo)>> MultiplyAddEstimateTestData()
    {
		yield return () => (Octo.One, Octo.One, Octo.One, Octo.Two);
		yield return () => (Octo.Ten, Octo.Ten, Octo.Zero, Octo.Hundred);
		yield return () => (Octo.Five, Octo.Zero, Octo.Five, Octo.Five);
		yield return () => (Octo.Half, Octo.Two, Octo.Two, Octo.Three);
		yield return () => (Octo.Two, Octo.Four, Octo.Two, Octo.Ten);
		yield return () => (Octo.Ten, Octo.Half, Octo.Five, Octo.Ten);
		yield return () => (Values.CreateFloat<Octo>(0xBFFF_F400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), Octo.One, Octo.Two, Values.CreateFloat<Octo>(0x3FFF_E800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
    }

    public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, Octo)>> ParseTestData()
    {
	    yield return () => ("10.0", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Ten);
	    yield return () => ("3", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Three);
	    yield return () => ("-3", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeThree);
	    yield return () => ("2.0", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Two);
	    yield return () => ("-2", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeTwo);
	    yield return () => ("0", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Zero);
	    yield return () => ("-0", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeZero);
	    yield return () => ("1.61132571748576047361957211845200501064402387454966951747637125049607183E+78913", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.MaxValue);
	    yield return () => (NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol, NumberStyles.Float, CultureInfo.InvariantCulture, Octo.PositiveInfinity);
	    yield return () => (NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol, NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeInfinity);
	    yield return () => (NumberFormatInfo.CurrentInfo.NaNSymbol, NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NaN);
	    
	    yield return () => ("2.5E-1", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Quarter);
	    yield return () => ("0.250", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Quarter);
	    yield return () => ("$-0.25", NumberStyles.Currency, Helper.CustomInfo, Octo.NegativeQuarter);
	    yield return () => ("1.000", NumberStyles.Float, CultureInfo.InvariantCulture, Octo.One);
	    yield return () => ("1,000.00", NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Octo.Thousand);
	    yield return () => ("-1,000.00", NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Octo.NegativeThousand);
    }

    public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Octo)>> ParseSpanTestData()
    {
	    yield return () => ("10.0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Ten);
	    yield return () => ("3".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Three);
	    yield return () => ("-3".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeThree);
	    yield return () => ("2.0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Two);
	    yield return () => ("-2".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeTwo);
	    yield return () => ("0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Zero);
	    yield return () => ("-0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeZero);
	    yield return () => ("1.61132571748576047361957211845200501064402387454966951747637125049607183E+78913".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.MaxValue);
	    yield return () => (NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.PositiveInfinity);
	    yield return () => (NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeInfinity);
	    yield return () => (NumberFormatInfo.CurrentInfo.NaNSymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NaN);
	    
	    yield return () => ("2.5E-1".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Quarter);
	    yield return () => ("0.250".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Quarter);
	    yield return () => ("$-0.25".ToCharArray(), NumberStyles.Currency, Helper.CustomInfo, Octo.NegativeQuarter);
	    yield return () => ("1.000".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.One);
	    yield return () => ("1,000.00".ToCharArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Octo.Thousand);
	    yield return () => ("-1,000.00".ToCharArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Octo.NegativeThousand);
    }

    public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Octo)>> ParseUtf8TestData()
    {
	    yield return () => ("10.0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Ten);
	    yield return () => ("3"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Three);
	    yield return () => ("-3"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeThree);
	    yield return () => ("2.0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Two);
	    yield return () => ("-2"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeTwo);
	    yield return () => ("0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Zero);
	    yield return () => ("-0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeZero);
	    yield return () => ("1.61132571748576047361957211845200501064402387454966951747637125049607183E+78913"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.MaxValue);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.PositiveInfinity);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NegativeInfinity);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.CurrentInfo.NaNSymbol), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.NaN);
	    
	    yield return () => ("2.5E-1"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Quarter);
	    yield return () => ("0.250"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.Quarter);
	    yield return () => ("$-0.25"u8.ToArray(), NumberStyles.Currency, Helper.CustomInfo, Octo.NegativeQuarter);
	    yield return () => ("1.000"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, Octo.One);
	    yield return () => ("1,000.00"u8.ToArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Octo.Thousand);
	    yield return () => ("-1,000.00"u8.ToArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, Octo.NegativeThousand);
    }

    public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Octo)>> TryParseTestData()
    {
		yield return () => ("10.0", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Ten);
		yield return () => ("3", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Three);
		yield return () => ("-3", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeThree);
		yield return () => ("2.0", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Two);
		yield return () => ("-2", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeTwo);
		yield return () => ("0", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Zero);
		yield return () => ("-0", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeZero);
		yield return () => ("1.61132571748576047361957211845200501064402387454966951747637125049607183E+78913", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.MaxValue);
		yield return () => (NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol, NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.PositiveInfinity);
		yield return () => (NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol, NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeInfinity);
		yield return () => (NumberFormatInfo.CurrentInfo.NaNSymbol, NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NaN);
		
		yield return () => ("2.5E-1", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Quarter);
		yield return () => ("0.250", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Quarter);
		yield return () => ("$-0.25", NumberStyles.Currency, Helper.CustomInfo, true, Octo.NegativeQuarter);
		yield return () => ("1.000", NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.One);
		yield return () => ("1,000.00", NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Octo.Thousand);
		yield return () => ("-1,000.00", NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Octo.NegativeThousand);
    }

    public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Octo)>> TryParseSpanTestData()
    {
	    yield return () => ("10.0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Ten);
	    yield return () => ("3".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Three);
	    yield return () => ("-3".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeThree);
	    yield return () => ("2.0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Two);
	    yield return () => ("-2".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeTwo);
	    yield return () => ("0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Zero);
	    yield return () => ("-0".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeZero);
	    yield return () => ("1.61132571748576047361957211845200501064402387454966951747637125049607183E+78913".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.MaxValue);
	    yield return () => (NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.PositiveInfinity);
	    yield return () => (NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeInfinity);
	    yield return () => (NumberFormatInfo.CurrentInfo.NaNSymbol.ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NaN);
	    
	    yield return () => ("2.5E-1".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Quarter);
	    yield return () => ("0.250".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Quarter);
	    yield return () => ("$-0.25".ToCharArray(), NumberStyles.Currency, Helper.CustomInfo, true, Octo.NegativeQuarter);
	    yield return () => ("1.000".ToCharArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.One);
	    yield return () => ("1,000.00".ToCharArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Octo.Thousand);
	    yield return () => ("-1,000.00".ToCharArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Octo.NegativeThousand);
    }

    public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Octo)>> TryParseUtf8TestData()
    {
	    yield return () => ("10.0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Ten);
	    yield return () => ("3"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Three);
	    yield return () => ("-3"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeThree);
	    yield return () => ("2.0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Two);
	    yield return () => ("-2"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeTwo);
	    yield return () => ("0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Zero);
	    yield return () => ("-0"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeZero);
	    yield return () => ("1.61132571748576047361957211845200501064402387454966951747637125049607183E+78913"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.MaxValue);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.PositiveInfinity);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NegativeInfinity);
	    yield return () => (Encoding.UTF8.GetBytes(NumberFormatInfo.CurrentInfo.NaNSymbol), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.NaN);
	    
	    yield return () => ("2.5E-1"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Quarter);
	    yield return () => ("0.250"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.Quarter);
	    yield return () => ("$-0.25"u8.ToArray(), NumberStyles.Currency, Helper.CustomInfo, true, Octo.NegativeQuarter);
	    yield return () => ("1.000"u8.ToArray(), NumberStyles.Float, CultureInfo.InvariantCulture, true, Octo.One);
	    yield return () => ("1,000.00"u8.ToArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Octo.Thousand);
	    yield return () => ("-1,000.00"u8.ToArray(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, true, Octo.NegativeThousand);
    }

    public static IEnumerable<Func<(Octo, string, IFormatProvider?, string)>> ToStringTestData()
    {
	    Octo value = Values.CreateFloat<Octo>(0x4000_C81C_D6E6_31F8, 0xA090_2DE0_0D1B_7175, 0x8E21_9652_BD3C_3611, 0x3404_EA4A_8C15_4C98);
	    
	    yield return () => (value, "F", CultureInfo.InvariantCulture, "12345.68");
	    yield return () => (value, "F", Helper.CustomInfo, "12345.67890");
	    yield return () => (value, "N3", CultureInfo.InvariantCulture, "12,345.679");
	    yield return () => (value, "N", Helper.CustomInfo, "1_23_45.67890");
	    yield return () => (value, "C", Helper.CustomInfo, "$12,345.68");
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, Octo)>> ClampTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (Octo.MinValue, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (-Octo.Epsilon, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (Octo.NaN, Octo.One, Octo.Thousand, Octo.NaN);
	    yield return () => (Octo.Zero, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (Octo.Epsilon, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (Octo.GreatestSubnormal, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (Octo.One, Octo.One, Octo.Thousand, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.Thousand, Octo.Thousand);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.Thousand, Octo.Thousand);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> CopySignTestData()
    {
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.One);
	    yield return () => (Octo.One, Octo.NegativeOne, Octo.NegativeOne);
	    yield return () => (Octo.Thousand, Octo.NegativeOne, Octo.NegativeThousand);
	    yield return () => (Octo.NegativeHundred, Octo.NegativeOne, Octo.NegativeHundred);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MaxTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.One);
	    yield return () => (Octo.MinValue, Octo.One, Octo.One);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.One);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, Octo.One);
	    yield return () => (-Octo.Epsilon, Octo.One, Octo.One);
	    yield return () => (Octo.NegativeZero, Octo.One, Octo.One);
	    yield return () => (Octo.NaN, Octo.One, Octo.NaN);
	    yield return () => (Octo.Zero, Octo.One, Octo.One);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.MaxValue);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.PositiveInfinity);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MaxNumberTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.One);
	    yield return () => (Octo.MinValue, Octo.One, Octo.One);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.One);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, Octo.One);
	    yield return () => (-Octo.Epsilon, Octo.One, Octo.One);
	    yield return () => (Octo.NegativeZero, Octo.One, Octo.One);
	    yield return () => (Octo.NaN, Octo.One, Octo.One);
	    yield return () => (Octo.Zero, Octo.One, Octo.One);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.MaxValue);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.PositiveInfinity);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MinTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.NegativeInfinity);
	    yield return () => (Octo.MinValue, Octo.One, Octo.MinValue);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.NegativeOne);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, -Octo.GreatestSubnormal);
	    yield return () => (-Octo.Epsilon, Octo.One, -Octo.Epsilon);
	    yield return () => (Octo.NegativeZero, Octo.One, Octo.NegativeZero);
	    yield return () => (Octo.NaN, Octo.One, Octo.NaN);
	    yield return () => (Octo.Zero, Octo.One, Octo.Zero);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.One);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.One);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MinNumberTestData()
    {
	    yield return () => (Octo.NegativeInfinity, Octo.One, Octo.NegativeInfinity);
	    yield return () => (Octo.MinValue, Octo.One, Octo.MinValue);
	    yield return () => (Octo.NegativeOne, Octo.One, Octo.NegativeOne);
	    yield return () => (-Octo.GreatestSubnormal, Octo.One, -Octo.GreatestSubnormal);
	    yield return () => (-Octo.Epsilon, Octo.One, -Octo.Epsilon);
	    yield return () => (Octo.NegativeZero, Octo.One, Octo.NegativeZero);
	    yield return () => (Octo.NaN, Octo.One, Octo.One);
	    yield return () => (Octo.Zero, Octo.One, Octo.Zero);
	    yield return () => (Octo.One, Octo.One, Octo.One);
	    yield return () => (Octo.MaxValue, Octo.One, Octo.One);
	    yield return () => (Octo.PositiveInfinity, Octo.One, Octo.One);
    }

    public static IEnumerable<Func<(Octo, int)>> SignTestData()
    {
	    yield return () => (Octo.One, 1);
	    yield return () => (Octo.NegativeOne, -1);
	    yield return () => (Octo.Ten, 1);
	    yield return () => (Octo.NegativeTen, -1);
	    yield return () => (Octo.Zero, 0);
	    yield return () => (Octo.NegativeZero, 0);
    }

    public static IEnumerable<Func<(Octo, bool)>> IsPow2TestData()
    {
	    yield return () => (Octo.Half, true);
	    yield return () => (Octo.One, true);
	    yield return () => (Octo.Two, true);
	    yield return () => (Octo.Three, false);
	    yield return () => (Octo.NegativeTwo, false);
    }

    public static IEnumerable<Func<(Octo, Octo)>> Log2TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> CeilingTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> FloorTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, int, MidpointRounding, Octo)>> RoundTestData()
    {
		yield return () => (Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Octo.Four);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.AwayFromZero, Octo.Three);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Octo.Three);
		yield return () => (Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.AwayFromZero, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.AwayFromZero, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Octo.NegativeThree);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.AwayFromZero, Octo.NegativeThree);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Octo.NegativeFour);
		
		yield return () => (Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Octo.Four);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToEven, Octo.Three);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToEven, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToEven, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToEven, Octo.NegativeThree);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Octo.NegativeFour);
		
		yield return () => (Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Octo.Three);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToNegativeInfinity, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToNegativeInfinity, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToNegativeInfinity, Octo.NegativeThree);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Octo.NegativeThree);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToNegativeInfinity, Octo.NegativeThree);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Octo.NegativeFour);
		
		yield return () => (Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Octo.Four);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToPositiveInfinity, Octo.Three);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Octo.Three);
		yield return () => (Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToPositiveInfinity, Octo.Three);
		yield return () => (Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToPositiveInfinity, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToPositiveInfinity, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Octo.NegativeThree);
		
		yield return () => (Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Octo.Three);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToZero, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToZero, Octo.Two);
		yield return () => (Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToZero, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToZero, Octo.NegativeTwo);
		yield return () => (Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Octo.NegativeThree);
    }

    public static IEnumerable<Func<(Octo, Octo)>> TruncateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, int)>> GetExponentByteCountTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, int)>> GetExponentShortestBitLengthTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, int)>> GetSignificandBitLengthTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, int)>> GetSignificandByteCountTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, byte[], bool, int)>> TryWriteExponentBigEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, byte[], bool, int)>> TryWriteExponentLittleEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, byte[], bool, int)>> TryWriteSignificandBigEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, byte[], bool, int)>> TryWriteSignificandLittleEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> Atan2TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> Atan2PiTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> BitDecrementTestData()
    {
	    yield return () => (Octo.One, Values.CreateFloat<Octo>(0x3FFFEFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
	    yield return () => (Octo.NegativeOne, Values.CreateFloat<Octo>(0xBFFFF00000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001));
	    yield return () => (Octo.Zero, -Octo.Epsilon);
	    yield return () => (Octo.NegativeInfinity, Octo.NegativeInfinity);
	    yield return () => (Octo.PositiveInfinity, Octo.MaxValue);
    }

    public static IEnumerable<Func<(Octo, Octo)>> BitIncrementTestData()
    {
	    yield return () => (Octo.One, Values.CreateFloat<Octo>(0x3FFFF00000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001));
	    yield return () => (Octo.NegativeOne, Values.CreateFloat<Octo>(0xBFFFEFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
	    yield return () => (Octo.NegativeZero, Octo.Epsilon);
	    yield return () => (Octo.NegativeInfinity, Octo.MinValue);
	    yield return () => (Octo.PositiveInfinity, Octo.PositiveInfinity);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, Octo)>> FusedMultiplyAddTestData()
    {
	    yield return () => (Octo.One, Octo.One, Octo.One, Octo.Two);
	    yield return () => (Octo.Ten, Octo.Ten, Octo.Zero, Octo.Hundred);
	    yield return () => (Octo.Five, Octo.Zero, Octo.Five, Octo.Five);
	    yield return () => (Octo.Half, Octo.Two, Octo.Two, Octo.Three);
	    yield return () => (Octo.Two, Octo.Four, Octo.Two, Octo.Ten);
	    yield return () => (Octo.Ten, Octo.Half, Octo.Five, Octo.Ten);
	    yield return () => (Values.CreateFloat<Octo>(0xBFFF_F400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), Octo.One, Octo.Two, Values.CreateFloat<Octo>(0x3FFF_E800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> Ieee754RemainderTestData()
    {
	    yield return () => (Octo.Ten, Octo.Three, Octo.One);
	    yield return () => (Octo.Ten, Octo.Two, Octo.Zero);
	    yield return () => (Octo.NegativeTen, Octo.Three, Octo.NegativeOne);
	    yield return () => (Octo.NegativeTen, Octo.Two, Octo.Zero);
	    yield return () => (Octo.NegativeTen, Octo.Zero, Octo.NaN);
    }

    public static IEnumerable<Func<(Octo, int)>> ILogBTestData()
    {
	    yield return () => (Values.CreateFloat<Octo>(0x4000_9000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 10);
	    yield return () => (Values.CreateFloat<Octo>(0x4003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 64);
	    yield return () => (Values.CreateFloat<Octo>(0x4007_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 128);
	    yield return () => (Values.CreateFloat<Octo>(0xC003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 64);
	    yield return () => (Octo.Zero, int.MinValue);
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, Octo)>> LerpTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> ReciprocalEstimateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> ReciprocalSqrtEstimateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, int, Octo)>> ScaleBTestData()
    {
	    yield return () => (Octo.Two, 3, Values.CreateFloat<Octo>(0x4000_3000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	    yield return () => (Octo.NegativeTwo, 3, Values.CreateFloat<Octo>(0xC000_3000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	    yield return () => (Octo.Zero, 6, Octo.Zero);
	    yield return () => (Octo.Two, 300000, Octo.PositiveInfinity);
	    yield return () => (Octo.Two, -300000, Octo.Zero);
    }
    public static IEnumerable<Func<(Octo, byte)>> ConvertToCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, byte)>> ConvertToSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, byte)>> ConvertToTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, ushort)>> ConvertToCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, uint)>> ConvertToCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, uint)>> ConvertToSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, uint)>> ConvertToTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, ulong)>> ConvertToCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, sbyte)>> ConvertToCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, short)>> ConvertToCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, short)>> ConvertToSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, short)>> ConvertToTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, int)>> ConvertToCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, int)>> ConvertToSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, int)>> ConvertToTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, long)>> ConvertToCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, long)>> ConvertToSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, long)>> ConvertToTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Int128)>> ConvertToCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int128)>> ConvertToSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int128)>> ConvertToTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Int256)>> ConvertToCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int256)>> ConvertToSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int256)>> ConvertToTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Int512)>> ConvertToCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int512)>> ConvertToSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int512)>> ConvertToTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, BigInteger)>> ConvertToCheckedBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, BigInteger)>> ConvertToSaturatingBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, BigInteger)>> ConvertToTruncatingBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Half)>> ConvertToCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Half)>> ConvertToSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Half)>> ConvertToTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, float)>> ConvertToCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, float)>> ConvertToSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, float)>> ConvertToTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, double)>> ConvertToCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, double)>> ConvertToSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, double)>> ConvertToTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Quad)>> ConvertToCheckedQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Quad)>> ConvertToSaturatingQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Quad)>> ConvertToTruncatingQuadTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte, Octo)>> ConvertFromCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Octo)>> ConvertFromSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Octo)>> ConvertFromTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ushort, Octo)>> ConvertFromCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Octo)>> ConvertFromSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Octo)>> ConvertFromTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(uint, Octo)>> ConvertFromCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Octo)>> ConvertFromSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Octo)>> ConvertFromTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ulong, Octo)>> ConvertFromCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Octo)>> ConvertFromSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Octo)>> ConvertFromTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt128, Octo)>> ConvertFromCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Octo)>> ConvertFromSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Octo)>> ConvertFromTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(sbyte, Octo)>> ConvertFromCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Octo)>> ConvertFromSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Octo)>> ConvertFromTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(short, Octo)>> ConvertFromCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Octo)>> ConvertFromSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Octo)>> ConvertFromTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(int, Octo)>> ConvertFromCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Octo)>> ConvertFromSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Octo)>> ConvertFromTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(long, Octo)>> ConvertFromCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Octo)>> ConvertFromSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Octo)>> ConvertFromTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int128, Octo)>> ConvertFromCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Octo)>> ConvertFromSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Octo)>> ConvertFromTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(BigInteger, Octo)>> ConvertFromCheckedBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(BigInteger, Octo)>> ConvertFromSaturatingBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(BigInteger, Octo)>> ConvertFromTruncatingBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Half, Octo)>> ConvertFromCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Octo)>> ConvertFromSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Octo)>> ConvertFromTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(float, Octo)>> ConvertFromCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Octo)>> ConvertFromSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Octo)>> ConvertFromTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(double, Octo)>> ConvertFromCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Octo)>> ConvertFromSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Octo)>> ConvertFromTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
}