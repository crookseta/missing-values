using System.Globalization;
using MissingValues.Tests.Data.Sources;

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
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, bool)>> op_CheckedAdditionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_CheckedDecrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_CheckedIncrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, bool)>> op_CheckedMultiplyTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, bool)>> op_CheckedSubtractionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> op_DecrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_DivisionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> op_IncrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_ModulusTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_MultiplyTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_SubtractionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_BitwiseAndTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_BitwiseOrTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> op_BitwiseXorTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> op_OnesComplementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_EqualityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_InequalityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_GreaterThanOrEqualTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_GreaterThanTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_LessThanOrEqualTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, bool)>> op_LessThanTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> AbsTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsCanonicalTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsComplexNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsEvenIntegerTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsFiniteTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsImaginaryNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsInfinityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsIntegerTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsNaNTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsNegativeTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsNegativeInfinityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsNormalTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsOddIntegerTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsPositiveTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsPositiveInfinityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsRealNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsSubnormalTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsZeroTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MaxMagnitudeTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MaxMagnitudeNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MinMagnitudeTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MinMagnitudeNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, Octo)>> MultiplyAddEstimateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, Octo)>> ParseTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Octo)>> ParseSpanTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Octo)>> ParseUtf8TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Octo)>> TryParseTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Octo)>> TryParseSpanTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Octo)>> TryParseUtf8TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, Octo)>> ClampTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> CopySignTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MaxTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MaxNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MinTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> MinNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> SignTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, bool)>> IsPow2TestData()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo)>> BitIncrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo, Octo)>> FusedMultiplyAddTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> Ieee754RemainderTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Octo, Octo, Octo)>> ILogBTestData()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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

	public static IEnumerable<Func<(UInt256, Octo)>> ConvertFromCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Octo)>> ConvertFromSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Octo)>> ConvertFromTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Octo)>> ConvertFromCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Octo)>> ConvertFromSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Octo)>> ConvertFromTruncatingUInt512TestData()
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

	public static IEnumerable<Func<(Int256, Octo)>> ConvertFromCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Octo)>> ConvertFromSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Octo)>> ConvertFromTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Octo)>> ConvertFromCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Octo)>> ConvertFromSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Octo)>> ConvertFromTruncatingInt512TestData()
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

	public static IEnumerable<Func<(Quad, Octo)>> ConvertFromCheckedQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Octo)>> ConvertFromSaturatingQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Octo)>> ConvertFromTruncatingQuadTestData()
	{
		throw new NotImplementedException();
	}
}