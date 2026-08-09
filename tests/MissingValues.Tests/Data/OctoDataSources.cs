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
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, byte)>> ConvertToSaturatingByteTestData()
	{
		yield return () => (Octo.NegativeOne, 0);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.MaxValue, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, byte)>> ConvertToTruncatingByteTestData()
	{
		yield return () => (Octo.NegativeOne, 0);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.MaxValue, byte.MaxValue);
	}

	public static IEnumerable<Func<(Octo, ushort)>> ConvertToCheckedUInt16TestData()
	{
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		yield return () => (Octo.NegativeOne, 0);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.MaxValue, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		yield return () => (Octo.NegativeOne, 0);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.MaxValue, ushort.MaxValue);
	}

	public static IEnumerable<Func<(Octo, uint)>> ConvertToCheckedUInt32TestData()
	{
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, uint)>> ConvertToSaturatingUInt32TestData()
	{
		yield return () => (Octo.NegativeOne, 0);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
		yield return () => (Octo.MaxValue, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, uint)>> ConvertToTruncatingUInt32TestData()
	{
		yield return () => (Octo.NegativeOne, 0);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
		yield return () => (Octo.MaxValue, uint.MaxValue);
	}

	public static IEnumerable<Func<(Octo, ulong)>> ConvertToCheckedUInt64TestData()
	{
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
		yield return () => (Octo.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		yield return () => (Octo.NegativeOne, 0);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
		yield return () => (Octo.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Octo.MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		yield return () => (Octo.NegativeOne, 0);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
		yield return () => (Octo.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Octo.MaxValue, ulong.MaxValue);
	}

	public static IEnumerable<Func<(Octo, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		yield return () => (Octo.Half, UInt128.Zero);
		yield return () => (Octo.One, UInt128.One);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
		yield return () => (Octo.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		yield return () => (Octo.NegativeOne, UInt128.Zero);
		yield return () => (Octo.Half, UInt128.Zero);
		yield return () => (Octo.One, UInt128.One);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
		yield return () => (Octo.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (Octo.MaxValue, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		yield return () => (Octo.NegativeOne, UInt128.Zero);
		yield return () => (Octo.Half, UInt128.Zero);
		yield return () => (Octo.One, UInt128.One);
		yield return () => (Octo.ByteMaxValue, byte.MaxValue);
		yield return () => (Octo.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Octo.UInt32MaxValue, uint.MaxValue);
		yield return () => (Octo.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (Octo.MaxValue, UInt128.MaxValue);
	}

	public static IEnumerable<Func<(Octo, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		yield return () => (Octo.Half, UInt256.Zero);
		yield return () => (Octo.One, UInt256.One);
		yield return () => (Octo.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Octo.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Octo.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Octo.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt256.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		yield return () => (Octo.NegativeOne, UInt256.Zero);
		yield return () => (Octo.Half, UInt256.Zero);
		yield return () => (Octo.One, UInt256.One);
		yield return () => (Octo.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Octo.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Octo.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Octo.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt256.UInt128MaxValue);
		yield return () => (Octo.TwoOver256, UInt256.MaxValue);
		yield return () => (Octo.MaxValue, UInt256.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		yield return () => (Octo.NegativeOne, UInt256.Zero);
		yield return () => (Octo.Half, UInt256.Zero);
		yield return () => (Octo.One, UInt256.One);
		yield return () => (Octo.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Octo.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Octo.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Octo.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt256.UInt128MaxValue);
		yield return () => (Octo.TwoOver256, UInt256.MaxValue);
		yield return () => (Octo.MaxValue, UInt256.MaxValue);
	}

	public static IEnumerable<Func<(Octo, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		yield return () => (Octo.Half, UInt512.Zero);
		yield return () => (Octo.One, UInt512.One);
		yield return () => (Octo.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Octo.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Octo.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Octo.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (Octo.TwoOver256, UInt512.UInt256MaxValue + UInt512.One);
	}
	
	public static IEnumerable<Func<(Octo, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		yield return () => (Octo.NegativeOne, UInt512.Zero);
		yield return () => (Octo.Half, UInt512.Zero);
		yield return () => (Octo.One, UInt512.One);
		yield return () => (Octo.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Octo.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Octo.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Octo.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (Octo.TwoOver256, UInt512.UInt256MaxValue + UInt512.One);
		yield return () => (Octo.TwoOver512, UInt512.MaxValue);
		yield return () => (Octo.MaxValue, UInt512.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		yield return () => (Octo.NegativeOne, UInt512.Zero);
		yield return () => (Octo.Half, UInt512.Zero);
		yield return () => (Octo.One, UInt512.One);
		yield return () => (Octo.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Octo.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Octo.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Octo.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Octo.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (Octo.TwoOver256, UInt512.UInt256MaxValue + UInt512.One);
		yield return () => (Octo.TwoOver512, UInt512.MaxValue);
		yield return () => (Octo.MaxValue, UInt512.MaxValue);
	}

	public static IEnumerable<Func<(Octo, sbyte)>> ConvertToCheckedSByteTestData()
	{
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		yield return () => (Octo.MinValue, sbyte.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.MaxValue, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		yield return () => (Octo.MinValue, sbyte.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.MaxValue, sbyte.MaxValue);
	}

	public static IEnumerable<Func<(Octo, short)>> ConvertToCheckedInt16TestData()
	{
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, short)>> ConvertToSaturatingInt16TestData()
	{
		yield return () => (Octo.MinValue, short.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.MaxValue, short.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, short)>> ConvertToTruncatingInt16TestData()
	{
		yield return () => (Octo.MinValue, short.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.MaxValue, short.MaxValue);
	}

	public static IEnumerable<Func<(Octo, int)>> ConvertToCheckedInt32TestData()
	{
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, int)>> ConvertToSaturatingInt32TestData()
	{
		yield return () => (Octo.MinValue, int.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, int)>> ConvertToTruncatingInt32TestData()
	{
		yield return () => (Octo.MinValue, int.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.MaxValue, int.MaxValue);
	}

	public static IEnumerable<Func<(Octo, long)>> ConvertToCheckedInt64TestData()
	{
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, long)>> ConvertToSaturatingInt64TestData()
	{
		yield return () => (Octo.MinValue, long.MinValue);
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
		yield return () => (Octo.MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, long)>> ConvertToTruncatingInt64TestData()
	{
		yield return () => (Octo.MinValue, long.MinValue);
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, -1);
		yield return () => (Octo.Half, 0);
		yield return () => (Octo.One, 1);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
		yield return () => (Octo.MaxValue, long.MaxValue);
	}

	public static IEnumerable<Func<(Octo, Int128)>> ConvertToCheckedInt128TestData()
	{
		yield return () => (Octo.Int128MinValue, Int128.MinValue);
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, Int128.NegativeOne);
		yield return () => (Octo.Half, Int128.Zero);
		yield return () => (Octo.One, Int128.One);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
		yield return () => (Octo.Int128MaxValue, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, Int128)>> ConvertToSaturatingInt128TestData()
	{
		yield return () => (Octo.MinValue, Int128.MinValue);
		yield return () => (Octo.Int128MinValue, Int128.MinValue);
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, Int128.NegativeOne);
		yield return () => (Octo.Half, Int128.Zero);
		yield return () => (Octo.One, Int128.One);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
		yield return () => (Octo.Int128MaxValue, Int128.MaxValue);
		yield return () => (Octo.MaxValue, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, Int128)>> ConvertToTruncatingInt128TestData()
	{
		yield return () => (Octo.MinValue, Int128.MinValue);
		yield return () => (Octo.Int128MinValue, Int128.MinValue);
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, Int128.NegativeOne);
		yield return () => (Octo.Half, Int128.Zero);
		yield return () => (Octo.One, Int128.One);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
		yield return () => (Octo.Int128MaxValue, Int128.MaxValue);
		yield return () => (Octo.MaxValue, Int128.MaxValue);
	}

	public static IEnumerable<Func<(Octo, Int256)>> ConvertToCheckedInt256TestData()
	{
		yield return () => (Octo.Int128MinValue, Int256.Int128MinValue);
		yield return () => (Octo.Int64MinValue, Int256.Int64MinValue);
		yield return () => (Octo.Int32MinValue, Int256.Int32MinValue);
		yield return () => (Octo.Int16MinValue, Int256.Int16MinValue);
		yield return () => (Octo.SByteMinValue, Int256.SByteMinValue);
		yield return () => (Octo.NegativeOne, Int256.NegativeOne);
		yield return () => (Octo.Half, Int256.Zero);
		yield return () => (Octo.One, Int256.One);
		yield return () => (Octo.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (Octo.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (Octo.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (Octo.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (Octo.Int128MaxValue, Int256.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, Int256)>> ConvertToSaturatingInt256TestData()
	{
		yield return () => (Octo.MinValue, Int256.MinValue);
		yield return () => (Octo.Int128MinValue, Int256.Int128MinValue);
		yield return () => (Octo.Int64MinValue, Int256.Int64MinValue);
		yield return () => (Octo.Int32MinValue, Int256.Int32MinValue);
		yield return () => (Octo.Int16MinValue, Int256.Int16MinValue);
		yield return () => (Octo.SByteMinValue, Int256.SByteMinValue);
		yield return () => (Octo.NegativeOne, Int256.NegativeOne);
		yield return () => (Octo.Half, Int256.Zero);
		yield return () => (Octo.One, Int256.One);
		yield return () => (Octo.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (Octo.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (Octo.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (Octo.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (Octo.Int128MaxValue, Int256.Int128MaxValue);
		yield return () => (Octo.TwoOver255, Int256.MaxValue);
		yield return () => (Octo.MaxValue, Int256.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, Int256)>> ConvertToTruncatingInt256TestData()
	{
		yield return () => (Octo.MinValue, Int256.MinValue);
		yield return () => (Octo.Int128MinValue, Int256.Int128MinValue);
		yield return () => (Octo.Int64MinValue, Int256.Int64MinValue);
		yield return () => (Octo.Int32MinValue, Int256.Int32MinValue);
		yield return () => (Octo.Int16MinValue, Int256.Int16MinValue);
		yield return () => (Octo.SByteMinValue, Int256.SByteMinValue);
		yield return () => (Octo.NegativeOne, Int256.NegativeOne);
		yield return () => (Octo.Half, Int256.Zero);
		yield return () => (Octo.One, Int256.One);
		yield return () => (Octo.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (Octo.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (Octo.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (Octo.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (Octo.Int128MaxValue, Int256.Int128MaxValue);
		yield return () => (Octo.TwoOver255, Int256.MaxValue);
		yield return () => (Octo.MaxValue, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Octo, Int512)>> ConvertToCheckedInt512TestData()
	{
		yield return () => (Octo.Int128MinValue, Int512.Int128MinValue);
		yield return () => (Octo.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Octo.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Octo.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Octo.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Octo.NegativeOne, Int512.NegativeOne);
		yield return () => (Octo.Half, Int512.Zero);
		yield return () => (Octo.One, Int512.One);
		yield return () => (Octo.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Octo.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Octo.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Octo.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (Octo.Int128MaxValue, Int512.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, Int512)>> ConvertToSaturatingInt512TestData()
	{
		yield return () => (Octo.MinValue, Int512.MinValue);
		yield return () => (Octo.Int128MinValue, Int512.Int128MinValue);
		yield return () => (Octo.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Octo.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Octo.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Octo.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Octo.NegativeOne, Int512.NegativeOne);
		yield return () => (Octo.Half, Int512.Zero);
		yield return () => (Octo.One, Int512.One);
		yield return () => (Octo.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Octo.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Octo.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Octo.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (Octo.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (Octo.TwoOver255, Int512.Int256MaxValue + Int512.One);
		yield return () => (Octo.TwoOver511, Int512.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, Int512)>> ConvertToTruncatingInt512TestData()
	{
		yield return () => (Octo.MinValue, Int512.MinValue);
		yield return () => (Octo.Int128MinValue, Int512.Int128MinValue);
		yield return () => (Octo.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Octo.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Octo.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Octo.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Octo.NegativeOne, Int512.NegativeOne);
		yield return () => (Octo.Half, Int512.Zero);
		yield return () => (Octo.One, Int512.One);
		yield return () => (Octo.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Octo.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Octo.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Octo.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (Octo.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (Octo.TwoOver255, Int512.Int256MaxValue + Int512.One);
		yield return () => (Octo.TwoOver511, Int512.MaxValue);
	}
	
	public static IEnumerable<Func<(Octo, BigInteger)>> ConvertToCheckedBigIntegerTestData()
	{
		yield return () => (Octo.QuadMinValue, (BigInteger)Quad.MinValue);
		yield return () => (Octo.DoubleMinValue, (BigInteger)double.MinValue);
		yield return () => (Octo.SingleMinValue, (BigInteger)float.MinValue);
		yield return () => (Octo.HalfMinValue, (BigInteger)Half.MinValue);
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, BigInteger.MinusOne);
		yield return () => (Octo.Half, BigInteger.Zero);
		yield return () => (Octo.One, BigInteger.One);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
		yield return () => (Octo.HalfMaxValue, (BigInteger)Half.MaxValue);
		yield return () => (Octo.SingleMaxValue, (BigInteger)float.MaxValue);
		yield return () => (Octo.DoubleMaxValue, (BigInteger)double.MaxValue);
		yield return () => (Octo.QuadMaxValue, (BigInteger)Quad.MaxValue);
	}

	public static IEnumerable<Func<(Octo, BigInteger)>> ConvertToSaturatingBigIntegerTestData()
	{
		yield return () => (Octo.QuadMinValue, (BigInteger)Quad.MinValue);
		yield return () => (Octo.DoubleMinValue, (BigInteger)double.MinValue);
		yield return () => (Octo.SingleMinValue, (BigInteger)float.MinValue);
		yield return () => (Octo.HalfMinValue, (BigInteger)Half.MinValue);
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, BigInteger.MinusOne);
		yield return () => (Octo.Half, BigInteger.Zero);
		yield return () => (Octo.One, BigInteger.One);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
		yield return () => (Octo.HalfMaxValue, (BigInteger)Half.MaxValue);
		yield return () => (Octo.SingleMaxValue, (BigInteger)float.MaxValue);
		yield return () => (Octo.DoubleMaxValue, (BigInteger)double.MaxValue);
		yield return () => (Octo.QuadMaxValue, (BigInteger)Quad.MaxValue);
	}

	public static IEnumerable<Func<(Octo, BigInteger)>> ConvertToTruncatingBigIntegerTestData()
	{
		yield return () => (Octo.QuadMinValue, (BigInteger)Quad.MinValue);
		yield return () => (Octo.DoubleMinValue, (BigInteger)double.MinValue);
		yield return () => (Octo.SingleMinValue, (BigInteger)float.MinValue);
		yield return () => (Octo.HalfMinValue, (BigInteger)Half.MinValue);
		yield return () => (Octo.Int64MinValue, long.MinValue);
		yield return () => (Octo.Int32MinValue, int.MinValue);
		yield return () => (Octo.Int16MinValue, short.MinValue);
		yield return () => (Octo.SByteMinValue, sbyte.MinValue);
		yield return () => (Octo.NegativeOne, BigInteger.MinusOne);
		yield return () => (Octo.Half, BigInteger.Zero);
		yield return () => (Octo.One, BigInteger.One);
		yield return () => (Octo.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Octo.Int16MaxValue, short.MaxValue);
		yield return () => (Octo.Int32MaxValue, int.MaxValue);
		yield return () => (Octo.Int64MaxValue, long.MaxValue);
		yield return () => (Octo.HalfMaxValue, (BigInteger)Half.MaxValue);
		yield return () => (Octo.SingleMaxValue, (BigInteger)float.MaxValue);
		yield return () => (Octo.DoubleMaxValue, (BigInteger)double.MaxValue);
		yield return () => (Octo.QuadMaxValue, (BigInteger)Quad.MaxValue);
	}

	public static IEnumerable<Func<(Octo, Half)>> ConvertToCheckedHalfTestData()
	{
		yield return () => (Octo.NegativeInfinity, Half.NegativeInfinity);
		yield return () => (Octo.HalfMinValue, Half.MinValue);
		yield return () => (Octo.NegativeOne, Half.NegativeOne);
		yield return () => (Octo.Half, (Half)0.5f);
		yield return () => (Octo.One, Half.One);
		yield return () => (Octo.HalfMaxValue, Half.MaxValue);
		yield return () => (Octo.PositiveInfinity, Half.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Octo, Half)>> ConvertToSaturatingHalfTestData()
	{
		yield return () => (Octo.NegativeInfinity, Half.NegativeInfinity);
		yield return () => (Octo.HalfMinValue, Half.MinValue);
		yield return () => (Octo.NegativeOne, Half.NegativeOne);
		yield return () => (Octo.Half, (Half)0.5f);
		yield return () => (Octo.One, Half.One);
		yield return () => (Octo.HalfMaxValue, Half.MaxValue);
		yield return () => (Octo.PositiveInfinity, Half.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Octo, Half)>> ConvertToTruncatingHalfTestData()
	{
		yield return () => (Octo.NegativeInfinity, Half.NegativeInfinity);
		yield return () => (Octo.HalfMinValue, Half.MinValue);
		yield return () => (Octo.NegativeOne, Half.NegativeOne);
		yield return () => (Octo.Half, (Half)0.5f);
		yield return () => (Octo.One, Half.One);
		yield return () => (Octo.HalfMaxValue, Half.MaxValue);
		yield return () => (Octo.PositiveInfinity, Half.PositiveInfinity);
	}

	public static IEnumerable<Func<(Octo, float)>> ConvertToCheckedSingleTestData()
	{
		yield return () => (Octo.NegativeInfinity, float.NegativeInfinity);
		yield return () => (Octo.SingleMinValue, float.MinValue);
		yield return () => (Octo.NegativeOne, -1f);
		yield return () => (Octo.Half, 0.5f);
		yield return () => (Octo.One, 1f);
		yield return () => (Octo.SingleMaxValue, float.MaxValue);
		yield return () => (Octo.PositiveInfinity, float.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Octo, float)>> ConvertToSaturatingSingleTestData()
	{
		yield return () => (Octo.NegativeInfinity, float.NegativeInfinity);
		yield return () => (Octo.SingleMinValue, float.MinValue);
		yield return () => (Octo.NegativeOne, -1f);
		yield return () => (Octo.Half, 0.5f);
		yield return () => (Octo.One, 1f);
		yield return () => (Octo.SingleMaxValue, float.MaxValue);
		yield return () => (Octo.PositiveInfinity, float.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Octo, float)>> ConvertToTruncatingSingleTestData()
	{
		yield return () => (Octo.NegativeInfinity, float.NegativeInfinity);
		yield return () => (Octo.SingleMinValue, float.MinValue);
		yield return () => (Octo.NegativeOne, -1f);
		yield return () => (Octo.Half, 0.5f);
		yield return () => (Octo.One, 1f);
		yield return () => (Octo.SingleMaxValue, float.MaxValue);
		yield return () => (Octo.PositiveInfinity, float.PositiveInfinity);
	}

	public static IEnumerable<Func<(Octo, double)>> ConvertToCheckedDoubleTestData()
	{
		yield return () => (Octo.NegativeInfinity, double.NegativeInfinity);
		yield return () => (Octo.DoubleMinValue, double.MinValue);
		yield return () => (Octo.NegativeOne, -1d);
		yield return () => (Octo.Half, 0.5d);
		yield return () => (Octo.One, 1d);
		yield return () => (Octo.DoubleMaxValue, double.MaxValue);
		yield return () => (Octo.PositiveInfinity, double.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Octo, double)>> ConvertToSaturatingDoubleTestData()
	{
		yield return () => (Octo.NegativeInfinity, double.NegativeInfinity);
		yield return () => (Octo.DoubleMinValue, double.MinValue);
		yield return () => (Octo.NegativeOne, -1d);
		yield return () => (Octo.Half, 0.5d);
		yield return () => (Octo.One, 1d);
		yield return () => (Octo.DoubleMaxValue, double.MaxValue);
		yield return () => (Octo.PositiveInfinity, double.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Octo, double)>> ConvertToTruncatingDoubleTestData()
	{
		yield return () => (Octo.NegativeInfinity, double.NegativeInfinity);
		yield return () => (Octo.DoubleMinValue, double.MinValue);
		yield return () => (Octo.NegativeOne, -1d);
		yield return () => (Octo.Half, 0.5d);
		yield return () => (Octo.One, 1d);
		yield return () => (Octo.DoubleMaxValue, double.MaxValue);
		yield return () => (Octo.PositiveInfinity, double.PositiveInfinity);
	}

	public static IEnumerable<Func<(Octo, Quad)>> ConvertToCheckedQuadTestData()
	{
		yield return () => (Octo.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (Octo.QuadMinValue, Quad.MinValue);
		yield return () => (Octo.NegativeOne, Quad.NegativeOne);
		yield return () => (Octo.Half, Quad.Half);
		yield return () => (Octo.One, Quad.One);
		yield return () => (Octo.QuadMaxValue, Quad.MaxValue);
		yield return () => (Octo.PositiveInfinity, Quad.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Octo, Quad)>> ConvertToSaturatingQuadTestData()
	{
		yield return () => (Octo.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (Octo.QuadMinValue, Quad.MinValue);
		yield return () => (Octo.NegativeOne, Quad.NegativeOne);
		yield return () => (Octo.Half, Quad.Half);
		yield return () => (Octo.One, Quad.One);
		yield return () => (Octo.QuadMaxValue, Quad.MaxValue);
		yield return () => (Octo.PositiveInfinity, Quad.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Octo, Quad)>> ConvertToTruncatingQuadTestData()
	{
		yield return () => (Octo.NegativeInfinity, Quad.NegativeInfinity);
		yield return () => (Octo.QuadMinValue, Quad.MinValue);
		yield return () => (Octo.NegativeOne, Quad.NegativeOne);
		yield return () => (Octo.Half, Quad.Half);
		yield return () => (Octo.One, Quad.One);
		yield return () => (Octo.QuadMaxValue, Quad.MaxValue);
		yield return () => (Octo.PositiveInfinity, Quad.PositiveInfinity);
	}

	public static IEnumerable<Func<(byte, Octo)>> ConvertFromCheckedByteTestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, Octo)>> ConvertFromSaturatingByteTestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, Octo)>> ConvertFromTruncatingByteTestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
	}

	public static IEnumerable<Func<(ushort, Octo)>> ConvertFromCheckedUInt16TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, Octo)>> ConvertFromSaturatingUInt16TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, Octo)>> ConvertFromTruncatingUInt16TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
	}

	public static IEnumerable<Func<(uint, Octo)>> ConvertFromCheckedUInt32TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, Octo)>> ConvertFromSaturatingUInt32TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, Octo)>> ConvertFromTruncatingUInt32TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
	}

	public static IEnumerable<Func<(ulong, Octo)>> ConvertFromCheckedUInt64TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Octo.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, Octo)>> ConvertFromSaturatingUInt64TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Octo.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, Octo)>> ConvertFromTruncatingUInt64TestData()
	{
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Octo.UInt64MaxValue);
	}

	public static IEnumerable<Func<(UInt128, Octo)>> ConvertFromCheckedUInt128TestData()
	{
		yield return () => (UInt128.Zero, Octo.Zero);
		yield return () => (UInt128.One, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Octo.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Octo.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt128, Octo)>> ConvertFromSaturatingUInt128TestData()
	{
		yield return () => (UInt128.Zero, Octo.Zero);
		yield return () => (UInt128.One, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Octo.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Octo.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt128, Octo)>> ConvertFromTruncatingUInt128TestData()
	{
		yield return () => (UInt128.Zero, Octo.Zero);
		yield return () => (UInt128.One, Octo.One);
		yield return () => (byte.MaxValue, Octo.ByteMaxValue);
		yield return () => (ushort.MaxValue, Octo.UInt16MaxValue);
		yield return () => (uint.MaxValue, Octo.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Octo.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Octo.UInt128MaxValue);
	}

	public static IEnumerable<Func<(sbyte, Octo)>> ConvertFromCheckedSByteTestData()
	{
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, Octo)>> ConvertFromSaturatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, Octo)>> ConvertFromTruncatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
	}

	public static IEnumerable<Func<(short, Octo)>> ConvertFromCheckedInt16TestData()
	{
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, Octo)>> ConvertFromSaturatingInt16TestData()
	{
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, Octo)>> ConvertFromTruncatingInt16TestData()
	{
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
	}

	public static IEnumerable<Func<(int, Octo)>> ConvertFromCheckedInt32TestData()
	{
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, Octo)>> ConvertFromSaturatingInt32TestData()
	{
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, Octo)>> ConvertFromTruncatingInt32TestData()
	{
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
	}

	public static IEnumerable<Func<(long, Octo)>> ConvertFromCheckedInt64TestData()
	{
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, Octo)>> ConvertFromSaturatingInt64TestData()
	{
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, Octo)>> ConvertFromTruncatingInt64TestData()
	{
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (-1, Octo.NegativeOne);
		yield return () => (0, Octo.Zero);
		yield return () => (1, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
	}

	public static IEnumerable<Func<(Int128, Octo)>> ConvertFromCheckedInt128TestData()
	{
		yield return () => (Int128.MinValue, Octo.Int128MinValue);
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (Int128.NegativeOne, Octo.NegativeOne);
		yield return () => (Int128.Zero, Octo.Zero);
		yield return () => (Int128.One, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
		yield return () => (Int128.MaxValue, Octo.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Int128, Octo)>> ConvertFromSaturatingInt128TestData()
	{
		yield return () => (Int128.MinValue, Octo.Int128MinValue);
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (Int128.NegativeOne, Octo.NegativeOne);
		yield return () => (Int128.Zero, Octo.Zero);
		yield return () => (Int128.One, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
		yield return () => (Int128.MaxValue, Octo.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Int128, Octo)>> ConvertFromTruncatingInt128TestData()
	{
		yield return () => (Int128.MinValue, Octo.Int128MinValue);
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (Int128.NegativeOne, Octo.NegativeOne);
		yield return () => (Int128.Zero, Octo.Zero);
		yield return () => (Int128.One, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
		yield return () => (Int128.MaxValue, Octo.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(BigInteger, Octo)>> ConvertFromCheckedBigIntegerTestData()
	{
		yield return () => (Values.OctoMinValue, Octo.MinValue);
		yield return () => (Values.QuadMinValue, Octo.QuadMinValue);
		yield return () => ((BigInteger)double.MinValue, Octo.DoubleMinValue);
		yield return () => ((BigInteger)float.MinValue, Octo.SingleMinValue);
		yield return () => ((BigInteger)Half.MinValue, Octo.HalfMinValue);
		yield return () => (Int128.MinValue, Octo.Int128MinValue);
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (BigInteger.MinusOne, Octo.NegativeOne);
		yield return () => (BigInteger.Zero, Octo.Zero);
		yield return () => (BigInteger.One, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
		yield return () => (Int128.MaxValue, Octo.Int128MaxValue);
		yield return () => ((BigInteger)Half.MaxValue, Octo.HalfMaxValue);
		yield return () => ((BigInteger)float.MaxValue, Octo.SingleMaxValue);
		yield return () => ((BigInteger)double.MaxValue, Octo.DoubleMaxValue);
		yield return () => (Values.QuadMaxValue, Octo.QuadMaxValue);
		yield return () => (Values.OctoMaxValue, Octo.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, Octo)>> ConvertFromSaturatingBigIntegerTestData()
	{
		yield return () => (Values.OctoMinValue, Octo.MinValue);
		yield return () => (Values.QuadMinValue, Octo.QuadMinValue);
		yield return () => ((BigInteger)double.MinValue, Octo.DoubleMinValue);
		yield return () => ((BigInteger)float.MinValue, Octo.SingleMinValue);
		yield return () => ((BigInteger)Half.MinValue, Octo.HalfMinValue);
		yield return () => (Int128.MinValue, Octo.Int128MinValue);
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (BigInteger.MinusOne, Octo.NegativeOne);
		yield return () => (BigInteger.Zero, Octo.Zero);
		yield return () => (BigInteger.One, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
		yield return () => (Int128.MaxValue, Octo.Int128MaxValue);
		yield return () => ((BigInteger)Half.MaxValue, Octo.HalfMaxValue);
		yield return () => ((BigInteger)float.MaxValue, Octo.SingleMaxValue);
		yield return () => ((BigInteger)double.MaxValue, Octo.DoubleMaxValue);
		yield return () => (Values.QuadMaxValue, Octo.QuadMaxValue);
		yield return () => (Values.OctoMaxValue, Octo.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, Octo)>> ConvertFromTruncatingBigIntegerTestData()
	{
		yield return () => (Values.OctoMinValue, Octo.MinValue);
		yield return () => (Values.QuadMinValue, Octo.QuadMinValue);
		yield return () => ((BigInteger)double.MinValue, Octo.DoubleMinValue);
		yield return () => ((BigInteger)float.MinValue, Octo.SingleMinValue);
		yield return () => ((BigInteger)Half.MinValue, Octo.HalfMinValue);
		yield return () => (Int128.MinValue, Octo.Int128MinValue);
		yield return () => (long.MinValue, Octo.Int64MinValue);
		yield return () => (int.MinValue, Octo.Int32MinValue);
		yield return () => (short.MinValue, Octo.Int16MinValue);
		yield return () => (sbyte.MinValue, Octo.SByteMinValue);
		yield return () => (BigInteger.MinusOne, Octo.NegativeOne);
		yield return () => (BigInteger.Zero, Octo.Zero);
		yield return () => (BigInteger.One, Octo.One);
		yield return () => (sbyte.MaxValue, Octo.SByteMaxValue);
		yield return () => (short.MaxValue, Octo.Int16MaxValue);
		yield return () => (int.MaxValue, Octo.Int32MaxValue);
		yield return () => (long.MaxValue, Octo.Int64MaxValue);
		yield return () => (Int128.MaxValue, Octo.Int128MaxValue);
		yield return () => ((BigInteger)Half.MaxValue, Octo.HalfMaxValue);
		yield return () => ((BigInteger)float.MaxValue, Octo.SingleMaxValue);
		yield return () => ((BigInteger)double.MaxValue, Octo.DoubleMaxValue);
		yield return () => (Values.QuadMaxValue, Octo.QuadMaxValue);
		yield return () => (Values.OctoMaxValue, Octo.MaxValue);
	}

	public static IEnumerable<Func<(Half, Octo)>> ConvertFromCheckedHalfTestData()
	{
		yield return () => (Half.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (Half.MinValue, Octo.HalfMinValue);
		yield return () => (Half.NegativeOne, Octo.NegativeOne);
		yield return () => (-(Half)0.5f, Octo.NegativeHalf);
		yield return () => (Half.Zero, Octo.Zero);
		yield return () => ((Half)0.5f, Octo.Half);
		yield return () => (Half.One, Octo.One);
		yield return () => (Half.MaxValue, Octo.HalfMaxValue);
		yield return () => (Half.PositiveInfinity, Octo.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Half, Octo)>> ConvertFromSaturatingHalfTestData()
	{
		yield return () => (Half.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (Half.MinValue, Octo.HalfMinValue);
		yield return () => (Half.NegativeOne, Octo.NegativeOne);
		yield return () => (-(Half)0.5f, Octo.NegativeHalf);
		yield return () => (Half.Zero, Octo.Zero);
		yield return () => ((Half)0.5f, Octo.Half);
		yield return () => (Half.One, Octo.One);
		yield return () => (Half.MaxValue, Octo.HalfMaxValue);
		yield return () => (Half.PositiveInfinity, Octo.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Half, Octo)>> ConvertFromTruncatingHalfTestData()
	{
		yield return () => (Half.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (Half.MinValue, Octo.HalfMinValue);
		yield return () => (Half.NegativeOne, Octo.NegativeOne);
		yield return () => (-(Half)0.5f, Octo.NegativeHalf);
		yield return () => (Half.Zero, Octo.Zero);
		yield return () => ((Half)0.5f, Octo.Half);
		yield return () => (Half.One, Octo.One);
		yield return () => (Half.MaxValue, Octo.HalfMaxValue);
		yield return () => (Half.PositiveInfinity, Octo.PositiveInfinity);
	}

	public static IEnumerable<Func<(float, Octo)>> ConvertFromCheckedSingleTestData()
	{
		yield return () => (float.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (float.MinValue, Octo.SingleMinValue);
		yield return () => (-1f, Octo.NegativeOne);
		yield return () => (-0.5f, Octo.NegativeHalf);
		yield return () => (0f, Octo.Zero);
		yield return () => (0.5f, Octo.Half);
		yield return () => (1f, Octo.One);
		yield return () => (float.MaxValue, Octo.SingleMaxValue);
		yield return () => (float.PositiveInfinity, Octo.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(float, Octo)>> ConvertFromSaturatingSingleTestData()
	{
		yield return () => (float.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (float.MinValue, Octo.SingleMinValue);
		yield return () => (-1f, Octo.NegativeOne);
		yield return () => (-0.5f, Octo.NegativeHalf);
		yield return () => (0f, Octo.Zero);
		yield return () => (0.5f, Octo.Half);
		yield return () => (1f, Octo.One);
		yield return () => (float.MaxValue, Octo.SingleMaxValue);
		yield return () => (float.PositiveInfinity, Octo.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(float, Octo)>> ConvertFromTruncatingSingleTestData()
	{
		yield return () => (float.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (float.MinValue, Octo.SingleMinValue);
		yield return () => (-1f, Octo.NegativeOne);
		yield return () => (-0.5f, Octo.NegativeHalf);
		yield return () => (0f, Octo.Zero);
		yield return () => (0.5f, Octo.Half);
		yield return () => (1f, Octo.One);
		yield return () => (float.MaxValue, Octo.SingleMaxValue);
		yield return () => (float.PositiveInfinity, Octo.PositiveInfinity);
	}

	public static IEnumerable<Func<(double, Octo)>> ConvertFromCheckedDoubleTestData()
	{
		yield return () => (double.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (double.MinValue, Octo.DoubleMinValue);
		yield return () => (-1d, Octo.NegativeOne);
		yield return () => (-0.5d, Octo.NegativeHalf);
		yield return () => (0d, Octo.Zero);
		yield return () => (0.5d, Octo.Half);
		yield return () => (1d, Octo.One);
		yield return () => (double.MaxValue, Octo.DoubleMaxValue);
		yield return () => (double.PositiveInfinity, Octo.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(double, Octo)>> ConvertFromSaturatingDoubleTestData()
	{
		yield return () => (double.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (double.MinValue, Octo.DoubleMinValue);
		yield return () => (-1d, Octo.NegativeOne);
		yield return () => (-0.5d, Octo.NegativeHalf);
		yield return () => (0d, Octo.Zero);
		yield return () => (0.5d, Octo.Half);
		yield return () => (1d, Octo.One);
		yield return () => (double.MaxValue, Octo.DoubleMaxValue);
		yield return () => (double.PositiveInfinity, Octo.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(double, Octo)>> ConvertFromTruncatingDoubleTestData()
	{
		yield return () => (double.NegativeInfinity, Octo.NegativeInfinity);
		yield return () => (double.MinValue, Octo.DoubleMinValue);
		yield return () => (-1d, Octo.NegativeOne);
		yield return () => (-0.5d, Octo.NegativeHalf);
		yield return () => (0d, Octo.Zero);
		yield return () => (0.5d, Octo.Half);
		yield return () => (1d, Octo.One);
		yield return () => (double.MaxValue, Octo.DoubleMaxValue);
		yield return () => (double.PositiveInfinity, Octo.PositiveInfinity);
	}
}